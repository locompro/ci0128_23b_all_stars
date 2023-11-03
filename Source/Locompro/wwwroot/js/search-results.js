var searchResultsPage;

document.addEventListener("DOMContentLoaded", function() {
    let url = `SearchResults?handler=GetSearchResults`;

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(searchResultsData => {
            searchResultsPage = new SearchResultsPage(searchResultsData.SearchResults, searchResultsData.Data);
            searchResultsPage.populateTableWithResults();
        })
        .catch(() => {
            console.error('Error loading pictures');
        });
});

class SearchResultsPage {
    constructor(searchResults, pageData) {
        this.resultsPerPage = pageData.ResultsPerPage;
        this.pageNumberComplex = new SearchResultsPageIndexComplex();
        this.searchResults = searchResults;
        this.rawSearchResults = searchResults;
        this.itemSelected = 0;
        this.filters = new SearchResultsFilterMenu();
        this.currentOrder = {
            attribute: '',
            isDescending: false
        };
    }

    populateTableWithResults() {
        this.pageNumberComplex.updatePageIndexComplex();
        
        this.searchResults = this.filters.applyFilters(this.rawSearchResults);
        
        this.totalResults = this.searchResults.length;
        this.pageNumberComplex.totalPages = Math.floor(this.totalResults / this.resultsPerPage);

        let pageAmountDisplay = document.getElementById("pageAmountDisplay");
        pageAmountDisplay.innerHTML = this.pageNumberComplex.totalPages + " paginas de resultados";

        let resultsAmountDisplay = document.getElementById("resultsAmountDisplay");
        resultsAmountDisplay.innerHTML = this.totalResults + " resultados encontrados";
        
        new SearchResultsTableBody(this.searchResults, this.resultsPerPage, this.pageNumberComplex.currentPage);
    }
    
    setOrder(order) {
        if (this.currentOrder.attribute === order) {
            this.currentOrder.isDescending = !this.currentOrder.isDescending;
        }
        
        this.currentOrder.attribute = order;
        
        this.orderList();
        
        this.pageNumberComplex.currentPage = 0;
        
        this.pageNumberComplex.updatePageIndexComplex();
        
        this.updateTableHeaders();

        new SearchResultsTableBody(this.searchResults, this.resultsPerPage, this.pageNumberComplex.currentPage);
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
    
    setFilter(filterType, filterValue) {
        this.filters.setFilter(filterType, filterValue);
        this.populateTableWithResults();
    }

    changeIndexButtonPressed(change) {
        this.pageNumberComplex.changeIndexButtonPressed(change);
        new SearchResultsTableBody(this.searchResults, this.resultsPerPage, this.pageNumberComplex.currentPage);
    }

    changeIndexPage() {
        this.pageNumberComplex.changeIndexPage();
        new SearchResultsTableBody(this.searchResults, this.resultsPerPage, this.pageNumberComplex.currentPage);
    }
    
    populateModal() {
        new SearchResultsModal(this.searchResults, this.itemSelected);
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

function applyFilter(filterField) {
    const filterFieldId = filterField.id;
    let filterType = filterFieldId.replace(/Filter$/, '');
    const filterValue = filterField.value === "todos" ? null : filterField.value;
    
    if (filterType === "productName") {
        filterType = "Name";
    } else {
        filterType = filterType.charAt(0).toUpperCase() + filterType.slice(1);
    }
    
    searchResultsPage.setFilter(filterType, filterValue);
    filterField.value = filterValue ? filterValue: "todos";
}