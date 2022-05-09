param location string
param environment string
param tags object
param sqlAdminPassword string
param sqlAdminLogin string

var databases = [
  'catalog'
  'customermanagement'
  'identity'
  'payments'
  'ratings'
  'recommendations'
  'shipping'
  'subscriptions'
]

module sqlServer 'sqlserver/sqlserver.bicep' = {
  name: 'sql-server'
  params: {
    resourceName: 'sql-tastybeans-${environment}'
    sqlAdminLogin: sqlAdminLogin
    sqlAdminPassword: sqlAdminPassword
    location: location
    databases: databases
    tags: tags
  }
}
