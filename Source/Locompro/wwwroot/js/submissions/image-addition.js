document.getElementById('file').addEventListener('change', function(e) {
    var fileInput = this;
    var dataTransfer = new DataTransfer();
    var hasLargeFile = false; // Flag to track if any file is too large

    for (var i = 0; i < fileInput.files.length; i++) {
        var file = fileInput.files[i];
        var fileSize = file.size / 1024 / 1024; // Size in MB

        if (fileSize <= 5) {
            // Add file to the DataTransfer object if it's within the size limit
            dataTransfer.items.add(file);
        } else {
            // Set the flag to true if any file is too large
            hasLargeFile = true;

            // Display an error message for the oversized file
            document.getElementById('fileError').style.display = 'block';
            document.getElementById('fileError').innerText = 'Imagen: "' + file.name + '" excede el tamaño máximo de 5MB.';
        }
    }

    // Replace the file input's files with the new list
    fileInput.files = dataTransfer.files;

    // Clear the error message if no file is too large
    if (!hasLargeFile) {
        document.getElementById('fileError').style.display = 'none';
    }
});


