//Servidor do Rabbitmq
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var servidor = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "usuario", Password = "Senha@123" };

// Conexão com o servidor
var conexao = servidor.CreateConnection();
{
    // Channel
    using (var canal = conexao.CreateModel())
    {
        canal.ExchangeDeclare(exchange: "mensagem_empresa", type: ExchangeType.Direct);

        var queueName = canal.QueueDeclare().QueueName;

        canal.QueueBind(
            queue: queueName,
            exchange: "mensagem_empresa",
            routingKey: "Compras");

        Console.WriteLine("[VENDAS]\nAguardando mensagens.");


        var consumidor = new EventingBasicConsumer(canal);
        // Evento do listener
        consumidor.Received += (model, ea) =>
        {
            // Recebe corpo do envio
            var corpo = ea.Body.ToArray();

            // Converte em Bytes
            var mensagem = Encoding.UTF8.GetString(corpo);

            Console.WriteLine(" [x] Recebido {0}", mensagem);
        };

        // Remove da fila
        canal.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumidor);

        Console.WriteLine(" Pressione [enter] para finalizar.");
        Console.ReadLine();
    }

}