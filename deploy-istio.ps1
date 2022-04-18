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

# Finally, enable the observability components
kubectl apply -f https://raw.githubusercontent.com/istio/istio/release-1.13/samples/addons/prometheus.yaml
kubectl apply -f https://raw.githubusercontent.com/istio/istio/release-1.13/samples/addons/grafana.yaml
kubectl apply -f https://raw.githubusercontent.com/istio/istio/release-1.13/samples/addons/kiali.yaml
kubectl apply -f https://raw.githubusercontent.com/istio/istio/release-1.13/samples/addons/jaeger.yaml
