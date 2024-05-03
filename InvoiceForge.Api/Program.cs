using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models.Interfaces;
using InvoiceForgeApi.Middleware;
using InvoiceForgeApi.Repository;
using Microsoft.EntityFrameworkCore;
using InvoiceForgeApi.Triggers;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddDbContext<InvoiceForgeDatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseTriggers(triggerOptions => {
        triggerOptions.AddTrigger<AddressUpdateTrigger>();
        triggerOptions.AddTrigger<InvoiceAddressCopyUpdateTrigger>();
        triggerOptions.AddTrigger<InvoiceEntityCopyUpdateTrigger>();
        triggerOptions.AddTrigger<ClientUpdateTrigger>();
        triggerOptions.AddTrigger<ContractorUpdateTrigger>();
        triggerOptions.AddTrigger<InvoiceTemplateUpdateTrigger>();
        triggerOptions.AddTrigger<UserAccountUpdateTrigger>();
        triggerOptions.AddTrigger<InvoiceUserAccountCopyUpdateTrigger>();
        triggerOptions.AddTrigger<TrackableTrigger>();
    });
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

//Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseDeveloperExceptionPage();

//Seed setup
if (args.Length == 1 && args[0].ToLower() == "seed")
{
    Seed.SeedData(app);
}

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Add middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.Run();

//Acess app from outside
public partial class InvoiceForgeApiProgram {}
