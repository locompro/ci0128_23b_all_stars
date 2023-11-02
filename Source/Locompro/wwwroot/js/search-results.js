var searchResultsPage;

document.addEventListener("DOMContentLoaded", function() {
    let url = `SearchResults?handler=GetSearchResults`;

    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json',
        success: function (searchResultsData) {
            searchResultsPage =
                new SearchResultsPage(searchResultsData.searchResults, searchResultsData.data);
            searchResultsPage.populateTableWithResults()
        },
        error: function () {
            console.error('Error loading pictures');
        }
    });
});


class SearchResultsPage {
    constructor(searchResults, pageData) {
        this.resultsPerPage = pageData.ResultsPerPage;
        this.currentPage = 0;
        this.totalResults = searchResults.length;
        this.totalPages = Math.floor(this.totalResults / this.resultsPerPage);
        this.searchResults = searchResults;
        this.itemSelected = 0;
        this.currentOrder = {
            attribute: '',
            isDescending: false
        };
        
        let pageAmountDisplay = document.getElementById("pageAmountDisplay");
        pageAmountDisplay.innerHTML = this.totalPages + " paginas de resultados";
        
        let resultsAmountDisplay = document.getElementById("resultsAmountDisplay");
        resultsAmountDisplay.innerHTML = this.totalResults + " resultados encontrados";
    }

    populateTableWithResults() {
        this.updatePageIndexComplex();
        
        var tableBody = document.getElementById("resultsTableBody");
        
        tableBody.innerHTML = "";

        for (let resultIndex = this.currentPage * this.resultsPerPage;
             resultIndex < this.searchResults.length && resultIndex < (this.currentPage + 1) * this.resultsPerPage;
             resultIndex++) {
            var item = this.searchResults[resultIndex];
            var row = tableBody.insertRow();
            row.setAttribute("data-bs-toggle", "modal");
            row.setAttribute("data-bs-target", "#ItemModal");
            
            row.addEventListener("click", function() {
                selectItem(resultIndex);
            });
            
            this.populateRow(item, row);
        }
    }
    
    populateRow(item, row) {
        var productNameCell = row.insertCell(0);
        productNameCell.innerHTML = item.Name;

        var priceCell = row.insertCell(1);
        priceCell.innerHTML = item.Price;
        priceCell.classList.add("prices-cell");

        var storeNameCell = row.insertCell(2);
        storeNameCell.innerHTML = item.Store;
        storeNameCell.classList.add("store-cell");

        var modelCell = row.insertCell(3);
        modelCell.innerHTML = item.Model;

        var provinceCell = row.insertCell(4);
        provinceCell.innerHTML = item.Province;

        var cantonCell = row.insertCell(5);
        cantonCell.innerHTML = item.Canton;

        var descriptionCell = row.insertCell(6);
        descriptionCell.innerHTML = item.Description;

        var submissionTimeCell = row.insertCell(7);
        submissionTimeCell.innerHTML = item.LastSubmissionDate;
    }

    changeIndexPage(field) {
        let value = field.value;
        
        value = value - 1;
        
        this.currentPage = this.validateIndexValue(value);

        this.populateTableWithResults();
    }
    
    changeIndexButtonPressed(change) {
        let value = this.currentPage + change;
        
        value = this.validateIndexValue(value);
        
        this.currentPage = value;
        
        this.populateTableWithResults();
    }
    
