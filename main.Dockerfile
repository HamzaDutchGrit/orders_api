# Use .NET 8.0 SDK as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory
WORKDIR /src

# Copy only the project file first
COPY ["ProductApi/ProductApi.csproj", "ProductApi/"]

# Restore dependencies
RUN dotnet restore "ProductApi/ProductApi.csproj"

# Copy the rest of the source code
COPY . .

# Set working directory to the project folder
WORKDIR /src/ProductApi

# Build and publish the application
RUN dotnet build "ProductApi.csproj" -c Release -o /app/build
RUN dotnet publish "ProductApi.csproj" -c Release -o /app/publish

# Use the ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copy build output
COPY --from=build /app/publish .

# Set the entrypoint
ENTRYPOINT ["dotnet", "ProductApi.dll"]
