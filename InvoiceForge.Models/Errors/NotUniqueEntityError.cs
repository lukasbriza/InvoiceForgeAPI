namespace InvoiceForgeApi.Errors
{
    public class NotUniqueEntityError: ApiError
    {
        public NotUniqueEntityError(): base("Entity is not unique.", "NotUniqueEntityError") {}
        public NotUniqueEntityError(string property): base($"{property} is not unique.", "NotUniqueEntityError") {}
        public NotUniqueEntityError(Exception inner): base(inner, "NotUniqueEntityError") {}
        
    }
}