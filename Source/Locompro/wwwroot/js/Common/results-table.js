import {SearchResultsFilterMenu, FilterField} from "../SearchResults/search-results-filter-menu.js";
import SearchResultsPageIndexComplex from "./search-results-page-index-complex.js";
import {TableHead} from "./table-head.js";

class ResultsTable {
    constructor(tableBody, tableData, pageData, pageConfiguration) {
        this.tableHead = new TableHead(pageConfiguration);
        this.table = pageConfiguration.table;
        this.tableBody = tableBody;
        this.tableBody.buildTableBody();
        
        this.tableData = [];
        this.pageData = pageData;
        this.resultsPerPage = pageData.ResultsPerPage;
        
        this.rawTableData = tableData;
        this.pageConfiguration = pageConfiguration;
        
        this.itemSelected = 0;
        this.filters = new SearchResultsFilterMenu();
        this.currentOrder = {
            attribute: '',
            isDescending: false
        }
        
        this.currentModal = null;
        
        this.pageNumberComplex = new SearchResultsPageIndexComplex();
    }

    /**
     * Populates the table with search results after applying any set filters and
     * sorting the results.
     */
    populateTable() {
        this.tableData = this.filters.applyFilters(this.rawTableData);
        this.totalResults = this.tableData.length;
        
        this.pageNumberComplex.updateTotalPages(Math.ceil(this.totalResults / this.resultsPerPage));
        this.pageNumberComplex.updatePageIndexComplex();
        
        this.pageConfiguration.pageAmountDisplay.innerHTML = this.pageNumberComplex.totalPages  + " paginas de resultados";
        this.pageConfiguration.resultsAmountDisplay.innerHTML = this.totalResults + " resultados encontrados";
        
        this.tableBody.populateTableBody(this.tableData, this.pageNumberComplex.currentPage);
    }

    /**
     * Sets the sorting order of the search results based on the specified attribute.
     * If the attribute is the same as the current, the method will toggle the sort
     * direction between ascending and descending.
     *
     * @param order The attribute on which to sort the results.
     */
    setOrder(order) {
        if (this.currentOrder.attribute === order) {
            this.currentOrder.isDescending = !this.currentOrder.isDescending;
        }
        
        this.orderList(order);
        this.updateTableHeaders();

        this.tableBody.populateTableBody(this.tableData, this.pageNumberComplex.currentPage);

        this.pageNumberComplex.currentPage = 0;
        console.log("updating page index complex" + this.pageNumberComplex.currentPage);
        this.pageNumberComplex.updatePageIndexComplex();
    }

    /**
     * Orders the list of search results based on the current sorting attribute and
     * direction specified in the currentOrder object.
     *
     * @param order The attribute on which to sort the results.
     */
    orderList(order) {
        this.currentOrder.attribute = order;
        const attribute = this.currentOrder.attribute;
        const direction = this.currentOrder.isDescending ? -1 : 1;
        
        this.tableData.sort((a, b) => {
            if (a[attribute] < b[attribute]) return -1 * direction;
            if (a[attribute] > b[attribute]) return 1 * direction;
            return 0;
        });
    }

    /**
     * Updates the table headers to indicate the current sorting attribute and direction.
     * The current sorting attribute is indicated by a down arrow if the results are
     * sorted in ascending order and an up arrow if the results are sorted in descending
     */
    updateTableHeaders() {
        this.tableHead.updateHead(this.currentOrder);
    }
    
    /**
     * Applies a filter to the search results based on the type and value of the
     * filter, then repopulates the table with the filtered results.
     *
     * @param filterType  The type of filter to apply.
     * @param filterValue The value of the filter to apply.
     */
    setFilter(filterType, filterValue) {
        this.filters.setFilter(filterType, filterValue);
        this.populateTable();
    }

    /**
     * Changes the current page in the pagination based on the user's button
     * interaction.
     *
     * @param change The direction and amount to change the current page index.
     */
    changeIndexButtonPressed(change) {
        this.pageNumberComplex.changeIndexButtonPressed(change);
        this.tableBody.populateTableBody(this.tableData, this.pageNumberComplex.currentPage);
    }

    /**
     * Updates the pagination index page display.
     */
    changeIndexPage() {
        this.pageNumberComplex.changeIndexPage();
        this.tableBody.populateTableBody(this.tableData, this.pageNumberComplex.currentPage);
    }

    /**
     * Updates the pagination index page display.
     */
    plusSlides(slideIndex) {
        this.currentModal.plusSlides(slideIndex);
    }
    
    clearFilters() {
        this.filters.clearFilters();
        this.populateTable();
    }
    
    applyFilter(filterField) {
        const filterFieldId = filterField.id;
        let filterType = filterFieldId.replace(/Filter$/, '');
        const filterValue = filterField.value === "Todos" ? null : filterField.value;

        if (filterType === "productName") {
            filterType = "Name";
        } else {
            filterType = filterType.charAt(0).toUpperCase() + filterType.slice(1);
        }

        this.setFilter(filterType, filterValue);
        filterField.value = filterValue ? filterValue : "Todos";
    }
}

class ResultsPageConfiguration {
    constructor(tableName, headerFields, pageAmountDisplayId, resultsAmountDisplay) {
        this.table = document.getElementById(tableName);
        this.headerFields = headerFields;
        this.pageAmountDisplay = document.getElementById(pageAmountDisplayId);
        this.resultsAmountDisplay = document.getElementById(resultsAmountDisplay);
        this.headerOrderingElements = [];
        
        for (const headerField of this.headerFields) {
            if (!headerField.isOrderable) {
                continue;
            }
            this.headerOrderingElements.push(new OrderingField(headerField.id, headerField.attribute));
        }
    }
}

class OrderingField {
    constructor(id, attribute) {
        this.elementId = id;
        this.attribute = attribute;
    }
    
    add(attribute) {
        let element = document.getElementById(this.elementId);
        element.classList.add(attribute);
    }
    
    remove(attribute) {
        let element = document.getElementById(this.elementId);
        element.classList.remove(attribute);
    }
}

export {ResultsTable, ResultsPageConfiguration, OrderingField};