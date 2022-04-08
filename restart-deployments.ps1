<#
    .SYNOPSIS
        Restarts the deployments for the application.

    .DESCRIPTION
        This script is used to restart all deployments in the default namespace.
        You can use this to restart the deployments if you find that they don't
        respond as expected.
#>

$deployments = kubectl get deployments -o json | ConvertFrom-Json

foreach($deployment in $deployments.items)
{
    $deploymentName = $deployment.metadata.name
    $deploymentResource = "deployments/$deploymentName"

    Write-Host "Restarting $deploymentName"

    kubectl rollout restart $deploymentResource
}