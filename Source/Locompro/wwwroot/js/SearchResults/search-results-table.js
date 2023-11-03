/**
 * Represents the body of a table that displays search results.
 * It handles the addition and updating of table rows based on the search results.
 */
class SearchResultsTableBody {
    /**
     * Constructs the SearchResultsTableBody with a specified number of results per page.
     * @param resultsPerPage - The number of results to display per page.
     */
    constructor(resultsPerPage) {
        this.tableBody = document.getElementById("resultsTableBody");
        this.tableBody.innerHTML = "";
        this.searchResults = [];
        this.resultsPerPage = resultsPerPage;
        this.currentPage = [];
    }

    /**
     * Populates the table body with the given search results for the specified page.
     * @param searchResults - Array of search result items to be displayed.
     * @param currentPage - The current page number in the pagination.
     */
    populateTableBody(searchResults, currentPage) {
        this.tableBody.innerHTML = "";
        this.searchResults = searchResults;
        this.currentPage = currentPage;
        
        for (let resultIndex = this.currentPage * this.resultsPerPage;
             resultIndex < this.searchResults.length && resultIndex < (this.currentPage + 1) * this.resultsPerPage;
             resultIndex++) {
            let item = this.searchResults[resultIndex];
            let row = this.addRow(resultIndex);

            this.populateRow(item, row);
        }
    }

    /**
     * Adds a row to the table body for a specific result item.
     * @param resultIndex - The index of the result item in the search results array.
     * @returns {HTMLElement} - The newly created table row element.
     */
    addRow(resultIndex) {
        let row = this.tableBody.insertRow();
        row.setAttribute("data-bs-toggle", "modal");
        row.setAttribute("data-bs-target", "#ItemModal");

        row.addEventListener("click", function() {
            selectItem(resultIndex);
        });
        
        return row;
    }

    /**
     * Populates a row with data from a search result item.
     * @param item - The search result item containing data to display.
     * @param row - The table row to be populated.
     */
    populateRow(item, row) {
        let productNameCell = row.insertCell(0);
        productNameCell.innerHTML = item.Name;

        let priceCell = row.insertCell(1);
        priceCell.innerHTML = item.Price;
        priceCell.classList.add("prices-cell");

        let storeNameCell = row.insertCell(2);
        storeNameCell.innerHTML = item.Store;
        storeNameCell.classList.add("store-cell");

        let modelCell = row.insertCell(3);
        modelCell.innerHTML = item.Model;

        let provinceCell = row.insertCell(4);
        provinceCell.innerHTML = item.Province;

        let cantonCell = row.insertCell(5);
        cantonCell.innerHTML = item.Canton;

        let descriptionCell = row.insertCell(6);
        descriptionCell.innerHTML = item.Description;

        let submissionTimeCell = row.insertCell(7);
        submissionTimeCell.innerHTML = item.LastSubmissionDate;
    }
    
}