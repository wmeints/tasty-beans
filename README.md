# Recommend-coffee sample solution

This solution implements microservices with related analytics capabilities.

## Goals

We're trying to achieve a number of goals in this sample:

* Demonstrate how to implement observability in ASP.NET Core 6
* Demonstrate how to integrate your microservices with a data mesh/data platform
* Demonstrate how to implement end-to-end ML solutions with MLOps

## Status

This sample is a work in progress and by no means complete. Currently, only the
microservices are available in the solution. Also, not all planned functionality
is in the microservices yet.

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

On Linux, you can still use the scripts, but you'll have to install Powershell.
This can be done using [the Powershell installation guide][PWSH_INSTALL].
Make sure you prefix the commands in this guide with `pwsh` to invoke the
scripts in Powershell.

### Deploying the service mesh

The solution uses the Istio service mesh to expose API endpoints to your local
machine. We'll also use Istio for communication between service and A/B tests at
a later stage.

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

### Building images

Please follow these steps to build the docker images for the solution:

```console
./build-images.ps1
```

### Deploying the Helm chart

After building the docker images and deploying the cluster infrastructure, you
can deploy the helm chart to your Kubernetes cluster. Use the following command
to deploy the helm chart:

```console
./deploy-chart.ps1
```

When asked, enter a password for the database server.
We're storing the database password as a secret in the Kubernetes cluster.

## Documentation

This section covers some of the common patterns used in the demo solution.
Please review [the wiki](https://github.com/wmeints/recommend-coffee/wiki) to
learn more.

[PWSH_INSTALL]: https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-linux?view=powershell-7.2