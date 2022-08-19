FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src/

COPY src/TastyBeans.Payments.Api/TastyBeans.Payments.Api.csproj TastyBeans.Payments.Api/TastyBeans.Payments.Api.csproj
COPY src/TastyBeans.Payments.Domain/TastyBeans.Payments.Domain.csproj TastyBeans.Payments.Domain/TastyBeans.Payments.Domain.csproj

RUN dotnet restore TastyBeans.Payments.Api/TastyBeans.Payments.Api.csproj

COPY src/ .

RUN dotnet publish TastyBeans.Payments.Api/TastyBeans.Payments.Api.csproj -c Release -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
COPY --from=build /dist /app
CMD ["sh", "-c", "dotnet /app/TastyBeans.Payments.Api.dll"]