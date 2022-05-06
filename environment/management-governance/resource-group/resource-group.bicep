targetScope = 'subscription'

// ############################################################################
// Resource group
//
// Deploys a new resource group in the subscription
// ############################################################################

// ############################################################################
// Parameters
// ############################################################################

@description('The name of the resource group')
param resourceGroupName string

@description('The location where to deploy the environment')
param location string

@description('The tags to attach')
param tags object

// ############################################################################
// Resources
// ############################################################################

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resourceGroupName
  location: location
  tags: tags
}

// ############################################################################
// Outputs
// ############################################################################

output resourceGroupName string = resourceGroupName
output resourceGroupId string = resourceGroup.id
