FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

EXPOSE 80
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY src/TaskDispatcher/TaskDispatcher.csproj src/TaskDispatcher/TaskDispatcher.csproj
RUN dotnet restore src/TaskDispatcher/TaskDispatcher.csproj

COPY . .

FROM build AS publish

RUN dotnet publish src/TaskDispatcher/TaskDispatcher.csproj -o /app/publish

FROM base AS runtime

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "RecommendCoffee.TaskDispatcher.dll"]