using EEPA.Producer;

namespace EEPA.Api.Bootstrapper
{
    public interface IMessageService
    {
        string Send(string message);
        TR Send<T,TR>(IDomainMessage<T> domainMessage);
    }
}