FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["StorageService.Api/StorageService.Api.csproj", "StorageService.Api/"]
COPY ["StorageService.Api.DataContracts/StorageService.Api.DataContracts.csproj", "StorageService.Api.DataContracts/"]
COPY . .
WORKDIR "/src/StorageService.Api"
RUN dotnet build "StorageService.Api.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "StorageService.Api.dll"]

