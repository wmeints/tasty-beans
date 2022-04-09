<#
    .SYNOPSIS
        Deploy the dapr operator to the Kubernetes cluster.

    .DESCRIPTION
        This script is used to deploy the dapr operator for the application.
        This should be run before deploying the helm chart.

        Please note that the script disables mTLS as this is handled by the
        istio service mesh.
#>

helm upgrade --install dapr dapr/dapr `
    --version=1.6 `
    --namespace dapr-system `
    --create-namespace `
    --set global.ha.enabled=false `
    --set global.mtls.enabled=false `
    --wait