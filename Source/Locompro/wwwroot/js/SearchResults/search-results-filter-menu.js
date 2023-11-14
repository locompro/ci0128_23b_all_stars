/**
 * Represents a menu for filtering search results. It can contain various filter fields and manage
 * both the current active filters and the possible options for each filter based on the search results.
 */
class SearchResultsFilterMenu {
    /**
     * Constructs a new SearchResultsFilterMenu and initializes the filters and currentFilters maps.
     * It also sets up the filter fields.
     */
    constructor() {
        this.filters = new Map();
        this.currentFilters = new Map();
        this.filterFields = this.setUpFields();
    }

    /**
     * Initializes the filter fields for the menu. Each field corresponds to a type of filter
     * such as product names, provinces, etc.
     *
     * @return An array of FilterField objects, each representing a filterable property.
     */
    setUpFields() {
        return [
            new FilterField("provinceFilter", "Provinces"),
            new FilterField("cantonFilter", "Cantons"),
            new FilterField("minPriceFilter", "MinPrice"),
            new FilterField("maxPriceFilter", "MaxPrice"),
            new FilterField("brandFilter", "Brands"),
            new FilterField("modelFilter", "Models")
        ];
    }

    /**
     * Populates the filter options based on the available filters and the currently applied filters.
     * It dynamically creates and adds options to the filter select elements in the UI.
     */
    populateFilters() {
        const categoryFilter = document.getElementById("categoryFilter");
        categoryFilter.innerHTML = "";

        for (let filterIndex = 0; filterIndex < this.filterFields.length; filterIndex++) {
            let filter = this.filterFields[filterIndex];
            filter.addOptions(this.currentFilters, this.filters);
        }

        if (this.filters.has("Categories")) {
            categoryFilter.innerHTML = "";

            const todosOption = document.createElement("option");
            todosOption.value = "Todos";
            todosOption.innerHTML = "Todos";

            categoryFilter.appendChild(todosOption);
            for (const category of this.filters.get("Categories")) {
                const option = document.createElement("option");
                option.value = category;
                option.innerHTML = category;

                categoryFilter.appendChild(option);
            }

            if (!this.filters.get("Category")) {
                return;
            }

            categoryFilter.value = filters.get("Category");
        }
    }

    /**
     * Applies the set filters to the raw search results, filtering out items that do not match
     * the filter criteria.
     *
     * @param rawSearchResults The unfiltered search results that need to be filtered.
     * @return An array of search result items that pass the filter criteria.
     */
    applyFilters(rawSearchResults) {
        // filter the results
        let searchResults = rawSearchResults.filter((item) => {
            let passedAllFilters = true;

            for (const [type, value] of this.currentFilters) {
                if (type === "MinPrice") {
                    passedAllFilters = passedAllFilters && item.Price >= value;
                } else if (type === "MaxPrice") {
                    passedAllFilters = passedAllFilters && item.Price <= value;
                } else if (type === "Category") {
                    passedAllFilters = passedAllFilters &&
                        item.Categories.some(category => value.includes(category))
                } else if(item[type] !== value) {
                    passedAllFilters = false;
                }
            }
            return passedAllFilters;
        });

        this.updateFilters(searchResults);

        this.populateFilters();

        return searchResults;
    }

    /**
     * Sets a specific filter with the given type and value. If the value is null, undefined,
     * or an empty string, the filter is removed. Otherwise, it is updated with the new value.
     *
     * @param filterType The type of filter to set (e.g., "MinPrice").
     * @param filterValue The value to set for the filter (e.g., 10.00).
     */
    setFilter(filterType, filterValue) {
        if (filterValue === null || filterValue === undefined || filterValue === '') {
            this.currentFilters.delete(filterType);
        } else {
            this.currentFilters.set(filterType, filterValue);
        }
    }

    /**
     * Updates the available filters based on the items present in the search results. This is
     * used to populate the filter options in the UI.
     *
     * @param searchResults The search results used to determine which filters are available.
     */
    updateFilters(searchResults) {
        this.filters = new Map();
        this.filters.set("Provinces", []);
        this.filters.set("Cantons", []);
        this.filters.set("Brands", []);
        this.filters.set("Models", []);
        this.filters.set("Categories", []);

        for (let itemIndex = 0; itemIndex < searchResults.length; itemIndex++) {
            let item = searchResults[itemIndex];

            if (!this.filters.get("Provinces").includes(item.Province)) {
                this.filters.get("Provinces").push(item.Province);
            }

            if (!this.filters.get("Cantons").includes(item.Canton)) {
                this.filters.get("Cantons").push(item.Canton);
            }

            if (!this.filters.get("Brands").includes(item.Brand)) {
                this.filters.get("Brands").push(item.Brand);
            }

            if (!this.filters.get("Models").includes(item.Model)) {
                this.filters.get("Models").push(item.Model);
            }
            for (let categoryIndex = 0;
                 categoryIndex < item.Categories.length;
                 categoryIndex++) {
                if (!this.filters.get("Categories").includes(item.Categories[categoryIndex])) {
                    this.filters.get("Categories").push(item.Categories[categoryIndex]);
                }
            }
        }
    }

    /*
     * Clears all filters from the filter menu.
     */
    clearFilters() {
        this.currentFilters = new Map();
    }
}

/**
 * Represents a single filter field within the filter menu. Manages the UI element associated
 * with the filter and the key used for filtering.
 */
class FilterField {
    /**
     * Constructs a new FilterField.
     *
     * @param id The DOM element ID of the filter field.
     */
    constructor(id, key) {
        this.element = document.getElementById(id);
        this.key = key;
        this.optionKey = key.substring(0, key.length - 1);
    }


    /**
     * Adds filter options to the filter field's select element based on the available and
     * currently applied filters.
     *
     * @param currentFilters The map of currently applied filters.
     * @param allFilters The map of all available filters.
     */
    addOptions(currentFilters, allFilters) {
        // if field has filters
        if (allFilters.has(this.key)) {
            this.element.innerHTML = "";

            const todosOption = document.createElement("option");
            todosOption.value = "Todos";
            todosOption.innerHTML = "Todos";

            this.element.appendChild(todosOption);

            // place all the filters in the field
            for (const value of allFilters.get(this.key)) {
                const option = document.createElement("option");
                option.value = value;
                option.innerHTML = value;

                this.element.appendChild(option);

                if (currentFilters.get(this.optionKey) === value) {
                    this.element.selected = value;
                }
            }
            if (!currentFilters.get(this.optionKey)) {
                return;
            }
            this.element.value = currentFilters.get(this.optionKey);
        }
    }
}