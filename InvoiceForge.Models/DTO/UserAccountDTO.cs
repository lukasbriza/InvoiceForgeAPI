using InvoiceForgeApi.Models;

namespace InvoiceForgeApi.DTO.Model
{
    public class UserAccountGetRequest
    {
        public UserAccountGetRequest () {}
        public UserAccountGetRequest(UserAccount? userAccount, bool? plain = false)
        {
            if (userAccount is not null)
            {
                Id = userAccount.Id;
                Owner = userAccount.Owner;
                BankId = userAccount.BankId;
                IBAN = userAccount.IBAN;
                AccountNumber = userAccount.AccountNumber;
                Bank = plain == false ? new BankGetRequest(userAccount.Bank) : null;
            }
        }
        public int Id { get; set; }
        public int Owner { get; set; }
        public int? BankId { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string? IBAN { get; set; }

        public BankGetRequest? Bank { get; set; } = null!;
    }
    public class UserAccountAddRequest
    {
        public int BankId { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string? IBAN { get; set; }
    }

    public class UserAccountUpdateRequest
    {
        public int Owner { get; set; }
        public int? BankId { get; set; }
        public string? AccountNumber { get; set; } = null!;
        public string? IBAN { get; set; }
    }
}

