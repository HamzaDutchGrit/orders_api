# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Zet de werkdirectory in /src
WORKDIR /src

# Kopieer de .csproj bestanden
COPY ./productapi/ProductsApi.csproj ./productapi/

# Herstel de afhankelijkheden
RUN dotnet restore ./productapi/ProductsApi.csproj

# Kopieer de rest van de bestanden
COPY . .

# Bouw de applicatie
WORKDIR /src/productapi
RUN dotnet build ./productapi/ProductsApi.csproj -c Release -o /app/build

# Publiceer de applicatie
RUN dotnet publish ./productapi/ProductsApi.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Zet de werkdirectory naar /app
WORKDIR /app

# Stel poorten bloot
EXPOSE 80
EXPOSE 443

# Kopieer de gepubliceerde bestanden
COPY --from=build /app/publish .

# Start de applicatie
ENTRYPOINT ["dotnet", "ProductsApi.dll"]
