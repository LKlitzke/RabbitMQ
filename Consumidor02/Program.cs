using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace Consumidor02
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //Servidor do Rabbitmq
            var servidor = new ConnectionFactory() { HostName = "localhost", Port = 5672, UserName = "usuario", Password = "Senha@123" };

            // Conexão com o servidor
            var conexao = servidor.CreateConnection();
            {
                // Channel
                using (var canal = conexao.CreateModel())
                {
                    // Declara listening queue
                    canal.QueueDeclare(queue: "task_queue",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    canal.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    Console.WriteLine("Aguardando mensagens.");

                    var consumidor = new EventingBasicConsumer(canal);

                    // Evento do listener
                    consumidor.Received += (model, ea) =>
                    {
                        // Recebe corpo do envio
                        var corpo = ea.Body.ToArray();

                        // Converte em Bytes
                        var mensagem = Encoding.UTF8.GetString(corpo);

                        Console.WriteLine(" [x] Recebido {0}", mensagem);

                        canal.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };

                    // Remove da fila
                    canal.BasicConsume(queue: "task_queue",
                                         autoAck: false,
                                         consumer: consumidor);

                    Console.WriteLine(" Pressione [enter] para finalizar.");
                    Console.ReadLine();
                }

            }
        }
    }
}