using RabbitMQ.Client;
using System.Text;

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
        // Criar Exchange
        canal.ExchangeDeclare(exchange: "mensagem_empresa", type: ExchangeType.Direct);

        while (true)
        {
            // Mensagem
            Console.WriteLine("Digite a mensagem: ");
            string mensagem = Console.ReadLine();

            if (mensagem == "!finalizar") break;

            // Converte mensagem em Bytes
            var corpoMensagem = Encoding.UTF8.GetBytes(mensagem);

            // RouteKey
            Console.WriteLine("Digite a rota: ");
            string routeKey = Console.ReadLine();

            canal.BasicPublish(exchange: "mensagem_empresa",
                routingKey: routeKey,
                basicProperties: null,
                body: corpoMensagem);

            Console.WriteLine("[X] enviou {0}", mensagem);
        }

    }
    Console.WriteLine("Aperte para sair");
    Console.ReadLine();
}