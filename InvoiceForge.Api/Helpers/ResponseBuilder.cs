using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models.Interfaces;

namespace InvoiceForgeApi.Helpers
{
    public class RepsonseError: IError
    {
        public string name { get; set;} = null!;
        public string message { get; set; } = null!;
    }
    public class CustomResponse<T>: IResponse<T>
    {
        public T? Data { get; set; }
        public IError? Error { get; set; }
    }

    public class ResponseBuilder<T>
    {
        public T Data { get; set; }
        public RepsonseError? Error { get; set; }

        public ResponseBuilder(T data, ApiError? error)
        {
            Data = data;
            if (error != null) Error = new RepsonseError { name = error.ErrorType, message = error.Message};
        }

        public CustomResponse<T> Get()
        {
            return new CustomResponse<T>
            {
                Data = Data,
                Error = Error
            };
        }
    }
}