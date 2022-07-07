FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

ARG SERVICE_NAME
ARG ENTRYPOINT

COPY shared/ /project/shared

ENV SERVICE_NAME=${SERVICE_NAME}

WORKDIR /project/services/${SERVICE_NAME}

COPY services/${SERVICE_NAME}/src/api/Api.csproj /project/services/${SERVICE_NAME}/src/api/Api.csproj
COPY services/${SERVICE_NAME}/src/application/Application.csproj /project/services/${SERVICE_NAME}/src/application/Application.csproj
COPY services/${SERVICE_NAME}/src/domain/Domain.csproj /project/services/${SERVICE_NAME}/src/domain/Domain.csproj
COPY services/${SERVICE_NAME}/src/infrastructure/Infrastructure.csproj /project/services/${SERVICE_NAME}/src/infrastructure/Infrastructure.csproj

RUN dotnet restore /project/services/${SERVICE_NAME}/src/api/Api.csproj

COPY services/${SERVICE_NAME} /project/services/${SERVICE_NAME}

RUN dotnet publish src/api/Api.csproj -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

ARG ENTRYPOINT

RUN apt-get update && apt-get -y install procps curl
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg

COPY --from=build /dist /app

ENV ENTRYPOINT=${ENTRYPOINT}

WORKDIR /app

CMD ["sh", "-c", "dotnet ${ENTRYPOINT}"]