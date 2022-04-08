<#
    .SYNOPSIS
        Removes the docker images from the local docker cache.

    .DESCRIPTION
        This script is used to remove all docker images for the application to
        ensure you have a clean slate to start with.
#>

$existingImages = docker images recommendcoffee.azurecr.io/* --format "{{ .Repository }}:{{ .Tag }}"

foreach($imageName in $existingImages) {
    docker rmi -f $imageName
}