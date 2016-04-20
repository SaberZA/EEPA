using System;
using System.Diagnostics;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EEPA.Domain
{
    public class RabbitMqDriver : IDomainDriver, IDisposable
    {
        public IConnection Connection;

        public RabbitMqDriver()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            Connection = factory.CreateConnection();
        }

        public void AttachToSystem(string handleType)
        {
            using (var channel = Connection.CreateModel())
            {
                channel.QueueDeclare(queue: handleType,
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queue: handleType,
                    noAck: false,
                    consumer: consumer);
                Debug.WriteLine(" [x] Awaiting RPC requests");

                //while (true)
                //{
                //    string response = null;
                //    var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                //    var body = ea.Body;
                //    var props = ea.BasicProperties;
                //    var replyProps = channel.CreateBasicProperties();
                //    replyProps.CorrelationId = props.CorrelationId;

                //    try
                //    {
                //        var message = Encoding.UTF8.GetString(body);
                //        int n = int.Parse(message);
                //        Console.WriteLine(" [.] fib({0})", message);
                //        response = fib(n).ToString();
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(" [.] " + e.Message);
                //        response = "";
                //    }
                //    finally
                //    {
                //        var responseBytes = Encoding.UTF8.GetBytes(response);
                //        channel.BasicPublish(exchange: "",
                //            routingKey: props.ReplyTo,
                //            basicProperties: replyProps,
                //            body: responseBytes);
                //        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                //            multiple: false);
                //    }
                //}
            }
        }

        public bool IsConnected
        {
            get
            {
                return Connection.IsOpen;
            }
        }

        public void CloseConnection()
        {
            Connection.Close();
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}