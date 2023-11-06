/**
 * The SearchResultsModal class is responsible for managing the modal pop-up that displays detailed information about a selected item from the search results.
 */
class SearchResultsModal {
    /**
     * The constructor initializes the modal with detailed information about the product selected from the search results.
     * It sets up the DOM elements that will display the product information, and creates a new SearchResultsPictureContainer to manage product images.
     *
     * @param searchResults An array of search result items.
     * @param itemSelected The index of the selected item within the search results array.
     */
    constructor(searchResults, itemSelected) {
        // DOM element references for displaying product information
        this.modalProductName = document.getElementById("modalProductName");
        this.modalStoreName = document.getElementById("modalStoreName");
        this.modalModel = document.getElementById("modalModel");
        this.modalBrand = document.getElementById("modalBrand");
        this.submissionsTable = document.getElementById("ItemModalSubmissionsTable");

        // The search results array and the index of the selected item
        this.searchResults = searchResults;
        this.itemSelected = itemSelected;

        // Creating a picture container for the selected item's images
        this.pictureContainer =
            new SearchResultsPictureContainer(
                this.searchResults[this.itemSelected].Name,
                this.searchResults[this.itemSelected].Store,
                "SearchResults");

        this.submissionsRatings = [];

        // Populate the modal with the selected item's details
        this.populateModal();
    }

    /**
     * This method populates the modal with the selected item's details, including product name, store name, model, brand, and submissions.
     * It also initializes the picture container with the item's images.
     */
    populateModal() {
        // Setting the text content for the product details in the modal
        this.modalProductName.innerHTML = this.searchResults[this.itemSelected].Name;
        this.modalStoreName.innerHTML = this.searchResults[this.itemSelected].Store;
        this.modalModel.innerHTML = "Modelo: " + this.searchResults[this.itemSelected].Model;
        this.modalBrand.innerHTML = "Marca: " + this.searchResults[this.itemSelected].Brand;

        // Building the picture container with the product images
        this.pictureContainer.buildPictureContainer();

        // Populating the submissions table with entries
        for (const submission of this.searchResults[this.itemSelected].Submissions) {
            const row = this.submissionsTable.insertRow();

            // Inserting and formatting the date cell
            const dateCell = row.insertCell(0);
            dateCell.innerHTML = submission.EntryTime;
            dateCell.classList.add("text-center");

            // Inserting the price cell
            const priceCell = row.insertCell(1);
            priceCell.innerHTML = submission.Price;

            // Inserting the description cell
            const descriptionCell = row.insertCell(2);
            descriptionCell.innerHTML = submission.Description;
            
            // Inserting the rating cell
            const ratingCell = row.insertCell(3);
            this.submissionsRatings.push(new SearchResultsSubmissionRating(submission, ratingCell));
            this.submissionsRatings[this.submissionsRatings.length - 1].buildRating();
        }
    }

    /**
     * This method controls the navigation between different slides (images) in the modal.
     *
     * @param slideIndex The index of the slide to navigate to.
     */
    plusSlides(slideIndex) {
        // Delegating the slide navigation to the picture container's method
        this.pictureContainer.plusSlides(slideIndex);
    }
}
