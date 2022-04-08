<#
    .SYNOPSIS
        Deploy the dapr operator to the Kubernetes cluster.

    .DESCRIPTION
        This script is used to deploy the dapr operator for the application.
        This should be run before deploying the helm chart.
#>

helm upgrade --install dapr dapr/dapr `
    --version=1.6 `
    --namespace dapr-system `
    --create-namespace `
    --set global.ha.enabled=false `
    --wait