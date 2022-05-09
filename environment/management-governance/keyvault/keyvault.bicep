param resourceName string
param location string
param tags object

resource keyvault 'Microsoft.KeyVault/vaults@2021-11-01-preview' = {
  name: resourceName
  location: location
  tags: tags
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enabledForDeployment: false
    enabledForTemplateDeployment: false
    publicNetworkAccess: 'Enabled'
  }
}
