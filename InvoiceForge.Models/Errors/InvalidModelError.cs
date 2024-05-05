namespace InvoiceForgeApi.Errors
{
    public class InvalidModelError: ApiError
    {
        public InvalidModelError(): base("Invalid model.", "InvalidModelError") {}
        public InvalidModelError(Exception inner) : base(inner, "InvalidModelError") {}
    }
}