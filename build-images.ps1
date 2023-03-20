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
    [System.String] $ContainerRegistry = "tastybeans.azurecr.io",
    [System.String] $ReleaseVersion = "",
    [System.String] $ServiceName = ""
)

# This table determines which components to build and how to build them.
# Set migrate = $true for components that need the database migration init container.
$ImagesToBuild = @(
    @{ name = "catalog"; migrate = $true; entrypoint = "TastyBeans.Catalog.Api.dll" }
    @{ name = "customermanagement"; migrate = $true; entrypoint = "TastyBeans.CustomerManagement.Api.dll" }
    @{ name = "payments"; migrate = $true; entrypoint = "TastyBeans.Payments.Api.dll" }
    @{ name = "ratings"; migrate = $true; entrypoint = "TastyBeans.Ratings.Api.dll" }
    @{ name = "registration"; migrate = $false; entrypoint = "TastyBeans.Registration.Api.dll" }
    @{ name = "subscriptions"; migrate = $true; entrypoint = "TastyBeans.Subscriptions.Api.dll" }
    @{ name = "identity"; migrate = $true; entrypoint = "TastyBeans.Identity.Api.dll" }
    @{ name = "timer"; migrate = $false; entrypoint = "TastyBeans.Timer.Api.dll" }
    @{ name = "shipping"; migrate = $true; entrypoint = "TastyBeans.Shipping.Api.dll" }
    @{ name = "recommendations"; migrate = $true; entrypoint = "TastyBeans.Recommendations.Api.dll" }
    @{ name = "transport"; migrate = $false; entrypoint = "TastyBeans.Transport.Api.dll" }
    @{ name = "simulation"; migrate = $false; entrypoint = "TastyBeans.Simulation.Api.dll" }
)

# We generate a timestamp for the image tag.
# This ensures we get fresh container images.
$Timestamp = Get-Date -Format 'yyyyMMddHHmmss'

if($ReleaseVersion -eq "") {
    $ReleaseVersion = $Timestamp
}

$FilteredImageList = $ImagesToBuild

if($ServiceName -ne "") {
    $FilteredImageList = $FilteredImageList | Where-Object { $_.name -eq $ServiceName }
}

# Build the migration tooling for the application first.
# We'll be generating migration images for each of the services.
docker build -t ${ContainerRegistry}/database-migrations:$Timestamp `
    -f ./tools/migrate-database/Dockerfile `
    ./tools/migrate-database

foreach($ServiceDefinition in $FilteredImageList) {
    $GenerateMigrationContainer = $ServiceDefinition.migrate

    $ServiceName = $ServiceDefinition.name

    $MigrationDockerFilePath = "./platform/services/$ServiceName/Dockerfile.migrations"
    $MigrationContextPath = "./platform/services/$ServiceName"
    $MigrationImageTag = "${ContainerRegistry}/${ServiceName}-migrations:$ReleaseVersion"

    $DockerFilePath = "./platform/services/$ServiceName/Dockerfile"
    $Entrypoint = $ServiceDefinition.entrypoint
    $ImageTag = "${ContainerRegistry}/${ServiceName}:$ReleaseVersion"
    $ContextPath = "./platform/"
    
    # Build the application container.
    docker build -t $ImageTag `
        -f $DockerFilePath `
        --build-arg SERVICE_NAME=$ServiceName `
        --build-arg ENTRYPOINT=$Entrypoint `
        $ContextPath

    # Build the database migration init container if we need to.
    # This container will be used to run the migrations.
    if($GenerateMigrationContainer) {
        docker build -t $MigrationImageTag `
            --build-arg REPOSITORY=$ContainerRegistry `
            --build-arg TOOL_VERSION=$Timestamp `
            -f $MigrationDockerFilePath `
            $MigrationContextPath
    }
}

if(($ServiceName -eq "portal") -or ($ServiceName -eq "")) {
    docker build -t "${ContainerRegistry}/portal:${ReleaseVersion}" -f ./platform/services/portal/Dockerfile ./platform/    
}
