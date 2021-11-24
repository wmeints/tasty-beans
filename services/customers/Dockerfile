FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

EXPOSE 80
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY src/Api/Api.csproj src/Api/Api.csproj
COPY src/Application/Application.csproj src/Application/Application.csproj
COPY src/Domain/Domain.csproj src/Domain/Domain.csproj
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/Infrastructure.csproj

RUN dotnet restore src/Api/Api.csproj

COPY . .

FROM build AS publish

RUN dotnet publish src/Api/Api.csproj -o /app/publish

FROM base AS runtime

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "RecommendCoffee.Customers.Api.dll"]