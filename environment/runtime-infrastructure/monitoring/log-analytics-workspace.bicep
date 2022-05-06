// ############################################################################
// Log analytics workspace
// 
// Deploys the log analytics workspace for the application monitoring.
// ############################################################################

// ############################################################################
// Parameters
// ############################################################################

@description('The name of the resource')
param resourceName string

@description('Tags to attach to the resources')
param tags object

@description('Location to use for the resources')
param location string

// ############################################################################
// Resources
// ############################################################################

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
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

// ############################################################################
// Outputs
// ############################################################################

output logAnalyticsWorkspaceId string = logAnalyticsWorkspace.id
