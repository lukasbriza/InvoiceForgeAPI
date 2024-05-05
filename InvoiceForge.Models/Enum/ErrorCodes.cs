using System.ComponentModel;

namespace InvoiceForgeApi.Models.Enum
{
    public enum ErrorCodes 
    {
        [Description("UndefinedError")]
        U_E,
        [Description("DatabaseCallError")]
        DB_C_E,
        [Description("NoPossessionError")]
        V_E
    }
}
