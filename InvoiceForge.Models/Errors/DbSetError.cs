namespace InvoiceForgeApi.Errors
{
    public class DbSetError: ApiError
    {
        public DbSetError(): base("There is not given set in context.", "DbSetError") {}
        public DbSetError(Exception inner): base(inner, "DbSetError") {}
    }
}