<#
    .SYNOPSIS
        Builds the docker images.

    .DESCRIPTION
        This script is used to build the dockeri mages. You need to call 
        the following command:

        minikube docker-env --shell powershell | invoke-expression

        This will set the environment variables for the docker images so they
        work with minikube. You can ignore this when you're using another
        solution to run Kubernetes.
    
    .PARAMETER ContainerRegistry
        The container registry name prefix to tag the images with.
#>

param(
    [System.String] $ContainerRegistry = "recommendcoffee.azurecr.io"
)

# This table determines which components to build and how to build them.
# Set migrate = $true for components that need the database migration init container.
$ImagesToBuild = @(
    @{ name = "catalog"; migrate = $true; entrypoint = "RecommendCoffee.Catalog.Api.dll" }
    @{ name = "customermanagement"; migrate = $true; entrypoint = "RecommendCoffee.CustomerManagement.Api.dll" }
    @{ name = "payments"; migrate = $true; entrypoint = "RecommendCoffee.Payments.Api.dll" }
    @{ name = "ratings"; migrate = $true; entrypoint = "RecommendCoffee.Ratings.Api.dll" }
    @{ name = "registration"; migrate = $false; entrypoint = "RecommendCoffee.Registration.Api.dll" }
    @{ name = "subscriptions"; migrate = $true; entrypoint = "RecommendCoffee.Subscriptions.Api.dll" }
    @{ name = "identity"; migrate = $true; entrypoint = "RecommendCoffee.Identity.Api.dll" }
    @{ name = "timer"; migrate = $false; entrypoint = "RecommendCoffee.Timer.Api.dll" }
    @{ name = "shipping"; migrate = $true; entrypoint = "RecommendCoffee.Shipping.Api.dll" }
    @{ name = "recommendations"; migrate = $true; entrypoint = "RecommendCoffee.Recommendations.Api.dll" }
)

# We generate a timestamp for the image tag.
# This ensures we get fresh container images.
$Timestamp = Get-Date -Format 'yyyyMMddHHmmss'

# Build the migration tooling for the application first.
# We'll be generating migration images for each of the services.
docker build -t recommendcoffee.azurecr.io/database-migrations:$Timestamp `
    -f ./tools/migrate-database/Dockerfile `
    ./tools/migrate-database

foreach($ServiceDefinition in $ImagesToBuild) {
    $GenerateMigrationContainer = $ServiceDefinition.migrate

    $ServiceName = $ServiceDefinition.name

    $MigrationDockerFilePath = "./services/$ServiceName/Dockerfile.migrations"
    $MigrationContextPath = "./services/$ServiceName"
    $MigrationImageTag = "${ContainerRegistry}/${ServiceName}-migrations:$Timestamp"

    $DockerFilePath = "./services/$ServiceName/Dockerfile"
    $Entrypoint = $ServiceDefinition.entrypoint
    $ImageTag = "${ContainerRegistry}/${ServiceName}:$Timestamp"
    
    # Build the application container.
    docker build -t $ImageTag -f $DockerFilePath --build-arg SERVICE_NAME=$ServiceName --build-arg ENTRYPOINT=$Entrypoint .

    # Build the database migration init container if we need to.
    # This container will be used to run the migrations.
    if($GenerateMigrationContainer) {
        docker build -t $MigrationImageTag --build-arg TOOL_VERSION=$Timestamp -f $MigrationDockerFilePath $MigrationContextPath
    }
}

docker build -t "${ContainerRegistry}/portal:$Timestamp" -f ./services/portal/Dockerfile .
