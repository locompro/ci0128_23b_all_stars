/**
 * Represents the body of a table that displays search results.
 * It handles the addition and updating of table rows based on the search results.
 */
class TableBody {
    /**
     * Constructs the SearchResultsTableBody with a specified number of results per page.
     * @param resultsPerPage - The number of results to display per page.
     * @param rowDefinition
     * @param columns
     */
    constructor(rowDefinition, columns, resultsPerPage) {
        this.table = null;
        this.rowDefinition = rowDefinition;
        this.columns = columns;
        this.resultsPerPage = resultsPerPage;
        this.searchResults = [];
        this.currentPage = [];
    }
    
    buildTableBody(table) {
        this.table = table;
        this.tableBody = document.createElement("tbody");
        this.tableBody.innerHTML = "";
        this.tableBody.id = "searchResultsTableBody";
        this.table.append(this.tableBody);
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
            this.rowDefinition.createRow(this.tableBody, resultIndex, item, this.columns);
        }
    }
}

class TableRowDefinition {
    constructor(attributes, classes, action) {
        this.attributes = attributes;
        this.classes = classes;
        this.action = action;
    }
    
    createRow(body, index, item, columns) {
        let row = body.insertRow();
        
        if (this.attributes.length > 0) {
            for (const attribute of this.attributes) {
                row.setAttribute(attribute.name, attribute.value);
            }
        }
        
        if (this.classes.length > 0) {
            row.classList.add(...this.classes);
        }
        
        this.action(row, index);
        
        this.populateRow(item, row, columns);
    }

    /**
     * Populates a row with data from a search result item.
     * @param item - The search result item containing data to display.
     * @param row - The table row to be populated.
     * @param columns
     */
    populateRow(item, row, columns) {
        let columnIndex = 0;

        // for each defined column
        for (const column of columns) {
            column.createColumn(row, item, columnIndex++);
        }
    }
}

class TableRowColumn {
    constructor(name, classes, optionalInnerContent = null) {
        this.name = name;
        this.classes = classes;
        this.optionalInnerContent = optionalInnerContent;
        this.rowColumn = null;
        this.textDiv = null;
    }
    
    createColumn(row, item, columnIndex) {
        let newColumn = row.insertCell(columnIndex);
        this.rowColumn = newColumn;

        // insert the item's data into the column
        this.textDiv = document.createElement("div");
        
        let textToAdd = "";
        
        if (typeof item[this.name] === "object") {
            for (let element = 0; element < item[this.name].length; element++) {
                textToAdd += item[this.name][element];
                if (element < item[this.name].length - 1) {
                    textToAdd += ", ";
                }
            }
        } else {
            textToAdd += item[this.name];
        }
        
        this.textDiv.innerHTML = textToAdd;
        
        newColumn.appendChild(this.textDiv);
        
        // if column has any classes add any classes defined for the column
        if (this.classes.length > 0) {
            this.textDiv.classList.add(...this.classes);
        }
        
        if (this.optionalInnerContent !== null) {
            this.optionalInnerContent = this.optionalInnerContent.cloneNode(true);
            
            // if the inner content has any children, find the inner most child
            let innerContent = this.optionalInnerContent;
            while (innerContent.children.length > 0) {
                innerContent = innerContent.children[0];
            }
            
            innerContent.innerHTML = textToAdd;
            
            newColumn.appendChild(this.optionalInnerContent);
            
            this.addMakeInnerHiddenContentVisibleListener(innerContent);
        }
    }
    
    
    addMakeInnerHiddenContentVisibleListener(hiddenInnerContent) {
        this.rowColumn.addEventListener("mouseover", function () { 
            hiddenInnerContent.classList.toggle("visible");
        });
        
        this.rowColumn.addEventListener("mouseout", function () { 
            hiddenInnerContent.classList.toggle("visible");
        });
    }
}

class Attribute {
    constructor(name, value) {
        this.name = name;
        this.value = value;
    }
}

export { TableBody, TableRowDefinition, TableRowColumn, Attribute };