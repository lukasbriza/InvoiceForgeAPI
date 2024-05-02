using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.DTO;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class AddressRepository: 
        RepositoryExtended<Address, AddressAddRequest>, 
        IAddressRepository
    {
        public AddressRepository(InvoiceForgeDatabaseContext dbContext) : base(dbContext){}

        public async Task<List<AddressGetRequest>?> GetAll(int userId, bool? plain = false)
        {
            DbSet<Address> addresses = _dbContext.Address;
            if (plain == false){
                await addresses.Include(a => a.Country).LoadAsync();
            }
            var addressList = await addresses
                .Where(a => a.Owner == userId)
                .ToListAsync();
            

            return addressList.ConvertAll(a => new AddressGetRequest(a, plain));
        }
        public async Task<AddressGetRequest?> GetById(int addressId, bool? plain = false)
        {
                var address = _dbContext.Address;
                if (plain == false)
                {
                    await address.Include(a => a.Country).LoadAsync();
                }
                var addressCall = await address.FindAsync(addressId);
                if (addressCall is null) throw new DatabaseCallError("Adress is not in database.");
                var addressResult  = new AddressGetRequest(addressCall, plain);
                return addressResult;
        }
        public async Task<bool> Update(int addressId, AddressUpdateRequest address)
        {
            var localAddress = await Get(addressId);
            if (localAddress is null) throw new DatabaseCallError("Address is not in database.");

            var localSelect = new { localAddress.CountryId, localAddress.City, localAddress.Street, localAddress.StreetNumber, localAddress.PostalCode};
            var updateSelect = new { address.CountryId, address.City, address.Street, address.StreetNumber, address.PostalCode };
            if (localSelect.Equals(updateSelect)) throw new ValidationError("One of properties must be different from actual ones.");

            localAddress.CountryId = address.CountryId;
            localAddress.City = address.City;
            localAddress.Street = address.Street;
            localAddress.StreetNumber = address.StreetNumber;
            localAddress.PostalCode = address.PostalCode;
            
            var update = _dbContext.Update(localAddress);
            return update.State == EntityState.Modified;
        }
        public async Task<bool> IsUnique(int userId, AddressAddRequest address)
        {
            var isInDatabase = await _dbContext.Address.AnyAsync((a) => 
                a.Owner == userId && 
                a.City == address.City && 
                a.Street == address.Street && 
                a.StreetNumber == address.StreetNumber && 
                a.CountryId == address.CountryId && 
                a.PostalCode == address.PostalCode
            );
            return !isInDatabase;
        }

        public async Task<bool> IsUnique(int userId, Address address)
        {
            var isInDatabase = await _dbContext.Address.AnyAsync((a) =>
                a.Owner == userId &&
                a.City == address.City && 
                a.Street == address.Street && 
                a.StreetNumber == address.StreetNumber && 
                a.CountryId == address.CountryId && 
                a.PostalCode == address.PostalCode
            );
            return !isInDatabase;
        }
    }
}