var modalShown = false;
async function advancedSearchButtonPressed() {
    const button = document.getElementById("advancedSearchButton");
    const modalContainer = document.getElementById("modalContainer");
    const inputGroup = document.getElementById("inputGroup");
    const searchBarGroup = document.getElementById("searchBarGroup");
    
    // if the modal is currently been shown, close it
    if (modalShown === true) {
        // get the modal
    
        // erase the contents
        modalContainer.innerHTML = "";

        inputGroup.classList.remove("search-input-group-on-advanced-search");
        searchBarGroup.classList.remove("search-bar-group-on-advanced-search");
        
        button.textContent = "Búsqueda avanzada";
        button.classList.remove("advanced-search-button-on-modal-shown");
        button.classList.add("col-sm-2");
        
        // change state to modal not shown
        modalShown = false;
        return;
    }

    try {
        const response = await fetch("SearchResults?handler=AdvancedSearch");
        if (response.ok) {
            // Append the modal content to the modal container
            modalContainer.innerHTML = await response.text();
        } else {
            console.error('Failed to load modal content.');
        }
    } catch (error) {
        console.error('An error occurred:', error);
    }

    modalShown = true;
    
    inputGroup.classList.add("search-input-group-on-advanced-search");
    searchBarGroup.classList.add("search-bar-group-on-advanced-search");
    button.textContent = "X";
    button.classList.remove("col-sm-2");
    button.classList.add("advanced-search-button-on-modal-shown");
}

function performSearchButton() {
    performSearchButtonShared(modalShown);
}

async function loadProvince(optionSelected){
    loadProvinceShared(optionSelected, "SearchResults");
}

function itemSelected(index) {
    var modalId = "#modal" + index;
    $(modalId).modal('show');
}