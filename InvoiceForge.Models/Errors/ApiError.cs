namespace InvoiceForgeApi.Errors
{
    public class ApiError: Exception
    {
        public string ErrorType { get; set; } = null!;
        public ApiError(Exception inner): base(inner.Message, inner) 
        {
            ErrorType = "UndefinedError";
        }
        public ApiError(string message, string errorType ) : base(message) 
        {
            ErrorType = errorType;
        }
        public ApiError(Exception inner, string errorType) : base(inner.Message, inner) {
            ErrorType = errorType;
        }
    }
}