class SearchResultsPageIndexComplex {
    constructor() {
        this.currentPage = 0;
        this.totalPages = 0;
        
        this.leftButton = document.getElementById("previousIndexButton");
        this.inputField = document.getElementById("indexPageInput");
        this.rightButton = document.getElementById("nextIndexButton");
    }

    changeIndexPage() {
        let value = this.inputField.value;

        value = value - 1;

        this.currentPage = this.validateIndexValue(value);
        
        this.updatePageIndexComplex();
    }

    changeIndexButtonPressed(change) {
        let value = this.currentPage + change;

        value = this.validateIndexValue(value);

        this.currentPage = value;
        
        this.updatePageIndexComplex();
    }

    updatePageIndexComplex() {
        this.inputField.value = this.currentPage + 1;
        
        if (this.currentPage === 0) {
            this.leftButton.classList.add("disabled");
        } else {
            this.leftButton.classList.remove("disabled");
        }

        if (this.currentPage === this.totalPages - 1) {
            this.rightButton.classList.add("disabled");
        } else {
            this.rightButton.classList.remove("disabled");
        }
    }

    validateIndexValue(value) {
        if (value > this.totalPages) {
            return this.totalPages;
        }

        if (value < 0) {
            return 0;
        }

        return value;
    }
}