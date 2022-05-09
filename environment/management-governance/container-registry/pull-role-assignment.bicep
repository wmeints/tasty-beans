param containerRegistryName string
param principalId string

var acrPullRoleDefinitionId = '/subscriptions/${subscription().id}/providers/Microsoft.Authorization/roleDefinitions/7f951dda-4ed3-4680-a7ca-43fe172d538d'

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2021-12-01-preview' existing = {
  name: containerRegistryName
}

resource acrPullRoleAssignment 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  name: guid(subscription().id, resourceGroup().name, principalId, 'acrpull')
  scope: containerRegistry
  properties: {
    principalId: principalId
    roleDefinitionId: acrPullRoleDefinitionId 
  }
}
