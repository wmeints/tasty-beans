# Recommend-coffee sample solution

This solution implements microservices with related analytics capabilities.

## System requirements

* Helm 3
* Docker for Desktop
* Minikube

## Getting started

This section covers building and deploying the sample solution on your local
machine. Ensure you meet the system requirements, or the solution will likely
not deploy correctly.

### Building images

Please follow these steps to build the docker images for the solution:

```console
./build-images.ps1
```

### Deploying the Helm chart

After building the images, you can deploy the helm chart to your local
Kubernetes cluster. Use the following command to deploy the helm chart:

```console
./deploy-chart.ps1
```

When asked, enter a password for the database server.
