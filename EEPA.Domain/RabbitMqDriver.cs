using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EEPA.Domain
{
    public class RabbitMqDriver : IDomainDriver, IDisposable
    {
        public IConnection Connection;
        private IModel _channel;
        private BasicDeliverEventArgs _ea;
        private IBasicProperties _props;
        private IBasicProperties _replyProps;

        public RabbitMqDriver()
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            Connection = factory.CreateConnection();
            _channel = Connection.CreateModel();
            DomainResponse += PublishMessage;
        }

        public void Dispose()
        {
            Connection.Close();
        }
        
        public void AttachToSystem(string handleType)
        {
            
            using (_channel)
            {
                _channel.QueueDeclare(handleType, false, false, false, null);
                _channel.BasicQos(0, 1, false);
                var consumer = new QueueingBasicConsumer(_channel);
                _channel.BasicConsume(handleType, false, consumer);
                Debug.WriteLine(" [x] Awaiting RPC requests");

                //await Task.Factory.StartNew(() =>
                //{
                    while (true)
                    {
                        _ea = consumer.Queue.Dequeue();

                        var body = _ea.Body;
                        _props = _ea.BasicProperties;
                        _replyProps = _channel.CreateBasicProperties();
                        _replyProps.CorrelationId = _props.CorrelationId;

                        var serviceResponse = "";
                        try
                        {
                            var message = Encoding.UTF8.GetString(body);
                            var n = int.Parse(message);
                            Console.WriteLine(" [.] fib({0})", message);
                            //response = fib(n).ToString();
                            serviceResponse = DomainEventHook(n);
                            DomainResponse(serviceResponse);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(" [.] " + e.Message);
                            serviceResponse = "";
                        }
                        finally
                        {
                            //PublishMessage(serviceResponse,ea,props,replyProps);
                            //_channel.BasicAck(_ea.DeliveryTag, false);
                        }
                    }
                //}
                //);
            }
        }


        private void PublishMessage(string domainResponse, BasicDeliverEventArgs basicDeliverEventArgs, IBasicProperties props, IBasicProperties replyProps)
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(domainResponse);
            _channel.BasicPublish("", props.ReplyTo, replyProps, responseBytes);
            _channel.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
        }

        private void PublishMessage(string domainResponse)
        {
            byte[] responseBytes = Encoding.UTF8.GetBytes(domainResponse);
            _channel.BasicPublish("default", _props.ReplyTo, _replyProps, responseBytes);
            _channel.BasicAck(_ea.DeliveryTag, false);
        }



        public bool IsConnected
        {
            get { return Connection.IsOpen; }
        }

        public event Func<dynamic, string> DomainEventHook;
        public event Action<string> DomainResponse;

        public void CloseConnection()
        {
            Connection.Close();
        }
    }
}