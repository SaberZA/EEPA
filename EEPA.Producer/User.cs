using Newtonsoft.Json;

namespace EEPA.Producer
{
    public class User : IDomainMessage<User>
    {
        public string UserName { get; set; }
    }

    public class Fibonacci : IDomainMessage<Fibonacci>
    {

        public Fibonacci(int number)
        {
            Number = number;
        }

        public int Number { get; set; }
    }

    public class AdminUser : User, IDomainMessage<AdminUser>
    {
        public bool Boss = true;
    }
}