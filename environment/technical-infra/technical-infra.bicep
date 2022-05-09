param location string
param environment string
param tags object

module stateStoreStorage 'storage-account/storage-account.bicep' = {
  name: 'state-store-account'
  params: {
    location: location
    resourceName: 'statestore${environment}'
    tags: tags
  }
}
