using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EEPA.Producer
{
    public class RpcClient
    {
        private IConnection connection;
        private IModel channel;
        private string replyQueueName;
        private QueueingBasicConsumer consumer;

        public RpcClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new QueueingBasicConsumer(channel);
            channel.BasicConsume(queue: replyQueueName,
                noAck: true,
                consumer: consumer);
        }

        public string Call(string message, string queueName = "rpc_queue")
        {
            var corrId = Guid.NewGuid().ToString();
            var props = channel.CreateBasicProperties();
            props.ReplyTo = replyQueueName;
            props.CorrelationId = corrId;

            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                routingKey: queueName,
                basicProperties: props,
                body: messageBytes);

            while (true)
            {
                var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                if (ea.BasicProperties.CorrelationId == corrId)
                {
                    return Encoding.UTF8.GetString(ea.Body);
                }
            }
        }

        public void Close()
        {
            connection.Close();
        }

        public string Call(IDomainMessage message)
        {
            var messageBody = JsonConvert.SerializeObject(message);
            return Call(messageBody, message.GetType().Name);
        }


        //public TR Execute<T, TR>(IDomainMessage<T> message)
        //{
        public TR Call<T, TR>(IDomainMessage message)
        {
            var messageBody = JsonConvert.SerializeObject(message);
            var answer =  Call(messageBody,typeof(T).Name.ToLower());

            return JsonConvert.DeserializeObject<TR>(answer);
        }
    }
}