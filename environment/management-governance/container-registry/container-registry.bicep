param location string
param resourceName string

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2021-12-01-preview' = {
  location: location
  name: resourceName
  sku: {
    name: 'Standard'
  }
  properties: {
    adminUserEnabled: true
    anonymousPullEnabled: false
    publicNetworkAccess: 'Enabled'
  }
}
