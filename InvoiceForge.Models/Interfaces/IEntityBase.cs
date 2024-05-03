namespace InvoiceForgeApi.Models.Interfaces
{

    public interface IEntityBase: IEntityId, ITrackable {
        int Owner {  get; set; }
    }
    public interface ICodeListBase: IEntityId, ITrackable {
        string Value { get; set; }
    }
    public interface IEntityId
    {
        int Id { get; set; }
    }
    public interface ITrackable
    {
        DateTime Created { get; set; }
        DateTime? Updated { get; set; }
    }
}