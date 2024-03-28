using System.Linq.Expressions;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.EntityFrameworkCore;

namespace InvoiceForgeApi.Repository
{
    public class AddressRepository: IAddressRepository
    {
        private readonly InvoiceForgeDatabaseContext _dbContext;
        public AddressRepository(InvoiceForgeDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
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
        public async Task<int?> Add(int userId, AddressAddRequest address)
        {
            var newAddress = new Address
            {
                Owner = userId,
                CountryId = address.CountryId,
                Street = address.Street,
                StreetNumber = address.StreetNumber,
                City = address.City,
                PostalCode = address.PostalCode
            };
            var entity = await _dbContext.Address.AddAsync(newAddress);

            if (entity.State == EntityState.Added) await _dbContext.SaveChangesAsync();
            return entity.State == EntityState.Unchanged ? entity.Entity.Id : null;
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
        public async Task<bool> Delete(int addressId)
        {
            var address = await Get(addressId);

            if (address is null)
            {
                throw new DatabaseCallError("Address is not in database.");
            }

            var entity = _dbContext.Address.Remove(address);
            return entity.State == EntityState.Deleted;
        }
        private async Task<Address?> Get(int id)
        {
            return await _dbContext.Address.FindAsync(id);
        }
        public async Task<List<Address>?> GetByCondition(Expression<Func<Address,bool>> condition)
        {
            var result = await _dbContext.Address.Where(condition).ToListAsync();
            return result;
        }

    }
}