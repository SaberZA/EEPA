using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EEPA.Consumer
{
    public class RpcServer
    {
        public static void Main()
        {
            const string queueName = "fibonacci";
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                channel.BasicQos(0, 1, false);
                var consumer = new QueueingBasicConsumer(channel);
                channel.BasicConsume(queue: queueName,
                    noAck: false,
                    consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                while (true)
                {
                    string response = null;
                    var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" [.] Fib({0})", message);
                        response = DoFib(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "",
                            routingKey: props.ReplyTo,
                            basicProperties: replyProps,
                            body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                            multiple: false);
                    }
                }
            }
        }

        private static string DoFib(string message)
        {
            var fib = JsonConvert.DeserializeObject<Fibonacci>(message);
            var number = fib.Number;

            var answer = Fib(number);
            return JsonConvert.SerializeObject(new MathAnswer(answer));
        }

        /// <summary>
        /// Assumes only valid positive integer input.
        /// Don't expect this one to work for big numbers,
        /// and it's probably the slowest recursive implementation possible.
        /// </summary>
        private static int Fib(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }

            return Fib(n - 1) + Fib(n - 2);
        }
    }

    internal class Fibonacci
    {
        public int Number { get; set; }
        public Fibonacci(int i)
        {
            Number = i;
        }
    }

    internal class MathAnswer
    {
        public int Answer { get; set; }
        public MathAnswer(int i)
        {
            Answer = i;
        }
    }
}