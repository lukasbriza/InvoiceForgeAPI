using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Interfaces.Handlers;


namespace InvoiceForgeApi.Handlers
{
    public class RequestHandler<T>: IRequestHandler<T>
    {
        public T? Data;
        public List<ApiError> Exceptions = new();
        public void SetData(T? data)
        {  
            Data = data;
        }
        public void AddError(ApiError ex)
        {
            Console.WriteLine(ex);
            Exceptions.Add(ex);
        }
        public void AddError(Exception ex) => AddError(new ApiError(ex));
        public void AddErrors(List<ApiError> exs) => exs.ForEach((ex) => Exceptions.Add(ex));
        public void ResetErrors() => Exceptions.Clear();

        public bool HasErrors() => Exceptions.Count > 0;
        public RequestHandler<T> GetHandler()
        {
            RequestHandler<T> handler = new RequestHandler<T>();
            handler.SetData(Data);
            handler.Exceptions = Exceptions;
            return handler;
        }
        public RequestHandler<T> ResetHandler(T? dataValue)
        {
            
            SetData(dataValue);
            ResetErrors();
            return GetHandler();

        }
        public ResponseHandler<T> BuildResponse(HttpResponse Response)
        {
            var response = new ResponseHandler<T>(Response, this);
            return response;
        }
    }
    
}

