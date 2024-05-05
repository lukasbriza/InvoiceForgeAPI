using FunctionalTests.Projects.InvoiceForgeApi;using InvoiceForge.Tests.Data;
using InvoiceForgeApi.Abl.contractor;

using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class DeleteContractor: WebApplicationFactory
    {
        [Fact]
        public Task CanDeleteContractor()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new DeleteContractorAbl(db._repository);

                var tContractor = new TestContractor();

                var contractor = new Contractor
                {
                    AddressId = 1,
                    Owner = 1,
                    Type = ClientType.LegalEntity,
                    Name = tContractor.Name,
                    IN = tContractor.IN,
                    TIN = tContractor.TIN,
                    Mobil = tContractor.Mobil,
                    Tel = tContractor.Tel,
                    Email = tContractor.Email
                };

                var entity = await db._context.Contractor.AddAsync(contractor);
                await db._context.SaveChangesAsync();
                var id = entity.Entity.Id;

                //ASSERT
                var result = await abl.Resolve(id);
                Assert.True(result);

                var control = await db._context.Contractor.FindAsync(id);
                Assert.Null(control);

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnDeleteTemplateLinkedContractor()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();

                var templates = await db._context.InvoiceTemplate.ToListAsync();
                var ids = templates.Select(x => x.ContractorId).ToList();

                //ASSERT
                ids.ForEach(async id => {
                    try
                    {
                        var abl = new DeleteContractorAbl(db._repository);
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
        public Task ThrowErrorOnDeleteNonExistentContractor()
        {
            return RunTest(async (client)  => {
                //SETUP
                var db = new DatabaseHelper();
                var abl = new DeleteContractorAbl(db._repository);

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