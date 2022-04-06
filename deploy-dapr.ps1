helm upgrade --install dapr dapr/dapr `
    --version=1.6 `
    --namespace dapr-system `
    --create-namespace `
    --set global.ha.enabled=false `
    --wait