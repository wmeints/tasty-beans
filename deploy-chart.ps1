[CmdletBinding()]
param (
    [System.String]
    $ReleaseVersion = "",

    [Parameter(Mandatory)]
    [SecureString] 
    $DatabasePassword
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

    $existingImages = docker images recommendcoffee.azurecr.io/catalog --format "{{ .Tag }}"
    $latestVersion = $existingImages | sort-object | select-object -last 1

    $ReleaseVersion = $latestVersion
}

Write-Host "Installing helm chart"

helm install --generate-name ./charts/recommend-coffee `
    --set global.releaseVersion=$ReleaseVersion `
    --set shared.databasePassword=$DatabasePassword `
    --wait