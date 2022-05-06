// ############################################################################
// AKS Cluster
//
// Deploys an AKS cluster with a cost-optimized configuration.
// ############################################################################

// ############################################################################
// Parameters
// ############################################################################

@allowed([
  'dev'
  'test'
  'acc'
  'prod'
])
@description('The name of the environment')
param environment string

@description('The location where to deploy the environment')
param location string

@description('The tags to attach to the resources')
param tags object

@description('The ID of the log analytics resource')
param logAnalyticsWorkspaceResourceId string

@description('The name of the container registry')
param containerRegistryName string

@description('The name of the management & governance resource group')
param managementGovernanceResourceGroupName string

// ############################################################################
// Resources
// ############################################################################


resource kubernetesCluster 'Microsoft.ContainerService/managedClusters@2021-08-01' = {
  name: 'aks-tastybeans-${environment}'
  location: location
  tags: tags
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    enableRBAC: true
    kubernetesVersion: '1.22.6'
    dnsPrefix: 'aks-tastybeans-${environment}'
    networkProfile: {
      loadBalancerSku: 'standard'
      networkPlugin: 'kubenet'
    }
    apiServerAccessProfile: {
      enablePrivateCluster: false
    }
    agentPoolProfiles: [
      {
        name: 'agentpool'
        osDiskSizeGB: 80
        count: 3
        enableAutoScaling: true
        minCount: 1
        maxCount: 5
        vmSize: 'Standard_B4ms'
        osType: 'Linux'
        type: 'VirtualMachineScaleSets'
        mode: 'System'
        maxPods: 110
        enableNodePublicIP: false
      }
      {
        name: 'userpool'
        osDiskSizeGB: 80
        count: 3
        enableAutoScaling: true
        minCount: 0
        maxCount: 5
        vmSize: 'Standard_B4ms'
        osType: 'Linux'
        type: 'VirtualMachineScaleSets'
        mode: 'User'
        maxPods: 110
        enableNodePublicIP: false
      }
    ]
    addonProfiles: {
      omsAgent: {
        enabled: true
        config: {
          logAnalyticsWorkspaceResourceId: logAnalyticsWorkspaceResourceId
        }
      }
    }
  }
}

module acrPullPermissions './acr-pull-permissions.bicep' = {
  name: 'acr-pull-permissions'
  scope: resourceGroup(managementGovernanceResourceGroupName)
  params: {
    environment: environment
    containerRegistryName: containerRegistryName
    aksClusterPrincipalId: kubernetesCluster.identity.principalId
  }
}

// ############################################################################
// Outputs
// ############################################################################

