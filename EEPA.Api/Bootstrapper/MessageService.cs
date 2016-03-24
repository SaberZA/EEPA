using EEPA.Producer;

namespace EEPA.Api.Bootstrapper
{
    public class MessageService : IMessageService
    {
        private RpcClient _rpcClient;

        public MessageService()
        {
            _rpcClient = new RpcClient();
        }

        public string Send(string message)
        {
            return _rpcClient.Call(message);
        }

        public TR Send<T,TR>(IDomainMessage<T> domainMessage)
        {
            var messageResponse = _rpcClient.Call<T,TR>(domainMessage);
            return messageResponse;
        }
    }
}