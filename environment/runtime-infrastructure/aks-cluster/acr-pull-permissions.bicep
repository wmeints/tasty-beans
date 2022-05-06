// ############################################################################
// Pull image permissions
//
// This assigns the AcrPull role to the AKS cluster principal so it can pull
// images from the configured container registry.
// ############################################################################

// ############################################################################
// Parameters
// ############################################################################

@description('The resource ID for the AKS principal')
param aksClusterPrincipalId string

@description('The environment name')
param environment string

@description('The name of the container registry')
param containerRegistryName string

// ############################################################################
// Variables
// ############################################################################

var acrPullRoleDefinitionId = subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d')

// ############################################################################
// Resources
// ############################################################################

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2021-06-01-preview' existing = {
  name: containerRegistryName
}

resource containerRegistryRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  name: guid(subscription().id, 'tastybeans', environment, 'pull-access-role-assignment')
  scope: containerRegistry
  properties: {
    description: 'Image Pull Access for AKS cluster'
    principalId: aksClusterPrincipalId
    roleDefinitionId: acrPullRoleDefinitionId
  }
}
