$appId = "customers"
$appPort = "5092"
$projectPath = "services/customers/src/Api/Api.csproj"

$configFile = "./dapr/configuration/config.yaml"
$componentsPath = "./dapr/components"

dapr run `
    --app-id $appId `
    --components-path $componentsPath `
    --config $configFile `
    --app-port $appPort -- dotnet run --project $projectPath