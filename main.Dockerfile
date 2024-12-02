# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Werkdirectory instellen
WORKDIR /src

# Kopieer de .csproj bestanden
COPY productapi/ProductsApi.csproj productapi/

# Zorg ervoor dat de dependencies worden gedownload
RUN dotnet restore productapi/ProductsApi.csproj

# Kopieer alle andere bestanden
COPY . .

# Bouw de applicatie
WORKDIR /src/productapi
RUN dotnet build productapi/ProductsApi.csproj -c Release -o /app/build

# Publiceer de applicatie
RUN dotnet publish productapi/ProductsApi.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Werkdirectory instellen
WORKDIR /app

# Poorten blootstellen voor webverkeer
EXPOSE 80
EXPOSE 443

# Kopieer de gepubliceerde bestanden uit de build stage
COPY --from=build /app/publish .

# Start de applicatie
ENTRYPOINT ["dotnet", "ProductsApi.dll"]
