<#

    .SYNOPSIS

    Deploys the istio service mesh to the Kubernetes cluster.

    .DESCRIPTION

    This script deploys the istio service mesh to the Kubernetes cluster.
    Using this enables automatic ingress and a ton of other features.
#>

# First, install the istio control plane with minimal resources.
# The demo profile is perfect for the demo solution we're deploying.
istioctl install `
    --set profile=demo `
    --set meshConfig.outboundTrafficPolicy.mode=REGISTRY_ONLY `
    -y

# Next, enable the istio sidecar component auto-injection.
kubectl label namespace default istio-injection=enabled