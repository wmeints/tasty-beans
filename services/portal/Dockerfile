FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /project/services/portal

COPY services/portal/Server/Server.csproj Server/Server.csproj
COPY services/portal/Shared/Shared.csproj Shared/Shared.csproj
COPY services/portal/Client/Client.csproj Client/Client.csproj

RUN dotnet restore Server/Server.csproj

COPY . .

RUN dotnet publish Server/Server.csproj -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

RUN apt-get update && apt-get -y install procps curl
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg

COPY --from=build /dist /app

WORKDIR /app

CMD ["dotnet", "RecommendCoffee.Portal.Server.dll"]