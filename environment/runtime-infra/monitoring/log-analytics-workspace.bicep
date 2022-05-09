param resourceName string
param location string
param tags object

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  name: resourceName
  location: location
  tags: tags
  properties: {
    retentionInDays: 30
    sku: {
      name: 'PerGB2018'
    }
  }
}

output logAnalyticsWorkspaceId string = logAnalyticsWorkspace.id
