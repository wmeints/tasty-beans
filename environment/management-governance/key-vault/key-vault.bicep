// ############################################################################
// Keyvault
//
// Deploys a keyvault resource
// ############################################################################

// ############################################################################
// Parameters
// ############################################################################

@description('The location where to deploy the environment')
param location string

@description('The tags to attach to the resources')
param tags object

@description('The name of the resource')
param resourceName string

// ############################################################################
// Resources
// ############################################################################

resource keyVault 'Microsoft.KeyVault/vaults@2021-06-01-preview' = {
  name: resourceName
  location: location
  tags: tags
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enableRbacAuthorization: true
    enabledForDeployment: false
    enabledForDiskEncryption: true
    enabledForTemplateDeployment: true
    enableSoftDelete: true
    enablePurgeProtection: true
    softDeleteRetentionInDays: 90
    publicNetworkAccess: 'disabled'
    networkAcls: {
      defaultAction: 'Deny'
      bypass: 'AzureServices'
    }
  }
}

// ############################################################################
// Outputs
// ############################################################################

