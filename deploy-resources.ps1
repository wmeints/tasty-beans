###############################################################################
# Deployment parameters
###############################################################################

param (
    [Parameter(Mandatory)][string] $SubscriptionId,
    [Parameter(Mandatory)][string] $ResourceGroupName,
    [string] $ContainerRegistryName = "tastybeans",
    [string] $Environment = "dev",
    [string] $Location = "eastus"
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

az account set --subscription "${SubscriptionId}"

# Deploy the resource separately so that we don't run into problems where the
# referenced resource groups don't exist yet.

# az deployment group create `
#     -g $ResourceGroupName `
#     --template-file ./environment/management-governance/resource-groups.bicep `
#     --parameters environment=$Environment `
#     --parameters location=$Location `

# Deploy the environment resources in the resource groups.

az deployment group create `
    -g $ResourceGroupName `
    --template-file ./environment/environment.bicep `
    --parameters environment=$Environment `
    --parameters location=$Location `
    --parameters containerRegistryName=$ContainerRegistryName `
    --parameters managementGovernanceResourceGroupName=$ResourceGroupName