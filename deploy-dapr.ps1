<#
    .SYNOPSIS
        Deploy the dapr operator to the Kubernetes cluster.

    .DESCRIPTION
        This script is used to deploy the dapr operator for the application.
        This should be run before deploying the helm chart.

        Please note that the script disables mTLS as this is handled by the
        istio service mesh.
#>

dapr init -k