    updatePageIndexComplex() {
        let indexField = document.getElementById("indexPageInput");
        indexField.value = this.currentPage + 1;
        
        let previousButton = document.getElementById("previousIndexButton");
        let nextButton = document.getElementById("nextIndexButton");
        
        if (this.currentPage === 0) {
            previousButton.classList.add("disabled");
        } else {
            previousButton.classList.remove("disabled");
        }
        
        if (this.currentPage === this.totalPages - 1) {
            nextButton.classList.add("disabled");
        } else {
            nextButton.classList.remove("disabled");
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
    
    setOrder(order) {
        if (this.currentOrder.attribute === order) {
            this.currentOrder.isDescending = !this.currentOrder.isDescending;
        }
        
        this.currentOrder.attribute = order;
        
        this.orderList();
        
        this.currentPage = 0;
        
        this.updatePageIndexComplex();
        
        this.updateTableHeaders();
        
        this.populateTableWithResults();
    }
    
    orderList() {
        const attribute = this.currentOrder.attribute;
        const direction = this.currentOrder.isDescending ? -1 : 1;
        
        this.searchResults.sort((a, b) => {
            if (a[attribute] < b[attribute]) return -1 * direction;
            if (a[attribute] > b[attribute]) return 1 * direction;
            return 0;
        });
    }

    updateTableHeaders() {
        let productNameHeader = document.getElementById("nameSortButton");
        let provinceHeader = document.getElementById("provinceSortButton");
        let cantonHeader = document.getElementById("cantonSortButton");
        
        emptyClassList(productNameHeader);
        emptyClassList(provinceHeader);
        emptyClassList(cantonHeader);
        
        productNameHeader.classList.add("no-underline-link");
        provinceHeader.classList.add("no-underline-link");
        cantonHeader.classList.add("no-underline-link");
        
        switch (this.currentOrder.attribute) {
            case "Name":
                productNameHeader.classList.add(this.currentOrder.isDescending ? "sort-desc" : "sort-asc");
                break;
            case "Province":
                provinceHeader.classList.add(this.currentOrder.isDescending ? "sort-desc" : "sort-asc");
                break;
            case "Canton":
                cantonHeader.classList.add(this.currentOrder.isDescending ? "sort-desc" : "sort-asc");
                break;               
        }
    }
    
    selectItem(index) {
        this.itemSelected = index;
    }
    
    populateModal() {
        const modalProductName = document.getElementById("modalProductName");
        const modalStoreName = document.getElementById("modalStoreName");
        
        modalProductName.innerHTML = this.searchResults[this.itemSelected].Name;
        modalStoreName.innerHTML = this.searchResults[this.itemSelected].Store;
        
        const modalModel = document.getElementById("modalModel");
        const modalBrand = document.getElementById("modalBrand");
        
        modalModel.innerHTML = "Modelo: " + this.searchResults[this.itemSelected].Model;
        modalBrand.innerHTML = "Marca: " + this.searchResults[this.itemSelected].Brand;
        
        const pictureContainer = document.getElementById("picturesContainer");

        loadPictures(pictureContainer, this.searchResults[this.itemSelected].Name, this.searchResults[this.itemSelected].Store);
        
        const submissionTable = document.getElementById("ItemModalSubmissionsTable");
        
        for(const submission of this.searchResults[this.itemSelected].Submissions) {
            const row = submissionTable.insertRow();
            
            const dateCell = row.insertCell(0);
            dateCell.innerHTML = submission.DateTime;
            dateCell.classList.add("text-center");
            
            const priceCell = row.insertCell(1);
            priceCell.innerHTML = submission.Price;
            
            const descriptionCell = row.insertCell(2);
            descriptionCell.innerHTML = submission.Description;
        }
    }
}

function changeIndexButtonPressed(change) {
    searchResultsPage.changeIndexButtonPressed(change);    
}

function changeIndexPage(field) {
    searchResultsPage.changeIndexPage(field);
}

function setOrder(order) {
    searchResultsPage.setOrder(order);
}


function selectItem(index) {
    searchResultsPage.selectItem(index);

}

function emptyClassList(element) {
    while (element.classList.length > 0) {
        element.classList.remove(element.classList[0]);
    }
}

const modal = document.getElementById('ItemModal');

// Listen for the modal 'shown.bs.modal' event
modal.addEventListener('shown.bs.modal', function () {
    searchResultsPage.populateModal();
});

