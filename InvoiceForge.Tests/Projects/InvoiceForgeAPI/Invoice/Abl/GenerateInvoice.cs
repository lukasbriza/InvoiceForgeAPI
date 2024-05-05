using FunctionalTests.Projects.InvoiceForgeApi;
using InvoiceForgeApi.Abl.invoice;

using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Models;
using InvoiceForgeApi.Models.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace Abl
{
    [Collection("Sequential")]
    public class GenerateInvoice: WebApplicationFactory
    {
        [Fact]
        public Task CanGenerateInvoice()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitInvoiceItems();
                var abl = new GenerateInvoiceAbl(db._repository);

                var invoiceAdd = new InvoiceAddRequest
                {
                    TemplateId = 1,
                    Maturity = new DateTime(2030, 1,1),
                    Exposure = new DateTime(2030, 1,1),
                    TaxableTransaction = new DateTime(2030, 1,1),
                    InvoiceServices = new List<InvoiceServiceAddRequest>
                    {
                        new InvoiceServiceAddRequest
                        {
                            Units = 1,
                            PricePerUnit = 100,
                            ItemId = 1
                        }
                    }
                };

                //ASSERT
                var result = await abl.Resolve(1,invoiceAdd);
                Assert.True(result);

                //CLEAN
                db.Dispose();
            });
        }
        
        [Fact]
        public Task CanExtendNumberingTemplateWhenOverflow()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();

                var numbering = new Numbering
                {
                    Owner = 1,
                    NumberingTemplate = new List<NumberingVariable>{
                        NumberingVariable.Year,
                        NumberingVariable.Month,
                        NumberingVariable.Day,
                        NumberingVariable.Number,
                    }
                };
                var entity = await db._context.Numbering.AddAsync(numbering);
                await db._context.SaveChangesAsync();
                var id = entity.Entity.Id;

                var template = new InvoiceTemplateAddRequest
                {
                    ClientId = 1,
                    ContractorId = 1,
                    UserAccountId = 1,
                    CurrencyId = 1,
                    TemplateName = "TestTemplateName",
                    NumberingId = id
                };
                var addTemplate = await db._repository.InvoiceTemplate.Add(numbering.Owner, template);
                await db._context.SaveChangesAsync();

                //ASSERT
                Assert.NotNull(addTemplate);

                async Task call()
                {
                    var abl = new GenerateInvoiceAbl(db._repository);
                    var addRequest = new InvoiceAddRequest
                    {
                        TemplateId = (int)addTemplate,
                        Maturity = new DateTime(2030, 1,1),
                        Exposure = new DateTime(2030, 1,1),
                        TaxableTransaction = new DateTime(2030, 1,1),
                        InvoiceServices = new List<InvoiceServiceAddRequest>
                        {
                            new InvoiceServiceAddRequest
                            {
                                Units = 1,
                                PricePerUnit = 100,
                                ItemId = 1
                            }
                        }
                    };
                    var result = await abl.Resolve(numbering.Owner, addRequest);
                    Assert.True(result);
                }

                for (int i = 0; i <= 99; i++)
                {
                    var task = call();
                    task.Wait();
                }

                var invoices = await db._context.Invoice.Where(i => i.NumberingId == id).ToListAsync();
                var controlNumbering = await db._context.Numbering.FindAsync(id);
                Assert.NotNull(invoices);
                Assert.Equal(100, invoices.Count());
                int actual = controlNumbering!.NumberingTemplate.FindAll(i => i == NumberingVariable.Number).Count();
                Assert.Equal(3, actual);
                var lastInvoice = invoices.MaxBy(i => i.OrderNumber);
                var date = DateTime.Today;
                var year = date.Year;
                var month = date.Month.ToString().Length == 1 ? $"0{date.Month}" : date.Month.ToString();
                var day = date.Day.ToString().Length == 1 ? $"0{date.Day}" : date.Day.ToString();
                Assert.Equal($"{year}{month}{day}100", lastInvoice!.InvoiceNumber);
                
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
                db.InitInvoiceItems();
                var abl = new GenerateInvoiceAbl(db._repository);

                var invoiceAdd = new InvoiceAddRequest
                {
                    TemplateId = 1,
                    Maturity = new DateTime(2030, 1,1),
                    Exposure = new DateTime(2030, 1,1),
                    TaxableTransaction = new DateTime(2030, 1,1),
                    InvoiceServices = new List<InvoiceServiceAddRequest>
                    {
                        new InvoiceServiceAddRequest
                        {
                            Units = 1,
                            PricePerUnit = 100,
                            ItemId = 1
                        }
                    }
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(100,invoiceAdd);
                }
                catch (Exception ex)
                {
                    Assert.IsType<NoEntityError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnInvalidTemplate()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitInvoiceItems();
                var abl = new GenerateInvoiceAbl(db._repository);

                var invoiceAdd = new InvoiceAddRequest
                {
                    TemplateId = 100,
                    Maturity = new DateTime(2030, 1,1),
                    Exposure = new DateTime(2030, 1,1),
                    TaxableTransaction = new DateTime(2030, 1,1),
                    InvoiceServices = new List<InvoiceServiceAddRequest>
                    {
                        new InvoiceServiceAddRequest
                        {
                            Units = 1,
                            PricePerUnit = 100,
                            ItemId = 1
                        }
                    }
                };

                //ASSERT
                try
                {
                    var resolve = await abl.Resolve(1, invoiceAdd);
                }
                catch (Exception ex)
                {
                    Assert.IsType<NoEntityError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }

        [Fact]
        public Task ThrowErrorOnTemplateOutOfPossession()
        {
            return RunTest(async (client) => {
                //SETUP
                var db = new DatabaseHelper();
                db.InitInvoiceItems();
                var abl = new GenerateInvoiceAbl(db._repository);

                var template  = new InvoiceTemplate
                {
                    Owner = 2,
                    ClientId = 1,
                    ContractorId = 1,
                    UserAccountId = 1,
                    TemplateName = "Template1",
                    Created = DateTime.UtcNow,
                    NumberingId = 1,
                    CurrencyId = 1
                };
                var entity = await db._context.InvoiceTemplate.AddAsync(template);
                await db._context.SaveChangesAsync();
                var templateId = entity.Entity.Id;

                var invoiceAdd = new InvoiceAddRequest
                {
                    TemplateId = templateId,
                    Maturity = new DateTime(2030, 1,1),
                    Exposure = new DateTime(2030, 1,1),
                    TaxableTransaction = new DateTime(2030, 1,1),
                    InvoiceServices = new List<InvoiceServiceAddRequest>
                    {
                        new InvoiceServiceAddRequest
                        {
                            Units = 1,
                            PricePerUnit = 100,
                            ItemId = 1
                        }
                    }
                };

                //ASSERT
                try
                {
                    var result = await abl.Resolve(1, invoiceAdd);
                }
                catch (Exception ex)
                {
                    Assert.IsType<NoPossessionError>(ex);
                }

                //CLEAN
                db.Dispose();
            });
        }
    }
}