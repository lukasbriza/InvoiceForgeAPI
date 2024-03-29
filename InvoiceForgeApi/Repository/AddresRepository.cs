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
                DbSet<Address> address = _dbContext.Address;
                if (plain == false){
                    address.Include(a => a.Country);
                }
                var addressList = await address
                    .Select(a => new AddressGetRequest(a, plain))
                    .Where(a => a.Id == addressId)
                    .ToListAsync();
                
                if (addressList.Count > 1)
                {
                    throw new DatabaseCallError("Something unexpected happended. There are more than one address with this ID.");
                }
                return addressList[0];
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
            return entity.State == EntityState.Added ? entity.Entity.Id : null;
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
            
            return _dbContext.Entry(localAddress).State == EntityState.Modified;
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