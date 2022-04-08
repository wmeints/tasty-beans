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

## Documentation

### Useful scripts

This solution includes several valuable scripts:

* `build-images.ps1`  
  This script builds the docker images for the solution.
* `remove-chart.ps1`  
  This script removes the deployed Helm chart from your Kubernetes cluster. You
  can use this script as a cleanup if you're using your Kubernetes installation
  for more than the demo solution.
* `remove-images.ps1`  
  This script removes any docker images related to the demo solution. This
  script is beneficial if you find that you're not getting the right
  images deployed on your Kubernetes cluster.
* `deploy-dapr.ps1`  
  This script deploys the Dapr operator on your Kubernetes cluster without
  high-availability.
* `deploy-chart.ps1`  
  This script deploys the application chart to your Kubernetes cluster. You can
  choose to deploy the latest version of a specific version by providing the
  `-ReleaseName` parameter.
* `clean-resources.ps1`  
  This script cleans up resources that remain behind when deploying the
  application chart fails.
