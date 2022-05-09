param location string
param resourceName string
param dnsPrefix string
param tags object

resource aksCluster 'Microsoft.ContainerService/managedClusters@2022-03-02-preview' = {
  location: location
  name: resourceName
  tags: tags
  sku: {
    name: 'Basic'
    tier: 'Free'
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    dnsPrefix: dnsPrefix
    publicNetworkAccess: 'Enabled'
    enableRBAC: true
    networkProfile: {
      networkPlugin: 'kubenet'
    }
    kubernetesVersion: '1.22.6'
    agentPoolProfiles: [
      {
        name: 'systempool'
        minCount: 1
        maxCount: 5
        maxPods: 110
        vmSize: 'Standard_B4ms'
        mode: 'System'
        osDiskSizeGB: 80
        osType: 'Linux'
        enableAutoScaling: true
        osDiskType: 'Managed'
        enableFIPS: false
      }
      {
        name: 'userpool'
        minCount: 0
        maxCount: 5
        maxPods: 110
        vmSize: 'Standard_B4ms'
        mode: 'User'
        osDiskSizeGB: 80
        osType: 'Linux'
        enableAutoScaling: true
        osDiskType: 'Managed'
        enableFIPS: false
      }
    ]
  }
}

output aksClusterPrincipalId string = aksCluster.properties.identityProfile.kubeletidentity.objectId
