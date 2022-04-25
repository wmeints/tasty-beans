param (
    [System.String] $ServiceName
)

pushd services/$ServiceName

# Clean up project references

pushd src/Api
dotnet remove reference ../Domain
dotnet remove reference ../Application
dotnet remove reference ../Infrastructure
popd

pushd src/Application
dotnet remove reference ../Domain
popd

pushd src/Infrastructure
dotnet remove reference ../Domain
dotnet remove reference ../Application

popd

# Move project files to correct location

mv src/Api src/api
mv src/Application src/application
mv src/Domain src/domain
mv src/Infrastructure src/infrastructure

# Restore references

pushd src/api
dotnet add reference ../domain
dotnet add reference ../application
dotnet add reference ../infrastructure
dotnet add reference ../../../../shared/api
popd

pushd src/application
dotnet add reference ../domain
dotnet add reference ../../../../shared/application
popd

pushd src/domain
dotnet add reference ../../../../shared/domain
popd

pushd src/infrastructure

dotnet add reference ../domain
dotnet add reference ../application
dotnet add reference ../../../../shared/infrastructure

dotnet remove package Dapr.Client

popd

# Add using statements

cp ../payments/src/domain/GlobalUsings.cs src/domain/GlobalUsings.cs
cp ../payments/src/application/GlobalUsings.cs src/application/GlobalUsings.cs
cp ../payments/src/api/GlobalUsings.cs src/api/GlobalUsings.cs

# Remove unwanted files

rm -rf src/domain/Common
rm -rf src/application/Common
rm -rf src/api/Common
rm -rf src/infrastructure/EventBus

# Clean up test references

pushd tests/Api.Tests
dotnet remove reference ../../src/Api
popd

pushd tests/Application.Tests
dotnet remove reference ../../src/Application
popd

pushd tests/Domain.Tests
dotnet remove reference ../../src/Domain
popd

pushd tests/Infrastructure.Tests
dotnet remove reference ../../src/Infrastructure
popd

mv tests/Api.Tests tests/api
mv tests/Application.Tests tests/application
mv tests/Domain.Tests tests/domain
mv tests/Infrastructure.Tests tests/infrastructure

pushd tests/api
dotnet add reference ../../src/api
popd

pushd tests/application
dotnet add reference ../../src/application
popd

pushd tests/domain
dotnet add reference ../../src/domain
popd

pushd tests/infrastructure
dotnet add reference ../../src/infrastructure
popd

popd