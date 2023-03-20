# MLOps environment

This part of the repository contains the MLOps environment for the tastybeans
sample solution. Please read the contents of this README to get up to speed with
the machine-learning models for this sample solution.

## Getting started

Make sure to install the cartographer operator for the environment. We use this
experimental operator for the sample to validate our working processes.

You can find it here: https://github.com/wmeints/cartographer

After installing the operator run the following command to install the workspace:

```
kubectl apply k8s/workspace.yml
```

This will create a workspace with the following nodes:

1. An experiment tracking node
1. A workflow environment with 1 controller and 1 worker node
3. A compute cluster with 1 head node and 3 worker nodes

Please make sure you have a cluster that can support the workload. You can verify
requirements in the `k8s/workspace.yml` file.