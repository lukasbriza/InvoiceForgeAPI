# .NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/InvoiceForge.Api/InvoiceForge.Api.csproj", "InvoiceForge.Api/"]
COPY ["/InvoiceForge.Abl/InvoiceForge.Abl.csproj", "InvoiceForge.Abl/"]
COPY ["/InvoiceForge.Models/InvoiceForge.Models.csproj", "InvoiceForge.Models/"]
RUN dotnet restore "InvoiceForge.Api/InvoiceForge.Api.csproj"
COPY . .
WORKDIR "/src/InvoiceForge.Api"
RUN dotnet build "InvoiceForge.Api.csproj" -c release -o /app/build

FROM build AS publish
RUN dotnet publish "InvoiceForge.Api.csproj" -c Release -o /app/publish

# final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InvoiceForge.Api.dll"]