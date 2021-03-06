﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost"
            };

            string exchangeName = "exchangeDirect";
            string routeKey = "Direct.Key1";
            string queueName = "DirectQueue";

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueBind(queueName, exchangeName, routeKey); //綁定一個消費者

                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
       
                channel.BasicQos(0, 1, false);
               
                //接收到消息事件 consumer.IsRunning
                consumer.Received += (ch, ea) =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body);

                    Console.WriteLine($"Queue:{queueName}收到資料： {message}");
                    channel.BasicAck(ea.DeliveryTag, false);
                };

                channel.BasicConsume(queueName, false, consumer);


                Console.ReadKey();
            }
        }
    }
}
