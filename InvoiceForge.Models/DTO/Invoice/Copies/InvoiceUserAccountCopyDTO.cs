
namespace InvoiceForgeApi.Models
{
    public class InvoiceUserAccountCopyEntityBase
    {
        public int OriginId {  get; set; }
        public int BankId { get; set; }
        public int Owner { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string? IBAN { get; set; } = null;
    }

    public class InvoiceUserAccountCopyGetRequest: InvoiceUserAccountCopyEntityBase
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
        public BankGetRequest? Bank { get; set; } = null!;
    }

    public class InvoiceUserAccountCopyAddRequest: InvoiceUserAccountCopyEntityBase
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
    }
}