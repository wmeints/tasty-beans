<#
    .SYNOPSIS
        Cleans up dangling resources in the Kubernetes cluster.

    .DESCRIPTION
        This script is used to remove any resources that are dangling after a
        failed helm deployment.
#>

kubectl delete service --all
kubectl delete deployment --all
kubectl delete configmap --all
kubectl delete secret --all
kubectl delete pvc --all
kubectl delete pv --all
kubectl delete configuration --all
kubectl delete component --all