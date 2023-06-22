using RabbitMQ.Client;
using System.Text;

namespace Consumidor1
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
                using(var canal = conexao.CreateModel())
                {
                    // Criar fila
                    canal.QueueDeclare(
                        queue: "Ola", // Nome da fila
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    // Mensagem
                    string mensagem = "Olá mundo!";

                    // Converte mensagem em Bytes
                    var corpoMensagem = Encoding.UTF8.GetBytes(mensagem);

                    canal.BasicPublish(exchange: "",
                        routingKey: "Ola",
                        basicProperties: null,
                        body: corpoMensagem);

                    Console.WriteLine("[X] enviou {0}", mensagem);
                }
                Console.WriteLine("Aperte para sair");
                Console.ReadLine();
            }
            
        }
    }
}