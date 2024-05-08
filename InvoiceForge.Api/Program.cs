using InvoiceForgeApi.Data;
using InvoiceForgeApi.Models.Interfaces;
using InvoiceForgeApi.Middleware;
using InvoiceForgeApi.Repository;
using InvoiceForgeApi.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = ConnectionStringBuilder.Build(builder.Configuration, "Production");
var isDevelopment = builder.Configuration["Development"];

//Add services to the container.
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
builder.Services.AddDbContext<InvoiceForgeDatabaseContext>(options =>
{
    DatabaseConfiguration.Configure(options, connectionString);
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Build
var app = builder.Build();

//Migrate database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<InvoiceForgeDatabaseContext>();
    context?.Database.Migrate();
}

//Seed codelists setup
Seed.SeedData(app);

//Configure the HTTP request pipeline.
if (isDevelopment == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

//Add middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.Run();

//Acess app from outside
public partial class InvoiceForgeApiProgram {}
