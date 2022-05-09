param location string
param environment string
param managementGovernanceResourceGroupName string
param technicalInfraResourceGroupName string
param runtimeInfraResourceGroupName string
param sqlAdminLogin string
param sqlAdminPassword string

var tags = {
  Project: 'tastybeans'
  Environment: environment
}

module managementGovernancePart1 'management-governance/management-governance-1.bicep' = {
  name: 'management-governance-1'
  scope: resourceGroup(managementGovernanceResourceGroupName)
  params: {
    location: location
    environment: environment
    tags: tags
  }
}

module technicalInfra 'technical-infra/technical-infra.bicep' = {
  name: 'technical-infra'
  scope: resourceGroup(technicalInfraResourceGroupName)
  params: {
    environment: environment
    location: location
    tags: tags
  }
  dependsOn: [
    managementGovernancePart1
  ]
}

module runtimeInfra 'runtime-infra/runtima-infra.bicep' = {
  name: 'runtime-infra'
  scope: resourceGroup(runtimeInfraResourceGroupName)
  params: {
    environment: environment
    location: location
    tags: tags
  }
  dependsOn: [
    technicalInfra
  ]
}

module appInfra 'app-infra/app-infra.bicep' = {
  name: 'app-infra'
  scope: resourceGroup(runtimeInfraResourceGroupName)
  params: {
    environment: environment
    location: location
    tags: tags
    sqlAdminLogin: sqlAdminLogin
    sqlAdminPassword: sqlAdminPassword
  }
  dependsOn: [
    runtimeInfra
  ]
}

module managementGovernancePart2 'management-governance/management-governance-2.bicep' = {
  name: 'management-governance-2'
  scope: resourceGroup(managementGovernanceResourceGroupName)
  params: {
    aksClusterPrincipalId: runtimeInfra.outputs.aksClusterPrincipalId
  }
  dependsOn: [
    appInfra
  ]
}
