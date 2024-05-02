using FunctionalTests.Projects.InvoiceForgeApi;
using FunctionalTests.Projects.InvoiceForgeAPI;
using InvoiceForgeApi.Abl.userAccount;
using InvoiceForgeApi.DTO;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class DeleteUserAccount: WebApplicationFactory
    {
        [Fact]
        public Task CanDeleteUserAccount()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();

                var ids = await db._context.UserAccount
                    .Where(a => a.InvoiceTemplates!.Count == 0)
                    .Select(a => a.Id)
                    .ToListAsync();

                //ASSERT
                async Task call(int id)
                {
                    var abl = new DeleteUserAccountAbl(db._repository);
                    var result = await abl.Resolve(id);
                    Assert.True(result);
                }

                ids.ForEach(id => {
                    var task = call(id);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnDeleteAccountWithReference()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();

                var ids = await db._context.UserAccount
                    .Where(a => a.InvoiceTemplates!.Count > 0)
                    .Select(a => a.Id)
                    .ToListAsync();

                //ASSERT
                async Task call(int id)
                {
                    try
                    {    
                        var abl = new DeleteUserAccountAbl(db._repository);
                        var result = await abl.Resolve(id);
                    }
                    catch (Exception ex)
                    {
                        Assert.IsType<ValidationError>(ex);
                    }
                }

                ids.ForEach(id => {
                    var task = call(id);
                    task.Wait();
                });

                //CLEAN
                db.Dispose();
            });
        }
    }
}