var modalShown = false;
async function advancedSearchButtonPressed() {
    var button = document.getElementById("advancedSearchButton");
    var modalContainer = document.getElementById("modalContainer");

    // if the modal is currently been shown, close it
    if (modalShown == true) {
        // get the modal
    
        // erase the contents
        modalContainer.innerHTML = "";
        button.classList.remove("search-results-button-rounded-top");
        button.classList.add("search-results-advanced-search-button-initial");
        button.classList.add("btn-primary");
        button.classList.add("rounded-pill");
    
        // change state to modal not shown
        modalShown = false;
        return;
    }

    try {
        const response = await fetch("SearchResults?handler=AdvancedSearch");
        if (response.ok) {
            const modalContent = await response.text();
    
                // Append the modal content to the modal container
                modalContainer.innerHTML = modalContent;
        } else {
            console.error('Failed to load modal content.');
        }
    } catch (error) {
        console.error('An error occurred:', error);
    }

    modalShown = true;
    modalContainer.classList.remove("modal-advanced-search");
    modalContainer.classList.add("modal-search-results-on-advanced-search");

    button.classList.remove("rounded-pill");
    button.classList.remove("btn-primary");
    button.classList.remove("search-results-advanced-search-button-initial");
    button.classList.add("search-results-button-rounded-top");

}

function performSearchButton() {
    performSearchButtonShared(modalShown);
}

function itemSelected() {
    window.location.href = "/SearchResults/SearchResults?query=buttonClicked!";
}


async function loadProvince(optionSelected){
    loadProvinceShared(optionSelected, "SearchResults");
}
