FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["PixelService.Api/PixelService.Api.csproj", "PixelService.Api/"]
COPY ["StorageService.Api.DataContracts/StorageService.Api.DataContracts.csproj", "StorageService.Api.DataContracts/"]
COPY . .
WORKDIR "/src/PixelService.Api"
RUN dotnet build "PixelService.Api.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080
COPY --from=build /app .
ENTRYPOINT ["dotnet", "PixelService.Api.dll"]

