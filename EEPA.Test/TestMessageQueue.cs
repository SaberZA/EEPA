using EEPA.Producer;

namespace EEPA.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class TestMessageQueue
    {
     
        [Test]
        public void SendRpcMessage_GivenHello_ShouldReturnHelloWorld()
        {
            var rpcClient = new RpcClient();

            var fibonacci = new Fibonacci(30);
            var fibAnswer = rpcClient.Call<Fibonacci,MathAnswer>(fibonacci);

            rpcClient.Close();

            Assert.AreEqual(832040, fibAnswer.Answer);
        }
    }
}
