namespace EEPA.Domain
{
    public interface IDomainService
    {
        string HandleQuery(dynamic args);
        IDomainDriver DomainDriver { get; set; }
        string HandleType { get; }
    }
}