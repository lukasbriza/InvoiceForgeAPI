namespace InvoiceForgeApi.Errors
{
    public class EntityReferenceError: ApiError
    {
        public EntityReferenceError(): base("Entity still have reference on another entity.", "EntityReferenceError") {}
        public EntityReferenceError(Exception inner): base(inner, "EntityReferenceError") {}
        
    }
}