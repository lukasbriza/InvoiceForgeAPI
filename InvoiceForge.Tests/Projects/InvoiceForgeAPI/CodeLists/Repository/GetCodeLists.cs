using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Data.SeedClasses;
using InvoiceForgeApi.DTO.Model;
using InvoiceForgeApi.Models.CodeLists;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CodeListsRepository
{
    [Collection("Sequential")]
    public class GetCodeLists: WebApplicationFactory
    {
        [Fact]
        public Task ReturnSeededCountAndTypeOfBanks()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var banksValidation = new BankSeed().Populate();

                //ASSERT
                var banks = await db._repository.CodeLists.GetBanks();

                Assert.Equal(banksValidation.Count, banks.Count);
                banks.ForEach(bank => {
                    Assert.IsType<BankGetRequest>(bank);
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ReturnRightBankById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var banks = await db._context.Bank.ToListAsync();

                //ASSERT
                Assert.NotNull(banks);
                Assert.IsType<List<Bank>>(banks);

                banks.ForEach(async dbBank => {
                    var repoBank = await db._repository.CodeLists.GetBankById(dbBank.Id);
                    Assert.NotNull(repoBank);

                    if (repoBank is not null){
                        Assert.IsType<BankGetRequest>(repoBank);
                        Assert.Equal(repoBank.Id.ToString(), dbBank.Id.ToString());
                        Assert.Equal(repoBank.Shortcut, dbBank.Shortcut);
                        Assert.Equal(repoBank.SWIFT, repoBank.SWIFT);
                    }
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ReturnSeededCountAndTypeOfCountries()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var countriesValidation = new CountrySeed().Populate();

                //ASSERT
                var countries = await db._repository.CodeLists.GetCountries();

                Assert.Equal(countriesValidation.Count, countries.Count);
                countries.ForEach(country => {
                    Assert.IsType<CountryGetRequest>(country);
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ReturnRightCountryById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var countries = await db._context.Country.ToListAsync();

                //ASSERT
                Assert.NotNull(countries);
                Assert.IsType<List<Country>>(countries);

                countries.ForEach(async dbCountry => {
                    var repoCountry = await db._repository.CodeLists.GetCountryById(dbCountry.Id);
                    Assert.NotNull(repoCountry);

                    if (repoCountry is not null)
                    {
                        Assert.IsType<CountryGetRequest>(repoCountry);
                        Assert.Equal(dbCountry.Id, repoCountry.Id);
                        Assert.Equal(dbCountry.Shortcut, repoCountry.Shortcut);
                        Assert.Equal(dbCountry.Value, repoCountry.Value);
                    }
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ReturnSeededCountAndTypeOfCurrencies()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var currenciesValidation = new CurrencySeed().Populate();

                //ASSERT
                var currencies = await db._repository.CodeLists.GetCurrencies();

                Assert.Equal(currenciesValidation.Count, currencies.Count);
                currencies.ForEach(currency => {
                    Assert.IsType<CurrencyGetRequest>(currency);
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ReturnRightCurrencyById()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var currencies = await db._context.Currency.ToListAsync();

                //ASSERT
                Assert.NotNull(currencies);
                Assert.IsType<List<Currency>>(currencies);

                currencies.ForEach(async dbCurrency => {
                    var repoCurrency = await db._repository.CodeLists.GetCurrencyById(dbCurrency.Id);
                    Assert.NotNull(repoCurrency);

                    if (repoCurrency is not null)
                    {
                        Assert.IsType<CurrencyGetRequest>(repoCurrency);
                        Assert.Equal(dbCurrency.Id, repoCurrency.Id);
                        Assert.Equal(dbCurrency.Value, repoCurrency.Value);
                    }
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ReturnSeededCountAndTypeOfTariffs()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitializeDbForTest();
                var tariffsValidation = new TariffSeed().Populate();

                //ASSERT
                var tariffs = await db._repository.CodeLists.GetTariffs();

                Assert.Equal(tariffsValidation.Count, tariffs.Count);
                tariffs.ForEach(tariff => {
                    Assert.IsType<TariffGetRequest>(tariff);
                });


                //CLEAN
                db.Dispose();
            });
        }
    }
}