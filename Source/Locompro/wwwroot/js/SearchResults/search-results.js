var searchResultsPage;

document.addEventListener("DOMContentLoaded", function () {
    let url = `SearchResults?handler=GetSearchResults`;

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(searchResultsData => {
            if (searchResultsData.Redirect === 'redirect') {
                location.href = '/';
            }
            searchResultsPage = new SearchResultsPage(searchResultsData.SearchResults, searchResultsData.Data);
            searchResultsPage.populateTableWithResults();
        })
        .catch(() => {
        });
});

/**
 * This class represents the search results page and encapsulates the operations
 * associated with it such as initializing the page with data, populating the
 * table with results, setting the order of results, and handling item selection.
 */
class SearchResultsPage {
    /**
     * Constructs a new SearchResultsPage with specified search results and page data.
     *
     * @param searchResults The list of search results to be displayed.
     * @param pageData      The data related to the pagination and display of search results.
     */
    constructor(searchResults, pageData) {
        this.pageSearchData = pageData;
        this.resultsPerPage = pageData.ResultsPerPage;
        this.searchResults = searchResults;
        this.rawSearchResults = searchResults;

        this.itemSelected = 0;
        this.filters = new SearchResultsFilterMenu();
        this.currentOrder = {
            attribute: '',
            isDescending: false
        };

        this.currentModal = null;
        this.resultsTable = new SearchResultsTableBody(this.resultsPerPage);
        this.pageNumberComplex = new SearchResultsPageIndexComplex();

        this.requestSent = false;
    }

    /**
     * Populates the table with search results after applying any set filters and
     * sorting the results.
     */
    populateTableWithResults() {
        this.pageNumberComplex.updatePageIndexComplex();

        this.searchResults = this.filters.applyFilters(this.rawSearchResults);

        this.totalResults = this.searchResults.length;
        this.pageNumberComplex.totalPages = Math.ceil(this.totalResults / this.resultsPerPage);

        let pageAmountDisplay = document.getElementById("pageAmountDisplay");
        pageAmountDisplay.innerHTML = this.pageNumberComplex.totalPages + " paginas de resultados";

        let resultsAmountDisplay = document.getElementById("resultsAmountDisplay");
        resultsAmountDisplay.innerHTML = this.totalResults + " resultados encontrados";

        this.resultsTable.populateTableBody(this.searchResults, this.pageNumberComplex.currentPage);
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

        this.resultsTable.populateTableBody(this.searchResults, this.pageNumberComplex.currentPage);

        this.pageNumberComplex.currentPage = 0;
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

        this.searchResults.sort((a, b) => {
            if (a[attribute] < b[attribute]) return -1 * direction;
            if (a[attribute] > b[attribute]) return 1 * direction;
            return 0;
        });
    }

    /**
     * Updates the visual indicators (e.g., arrows) on the table headers to reflect
     * the current sorting state.
     */
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

    /**
     * Selects an item from the search results and displays its details in a modal
     * dialog.
     *
     * @param index The index of the selected item in the search results.
     */
    selectItem(index) {
        this.itemSelected = index;
        this.currentModal = new SearchResultsModal(this.searchResults, this.itemSelected);
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
        this.populateTableWithResults();
    }

    /**
     * Changes the current page in the pagination based on the user's button
     * interaction.
     *
     * @param change The direction and amount to change the current page index.
     */
    changeIndexButtonPressed(change) {
        this.pageNumberComplex.changeIndexButtonPressed(change);
        this.resultsTable.populateTableBody(this.searchResults, this.pageNumberComplex.currentPage);
    }

    /**
     * Updates the pagination index page display.
     */
    changeIndexPage() {
        this.pageNumberComplex.changeIndexPage();
        this.resultsTable.populateTableBody(this.searchResults, this.pageNumberComplex.currentPage);
    }

    /**
     * Updates the pagination index page display.
     */
    plusSlides(slideIndex) {
        this.currentModal.plusSlides(slideIndex);
    }
}

