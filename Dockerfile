# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files first to cache restore
COPY ["ConvenienceStore.sln", "./"]
COPY ["ConvenienceStore.API/ConvenienceStore.API.csproj", "ConvenienceStore.API/"]
COPY ["ConvenienceStore.Application/ConvenienceStore.Application.csproj", "ConvenienceStore.Application/"]
COPY ["ConvenienceStore.Domain/ConvenienceStore.Domain.csproj", "ConvenienceStore.Domain/"]
COPY ["ConvenienceStore.Infrastructure/ConvenienceStore.Infrastructure.csproj", "ConvenienceStore.Infrastructure/"]

# Restore packages
RUN dotnet restore "ConvenienceStore.sln"

# Copy the remaining source code
COPY . .

# Build and Publish
WORKDIR "/src/ConvenienceStore.API"
RUN dotnet publish "ConvenienceStore.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Expose ports
EXPOSE 8080
EXPOSE 8081

# Copy published files from build stage
COPY --from=build /app/publish .

# Setup entrypoint
ENTRYPOINT ["dotnet", "ConvenienceStore.API.dll"]
