$existingImages = docker images recommendcoffee.azurecr.io/* --format "{{ .Repository }}:{{ .Tag }}"

foreach($imageName in $existingImages) {
    docker rmi -f $imageName
}