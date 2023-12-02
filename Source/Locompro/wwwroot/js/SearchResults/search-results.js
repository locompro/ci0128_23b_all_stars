import SearchResultsTableBody from './search-results-table.js';
import {HeaderField} from "../Common/table-head.js";
import {ResultsTable, ResultsPageConfiguration} from '../Common/results-table.js';


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
            searchResultsPage = new SearchResultsTable(searchResultsData.SearchResults, searchResultsData.Data);
            searchResultsPage.populateTable();
        })
        .catch(() => {
        });
});

window.addEventListener('beforeunload', function (e) {
    /*alert("leaving page!!!!");
    if (searchResultsPage.requestSent) {
        searchResultsPage.requestSent = false;
        e.returnValue = '';
        return;
    }*/
    /*
    let url = window.location.pathname;
    let handler = '?handler=ReturnResults';
    let location = url + handler;

    let data = searchResultsPage.pageSearchData;

    fetch(location, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(data)
    })
        .then(response => {

            alert("leaving page");
            if (!response.ok) {
                throw new Error('Network response was not ok.');
            }
        });*/

    e.returnValue = '';
});

/**
 * This class represents the search results page and encapsulates the operations
 * associated with it such as initializing the page with data, populating the
 * table with results, setting the order of results, and handling item selection.
 */
class SearchResultsTable extends ResultsTable {
    /**
     * Constructs a new SearchResultsPage with specified search results and page data.
     *
     * @param searchResults The list of search results to be displayed.
     * @param pageData      The data related to the pagination and display of search results.
     */
    constructor(searchResults, pageData) {
        const storeField = new HeaderField("Tienda", true, 'Store');
        storeField.addStyle("paddingLeft", "30px");
        
        const headerFields = [
            new HeaderField("Producto", true, 'Name'),
            new HeaderField("Precio", true, 'FormattedPrice'),
            new HeaderField("Categorías", true, 'Categories'),
            storeField,
            new HeaderField("Marca", true, 'Brand'),
            new HeaderField("Modelo", true, 'Model'),
            new HeaderField("Provincia", true, 'Province'),
            new HeaderField("Cantón", true, 'Canton'),
            new HeaderField("Descripción", false, 'Description'),
            new HeaderField("Último aporte", true, 'LastSubmissionDate')
        ];
        
        const pageConfiguration
            = new ResultsPageConfiguration(
            "resultsCard",
            headerFields);

        let selectItem = (index) => {
            this.selectItem(index);
        };
        
        const tableBody =
            new SearchResultsTableBody(pageData.ResultsPerPage, selectItem);
        super(tableBody, searchResults, pageData, pageConfiguration);
    }

    /**
     * Selects an item from the search results and displays its details in a modal
     * dialog.
     *
     * @param index The index of the selected item in the search results.
     */
    selectItem(index) {
        this.itemSelected = index;
        this.currentModal = createSearchResultsModalInstance(this.tableData, this.itemSelected);
    }
}

window.getPage = () => {
    return searchResultsPage;
}

window.modal = document.getElementById('ItemModal');



// Inside your document ready function, when setting up the form submission listener:
document.addEventListener('DOMContentLoaded', function () {
    const reportForm = document.getElementById('reportForm');
    reportForm.addEventListener('submit', function (event) {
        event.preventDefault(); // Prevent the default form submit

        // Get the submission ID from the form (assuming you set it somewhere on click before form submit)
        const submissionUserId = reportForm.querySelector('input[name="UserReportVm.SubmissionUserId"]').value;
        const submissionEntryTime = reportForm.querySelector('input[name="UserReportVm.SubmissionEntryTime"]').value;
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

async function createSearchResultsModalInstance(searchResults, itemSelected) {
    try {
        const response = await fetch("SearchResults?handler=GetUsersReportedSubmissions");
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        const usersReportedSubmissions = await response.json();
        return new SearchResultsModal(searchResults, itemSelected, usersReportedSubmissions);
    } catch (error) {
        console.error('Failed to obtain user reported submissions', error);
        return new SearchResultsModal(searchResults, itemSelected);
    }
}

// export {changeIndexButtonPressed, changeIndexPage, setOrder, selectItem, applyFilter, plusSlides, clearFilters};