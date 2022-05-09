targetScope = 'subscription'

param resourceName string
param location string
param tags object

resource resourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resourceName
  location: location
  tags: tags
}

output resourceGroupName string = resourceGroup.name
