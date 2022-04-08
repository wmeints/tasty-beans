# Recommend-coffee sample solution

This solution implements microservices with related analytics capabilities.

## System requirements

Please make sure you have the following tools available:

* [Helm 3](https://helm.sh/docs/intro/quickstart/)
* [Docker](https://www.docker.com/get-started/)
* [Minikube](https://minikube.sigs.k8s.io/docs/start/)
* [Powershell](https://github.com/PowerShell/PowerShell)

You'll need 2 CPU cores available and 16GB of memory for all the containers in
the solution.

## Getting started

This section covers building and deploying the sample solution on your local
machine. Ensure you meet the system requirements, or the solution will likely
not deploy correctly.

### Start the Kubernetes cluster

Run the following commands to set up the Minikube Kubernetes cluster:

```console
minikube start --memory=16G --cpus=2
minikube addons enable ingress
```

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
We're storing the secret in the Kubernetes cluster.
