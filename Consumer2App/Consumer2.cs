using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace App
{
    public static class Consumer2
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
                var name = "client.queue.consoleapp.v2";

                channel.QueueDeclare(queue: name,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                var consumer = new EventingBasicConsumer(channel);

                //RabbitMQ.Client recebe uma mensagem por vez
                consumer.Received += (sender, ea) =>
                {
                    var body = ea.Body.ToArray();

                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine("Received: {0}", message);

                    int dots = message.Split('.').Length - 1;

                    Thread.Sleep(dots * 1000);

                    Console.WriteLine("Value: {0}", message);

                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                };

                channel.BasicConsume(queue: name,
                                     autoAck: false,
                                     consumer: consumer);

                Console.WriteLine("Waiting messages (press [enter] to exit)");

                Console.ReadLine();
            }
        }
    }
}
