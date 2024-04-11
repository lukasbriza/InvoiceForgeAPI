using System.ComponentModel;

namespace InvoiceForgeApi.Models.Enum
{
    public enum NumberingVariable
    {
        [Description("N")]
        Number,
        [Description("DD")]
        Day,
        [Description("MM")]
        Month,
        [Description("YYYY")]
        Year
    }
}