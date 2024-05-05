namespace InvoiceForgeApi.Errors
{
    public class NoEntityError: ApiError
    {
        public NoEntityError(): base("Entity is not in database.", "NoEntityError") {}
        public NoEntityError(Exception inner) : base(inner, "NoEntityError") {}
    }
}