FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /project

COPY src/Api/Api.csproj src/Api/Api.csproj
COPY src/Application/Application.csproj src/Application/Application.csproj
COPY src/Domain/Domain.csproj src/Domain/Domain.csproj
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/Infrastructure.csproj

RUN dotnet restore src/Api/Api.csproj

COPY . .

RUN dotnet publish src/Api/Api.csproj -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

RUN apt-get update && apt-get -y install procps curl
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg

COPY --from=build /dist /app


WORKDIR /app

CMD ["dotnet", "RecommendCoffee.Identity.Api.dll"]