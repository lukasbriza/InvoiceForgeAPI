

using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;

namespace InvoiceForge.Tests.Data
{
    public class TestUser: User
    {
        public TestUser(): base() {
            Id = 10;
            AuthenticationId = 10;
        }
    }

    public class TestAddress: Address
    {
        public TestAddress(): base() {
            Id = 10;
            Owner = new TestUser().Id;
            CountryId = 1;
            Street = "TestStreet";
            StreetNumber = 10;
            City = "TestCity";
            PostalCode = 28002;
        }
    }

    public class TestClient: Client 
    {
        public TestClient(): base() {
            Id = 100;
            Owner = new TestUser().Id;
            AddressId = new TestAddress().Id;
            Type = ClientType.Entrepreneur;
            Name = "TestClientName";
            IN = 123456789;
            TIN = "CZ123456789";
            Mobil = "+420774876504";
            Tel = null;
            Email = null;
        }
    }

    public class TestContractor: Contractor
    {
        public TestContractor(): base() {
            Id = 100;
            Owner = new TestUser().Id;
            AddressId = new TestAddress().Id;
            Type = ClientType.Entrepreneur;
            Name = "TestContractorName";
            IN = 123456789;
            TIN = "CZ123456789";
            Mobil = "+420774876504";
            Email = "email@mail.cz";
            Tel = null;
            Www = null;
        }
    }

    public class TestInvoiceItem: InvoiceItem
    {
        public TestInvoiceItem(): base() {
            Id = 100;
            Owner = new TestUser().Id;
            ItemName = "TestItemName";
            TariffId = 1;
        }
    }

    public class TestUserAccount: UserAccount
    {
        public TestUserAccount(): base() {
            Id = 100;
            Owner = new TestUser().Id;
            BankId = 1;
            AccountNumber = "123-123-123";
            IBAN = "123344631CZ";
        }
    }

    public class TestNumbering: Numbering
    {
        public TestNumbering(): base() {
            Id = 100;
            Owner = new TestUser().Id;
            NumberingTemplate = new List<NumberingVariable> {
                NumberingVariable.Year,
                NumberingVariable.Month,
                NumberingVariable.Day,
                NumberingVariable.Number,
                NumberingVariable.Number
            };
            NumberingPrefix = "CZ";
        }
    }

    public class TestInvoiceTemplate: InvoiceTemplate
    {
        public TestInvoiceTemplate(): base() {
            Id = 100;
            Owner = new TestUser().Id;
            ClientId = new TestClient().Id;
            ContractorId = new TestContractor().Id;
            UserAccountId = new TestUserAccount().Id;
            CurrencyId = 1;
            TemplateName = "TestTemplateName";
            Created = new DateTime().ToUniversalTime();
            NumberingId = new TestNumbering().Id;
        }
    }

}