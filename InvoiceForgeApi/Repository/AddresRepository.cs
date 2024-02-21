using System.Net.WebSockets;
using InvoiceForgeApi.Data;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Interfaces;
using InvoiceForgeApi.Model;
using Microsoft.AspNetCore.Mvc;
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
         public async Task<List<AddressGetRequest>?> GetAll(int userId)
         {
            var addresses = await _dbContext.Address
                .Include(a => a.Country)
                .Select( a => new AddressGetRequest
                    {
                        Id = a.Id,
                        Owner = a.Owner,
                        Street = a.Street,
                        StreetNumber = a.StreetNumber,
                        City = a.City,
                        PostalCode = a.PostalCode,
                        Country = new CountryGetRequest
                        {
                            Id = a.Country!.Id,
                            Value = a.Country.Value,
                            Shortcut = a.Country.Shortcut
                        }
                    }
                )
                .Where(a => a.Owner == userId).ToListAsync();
            return addresses;
         }
         public async Task<AddressGetRequest?> GetById(int addressId)
         {
                var address = await _dbContext.Address
                    .Include(a => a.Country)
                    .Select( a => new AddressGetRequest
                        {
                            Id = a.Id,
                            Owner = a.Owner,
                            Street = a.Street,
                            StreetNumber = a.StreetNumber,
                            City = a.City,
                            PostalCode = a.PostalCode,
                            Country = new CountryGetRequest
                            {
                                Id = a.Country!.Id,
                                Value = a.Country.Value,
                                Shortcut = a.Country.Shortcut
                            }
                        }
                    )
                    .Where(a => a.Id == addressId).ToListAsync();
                
                if (address.Count > 1)
                {
                    throw new DatabaseCallError("Something unexpected happended. There are more than one address with this ID.");
                }
                return address[0];
         }
         public async Task<bool> Add(int userId, AddressAddRequest address)
         {
            if (address is null)
            {
                throw new ValidationError("Address is not provided.");
            }

            var countryExists = await _dbContext.Country.FindAsync(address.CountryId);

            if (countryExists is null)
            {
                throw new ValidationError("Provided countryId is invalid.");
            }

            var newAddress = new Address
            {
                Owner = userId,
                CountryId = address.CountryId,
                Street = address.Street,
                StreetNumber = address.StreetNumber,
                City = address.City,
                PostalCode = address.PostalCode
            };
            await _dbContext.Address.AddAsync(newAddress);
            return true;
         }
         public async Task<bool> Update(int addressId, AddressUpdateRequest address)
         {
            if (address is null)
            {
                throw new ValidationError("Address is not provided");
            }

            if (address.CountryId is not null)
            {
                var country = await _dbContext.Country.FindAsync(address.CountryId);
                if (country is null)
                {
                    throw new ValidationError("Provided countryId is invalid.");
                }
            }

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
            return true;
         }
         public async Task<bool> Delete(int addressId)
         {
            var address = await Get(addressId);

            if (address is null)
            {
                throw new DatabaseCallError("Address is not in database.");
            }

            _dbContext.Address.Remove(address);
            return true;
         }
         private async Task<Address?> Get(int id)
         {
            return await _dbContext.Address.FindAsync(id);
         }

    }
}