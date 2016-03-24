using EEPA.Api.Bootstrapper;
using EEPA.Producer;
using Nancy;
using Nancy.ModelBinding;

namespace EEPA.Api.Modules
{
    public class UserModule : NancyModule
    {
        private readonly IMessageService _messageService;

        public UserModule(IMessageService messageService)
            : base("/api")
        {
            _messageService = messageService;
            Post["/user"] = CreateUser;
        }

        private dynamic CreateUser(dynamic o)
        {
            var user = this.Bind<User>();

            _messageService.Send<User,User>(user);

            return user;
        }
    }
}