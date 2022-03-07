$appId = "registration"
$appPort = "5174"
$projectPath = "services/registration/src/Api/Api.csproj"

$configFile = "./dapr/configuration/config.yaml"
$componentsPath = "./dapr/components"

dapr run `
    --app-id $appId `
    --components-path $componentsPath `
    --config $configFile `
    --app-port $appPort -- dotnet run --project $projectPath