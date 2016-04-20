namespace EEPA.Domain
{
    public class FibService : IDomainService
    {
        public string HandleQuery(dynamic args)
        {
            return Fib(args).ToString();
        }

        public IDomainDriver DomainDriver { get; set; }

        public string HandleType
        {
            get { return typeof(FibMessage).Name; }
        }

        public FibService(IDomainDriver domainDriver)
        {
            DomainDriver = domainDriver;

            domainDriver.AttachToSystem(HandleType);

            domainDriver.DomainEventHook += HandleQuery;
        }

        /// <summary>
        /// Assumes only valid positive integer input.
        /// Don't expect this one to work for big numbers,
        /// and it's probably the slowest recursive implementation possible.
        /// </summary>
        private static int Fib(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }

            return Fib(n - 1) + Fib(n - 2);
        }
    }
}