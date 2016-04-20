using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEPA.Domain;
using NUnit.Framework;
using RabbitMQ.Client;

namespace EEPA.Test
{
    [TestFixture]
    public class TestDomainDriver
    {
        [Test]
        public void ConstructRabbitMqDriver()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var rabbitMqDriver = new RabbitMqDriver();
            //---------------Test Result -----------------------
            Assert.IsNotNull(rabbitMqDriver);
        }

        [Test]
        public void AttachToSystem_ShouldBeConnectedToQueue()
        {
            //---------------Set up test pack-------------------
            var rabbitMqDriver = CreateRabbitMqDriver();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            rabbitMqDriver.AttachToSystem(typeof(FibMessage).Name);
            //---------------Test Result -----------------------
            Assert.IsTrue(rabbitMqDriver.IsConnected);

            ((RabbitMqDriver)rabbitMqDriver).CloseConnection();
        }

        [Test]
        public void AttachToSystem_GivenNumber_ShouldReturnFibonacciNumber()
        {
            //---------------Set up test pack-------------------
            var rabbitMqDriver = CreateRabbitMqDriver();
            var messageType = typeof (FibMessage).Name;
            DeleteQueue(((RabbitMqDriver)rabbitMqDriver).Connection, messageType);
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            rabbitMqDriver.AttachToSystem(messageType);
            //---------------Test Result -----------------------
            
        }

        private void DeleteQueue(IConnection connection, string messageType)
        {
            using (var channel = connection.CreateModel())
            {
                channel.QueueDelete(messageType);
            }
        }

        private static IDomainDriver CreateRabbitMqDriver()
        {
            var rabbitMqDriver = new RabbitMqDriver();
            return rabbitMqDriver;
        }
    }
}
