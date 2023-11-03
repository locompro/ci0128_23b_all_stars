class SearchResultsTableBody {
    constructor(searchResults, resultsPerPage, currentPage) {
        this.tableBody = document.getElementById("resultsTableBody");
        this.tableBody.innerHTML = "";
        this.searchResults = searchResults;
        this.resultsPerPage = resultsPerPage;
        this.currentPage = currentPage;
        
        this.populateTableBody();
    }
    
    populateTableBody() {
        for (let resultIndex = this.currentPage * this.resultsPerPage;
             resultIndex < this.searchResults.length && resultIndex < (this.currentPage + 1) * this.resultsPerPage;
             resultIndex++) {
            var item = this.searchResults[resultIndex];
            var row = this.addRow(resultIndex);

            this.populateRow(item, row);
        }
    }

    addRow(resultIndex) {
        let row = this.tableBody.insertRow();
        row.setAttribute("data-bs-toggle", "modal");
        row.setAttribute("data-bs-target", "#ItemModal");

        row.addEventListener("click", function() {
            selectItem(resultIndex);
        });
        
        return row;
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
}