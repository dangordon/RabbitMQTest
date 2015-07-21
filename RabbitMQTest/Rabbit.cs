using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.Impl;
using QueueDeclareOk = RabbitMQ.Client.QueueDeclareOk;

namespace RabbitMQTest
{
    class Rabbit : IDisposable
    {
        private ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        private QueueingBasicConsumer _consumer;

        public void Connect()
        {
            _connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("hello", false, false, false, null);

            _consumer = new QueueingBasicConsumer(_channel);
            _channel.BasicConsume("hello", true, _consumer);
        }

        public void SendMessage(Message message)
        {
            Byte[] body = Encoding.UTF8.GetBytes(message.ToString());

            _channel.BasicPublish("", "hello", null, body);
        }

        public Message FetchMessage()
        {
            var ea = (BasicDeliverEventArgs)_consumer.Queue.Dequeue();

            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            return Message.FromString(message);
        }

        public uint Count()
        {
            QueueDeclareOk queue = _channel.QueueDeclare("hello", false, false, false, null);
            return queue.MessageCount;
        }

        public void Dispose()
        {
            _channel.Dispose();
            _connection.Dispose();
        }
    }
}
