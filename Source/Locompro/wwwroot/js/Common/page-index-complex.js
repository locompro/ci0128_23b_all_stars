/**
 * This class handles the pagination logic for a search results page. It controls the index of the currently viewed page and updates navigation buttons.
 */
export default class PageIndexComplex {
    /**
     * Constructs the pagination handler and binds the navigation buttons and input field from the DOM.
     */
    constructor(pageChangeFunction) {
        this.pageChangeFunction = pageChangeFunction;
        this.currentPage = 0;
        this.totalPages = 0;
        
        this.complexElement = this.setUpComplexElement();
    }
    
    setUpComplexElement() {
        const complexElement = document.createElement("div");
        complexElement.classList.add("previous-next-button-area");
        
        const previousButton = this.createLeftButton();
        complexElement.appendChild(previousButton);
        this.leftButton = previousButton;

        const indexInput = this.createInputField();
        complexElement.appendChild(indexInput);
        this.inputField = indexInput;
        
        const nextButton = this.createRightButton();
        complexElement.appendChild(nextButton);
        this.rightButton = nextButton;
        
        const pageAmountDisplay = document.createElement("h6");
        pageAmountDisplay.classList.add("page-amount-text");
        pageAmountDisplay.id = "pageAmountDisplay";
        complexElement.appendChild(pageAmountDisplay);
        this.pageAmountDisplay = pageAmountDisplay;
        
        return complexElement;
    }
    
    createLeftButton() {
        const previousButton = document.createElement("button");
        previousButton.classList.add("btn");
        previousButton.classList.add("btn-primary");
        previousButton.onclick = () => this.changeIndexButtonPressed(-1);
        previousButton.id = "previousIndexButton";
        previousButton.innerHTML = "Previo";
        previousButton.type = "button";
        
        return previousButton;
    }
    
    createInputField() {
        const indexInput = document.createElement("input");
        indexInput.classList.add("number-input-pages");
        indexInput.id = "indexPageInput";
        indexInput.type = "number";
        indexInput.onchange = () => this.changeIndexPage(indexInput);
        
        return indexInput;
    }
    
    createRightButton() {
        const nextButton = document.createElement("button");
        nextButton.classList.add("btn");
        nextButton.classList.add("btn-primary");
        nextButton.onclick = () => this.changeIndexButtonPressed(1);
        nextButton.id = "nextIndexButton";
        nextButton.innerHTML = "Siguiente";
        nextButton.type = "button";
        
        return nextButton;
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
        if (this.currentPage >= this.totalPages - 1) {
            this.rightButton.classList.add("disabled");
        } else {
            this.rightButton.classList.remove("disabled");
        }
        
        this.pageAmountDisplay.innerHTML = this.totalPages  + " paginas de resultados";
        
        this.pageChangeFunction();
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
