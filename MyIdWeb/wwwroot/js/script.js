function openInNewTab(array, mime) {
    // Create a Blob object from the array
    var file = new Blob([array], { type: mime });
    // Create a URL for the Blob
    var fileURL = URL.createObjectURL(file);
    // Open the URL in a new tab
    window.open(fileURL, '_blank');
}