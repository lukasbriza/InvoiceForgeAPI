namespace InvoiceForgeApi.Errors
{
    public class NoPossessionError: ApiError
    {
        public NoPossessionError(): base("Provided entity or entity reference is not in your possession.", "NoPossessionError") {}
        public NoPossessionError(Exception inner): base(inner, "NoPossessionError") {}
        
    }
} 
