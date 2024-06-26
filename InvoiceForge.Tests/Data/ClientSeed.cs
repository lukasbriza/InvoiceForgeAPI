﻿using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;

namespace InvoiceForgeApi.Data.SeedClasses
{
    public class ClientSeed
    {
        public List<Client> Populate()
        {
            return new List<Client>()
            {
                new Client()
                {
                    AddressId = 1,
                    Owner = 1,
                    Type = ClientType.LegalEntity,
                    Name = "ClientName1",
                    IN = 9611280833,
                    TIN = "CZ9611280833",
                },
                new Client()
                {
                    AddressId = 1,
                    Owner = 1,
                    Type = ClientType.Entrepreneur,
                    Name = "ClientName2",
                    IN = 1111111111,
                    TIN = "CZ1111111111",
                    Mobil = "774876504",
                    Tel = "774876504",
                    Email = "lukasbriza@seznam.cz"
                }
            };
        }
    }
}
