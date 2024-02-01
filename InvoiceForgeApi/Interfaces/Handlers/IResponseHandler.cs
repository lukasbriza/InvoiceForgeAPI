using InvoiceForgeApi.DTO;

namespace InvoiceForgeApi.Interfaces.Handlers
{
    public interface IResponseHandler<T>
    {
        T Data { get; set; }
        List<ResponseErrorDTO> ErrorMap { get; set; }
    }
}
