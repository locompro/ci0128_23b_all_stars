/**
 * Represents a container for search result images, allowing users to navigate through images related to a product from a specific store.
 * It dynamically loads images and creates a slideshow for the user to view them.
 */
class SearchResultsPictureContainer {
    /**
     * Constructs a new SearchResultsPictureContainer with specific product and store names, along with the page URL to fetch images from.
     *
     * @param productName the name of the product to display images for
     * @param storeName   the name of the store where the product is available
     * @param page        the URL of the page to send the request for images
     */
    constructor(productName, storeName, page) {
        this.pictureDataRequest = `${page}?handler=GetPictures&productName=${productName}&storeName=${storeName}`;
        this.slides = document.getElementsByClassName("mySlides");
        this.picturesContainer = document.getElementById("picturesContainer");
        this.currentSlideIndex = 1;
        this.pictureData = [];

        this.prevButton = document.getElementById("prevButton");
        this.nextButton = document.getElementById("nextButton");
    }

    /**
     * Initiates the building of the picture container by loading pictures and setting up the slideshow.
     * It also configures the visibility of navigation buttons based on the number of images received.
     */
    buildPictureContainer() {
        this.loadPictures()
            .then(r => {
                if (this.pictureData.length > 0) {
                    this.displayPicturesIfReceived();
                } else {
                    this.displayPicturesIfNoneReceived();
                }

                this.showSlides(this.currentSlideIndex);

                if (this.pictureData.length < 2) {
                    this.prevButton.style.display = "none";
                    this.nextButton.style.display = "none";
                } else {
                    this.prevButton.style.display = "block";
                    this.nextButton.style.display = "block";
                }
            });
    }

    /**
     * Asynchronously loads pictures from the server and stores them in an array.
     * If the network response is not OK, throws an error.
     *
     * @return a Promise that resolves once the pictures have been loaded
     */
    async loadPictures() {
        try {
            const response = await fetch(this.pictureDataRequest);
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            this.pictureData = await response.json();
        } catch (error) {
            console.error('Error loading pictures:', error);
        }
    }

    /**
     * Displays pictures in the container if any are received from the server.
     * It creates HTML elements for each image and adds them to the container.
     */
    displayPicturesIfReceived() {
        let picturesContainer = this.picturesContainer;

        this.pictureData.forEach(function (picture) {
            let slideDiv = document.createElement('div');
            slideDiv.className = 'col';
            slideDiv.style = 'width: 100%;';

            let slideContent = document.createElement('div');
            slideContent.className = 'mySlides';
            slideContent.style = 'background-color: darkgray; border: 1px solid #e4e4e4';

            let img = document.createElement('img');
            img.src = picture;
            img.style = 'max-height: 220px; max-width: 94%; width: auto; height: auto; transform: translate(-50%, -50%); position: absolute; top: 50%; left: 50%;';
            img.alt = 'Product Image';

            slideContent.appendChild(img);
            slideDiv.appendChild(slideContent);
            picturesContainer.appendChild(slideDiv);
        });
    }

    /**
     * Displays a placeholder image in the container if no images are received from the server.
     */
    displayPicturesIfNoneReceived() {
        let placeholder = document.createElement('img');
        placeholder.src = 'https://via.placeholder.com/400';
        placeholder.alt = 'No images available';
        this.picturesContainer.appendChild(placeholder);
    }

    /**
     * Shows the slide at the specified index. If the index is out of bounds, it will circle back to the beginning or end of the slide list.
     * Adjusts the display of each slide accordingly.
     *
     * @param slideIndex the index of the slide to show
     */
    showSlides(slideIndex) {
        if (slideIndex > this.slides.length) {
            this.currentSlideIndex = 1;
        }
        if (slideIndex < 1) {
            this.currentSlideIndex = this.slides.length;
        }
        for (let currentSlide = 0; currentSlide < this.slides.length; currentSlide++) {
            this.slides[currentSlide].style.display = "none";
        }

        this.slides[this.currentSlideIndex-1].style.display = "block";
    }

    /**
     * Changes the current slide by a given number, which can be positive or negative.
     * It calls showSlides to update the display to the new slide.
     *
     * @param n the number of slides to move forward or backward
     */
    plusSlides(n) {
        this.showSlides(this.currentSlideIndex += n);
    }
}
