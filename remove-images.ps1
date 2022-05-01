<#
    .SYNOPSIS
        Removes the docker images from the local docker cache.

    .DESCRIPTION
        This script is used to remove all docker images for the application to
        ensure you have a clean slate to start with.

    .PARAMETER ContainerRegistry
        The container registry name prefix to tag the images with.
#>

param(
    $ContainerRegistry = "tastybeans.azurecr.io"
)

$existingImages = docker images $ContainerRegistry/* --format "{{ .Repository }}:{{ .Tag }}"

foreach($imageName in $existingImages) {
    docker rmi -f $imageName
}