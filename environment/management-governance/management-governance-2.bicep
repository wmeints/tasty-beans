param aksClusterPrincipalId string

module acrPullRoleAssignment 'container-registry/pull-role-assignment.bicep' = {
  name: 'acr-pull-access-aks'
  params: {
    principalId: aksClusterPrincipalId
    containerRegistryName: 'tastybeans'
  }
}
