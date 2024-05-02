
namespace InvoiceForgeApi.Models.DTO
{
    public class InvoiceUserAccountCopyGetRequest
    {
        public InvoiceUserAccountCopyGetRequest() {}

        public InvoiceUserAccountCopyGetRequest(InvoiceUserAccountCopy? userAccountCopy, bool? plain = false) 
        {
            if (userAccountCopy is not null)
            {
                Id = userAccountCopy.Id;
                OriginId = userAccountCopy.OriginId;
                Owner = userAccountCopy.Owner;
                BankId = userAccountCopy.BankId;
                AccountNumber = userAccountCopy.AccountNumber;
                IBAN = userAccountCopy?.IBAN;
                Bank = plain == false ? new BankGetRequest(userAccountCopy?.Bank) : null;
            }
        }

        public int Id { get; set; }
        public int OriginId {  get; set; }
        public int Owner { get; set; }
        public int BankId { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string? IBAN { get; set; }

        public BankGetRequest? Bank { get; set; } = null!;
    }

    public class InvoiceUserAccountCopyAddRequest
    {
        public InvoiceUserAccountCopyAddRequest() {}
        public InvoiceUserAccountCopyAddRequest(UserAccountGetRequest? userAccount)
        {
            if (userAccount is not null)
            {
                OriginId = userAccount.Id;
                Owner = userAccount.Owner;
                BankId = userAccount.BankId;
                AccountNumber = userAccount.AccountNumber;
                IBAN = userAccount?.IBAN;
            }
        }
        public int OriginId {  get; set; }
        public int BankId { get; set; }
        public int Owner { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string? IBAN { get; set; } = null;
    }
}