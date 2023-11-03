var modalShown = false;

async function advancedSearchButtonPressed() {
    const button = document.getElementById("advancedSearchButton");
    const modalContainer = document.getElementById("modalContainer");
    const inputGroup = document.getElementById("inputGroup");
    const searchBarGroup = document.getElementById("searchBarGroup");
    const closeModalButton = document.getElementById("closeModalButton");

    // if the modal is currently been shown, close it
    if (modalShown === true) {
        // erase the contents
        modalContainer.innerHTML = "";

        inputGroup.classList.remove("search-input-group-on-advanced-search");
        searchBarGroup.classList.remove("search-bar-group-on-advanced-search");

        button.textContent = "Búsqueda avanzada";
        button.classList.remove("advanced-search-button-on-modal-shown");
        button.classList.add("btn-primary");

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
    button.style.display = "none";
    button.classList.add("advanced-search-button-on-modal-shown");
    button.classList.remove("btn-primary");

    closeModalButton.classList.remove("close-modal-button-hidden");
    closeModalButton.classList.add("close-modal-button-shown");

}

function performSearchButton() {
    performSearchButtonShared(modalShown);
}

async function loadProvince(optionSelected){
    await loadProvinceShared(optionSelected, "SearchResults");
}

function itemSelected(index) {
    const modalId = "#modal" + index;
    $(modalId).modal('show');
}

function closeModal() {
    const button = document.getElementById("advancedSearchButton");
    const modalContainer = document.getElementById("modalContainer");
    const inputGroup = document.getElementById("inputGroup");
    const searchBarGroup = document.getElementById("searchBarGroup");
    const closeModalButton = document.getElementById("closeModalButton");

    modalContainer.innerHTML = "";
    inputGroup.classList.remove("search-input-group-on-advanced-search");
    searchBarGroup.classList.remove("search-bar-group-on-advanced-search");

    button.textContent = "Búsqueda avanzada";
    button.classList.remove("advanced-search-button-on-modal-shown");
    button.classList.add("btn-primary");
    button.style.display = "block";

    closeModalButton.classList.add("close-modal-button-hidden");
    closeModalButton.classList.remove("close-modal-button-shown");

    modalShown = false;
}

// removes loaded pictures when the modal is closed
function onModalClose(modalId) {
    const modalIndex = modalId.split('ItemModal')[1];
    const pictureContainerId = "picturesContainer";
    const pictureContainer = document.getElementById(pictureContainerId);
    pictureContainer.innerHTML = "";

    const submissionTable = document.getElementById("ItemModalSubmissionsTable");
    submissionTable.innerHTML = "";
}

// Observer for changes in style attribute of the modal
var observer = new MutationObserver(function(mutationsList) {
    mutationsList.forEach(function(mutation) {
        if (mutation.type === "attributes" && mutation.attributeName === "style" && mutation.target.style.display === "none") {
            onModalClose(mutation.target.id);
        }
    });
});

// Observe changes in all elements with IDs containing "modal"
var modalElements = document.querySelectorAll('[id*="ItemModal"]');
modalElements.forEach(function(modal) {
    observer.observe(modal, { attributes: true });
});