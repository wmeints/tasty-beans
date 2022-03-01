FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /solution

COPY src/Api/Api.csproj src/Api/Api.csproj
COPY src/Application/Application.csproj src/Application/Application.csproj
COPY src/Domain/Domain.csproj src/Domain/Domain.csproj
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/Infrastructure.csproj
COPY tests/Api.Tests/Api.Tests.csproj tests/Api.Tests/Api.Tests.csproj
COPY tests/Application.Tests/Application.Tests.csproj tests/Application.Tests/Application.Tests.csproj
COPY tests/Domain.Tests/Domain.Tests.csproj tests/Domain.Tests/Domain.Tests.csproj
COPY tests/Infrastructure.Tests/Infrastructure.Tests.csproj tests/Infrastructure.Tests/Infrastructure.Tests.csproj
COPY CustomerManagement.sln CustomerManagement.sln

RUN dotnet restore

COPY . .

RUN dotnet publish src/Api/Api.csproj -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

WORKDIR /app
COPY --from=build /dist/ .

CMD ["dotnet", "RecommendCoffee.CustomerManagement.Api.dll"]