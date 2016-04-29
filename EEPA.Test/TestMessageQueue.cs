using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Drawing;
using EEPA.Producer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace EEPA.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class TestMessageQueue
    {
        private const string QueueName = "hello";
        //private const string QueueName = "amq.rabbitmq.reply-to";

        [Test]
        public void TestMessageQueueIsRunning()
        {
            //---------------Set up test pack-------------------
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            
            var message = "Hello World!";
            var encodedMessage = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", QueueName, null, encodedMessage);
            Console.WriteLine(" [x] Sent '" + message + "'");
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            channel.Close();
            connection.Close();
            
        }

        [Test]
        public void SendRpcMessage_GivenHello_ShouldReturnHelloWorld()
        {
            //---------------Set up test pack-------------------
            Task.Factory.StartNew(RPCServer.Main);
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var rpcClient = new RpcClient();

            Debug.WriteLine(" [x] Requesting fib(30)");
            var response = rpcClient.Call("30");
            Debug.WriteLine(string.Format(" [.] Got '{0}'", response));

            rpcClient.Close();
            //---------------Test Result -----------------------
            Assert.AreEqual("832040", response);
        }

        [Test]
        public void SendRpcMessageToQueue()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var rpcClient = new RpcClient();

            Debug.WriteLine(" [x] Requesting fib(30)");
            var response = rpcClient.Call("30");
            //---------------Test Result -----------------------
            rpcClient.Close();
            
        }

        [Test]
        public void StartRpcServer()
        {
            //---------------Set up test pack-------------------
            RPCServer.Main();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            
        }
    }
}