/**
 * Changes the current page of search results when a pagination button is pressed.
 * It updates the view to the new page of results.
 *
 * @param change - The number indicating how many pages to move forward or backward.
 */
function changeIndexButtonPressed(change) {
    searchResultsPage.changeIndexButtonPressed(change);
}

/**
 * Changes the current page to the page number entered in the pagination input field.
 * It updates the view to the new page of results.
 *
 * @param field - The page number to navigate to.
 */
function changeIndexPage(field) {
    searchResultsPage.changeIndexPage(field);
}

/**
 * Sets the order of the search results based on the selected attribute. It toggles
 * between ascending and descending order for the attribute.
 *
 * @param order - The attribute name to sort by.
 */
function setOrder(order) {
    searchResultsPage.setOrder(order);
}

/**
 * Selects an item from the search results to display more information. It triggers
 * the modal to open and show the item details.
 *
 * @param index - The index of the selected item in the search results array.
 */
function selectItem(index) {
    searchResultsPage.selectItem(index);
}

/**
 * Clears all classes from the given DOM element. It is typically used to reset the
 * state of a UI element before applying new classes.
 *
 * @param element - The DOM element to clear classes from.
 */
function emptyClassList(element) {
    while (element.classList.length > 0) {
        element.classList.remove(element.classList[0]);
    }
}

/**
 * Navigates through the slides in the modal dialog when viewing detailed information
 * about a search result item.
 *
 * @param slideIndex - The index indicating the next slide to navigate to.
 */
function plusSlides(slideIndex) {
    searchResultsPage.plusSlides(slideIndex);
}

/*
 * Clears all filters from the filter menu.
 */
function clearFilters() {
    searchResultsPage.filters.clearFilters();
    searchResultsPage.populateTableWithResults();
}

const modal = document.getElementById('ItemModal');

/**
 * Applies a filter to the search results based on the selected value from a filter field.
 * It updates the view to only display the results that match the filter criteria.
 *
 * @param filterField - The input element that contains the filter value.
 */
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
    filterField.value = filterValue ? filterValue : "todos";
}

window.addEventListener('beforeunload', function (e) {
    if (searchResultsPage.requestSent) {
        searchResultsPage.requestSent = false;
        e.returnValue = '';
        return;
    }

    let url = window.location.pathname;
    let handler = '?handler=ReturnResults';
    let location = url + handler;

    let data = searchResultsPage.pageSearchData;
    
    //console.log(data);
    //alert("data: "  +  data);
    //e.preventDefault();

    fetch(location, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok.');
            }
        });
    
    e.returnValue = '';
});

// Inside your document ready function, when setting up the form submission listener:
document.addEventListener('DOMContentLoaded', function () {
    const reportForm = document.getElementById('reportForm');
    reportForm.addEventListener('submit', function (event) {
        event.preventDefault(); // Prevent the default form submit

        // Get the submission ID from the form (assuming you set it somewhere on click before form submit)
        const submissionUserId = reportForm.querySelector('input[name="ReportVm.SubmissionUserId"]').value;
        const submissionEntryTime = reportForm.querySelector('input[name="ReportVm.SubmissionEntryTime"]').value;
        const submissionId = submissionUserId + submissionEntryTime;

        // Send the form data using fetch API
        fetch(reportForm.action, {
            method: 'POST',
            body: new FormData(reportForm),
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok.');
                }
                return response.json();
            })
            .then(data => {
                if (data.redirectUrl) {
                    // If a redirect URL is provided, redirect the page
                    window.location.href = data.redirectUrl;
                } else {
                    // Find the button by the submission ID and disable it
                    const reportButtonToDisable = document.querySelector(`button[data-id="${submissionId}"]`);
                    if (reportButtonToDisable) {
                        reportButtonToDisable.disabled = true;
                    }

                    // Display a success message or update the UI as necessary
                    $('#descriptionModal').modal('hide');
                }
            })
            .catch(error => {
                console.error('There has been a problem with your fetch operation:', error);
                // Handle any errors
            });
    });
});