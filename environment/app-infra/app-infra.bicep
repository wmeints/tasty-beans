param location string
param environment string
param tags object
param sqlAdminPassword string
param sqlAdminLogin string
param applicationInsightsName string
param keyVaultName string
param containerRegistryName string
param mlopsStorageAccountName string
param managementGovernanceResourceGroupName string
param runtimeInfraResourceGroupName string
param technicalInfraResourceGroupName string

var databases = [
  'catalog'
  'customermanagement'
  'identity'
  'payments'
  'ratings'
  'recommendations'
  'shipping'
  'subscriptions'
]

module sqlServer 'sqlserver/sqlserver.bicep' = {
  name: 'sql-server'
  params: {
    resourceName: 'sql-tastybeans-${environment}'
    sqlAdminLogin: sqlAdminLogin
    sqlAdminPassword: sqlAdminPassword
    location: location
    databases: databases
    tags: tags
  }
}

module mlopsWorkspace 'mlops/mlops.bicep' = {
  name: 'mlops-workspace'
  params: {
    applicationInsightsName: applicationInsightsName
    containerRegistryName: containerRegistryName
    keyVaultName: keyVaultName
    location: location
    tags: tags
    resourceName: 'mlops-tastybeans-${environment}'
    managementGovernanceResourceGroupName: managementGovernanceResourceGroupName
    runtimeInfraResourceGroupName: runtimeInfraResourceGroupName
    technicalInfraResourceGroupName: technicalInfraResourceGroupName
    storageAccountName: mlopsStorageAccountName
  }
}
