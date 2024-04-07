using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IEntityBase: IEntityId {
        int Owner {  get; set; }
        User User { get; set; }
    }
    public interface ICodeListBase: IEntityId {
        string Value { get; set; }
    }
    public interface IEntityId
    {
        int Id { get; set; }
    }
}