<#

    .SYNOPSIS

    Deploys the istio service mesh to the Kubernetes cluster.

    .DESCRIPTION

    This script deploys the istio service mesh to the Kubernetes cluster.
    Using this enables automatic ingress and a ton of other features.
#>

istioctl install --set profile=demo -y