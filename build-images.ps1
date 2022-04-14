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
#>

# This table determines which components to build and how to build them.
# Set migrate = $true for components that need the database migration init container.
$ImagesToBuild = @(
    @{ name = "catalog"; migrate = $true },
    @{ name = "customermanagement"; migrate = $true },
    @{ name = "payments"; migrate = $true },
    @{ name = "ratings"; migrate = $true },
    @{ name = "registration"; migrate = $false },
    @{ name = "subscriptions"; migrate = $true },
    @{ name = "identity"; migrate = $true }
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
    $ServiceName = $ServiceDefinition.name
    $GenerateMigrationContainer = $ServiceDefinition.migrate

    $DockerFilePath = "./services/$ServiceName/Dockerfile"
    $MigrationDockerFilePath = "./services/$ServiceName/Dockerfile.migrations"
    $ContextPath = "./services/$ServiceName"
    $ImageTag = "recommendcoffee.azurecr.io/${ServiceName}:$Timestamp"
    $MigrationImageTag = "recommendcoffee.azurecr.io/${ServiceName}-migrations:$Timestamp"

    # Build the application container.
    docker build -t $ImageTag -f $DockerFilePath $ContextPath

    # Build the database migration init container if we need to.
    # This container will be used to run the migrations.
    if($GenerateMigrationContainer) {
        docker build -t $MigrationImageTag --build-arg TOOL_VERSION=$Timestamp -f $MigrationDockerFilePath $ContextPath
    }
}


