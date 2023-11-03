async function loadPictures(pictureContainer, productName, storeName){
    let pictureDataRequest = `SearchResults?handler=GetPictures&productName=${productName}&storeName=${storeName}`;

    $.ajax({
        url: pictureDataRequest,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Process and display the data
            if (data.length > 0) {
                data.forEach(function (picture) {
                    var slideDiv = document.createElement('div');
                    slideDiv.className = 'col';
                    slideDiv.style = 'width: 100%;';

                    var slideContent = document.createElement('div');
                    slideContent.className = 'mySlides';
                    slideContent.style = 'background-color: darkgray; border: 151px solid #e4e4e4';

                    var img = document.createElement('img');
                    img.src = picture;
                    img.style = 'max-height: 300px; max-width: 94%; width: auto; height: auto; transform: translate(-50%, -50%); position: absolute; top: 50%; left: 50%;';
                    img.alt = 'image';

                    slideContent.appendChild(img);
                    slideDiv.appendChild(slideContent);
                    pictureContainer.appendChild(slideDiv);
                });
            } else {
                // Handle no pictures case
                var placeholder = document.createElement('img');
                placeholder.src = 'https://via.placeholder.com/400';
                placeholder.alt = 'Placeholder';
                pictureContainer.appendChild(placeholder);
            }

            showSlides(1);
        },
        error: function () {
            console.error('Error loading pictures');
        }
    });
}