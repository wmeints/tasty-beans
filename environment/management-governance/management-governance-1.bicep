param location string
param environment string
param tags object

var keyVaultResourceName = 'kv-tastybeans-${environment}'
var containerRegistryResourceName = 'tastybeans'

module appInfraResourceGroup 'resource-group/resource-group.bicep' = {
  name: 'app-infra-resource-group'
  scope: subscription()
  params: {
    location: location
    tags: tags
    resourceName: 'rg-app-infra-${environment}'
  }
}

module runtimeInfraResourceGroup 'resource-group/resource-group.bicep' = {
  name: 'runtime-infra-resource-group'
  scope: subscription()
  params: {
    location: location
    tags: tags
    resourceName: 'rg-runtime-infra-${environment}'
  }
}

module technicalInfraResourceGroup 'resource-group/resource-group.bicep' = {
  name: 'technical-infra-resource-group'
  scope: subscription()
  params: {
    location: location
    tags: tags
    resourceName: 'rg-technical-infra-${environment}'
  }
}

module containerRegistry 'container-registry/container-registry.bicep' = {
  name: 'container-registry'
  params: {
    location: location
    resourceName: containerRegistryResourceName
  }
}

module keyvault 'keyvault/keyvault.bicep' = {
  name: 'keyvault'
  params: {
    resourceName: keyVaultResourceName
    location: location
    tags: tags
  }
}

output keyVaultName string = keyVaultResourceName
output containerRegistryName string = containerRegistryResourceName
