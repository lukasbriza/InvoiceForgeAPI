namespace InvoiceForgeApi.Errors
{
    public class OperationError: ApiError
    {
        public OperationError(): base("Operation failed.", "OperationError") {}
        public OperationError(string message): base("message", "OperationError") {}
        public OperationError(Exception inner): base(inner, "OperationError") {}
        
    }

    public class EqualEntityError: OperationError
    {
        public EqualEntityError(): base("One of properties must be different from actual ones."){}
    }
} 