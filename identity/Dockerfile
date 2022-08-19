FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src/

COPY src/TastyBeans.Identity.Api/TastyBeans.Identity.Api.csproj TastyBeans.Identity.Api/TastyBeans.Identity.Api.csproj

RUN dotnet restore TastyBeans.Identity.Api/TastyBeans.Identity.Api.csproj

COPY src/ .

RUN dotnet publish TastyBeans.Identity.Api/TastyBeans.Identity.Api.csproj -c Release -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
COPY --from=build /dist /app
CMD ["sh", "-c", "dotnet /app/TastyBeans.Identity.Api.dll"]