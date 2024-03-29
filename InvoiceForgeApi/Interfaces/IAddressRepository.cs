﻿using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Model;

namespace InvoiceForgeApi.Interfaces
{
    public interface IAddressRepository: IRepositoryBaseExtended<AddressGetRequest, AddressAddRequest, AddressUpdateRequest, Address>
    {
        private static Task<Address?> Get(int id) => null!;
    }
}
