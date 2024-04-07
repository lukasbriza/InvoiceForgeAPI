using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class AddressRepository: RepositoryExtended<Address, AddressAddRequest>, IAddressRepository
    {
        public AddressRepository(InvoiceForgeDatabaseContext dbContext) : base(dbContext){}

        public async Task<List<AddressGetRequest>?> GetAll(int userId, bool? plain = false)
        {
            await _dbContext.Database.BeginTransactionAsync();
            DbSet<Address> addresses = _dbContext.Address;
            if (plain == false){
                addresses.Include(a => a.Country);
            }
            var addressList = await addresses
                .Select(a => new AddressGetRequest(a, plain))
                .Where(a => a.Owner == userId)
                .ToListAsync();
            return addressList;
        }
        public async Task<AddressGetRequest?> GetById(int addressId, bool? plain = false)
        {
                var address = _dbContext.Address;
                if (plain == false)
                {
                    address.Include(a => a.Country);
                }
                var addressCall = await address.FindAsync(addressId);
                if (addressCall is null) throw new DatabaseCallError("Adress is not in database.");
                var addressResult  = new AddressGetRequest(addressCall, plain);
                return addressResult;
        }
        public async Task<bool> Update(int addressId, AddressUpdateRequest address)
        {
            var localAddress = await Get(addressId);
            if (localAddress is null)
            {
                throw new DatabaseCallError("Address is not in database.");
            }

            localAddress.CountryId = address.CountryId ?? localAddress.CountryId;
            localAddress.City = address.City ?? localAddress.City;
            localAddress.Street = address.Street ?? localAddress.Street;
            localAddress.StreetNumber = address.StreetNumber ?? localAddress.StreetNumber;
            localAddress.PostalCode = address.PostalCode ?? localAddress.PostalCode;
            
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
    }
}