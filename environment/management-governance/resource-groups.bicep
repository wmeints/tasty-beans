// ############################################################################
// Resource groups
//
// Deploys resource groups to the subscription.
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

// ############################################################################
// Variables
// ############################################################################

var tags = {
  Environment: environment
  Project: 'tastybeans'
}

// ############################################################################
// Resources
// ############################################################################

module technicalInfraResourceGroup './resource-group/resource-group.bicep' = {
  name: 'technical-infra-resource-group'
  scope: subscription()
  params: {
    resourceGroupName: 'tastybeans-technical-infra-${environment}-rg'
    location: location
    tags: tags
  }
}

module runtimeInfraResourceGroup './resource-group/resource-group.bicep' = {
  name: 'runtime-infra-resource-group'
  scope: subscription()
  params: {
    resourceGroupName: 'tastybeans-runtime-infra-${environment}-rg'
    location: location
    tags: tags
  }
}

module appInfraResourceGroup './resource-group/resource-group.bicep' = {
  name: 'app-infra-resource-group'
  scope: subscription()
  params: {
    resourceGroupName: 'tastybeans-app-infra-${environment}-rg'
    location: location
    tags: tags
  }
}

// ############################################################################
// Outputs
// ############################################################################

