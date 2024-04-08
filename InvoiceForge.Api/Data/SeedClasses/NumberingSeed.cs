using InvoiceForgeApi.Enum;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class NumberingSeed
    {
        public List<Numbering> Populate()
        {
            return new List<Numbering>
            {
                new Numbering()
                {
                    Owner = 1,
                    NumberingTemplate = new List<NumberingVariable>{
                        NumberingVariable.Year,
                        NumberingVariable.Month,
                        NumberingVariable.Day,
                        NumberingVariable.Number,
                        NumberingVariable.Number,
                        NumberingVariable.Number
                    },
                    NumberingPrefix = "CZ",
                },
                new Numbering()
                {
                    Owner = 1,
                    NumberingTemplate = new List<NumberingVariable>{
                        NumberingVariable.Year,
                        NumberingVariable.Month,
                        NumberingVariable.Day,
                        NumberingVariable.Number,
                    }
                }
            };
        }
    }
}