using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EEPA.Domain;
using NUnit.Framework;
using TinyIoC;

namespace EEPA.Test
{
    [TestFixture]
    public class TestDomainServiceFactory
    {

        [Test]
        public void ConstructDomainServiceFactory()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var domainServiceFactory = new DomainServiceFactory();
            //---------------Test Result -----------------------
            Assert.IsNotNull(domainServiceFactory);
            Assert.IsInstanceOf<IDomainServiceFactory>(domainServiceFactory);
        }

        [Test]
        public void ConstructDsfWithContainer()
        {
            //---------------Set up test pack-------------------
            var container = CreateContainer();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            
            var domainServiceFactory = new DomainServiceFactory(container);
            //---------------Test Result -----------------------
        }

        private static TinyIoCContainer CreateContainer()
        {
            var container = new TinyIoCContainer();
            container.Register<IDomainDriver>(new RabbitMqDriver());
            return container;
        }

        [Test]
        public void ConstructFibonacciDomainService()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var fibService = new FibService(new RabbitMqDriver());
            var result = fibService.HandleQuery(null);
            //---------------Test Result -----------------------
            Assert.IsInstanceOf<IDomainService>(fibService);
            Assert.AreEqual("0",result);
        }

        [Test]
        public void CreateFibonacciServiceWithDomainServiceFactory()
        {
            //---------------Set up test pack-------------------
            var domainServiceFactory = CreateDomainServiceFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var domainService = domainServiceFactory.Create<FibService>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(domainService);
            Assert.IsInstanceOf<FibService>(domainService);
            Assert.IsInstanceOf<IDomainService>(domainService);
        }

        [Test]
        public void CreateFibServiceWithRabbitMqDriver()
        {
            //---------------Set up test pack-------------------
            var domainServiceFactory = CreateDomainServiceFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var domainService = domainServiceFactory.Create<FibService>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(domainService.DomainDriver);
        }

        [Test]
        public void CreateFibService_GivenDomainServiceFactory_ShouldReturnFibServiceWithRabbitMqDriver()
        {
            //---------------Set up test pack-------------------
            var domainServiceFactory = CreateDomainServiceFactory();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var domainService = domainServiceFactory.Create<FibService>();
            //---------------Test Result -----------------------
            Assert.IsNotNull(domainService.DomainDriver);
        }


        private IDomainServiceFactory CreateDomainServiceFactory()
        {
            var container = CreateContainer();
            return new DomainServiceFactory(container);
        }
    }
}
