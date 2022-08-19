FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src/

COPY src/TastyBeans.Registration.Api/TastyBeans.Registration.Api.csproj TastyBeans.Registration.Api/TastyBeans.Registration.Api.csproj

RUN dotnet restore TastyBeans.Registration.Api/TastyBeans.Registration.Api.csproj

COPY src/ .

RUN dotnet publish TastyBeans.Registration.Api/TastyBeans.Registration.Api.csproj -c Release -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
COPY --from=build /dist /app
CMD ["sh", "-c", "dotnet /app/TastyBeans.Registration.Api.dll"]