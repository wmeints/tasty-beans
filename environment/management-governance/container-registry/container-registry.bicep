// ############################################################################
// Container registry
//
// Deploys the container registry in the management & governance resource group
// ############################################################################

// ############################################################################
// Parameters
// ############################################################################

@description('The location where to deploy the environment')
param location string

@description('The name of the container registry')
param resourceName string

@description('The tags to attach to the resources')
param tags object

// ############################################################################
// Resources
// ############################################################################

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2021-06-01-preview' = {
  name: resourceName
  location: location
  tags: tags
  sku: {
    name: 'Standard'
  }
  properties: {
    adminUserEnabled: true
    anonymousPullEnabled: false
    publicNetworkAccess: 'Enabled'
  }
}

// ############################################################################
// Outputs
// ############################################################################

