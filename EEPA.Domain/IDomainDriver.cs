using System;
using System.Threading.Tasks;

namespace EEPA.Domain
{
    public interface IDomainDriver
    {
        void AttachToSystem(string handleType);
        bool IsConnected { get; }
        event Func<dynamic, string> DomainEventHook;
        event Action<string> DomainResponse;
    }
}