namespace InvoiceForgeApi.Models
{
    public class UserAccountEntityBase
    {
        public int BankId { get; set; }
        public string AccountNumber { get; set; } = null!;
        public string? IBAN { get; set; }
    }

    public class UserAccountGetRequest: UserAccountEntityBase
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
        public BankGetRequest? Bank { get; set; } = null!;
    }
    public class UserAccountAddRequest: UserAccountEntityBase {}

    public class UserAccountUpdateRequest: UserAccountEntityBase
    {
        public int Owner { get; set; }
    }
}

