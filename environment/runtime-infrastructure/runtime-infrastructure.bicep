// ############################################################################
// Runtime infrastructure
//
// Deploys the runtime infrastructure for the application.
// ############################################################################

// ############################################################################
// Parameters
// ############################################################################

@description('The environment of the resources')
param environment string

@description('Tags to attach to the resources')
param tags object

@description('Location to use for the resources')
param location string

@description('The name of the container registry to use')
param containerRegistryName string

@description('The name of the management & governance resource group')
param managementGovernanceResourceGroupName string

// ############################################################################
// Resources
// ############################################################################

module logAnalyticsWorkspace './monitoring/log-analytics-workspace.bicep' = {
  name: 'log-analytics-workspace'
  params: {
    location: location
    resourceName: 'law-tastybeans-${environment}'
    tags: tags
  }
}

module aksCluster './aks-cluster/aks-cluster.bicep' = {
  name: 'aks-cluster'
  params: {
    tags: tags
    environment: environment
    location: location
    logAnalyticsWorkspaceResourceId: logAnalyticsWorkspace.outputs.logAnalyticsWorkspaceId
    managementGovernanceResourceGroupName: managementGovernanceResourceGroupName
    containerRegistryName: containerRegistryName
  }
}

// ############################################################################
// Outputs
// ############################################################################
