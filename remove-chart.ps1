# Find existing installations and remove them.
# We need to get a clean install every time we run this script.

$existingInstallations = (helm list -o json | ConvertFrom-Json)

if($existingInstallations.Count -gt 0)
{
    Write-Host "There are existing installations. Uninstalling them..."

    foreach($existingInstallation in $existingInstallations) 
    {
        $installationName = $existingInstallation.name
        helm uninstall $installationName --wait
    }
}