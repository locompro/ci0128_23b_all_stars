import { TableBody, TableRowDefinition, TableRowColumn, Attribute} from '../Common/table-body.js';

/**
 * Represents the body of a table that displays search results.
 * It handles the addition and updating of table rows based on the search results.
 */
export default class SearchResultsTableBody extends TableBody {
    /**
     * Constructs the SearchResultsTableBody with a specified number of results per page.
     * @param resultsPerPage - The number of results to display per page.
     * @param selectItem - The function to call when an item is selected.
     * @param {*} resultsPerPage
     * @param {selectItem} selectItem
     */
    constructor(resultsPerPage, selectItem) {
        const attributes = [
            new Attribute("data-bs-toggle", "modal"),
            new Attribute("data-bs-target", "#ItemModal")
        ];
        
        const modalFunction = function (row, index) {
            if (selectItem === null || selectItem === undefined) {
                return;
            }
            
            row.addEventListener("click", function () {
                selectItem(index);
            });
        }
        
        let row = new TableRowDefinition(attributes, [], modalFunction);
        const columns = [
            new TableRowColumn('Name', []),
            new TableRowColumn('Price', ['prices-cell']),
            new TableRowColumn('Store', ['store-cell']),
            new TableRowColumn('Brand', []),
            new TableRowColumn('Model', []),
            new TableRowColumn('Province', []),
            new TableRowColumn('Canton', []),
            new TableRowColumn('Description', []),
            new TableRowColumn('LastSubmissionDate', [])
        ];
        
        super(row, columns, resultsPerPage);
    }
}