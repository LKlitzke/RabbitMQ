using RabbitMQ.Client;
using System.Text;

namespace Publicador01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var servidor = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672, // Porta para acessar o RabbitMQ
                UserName = "usuario",
                Password = "Senha@123"
            };

            // Criação de conexão com servidor
            var conexao = servidor.CreateConnection();
            {
                // Channel
                using (var canal = conexao.CreateModel())
                {
                    // Criar fila
                    canal.QueueDeclare(
                        queue: "task_queue", // Nome da fila
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    while(true)
                    {
                        // Mensagem
                        string mensagem = Console.ReadLine();

                        if (mensagem == "!finalizar") break;

                        // Converte mensagem em Bytes
                        var corpoMensagem = Encoding.UTF8.GetBytes(mensagem);

                        var props = canal.CreateBasicProperties();
                        props.Persistent = true;

                        canal.BasicPublish(exchange: "",
                            routingKey: "task_queue",
                            basicProperties: null,
                            body: corpoMensagem);

                        Console.WriteLine("[X] enviou {0}", mensagem);
                    }
                    
                }
                Console.WriteLine("Aperte para sair");
                Console.ReadLine();
            }

        }
    }
}