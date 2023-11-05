

var modalShown = false;

// event listener for the advanced search button
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("advancedSearchButton").addEventListener("click", async function () {
        // get button for advanced search button
        const button = document.getElementById("advancedSearchButton");
        const buttonContainer = document.getElementById("advancedSearchButtonContainer");

        const searchGroup = document.getElementById("mainPageSearchGroup");
        const modalContainer = document.getElementById("modalContainer");
        const searchBar = document.getElementById("searchBar");

        // if the modal is currently been shown, close it
        if (modalShown === true) {
            // get the modal
            let modal = document.getElementById("modalContainer");
            // erase the contents
            modal.innerHTML = "";

            // reset the button
            buttonContainer.classList.remove("advanced-search-button-in-modal");
            buttonContainer.classList.add("advanced-search-button");
            button.textContent = "Búsqueda avanzada";
            button.classList.add("index-advanced-search-button-initial");
            button.classList.remove("index-advanced-search-button-in-modal");

            modalContainer.classList.remove("index-modal-advanced-search");
            modalContainer.classList.add("advanced-search-modal-default");
            searchGroup.classList.remove("main-page-input-group-on-advanced-search");

            // change the search bar
            searchBar.classList.remove("search-bar-on-advanced-search");
            searchBar.classList.add("search-bar-default");

            // change state to modal not shown
            modalShown = false;
            return;
        }

        try {
            const response = await fetch("Index?handler=AdvancedSearch");
            if (response.ok) {
                const modalContent = await response.text();

                // Append the modal content to the modal container

                modalContainer.innerHTML = modalContent;

                initSelect2();
            } else {
                console.error('Failed to load modal content.');
            }
        } catch (error) {
            console.error('An error occurred:', error);
        }

        modalContainer.classList.add("index-modal-advanced-search");
        modalContainer.classList.remove("advanced-search-modal-default");
        searchGroup.classList.add("main-page-input-group-on-advanced-search");

        // change the button style
        buttonContainer.classList.remove("advanced-search-button");
        buttonContainer.classList.add("advanced-search-button-in-modal");
        button.classList.remove("index-advanced-search-button-initial");
        button.classList.add("index-advanced-search-button-in-modal");
        button.textContent = "";

        // change the search bar
        searchBar.classList.remove("search-bar-default");
        searchBar.classList.add("search-bar-on-advanced-search");

        // if the modal has been displayed, then store the state
        modalShown = true;
    });
});

function performIndexSearchButton() {
    performSearchButtonShared(modalShown);
}

async function loadProvince(optionSelected) {
    await loadProvinceShared(optionSelected, "Index");
}