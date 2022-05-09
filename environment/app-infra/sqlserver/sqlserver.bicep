param resourceName string
param location string
param tags object
param sqlAdminLogin string
param sqlAdminPassword string
param databases array

resource sqlServer 'Microsoft.Sql/servers@2021-11-01-preview' = {
  name: resourceName
  location: location
  tags: tags
  properties: {
    administratorLogin: sqlAdminLogin
    administratorLoginPassword: sqlAdminPassword
  }

  resource database 'databases' = [for databaseName in databases: {
    location: location
    name: databaseName
    sku: {
      name: 'GP_S_Gen5_1'
      tier: 'GeneralPurpose'
    }
    properties: {
      minCapacity: 1
      maxSizeBytes: 34359738368
      autoPauseDelay: 60
      zoneRedundant: false
      readScale: 'Disabled'
      collation: 'SQL_Latin1_General_CP1_CI_AS'
    }
  }]
}
