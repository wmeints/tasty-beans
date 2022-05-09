param location string
param environment string
param tags object

module aksCluster 'aks-cluster/aks-cluster.bicep' = {
  name: 'aks-cluster'
  params: {
    dnsPrefix: 'aks-tastybeans-${environment}'
    location: location
    resourceName: 'aks-tastybeans-${environment}'
    tags: tags
  }
}

module analyticsWorkspace 'monitoring/log-analytics-workspace.bicep' = {
  name: 'log-analytics-workspace'
  params: {
    location: location
    resourceName: 'law-tastybeans-${environment}'
    tags: tags
  }
}

module applicationInsights 'monitoring/app-insights.bicep' = {
  name: 'app-insights'
  params: {
    analyticsWorkspaceId: analyticsWorkspace.outputs.logAnalyticsWorkspaceId
    location: location
    resourceName: 'as-tastybeans-${environment}'
  }
}

output aksClusterPrincipalId string = aksCluster.outputs.aksClusterPrincipalId
