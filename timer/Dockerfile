FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src/

COPY src/TastyBeans.Timer/TastyBeans.Timer.csproj TastyBeans.Timer/TastyBeans.Timer.csproj

RUN dotnet restore TastyBeans.Timer/TastyBeans.Timer.csproj

COPY src/ .

RUN dotnet publish TastyBeans.Timer/TastyBeans.Timer.csproj -c Release -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
COPY --from=build /dist /app
CMD ["sh", "-c", "dotnet /app/TastyBeans.Timer.dll"]