# Recommend-coffee sample solution

This solution implements microservices with related analytics capabilities.

## Goals

This demo solution is meant for me as a sample project to show off various pieces
of technology. Currently, I'm using this to:

* Demonstrate how to implement observability in ASP.NET Core 6
* Demonstrate how to integrate your microservices with a data mesh/data platform
* Demonstrate how to implement end-to-end ML solutions with MLOps

## Status

This is a work in progress and by no means complete. Currently, only the microservices
are available in the solution. Also, not all planned functionality is in the microservices
yet.

All features are subject to change. But that's part of the fun :grin:

## System requirements

Please make sure you have the following tools available:

* [Helm 3](https://helm.sh/docs/intro/quickstart/)
* [Docker](https://www.docker.com/get-started/)
* [Minikube](https://minikube.sigs.k8s.io/docs/start/) or other Kubernetes cluster.
* [Powershell](https://github.com/PowerShell/PowerShell)
* [Istio](https://istio.io/latest/docs/setup/getting-started/)
* [Dapr](https://docs.dapr.io/getting-started/install-dapr-cli/)

You'll need 2 CPU cores available and 16GB of memory for all the containers in
the solution. You can deploy with less, but it's untested, so you've been warned.

Make sure you have `istio`, `helm`, and `kubectl` in your `PATH` variable. The
utility scripts expect you to have these tools available.

## Getting started

This section covers building and deploying the sample solution on your local
machine. Ensure you meet the system requirements, or the solution will likely
not deploy correctly.

### Building images

Please follow these steps to build the docker images for the solution:

```console
./build-images.ps1
```

### Deploying the service mesh

The solution uses the Istio service mesh to expose API endpoints to your local machine.
We'll also use Istio for communication between service and A/B tests at a later stage.

You can install Istio using the following command:

```console
./deploy-istio.ps1
```

It will take a few minutes for the script to complete. After you've installed
Istio you can install dapr.

### Installing dapr

We use Dapr in the solution to provide a layer of abstraction on top of common
application components such as state storage, and pub/sub. You can install Dapr
using the following command:

```console
./deploy-dapr.ps1
```

### Installing OpenTelemetry Collector

The OpenTelemetry collector centralizes the shipping of metrics and traces
to a single processor that ships the information to the appropriate backend tools.
We're using zipkin to store traces. Metrics aren't stored yet.

### Deploying the Helm chart

After building the images, and deploying the cluster infrastructure, you can
deploy the helm chart to your Kubernetes cluster. Use the following command to
deploy the helm chart:

```console
./deploy-chart.ps1
```

When asked, enter a password for the database server.
We're storing the database password as a secret in the Kubernetes cluster.

## Documentation

This section covers some of the common patterns used in the demo solution.
Please review the wiki for more details.

### Solution structure

TODO: Solution structure

### Networking

TODO: Networking configuration with the service mesh.

### Observability

TODO: Observability setup.

### Useful scripts

This solution includes several useful scripts:

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
* `deploy-istio.ps1`  
  This script deploys Istio with the demo profile to provide a light-weight
  service mesh on top of Kubernetes.
* `deploy-dapr.ps1`  
  This script deploys the Dapr operator on your Kubernetes cluster without
  high-availability.
* `deploy-otel.ps1`  
  This script deploys the OpenTelemetry operator to the Kubernetes cluster.
* `deploy-chart.ps1`  
  This script deploys the application chart to your Kubernetes cluster. You can
  choose to deploy the latest version of a specific version by providing the
  `-ReleaseName` parameter.
* `restart-deployments.ps1`  
  This script restarts all deployments. Use this if you've forgotten to deploy
  Istio or Dapr before deploying the Helm chart.
* `clean-resources.ps1`  
  This script cleans up resources that remain behind when deploying the
  application chart fails.
