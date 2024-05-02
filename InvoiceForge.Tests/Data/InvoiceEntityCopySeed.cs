using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class InvoiceEntityCopySeed
    {
        public List<InvoiceEntityCopy> Populate()
        {
            return new List<InvoiceEntityCopy>() {
                //COPY CLIENT
                new InvoiceEntityCopy() {
                    OriginClientId = 1,
                    OriginContractorId = null,
                    AddressCopyId = 1,
                    Owner = 1,
                    Type = ClientType.LegalEntity,
                    Name = "ClientName1",
                    IN = 9611280833,
                    TIN = "CZ9611280833",
                },
                //COPY CONTRACTOR
                new InvoiceEntityCopy() {
                    OriginContractorId = 1,
                    OriginClientId = null,
                    AddressCopyId = 2,
                    Owner = 1,
                    Type = ClientType.Entrepreneur,
                    Name = "Contractor1",
                    IN = 123456789,
                    TIN = "CZ123456789",
                    Email = "email@seznam.cz",
                    Www = "www.web.cz",
                    Tel = "774876504",
                    Mobil = "774876504"
                },
            };
        }
    }
}