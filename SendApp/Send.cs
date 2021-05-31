using RabbitMQ.Client;
using System;
using System.Text;

namespace SendApp
{
    public static class Send
    {
        public static void Main()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                VirtualHost = "Client",
                Port = 5672,
                UserName = "admin",
                Password = "admin"
            };

            using (var connection = factory.CreateConnection())

            using (var channel = connection.CreateModel())
            {
                //No Publisher não tem a necessidade de criar a fila.
                //Apenas criamos por segurança para não perder a mensagem caso o Consumer e a fila ainda não tenham sido criados.

                //var name = "client.queue.consoleapp.v1";

                //channel.QueueDeclare(queue: name,
                //                     durable: false,
                //                     exclusive: false,
                //                     autoDelete: false,
                //                     arguments: null);

                //channel.QueueBind(queue: name,
                //              exchange: "amq.direct",
                //              routingKey: "hello");

                while (true)
                {
                    Console.Write("Digite uma mensagem:");

                    var message = Console.ReadLine();

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "amq.direct",
                                         routingKey: "hello",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine(" [x] Sent {0}", message);

                    Console.WriteLine();
                }
            }
        }
    }
}
