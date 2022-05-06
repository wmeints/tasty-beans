<#
    .SYNOPSIS
        Deploys the Helm chart for the demo solution.

    .DESCRIPTION
        This script is used to deploy the Helm chart for the demo solution. 

    .PARAMETER ReleaseVersion
        The docker image tag to deploy. You can find this when invoking
        build-images.ps1 in the same folder as this script.
    
    .PARAMETER DatabasePassword
        The password to use for the database access.

    .PARAMETER ContainerRegistry
        The name of the container registry to use.
#>

[CmdletBinding()]
param (
    [System.String]
    $ReleaseVersion = "",

    [Parameter(Mandatory)]
    [SecureString] 
    $DatabasePassword,

    [System.String]
    $ContainerRegistry = "tastybeans.azurecr.io"
)

# Find existing installations and remove them.
# We need to get a clean install every time we run this script.

$existingInstallations = (helm list -o json | ConvertFrom-Json)

if($existingInstallations.Count -gt 0)
{
    Write-Host "There are existing installations. Uninstalling them..."

    foreach($existingInstallation in $existingInstallations) 
    {
        $installationName = $existingInstallation.name
        helm uninstall $installationName --wait
    }
}

Write-Host "Installing the helm chart for release $ReleaseVersion"

# Determine the latest version of the docker images if the release version wasn't specified.
# We assume that there's a catalog image available to determine the latest version from.
if($ReleaseVersion -eq "") {
    Write-Host "No explicit release version was specified. Using the latest version."

    $existingImages = docker images $ContainerRegistry/catalog --format "{{ .Tag }}"
    $latestVersion = $existingImages | sort-object | select-object -last 1

    $ReleaseVersion = $latestVersion
}

if($ReleaseVersion -eq "") {
    Write-Host "No release version could be determined. Exiting."
    exit 1
}

Write-Host "Installing helm chart"

helm install --generate-name ./charts/tasty-beans `
    --set global.releaseVersion=$ReleaseVersion `
    --set global.containerRegistry="${ContainerRegistry}" `
    --set shared.databasePassword="${DatabasePassword}"