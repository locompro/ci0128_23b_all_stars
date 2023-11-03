class SearchResultsFilterMenu {
    constructor() {
        this.filters = new Map();
        this.currentFilters = new Map();
        this.filterFields = this.setUpFields();
    }
    
    setUpFields() {
        return [
            new FilterField("productNameFilter", "ProductNames"),
            new FilterField("provinceFilter", "Provinces"),
            new FilterField("cantonFilter", "Cantons"),
            new FilterField("minPriceFilter", "MinPrice"),
            new FilterField("maxPriceFilter", "MaxPrice"),
            new FilterField("brandFilter", "Brands"),
            new FilterField("modelFilter", "Models")
        ];
    }

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
            todosOption.value = "todos";
            todosOption.innerHTML = "todos";

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

    applyFilters(rawSearchResults) {
        // filter the results
        let searchResults = rawSearchResults.filter((item) => {
            let passedAllFilters = true; 

            for (const [type, value] of this.currentFilters) {
                if (type === "MinPrice") {
                    passedAllFilters = passedAllFilters && item.Price >= value;
                } else if (type === "MaxPrice") {
                    passedAllFilters = passedAllFilters && item.Price <= value;
                } else if (item[type] !== value) {
                    passedAllFilters = false;
                }
            }
            return passedAllFilters;
        });

        this.updateFilters(searchResults);

        this.populateFilters();
        
        return searchResults;
    }

    setFilter(filterType, filterValue) {
        if (filterValue === null || filterValue === undefined || filterValue === '') {
            this.currentFilters.delete(filterType);
        } else {
            this.currentFilters.set(filterType, filterValue);
        }
    }

    updateFilters(searchResults) {
        this.filters = new Map();
        this.filters.set("Provinces", []);
        this.filters.set("Cantons", []);
        this.filters.set("ProductNames", []);
        this.filters.set("Brands", []);
        this.filters.set("Models", []);
        this.filters.set("Categories", []);
        
        // this.filters.set("MinPrice", searchResults[0].Price);
        // this.filters.set("MaxPrice", searchResults[0].Price);

        for (let itemIndex = 0; itemIndex < searchResults.length; itemIndex++) {
            let item = searchResults[itemIndex];

            if (!this.filters.get("Provinces").includes(item.Province)) {
                this.filters.get("Provinces").push(item.Province);
            }

            if (!this.filters.get("Cantons").includes(item.Canton)) {
                this.filters.get("Cantons").push(item.Canton);
            }

            if (!this.filters.get("ProductNames").includes(item.Name)) {
                this.filters.get("ProductNames").push(item.Name);
            }

            if (!this.filters.get("Brands").includes(item.Brand)) {
                this.filters.get("Brands").push(item.Brand);
            }

            if (!this.filters.get("Models").includes(item.Model)) {
                this.filters.get("Models").push(item.Model);
            }

            for (let category in item.Categories) {
                if (!this.filters.get("Categories").includes(category)) {
                    this.filters.get("Categories").push(category);
                }
            }
        }
    }
}

class FilterField {
    constructor(id, key) {
        this.element = document.getElementById(id);
        this.key = key;
        this.optionKey = key.substring(0, key.length - 1);
    }
    
    addOptions(currentFilters, allFilters) {
        // if field has filters
        if (allFilters.has(this.key)) {
            this.element.innerHTML = "";
            
            const todosOption = document.createElement("option");
            todosOption.value = "todos";
            todosOption.innerHTML = "todos";
            
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