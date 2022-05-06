// ############################################################################
// Technical infrastructure
//
// Deploys the technical infrastructure for the application.
// ############################################################################

// ############################################################################
// Parameters
// ############################################################################

@allowed([
  'dev'
  'test'
  'acc'
  'prod'
])
@description('The name of the environment')
param environment string

@description('The location where to deploy the environment')
param location string

@description('The name of the container registry')
param containerRegistryName string

@description('The tags to attach to the resources')
param tags object

// ############################################################################
// Resources
// ############################################################################

module containerRegistry './container-registry/container-registry.bicep' = {
  name: 'container-registry'
  params: {
    resourceName: containerRegistryName
    location: location
    tags: tags
  }
}

module keyVault './key-vault/key-vault.bicep' = {
  name: 'key-vault'
  params: {
    location: location
    resourceName: 'kv-tastybeans-${environment}'
    tags: tags
  }
}

// ############################################################################
// Outputs
// ############################################################################

