/**
 * This class handles the pagination logic for a search results page. It controls the index of the currently viewed page and updates navigation buttons.
 */
export default class SearchResultsPageIndexComplex {
    /**
     * Constructs the pagination handler and binds the navigation buttons and input field from the DOM.
     */
    constructor() {
        this.currentPage = 0;
        this.totalPages = 0;

        // Binding DOM elements
        this.leftButton = document.getElementById("previousIndexButton");
        this.inputField = document.getElementById("indexPageInput");
        this.rightButton = document.getElementById("nextIndexButton");
    }

    /**
     * Called when the index page number is changed manually by the user. Validates and updates the current page number.
     */
    changeIndexPage() {
        let value = this.inputField.value;

        value = value - 1;

        this.currentPage = this.validateIndexValue(value);

        this.updatePageIndexComplex();
    }

    /**
     * Called when either the previous or next navigation button is pressed. Calculates the new page index, validates it, and updates the current page.
     *
     * @param change The amount to change the current page by. Can be negative or positive.
     */
    changeIndexButtonPressed(change) {
        let value = this.currentPage + change;

        value = this.validateIndexValue(value);

        this.currentPage = value;
        
        this.updatePageIndexComplex();
    }
    
    updateTotalPages(totalPages) {
        this.totalPages = totalPages;
    }

    /**
     * Updates the input field and the state of the navigation buttons based on the current page index.
     */
    updatePageIndexComplex() {
        this.inputField.value = this.currentPage + 1;

        // Disable the left button if on the first page, otherwise enable it
        if (this.currentPage === 0) {
            this.leftButton.classList.add("disabled");
        } else {
            this.leftButton.classList.remove("disabled");
        }
        
        // Disable the right button if on the last page, otherwise enable it
        if (this.currentPage === this.totalPages - 1) {
            this.rightButton.classList.add("disabled");
        } else {
            this.rightButton.classList.remove("disabled");
        }
    }

    /**
     * Validates that a given page index is within the range of available pages.
     *
     * @param value The page index to validate.
     * @return The corrected page index if out of range, or the original value if within range.
     */
    validateIndexValue(value) {
        if (value >= this.totalPages) {
            return this.totalPages - 1;
        }

        if (value < 0) {
            return 0;
        }

        return value;
    }
}
