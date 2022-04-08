<#
    .SYNOPSIS
        Removes the Helm chart from the Kubernetes cluster.

    .DESCRIPTION
        This script is used to clean up your Kubernetes cluster when you're done
        working with the demo solution.
#>

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