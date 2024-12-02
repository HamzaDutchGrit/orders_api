# Gebruik .NET 8.0 SDK als basisafbeelding
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Stel de werkdirectory in
WORKDIR /src

# Kopieer het projectbestand en herstel dependencies
COPY ProductsApi.csproj ./
RUN dotnet restore "./ProductsApi.csproj"

# Kopieer de rest van de code en bouw het project
COPY . ./
RUN dotnet build "./ProductsApi.csproj" -c Release -o /app/build

# Publiceer de applicatie
RUN dotnet publish "./ProductsApi.csproj" -c Release -o /app/publish

# Gebruik de ASP.NET Core-runtime-afbeelding voor de definitieve fase
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Kopieer de build-uitvoer van de build-container
COPY --from=build /app/publish .

# Stel het startpunt in naar je applicatie
ENTRYPOINT ["dotnet", "ProductsApi.dll"]
