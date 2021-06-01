using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace App
{
    public static class Consumer1
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
                var name = "client.queue.consoleapp.v1";

                channel.QueueDeclare(queue: name,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.QueueBind(queue: name,
                              exchange: "amq.direct",
                              routingKey: "hello");

                var consumer = new EventingBasicConsumer(channel);

                //RabbitMQ.Client recebe uma mensagem por vez
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();

                    var message = Encoding.UTF8.GetString(body);

                    //Console.WriteLine("Received: {0}", message);

                    //Thread.Sleep(3000);

                    Console.WriteLine("Value: {0}", message);
                };

                channel.BasicConsume(queue: name,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine("Waiting messages (press [enter] to exit)");

                Console.ReadLine();
            }
        }
    }
}
