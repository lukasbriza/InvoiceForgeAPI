using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Abl.invoiceItem;
using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class UpdateInvoiceItem: WebApplicationFactory
    {
        [Fact]
        public Task CanUpdateInvoiceItem()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceItemAbl(db._repository);

                var itemToUpdateIds = await db._context.InvoiceItem
                    .Where(i => i.InvoiceServices!.Count == 0)
                    .Select(i => new {i.Id, i.Owner})
                    .ToListAsync();

                //ASSERT
                async Task call(int id, int owner)
                {
                    var update = new InvoiceItemUpdateRequest
                    {
                        Owner = owner,
                        ItemName = $"TestInvoiceItemName{id}",
                        TariffId = 1
                    };
                    var result = await abl.Resolve(id, update);
                    Assert.True(result);

                    var control = await db._context.InvoiceItem.FindAsync(id);
                    Assert.NotNull(control);
                    Assert.Equal(update.ItemName, control!.ItemName);
                    Assert.Equal(update.TariffId, control.TariffId);
                }

                itemToUpdateIds?.ForEach(obj => {
                    var task = call(obj.Id, obj.Owner);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnInvalidUser()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceItemAbl(db._repository);

                var update = new InvoiceItemUpdateRequest
                {
                    Owner = 1000,
                    ItemName = "TestInvoiceItemName",
                    TariffId = 1
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, update);
                }
                catch (Exception ex)
                {
                    Assert.IsType<DatabaseCallError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnInvalidInvoiceItem()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceItemAbl(db._repository);

                var update = new InvoiceItemUpdateRequest
                {
                    Owner = 1,
                    ItemName = "TestInvoiceItemName",
                    TariffId = 1
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(100, update);
                }
                catch (Exception ex)
                {
                    Assert.IsType<DatabaseCallError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnInvoiceItemOutOfPossession()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceItemAbl(db._repository);

                var update = new InvoiceItemUpdateRequest
                {
                    Owner = 2,
                    ItemName = "TestInvoiceItemName",
                    TariffId = 1
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, update);
                }
                catch (Exception ex)
                {
                    Assert.IsType<ValidationError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }
        
        [Fact]
        public Task ThrowErrorWhenUpdateLinkedItems()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();

                var update = new InvoiceItemUpdateRequest
                {
                    Owner = 1,
                    ItemName = "TestInvoiceItemName",
                    TariffId = 1
                };

                var linkedIds = await db._context.InvoiceItem
                    .Where(i => i.InvoiceServices!.Count > 0)
                    .Select(i => i.Id)
                    .ToListAsync();

                //ASSERT
                async Task call(int id)
                {
                    try
                    {
                        var abl = new UpdateInvoiceItemAbl(db._repository);
                        var result = await abl.Resolve(id, update);
                    }
                    catch (Exception ex)
                    {
                        Assert.IsType<ValidationError>(ex);
                    }
                }

                linkedIds?.ForEach(id => {
                    var task = call(id);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnInvalidTariff()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceItemAbl(db._repository);

                var update = new InvoiceItemUpdateRequest
                {
                    Owner = 1,
                    ItemName = "TestInvoiceItemName",
                    TariffId = 100
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, update);
                }
                catch (Exception ex)
                {
                    Assert.IsType<DatabaseCallError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnDuplicitItemName()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new UpdateInvoiceItemAbl(db._repository);

                var item = await db._context.InvoiceItem.FindAsync(1);
                var update = new InvoiceItemUpdateRequest
                {
                    Owner = 1,
                    ItemName = item!.ItemName,
                    TariffId = 1
                };

                //ASSERT
                Assert.NotNull(item);

                try
                {
                    var result = await abl.Resolve(1, update);
                }
                catch (Exception ex)
                {
                    Assert.IsType<ValidationError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}