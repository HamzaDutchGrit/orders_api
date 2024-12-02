FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ./productapi/ProductsApi.csproj ./productapi/

RUN dotnet restore ./productapi/ProductsApi.csproj

COPY . .

WORKDIR /src/productapi
RUN dotnet build "./ProductApi/ProductsApi.csproj" -c Release -o /app/build

RUN dotnet publish "./ProductApi/ProductsApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ProductsApi.dll"]
