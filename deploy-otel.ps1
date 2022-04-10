<#
    .SYNOPSIS

    Deploy open telemetry operator

    .DESCRIPTION

    This script deploys the cert manager and open telemetry operator in the cluster.
    You'll need this to collect tracing and metrics from your application.
#>

kubectl apply -f https://github.com/cert-manager/cert-manager/releases/download/v1.8.0/cert-manager.yaml
kubectl apply -f https://github.com/open-telemetry/opentelemetry-operator/releases/latest/download/opentelemetry-operator.yaml