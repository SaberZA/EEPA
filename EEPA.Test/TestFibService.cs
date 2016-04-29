using System;
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
    public class TestFibService
    {
        private IDomainServiceFactory _domainServiceFactory;
        private IDomainService _domainService;

        [SetUp]
        public void Setup()
        {
            _domainServiceFactory = CreateDomainServiceFactory();
            _domainService = _domainServiceFactory.Create<FibService>();
        }

        [Test]
        public void Construct_FibService()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            
            //---------------Test Result -----------------------
            Assert.IsNotNull(_domainService.DomainDriver);
        }

        [Test]
        public void HandleQuery_GivenFibServiceAnd30_ShouldReturn832040()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var serviceResult = _domainService.HandleQuery(30);
            //---------------Test Result -----------------------
            Assert.AreEqual("832040", serviceResult);
        }

        [Test]
        public void IsAttached_GivenFibService_ShouldReturnTrue()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var isAttached = _domainService.IsAttached;
            //---------------Test Result -----------------------
            Assert.IsTrue(isAttached);
        }

        [Test]
        public void DomainResponse_GivenFibServiceAnd30_ShouldReturn832040()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------
            Assert.IsTrue(_domainService.IsAttached);
            //---------------Execute Test ----------------------
            _domainService.DomainDriver.DomainResponse += s =>
            {
                Assert.AreEqual("832040", s);
            };

            

            //---------------Test Result -----------------------

        }



        private IDomainServiceFactory CreateDomainServiceFactory()
        {
            var container = CreateContainer();
            return new DomainServiceFactory(container);
        }

        private static TinyIoCContainer CreateContainer()
        {
            var container = new TinyIoCContainer();
            container.Register<IDomainDriver>(new RabbitMqDriver());
            return container;
        }
    }
}
