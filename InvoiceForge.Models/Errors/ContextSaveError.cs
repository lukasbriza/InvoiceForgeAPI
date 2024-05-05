namespace InvoiceForgeApi.Errors
{
    public class ContextSaveError: ApiError
    {
        public ContextSaveError(): base("Context saving failed.", "ContextSaveError") {}
        public ContextSaveError(Exception inner): base(inner, "ContextSaveError") {}
    }
}