# Gebruik een officiële .NET SDK image om de buildomgeving op te zetten
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Zet de werkdirectory voor de build
WORKDIR /src

# Kopieer de .csproj bestanden naar de container
COPY orders_api/ProductApi/ProductsApi.csproj orders_api/ProductApi/

# Restore de dependencies
RUN dotnet restore orders_api/ProductApi/ProductsApi.csproj

# Kopieer de overige broncode
COPY . .

# Bouw de applicatie
RUN dotnet build orders_api/ProductApi/ProductsApi.csproj -c Release -o /app/build

# Publiceer de applicatie
RUN dotnet publish orders_api/ProductApi/ProductsApi.csproj -c Release -o /app/publish

# Gebruik een officiële .NET runtime image om de gepubliceerde applicatie te draaien
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Zet de werkdirectory voor de runtime container
WORKDIR /app

# Kopieer de gepubliceerde applicatie vanuit de buildcontainer
COPY --from=build /app/publish .

# Stel het commando in om de applicatie te starten
ENTRYPOINT ["dotnet", "ProductsApi.dll"]
