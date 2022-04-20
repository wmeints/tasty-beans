# Recommend-coffee sample solution

Welcome to the recommend-coffee sample solution! This solution implements a fake
company that sells coffee subscriptions.

Customers can subscribe to this service to get coffee delivered every month
or every week. They can rate the coffee which helps recommend-coffee deliver
better quality coffee to them.

## :goal_net: Goals

This sample achieves the following goals:

* Demonstrate how to implement 12-factor cloud-native applications in ASP.NET Core 6.
* Demonstrate how to integrate microservices with a data mesh/data platform.
* Demonstrate how to implement end-to-end ML solutions with MLOps.

## :triangular_flag_on_post: Status

This sample is a work in progress and by no means complete. Currently, only the
microservices are available in the solution. Also, not all planned functionality
is in the microservices yet.

All features are subject to change. But that's part of the fun :grin:

## :computer: System requirements

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

## :rocket: Getting started

Please find [the installation guide](https://github.com/wmeints/recommend-coffee/wiki/Installation-guide) on the Wiki.

## :book: Documentation

This section covers some of the common patterns used in the demo solution.
Please review [the wiki](https://github.com/wmeints/recommend-coffee/wiki) to
learn more.

[PWSH_INSTALL]: https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-linux?view=powershell-7.2