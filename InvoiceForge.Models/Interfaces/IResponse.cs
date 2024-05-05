namespace InvoiceForgeApi.Models.Interfaces
{
    public interface IResponse<T>
    {
        T? Data { get; set; }
        IError? Error { get; set; }
    }
}