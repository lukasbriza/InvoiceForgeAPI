namespace InvoiceForgeApi.Models
{
    public class CodeListsAllGetRequest
    {
        public List<CountryGetRequest>? Countries { get; set; } = new List<CountryGetRequest>();
        public List<BankGetRequest>? Banks { get; set; } = new List<BankGetRequest>();
        public List<ClientTypeGetRequest>? ClientTypes { get; set; } = new List<ClientTypeGetRequest>();
        public List<TariffGetRequest>? Tariffs { get; set; } = new List<TariffGetRequest>();
        public List<NumberingVariableGetRequest>? NumberingVariables { get; set; } = new List<NumberingVariableGetRequest>();
        public List<CurrencyGetRequest>? Currencies { get; set; } = new List<CurrencyGetRequest>();
    }

    public class CodeListEntityBase
    {
        public int Id { get; set; }
        public string Value { get; set; } = null!;
    }

    public class BankGetRequest: CodeListEntityBase
    {
        public BankGetRequest() {}
        public BankGetRequest(Bank? bank)
        {
            if (bank is not null)
            {
                Id = bank.Id;
                Value = bank.Value;
                Shortcut = bank.Shortcut;
                SWIFT = bank.SWIFT;
            }
        }
        public string Shortcut { get; set; } = null!;
        public string? SWIFT { get; set; }
    }

    public class BankUpdateRequest 
    {
        public string Value { get; set; } = null!;  
        public string Shortcut { get; set; } = null!;
        public string? SWIFT { get; set; }
    };
    public class BankAddRequest 
    {
        public string Value { get; set; } = null!;  
        public string Shortcut { get; set; } = null!;
        public string? SWIFT { get; set; }
    }

    public class CountryGetRequest: CodeListEntityBase
    {
        public CountryGetRequest(){} 
        public CountryGetRequest(Country? country)
        {
            if (country is not null)
            {
                Id = country.Id;
                Value = country.Value;
                Shortcut = country.Shortcut;
            }
        }
        public string Shortcut { get; set; } = null!;
    }

    public class CurrencyGetRequest: CodeListEntityBase
    {
        public CurrencyGetRequest(){} 
        public CurrencyGetRequest(Currency? currency)
        {
            if (currency is not null)
            {
                Id = currency.Id;
                Value = currency.Value;
            }
        }
    }

    public class TariffGetRequest
    {
        public TariffGetRequest() {}
        public TariffGetRequest(Tariff? tariff)
        {
            if (tariff is not null)
            {
                Id = tariff.Id;
                Value = tariff.Value;
            }
        }
        public int Id { get; set; }
        public int Value { get; set; }
    }

    public class NumberingVariableGetRequest: CodeListEntityBase {}
}
