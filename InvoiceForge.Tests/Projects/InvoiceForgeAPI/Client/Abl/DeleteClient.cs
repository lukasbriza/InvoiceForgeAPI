using FunctionalTests.Projects.InvoiceForgeApi;using InvoiceForge.Tests.Data;
using InvoiceForgeApi.Abl.client;

using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class DeleteClient: WebApplicationFactory
    {
        [Fact]
        public Task CanDeleteClient()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new DeleteClientAbl(db._repository);

                var tClient = new TestClient();

                var clientTestAdd = new Client
                {
                    AddressId = 1,
                    Owner = 1,
                    Type = ClientType.LegalEntity,
                    Name = tClient.Name,
                    IN = tClient.IN,
                    TIN = tClient.TIN,
                    Mobil = tClient.Mobil,
                    Tel = tClient.Tel,
                    Email = tClient.Email
                };

                var entity = await db._context.Client.AddAsync(clientTestAdd);
                await db._context.SaveChangesAsync();
                var id = entity.Entity.Id;

                //ASSERT
                var result = await abl.Resolve(id);
                Assert.True(result);

                var control = await db._context.Client.FindAsync(id);
                Assert.Null(control);

                //CLEAN
                db.Dispose();
            });
        }
        
        [Fact]
        public Task ThrowErrorOnDeleteTemplateLinkedClient()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();

                var templates = await db._context.InvoiceTemplate.ToListAsync();
                var ids = templates.Select(x => x.ClientId).ToList();

                //ASSERT
                ids.ForEach(async id => {
                    try
                    {
                        var abl = new DeleteClientAbl(db._repository);
                        var result = await abl.Resolve(id);
                    }
                    catch (Exception ex)
                    {
                        Assert.IsType<NoPossessionError>(ex);
                    }
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnDeleteNonExistentClient()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new DeleteClientAbl(db._repository);

                //ASSERT
                try
                {
                    var result = await abl.Resolve(100);
                }
                catch (Exception ex)
                {
                    Assert.IsType<NoEntityError>(ex);
                }
                //CLEAN
                db.Dispose();
            });
        }
    }
}