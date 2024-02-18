using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Handlers;

namespace InvoiceForgeApi.Interfaces.Handlers
{
    public interface IRequestHandler<T>
    {
        public void SetData(T? data);
        public void AddError(ApiError ex);
        public void AddError(Exception ex);
        public void AddErrors(List<ApiError> exs);
        public void ResetErrors();
        public bool HasErrors();
        public RequestHandler<T> GetHandler();
        public RequestHandler<T> ResetHandler(T? dataValue);
        public ResponseHandler<T> BuildResponse(HttpResponse response);
    }
}
