using RabbitMQ.Client;
using System;
using System.Text;

namespace App
{
    public static class Publisher2
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
                while (true)
                {
                    Console.WriteLine("Enter message (or quit to exit)");

                    Console.Write("> ");

                    var value = Console.ReadLine();

                    if ("quit".Equals(value, StringComparison.OrdinalIgnoreCase))
                        break;

                    var body = Encoding.UTF8.GetBytes(value);

                    var properties = channel.CreateBasicProperties();

                    properties.Persistent = true;

                    var name = "client.queue.consoleapp.v2";

                    channel.BasicPublish(exchange: "",
                                         routingKey: name,
                                         basicProperties: properties,
                                         body: body);
                }
            }
        }
    }
}
