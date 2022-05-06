<#
    .SYNOPSIS
        Pushes the docker images to the remote container registry
    
    .PARAMETER ContainerRegistry
        The name of the container registry to push to.
    
    .PARAMETER ReleaseVersion
        The version of the images to push.
#>
param (
    [string] $ContainerRegistry = "tastybeans.azurecr.io",
    [string] $ReleaseVersion
)

# Determine the latest version of the docker images if the release version wasn't specified.
# We assume that there's a catalog image available to determine the latest version from.
if($ReleaseVersion -eq "") {
    Write-Host "No explicit release version was specified. Using the latest version."

    $existingImages = docker images $ContainerRegistry/catalog --format "{{ .Tag }}"
    $latestVersion = $existingImages | sort-object | select-object -last 1

    $ReleaseVersion = $latestVersion
}

docker push $ContainerRegistry/catalog:$ReleaseVersion
docker push $ContainerRegistry/customermanagement:$ReleaseVersion
docker push $ContainerRegistry/identity:$ReleaseVersion
docker push $ContainerRegistry/payments:$ReleaseVersion
docker push $ContainerRegistry/portal:$ReleaseVersion
docker push $ContainerRegistry/ratings:$ReleaseVersion
docker push $ContainerRegistry/recommendations:$ReleaseVersion
docker push $ContainerRegistry/registration:$ReleaseVersion
docker push $ContainerRegistry/shipping:$ReleaseVersion
docker push $ContainerRegistry/simulation:$ReleaseVersion
docker push $ContainerRegistry/subscriptions:$ReleaseVersion
docker push $ContainerRegistry/timer:$ReleaseVersion
docker push $ContainerRegistry/transport:$ReleaseVersion