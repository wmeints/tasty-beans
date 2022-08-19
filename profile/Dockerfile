FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src/

COPY src/TastyBeans.Profile.Api/TastyBeans.Profile.Api.csproj TastyBeans.Profile.Api/TastyBeans.Profile.Api.csproj
COPY src/TastyBeans.Profile.Domain/TastyBeans.Profile.Domain.csproj TastyBeans.Profile.Domain/TastyBeans.Profile.Domain.csproj

RUN dotnet restore TastyBeans.Profile.Api/TastyBeans.Profile.Api.csproj

COPY src/ .

RUN dotnet publish TastyBeans.Profile.Api/TastyBeans.Profile.Api.csproj -c Release -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
COPY --from=build /dist /app
CMD ["sh", "-c", "dotnet /app/TastyBeans.Profile.Api.dll"]