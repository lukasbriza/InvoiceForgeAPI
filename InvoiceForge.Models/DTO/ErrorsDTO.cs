using InvoiceForgeApi.Models.Enum;

namespace InvoiceForgeApi.DTO
{   
    public class ApiError: Exception
    {
        public ErrorCodes? Code { get; set; } = null;
        public ApiError(Exception inner): base(inner.Message, inner) 
        {
            Code = ErrorCodes.U_E;
        }
        public ApiError(string message, ErrorCodes code ) : base(message) 
        {
            Code = code;
        }
        public ApiError(Exception inner, ErrorCodes code) : base(inner.Message, inner) {
            Code = code;
        }
    }

    public class DatabaseCallError: ApiError
    {
        public DatabaseCallError(string message): base(message, ErrorCodes.DB_C_E) {}
        public DatabaseCallError(Exception inner) : base(inner, ErrorCodes.DB_C_E) {}

    }

    public class ValidationError: ApiError
    {
        public ValidationError(string message) : base(message, ErrorCodes.V_E) { }
        public ValidationError(Exception inner) : base(inner, ErrorCodes.V_E) { }
    }
}
