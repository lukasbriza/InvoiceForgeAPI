using System.ComponentModel;

namespace InvoiceForgeApi.Data.Enum
{
    public enum ErrorCodes 
    {
        [Description("UndefinedError")]
        U_E,
        [Description("DatabaseCallError")]
        DB_C_E,
        [Description("ValidationError")]
        V_E
    }
}
