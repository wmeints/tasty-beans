param resourceName string
param location string
param tags object
param containerRegistryName string
param managementGovernanceResourceGroupName string
param runtimeInfraResourceGroupName string
param technicalInfraResourceGroupName string
param applicationInsightsName string
param keyVaultName string
param storageAccountName string

resource keyVault 'Microsoft.KeyVault/vaults@2021-11-01-preview' existing = {
  name: keyVaultName
  scope: resourceGroup(managementGovernanceResourceGroupName)
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: applicationInsightsName
  scope: resourceGroup(runtimeInfraResourceGroupName)
}

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2021-12-01-preview' existing = {
  name: containerRegistryName
  scope: resourceGroup(managementGovernanceResourceGroupName)
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-09-01' existing = {
  name: storageAccountName
  scope: resourceGroup(technicalInfraResourceGroupName)
}

resource mlOpsWorkspace 'Microsoft.MachineLearningServices/workspaces@2022-01-01-preview' = {
  name: resourceName
  location: location
  tags: tags
  properties: {
    containerRegistry: containerRegistry.id
    applicationInsights: applicationInsights.id
    friendlyName: 'TastyBeans MLOps Environment'
    keyVault: keyVault.id
    publicNetworkAccess: 'Enabled'
    storageAccount: storageAccount.id
  }
}
