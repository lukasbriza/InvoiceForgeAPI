using InvoiceForgeApi.Data;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using InvoiceForgeApi.DTO;
using Microsoft.EntityFrameworkCore;
using InvoiceForgeApi.DTO.Model;

namespace InvoiceForgeApi.Repository
{
    public class UserRepository: IUserRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public UserRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserGetRequest?> GetById(int id)
        {
            var user = await _dbContext.User
                .Include(u => u.Clients)
                .Include(u => u.Addresses)
                .Include(u => u.Contractors)
                .Include(u => u.UserAccounts)
                .Select(u => new UserGetRequest
                    {
                        Id =  u.Id,
                        Contractors = u.Contractors.Select(c => new ContractorGetRequest
                            {
                                Id = c.Id,
                                Owner = c.Owner,
                                ClientType = c.ClientType,
                                ContractorName = c.ContractorName,
                                IN = c.IN,
                                TIN = c.TIN,
                                Email = c.Email,
                                Mobil = c.Mobil,
                                Tel = c.Tel,
                                Www = c.Www,
                                Address = new AddressGetRequest
                                {
                                    Id = c.Address!.Id,
                                    Owner = c.Address.Owner,
                                    Street = c.Address.Street,
                                    StreetNumber = c.Address.StreetNumber,
                                    City = c.Address.City,
                                    PostalCode = c.Address.PostalCode,
                                    Country = new CountryGetRequest
                                    {
                                        Id = c.Address.Country!.Id,
                                        Value = c.Address.Country.Value,
                                        Shortcut = c.Address.Country.Shortcut,
                                    }
                                }
                            }
                        ),
                        Clients =  u.Clients.Select(c => new ClientGetRequest
                            {
                                Id = c.Id,
                                AddressId = (int)c.AddressId!,
                                Owner = c.Owner,
                                Type = c.Type,
                                ClientName = c.ClientName,
                                IN = c.IN,
                                TIN = c.TIN,
                                Mobil = c.Mobil,
                                Tel = c.Tel,
                                Email = c.Email,
                                Address = new AddressGetRequest
                                {
                                    Id = c.Address!.Id,
                                    Owner = c.Address.Owner,
                                    Street = c.Address.Street,
                                    StreetNumber = c.Address.StreetNumber,
                                    City = c.Address.City,
                                    PostalCode = c.Address.PostalCode,
                                    Country = new CountryGetRequest
                                    {
                                        Id = c.Address.Country!.Id,
                                        Value = c.Address.Country.Value,
                                        Shortcut = c.Address.Country.Shortcut,
                                    }

                                }
                            }
                        ),
                        UserAccounts = u.UserAccounts.Select(u => new UserAccountGetRequest
                            {
                                Id = u.Id,
                                Owner = u.Owner,
                                BankId = u.BankId,
                                AccountNumber = u.AccountNumber,
                                IBAN = u.IBAN,
                                Bank = new BankGetRequest
                                {
                                    Id = u.Bank!.Id,
                                    Value = u.Bank.Value,
                                    Shortcut = u.Bank.Shortcut,
                                    SWIFT = u.Bank.SWIFT
                                }
                            }
                        ),
                        Addresses = u.Addresses.Select(u => new AddressGetRequest
                            {
                                Id = u.Id,
                                Owner = u.Owner,
                                City = u.City,
                                PostalCode = u.PostalCode,
                                Street = u.Street,
                                StreetNumber = u.StreetNumber,
                                Country = new CountryGetRequest
                                {
                                    Id = u.Country!.Id,
                                    Value = u.Country!.Value,
                                    Shortcut = u.Country.Shortcut
                                }
                            }
                        )
                    }
                )
                .Where(u => u.Id == id).ToListAsync();

            if (user.Count == 0)
            {
                throw new DatabaseCallError("User is not in database.");
            }

            return user[0];
        }
        public async Task<bool> Delete(int id)
        {
            var user = await Get(id);
                
            if (user is null)
            {
                throw new DatabaseCallError("User is not in database.");
            }

            _dbContext.User.Remove(user);
            return true;
        }

        public async Task<bool> Update(int userId, UserUpdateRequest user)
        {
            if (user is null)
            {
                throw new ValidationError("User is not provided.");
            }

            var localUser = await Get(userId);

            if (localUser == null)
            {
                throw new DatabaseCallError("User is not in database.");
            }

            var userWithNewAuthIdExists = await _dbContext.User
                .Where(u => u.AuthenticationId == user.AuthenticationId)
                .ToListAsync();

            if (userWithNewAuthIdExists.Count > 0)
            {
                throw new ValidationError("Provided values are incorrect.");
            }

            localUser.AuthenticationId = user.AuthenticationId;
            return true;
        }

        public async Task<bool> Add(int userId, UserAddRequest user)
        {

            if(user is null)
            {
                throw new ValidationError("User is not provided.");
            }

            var newUser = new User {AuthenticationId = user.AuthenticationId};
            await _dbContext.User.AddAsync(newUser);
            return true;
        }
        private async Task<User?> Get(int id)
        {
            return await _dbContext.User.FindAsync(id);
        }

    }
}
