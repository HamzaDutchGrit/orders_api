# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy the project file and restore dependencies
COPY ./ProductApi/ProductApi.csproj ./ProductApi/
RUN dotnet restore ./ProductApi/ProductApi.csproj

# Copy the rest of the files and build
COPY ./ProductApi ./ProductApi
WORKDIR /app/ProductApi
RUN dotnet publish -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/ProductApi/out ./

ENTRYPOINT ["dotnet", "ProductApi.dll"]
