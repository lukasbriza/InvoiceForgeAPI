using InvoiceForgeApi.Models;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class UserSeed
    {
        public List<User> Populate()
        {
            return new List<User>()
            {
                new User()
                {
                    AuthenticationId = 1,

                },
                new User()
                {
                    AuthenticationId = 2,
                }
            };
        }
    }
}
