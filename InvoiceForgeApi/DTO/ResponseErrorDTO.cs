using InvoiceForgeApi.Data.Enum;

namespace InvoiceForgeApi.DTO
{
    public class ResponseErrorDTO
    {
        public ErrorCodes Code { get; set; } = ErrorCodes.U_E;
        public string? Message { get; set; } = null;
        public string? Trace { get; set; } = null;

        public ResponseErrorDTO(ErrorCodes code, string? message, string? trace)
        {
            Code = code;
            Message = message;
            Trace = trace;
        }
    }
}
