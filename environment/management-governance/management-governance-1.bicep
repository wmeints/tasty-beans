param location string
param environment string
param tags object

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
    resourceName: 'tastybeans'
  }
}
