# Tasty Beans sample solution

![Tasty Beans logo](./images/tasty-beans.png)

Welcome to Tasty Beans, the subscription-based coffee beans shop that makes it
easy to get your hands on the best beans around!

With our recommendations feature, you can easily find the beans that suit your
taste, and with our start-up guide, you can get brewing in no time.

Our selection of beans is based on recommendations from coffee aficionados
around the world. Whether you like your coffee light and delicate or dark and
bold, we have the perfect beans for you. Start your subscription today and enjoy
the best coffee beans delivered right to your door.

## :goal_net: Goals

This sample achieves the following goals:

* Demonstrate how to implement 12-factor cloud-native applications in ASP.NET Core 6.
* Demonstrate how to integrate microservices with a data mesh/data platform.
* Demonstrate how to implement end-to-end ML solutions with MLOps.

## :triangular_flag_on_post: Status

This sample is a work in progress and by no means complete. Currently, only the
microservices are available in the solution. Also, not all planned functionality
is in the microservices yet.

Please note, the solution was previously called recommend-coffee. We're renaming it to tasty beans.
You may or may not find a lot of references to the old solution name :smile:

All features are subject to change. But that's part of the fun :grin:

## :computer: System requirements

Please make sure you have the following tools available:

* [Helm 3](https://helm.sh/docs/intro/quickstart/)
* [Docker Desktop](https://www.docker.com/get-started/)
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

Please review [the wiki](https://github.com/wmeints/recommend-coffee/wiki) to
learn more about using the solution.

[PWSH_INSTALL]: https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-linux?view=powershell-7.2
