// ############################################################################
// Environment script
// 
// This deploys the environment to Azure.
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

@description('The name of the resource group containing the management & governance resources')
param managementGovernanceResourceGroupName string

@description('The name of the container registry to deploy')
param containerRegistryName string

// ############################################################################
// Variables
// ############################################################################

var tags = {
  Project: 'tastybeans'
  Environment: environment
}

// ############################################################################
// Modules
// ############################################################################

module managementGovernanceResources './management-governance/management-governance-resources.bicep' = {
  name: 'management-governance-resources'
  scope: resourceGroup(managementGovernanceResourceGroupName)
  params: {
    containerRegistryName: containerRegistryName
    environment: environment
    location: location
    tags: tags
  }
}

module runtimeInfrastructure './runtime-infrastructure/runtime-infrastructure.bicep' = {
  name: 'runtime-infrastructure'
  scope: resourceGroup('tastybeans-runtime-infra-${environment}-rg')
  params: {
    managementGovernanceResourceGroupName: managementGovernanceResourceGroupName
    containerRegistryName: containerRegistryName
    environment: environment
    location: location
    tags: tags
  }
}

// ############################################################################
// Outputs
// ############################################################################
