var modalShown = false;

// event listener for the advanced search button
document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("advancedSearchButton").addEventListener("click", async function () {
        // get button for advanced search button
        button = document.getElementById("advancedSearchButton");

        // if the modal is currently been shown, close it
        if (modalShown == true) {
            // get the modal
            var modal = document.getElementById("modalContainer");
            // erase the contents
            modal.innerHTML = "";

            // reset the button
            button.classList.remove("button-rounded-top");
            button.classList.add("index-advanced-search-button");

            modalContainer.classList.remove("index-modal-advanced-search");
            modalContainer.classList.add("advanced-search-modal-default");

            // change state to modal not shown
            modalShown = false;
            return;
        }

        try {
            const response = await fetch("Index?handler=AdvancedSearch");
            if (response.ok) {
                const modalContent = await response.text();

                // Append the modal content to the modal container
                const modalContainer = document.getElementById("modalContainer");
                modalContainer.innerHTML = modalContent;
            } else {
                console.error('Failed to load modal content.');
            }
        } catch (error) {
            console.error('An error occurred:', error);
        }

        modalContainer.classList.add("index-modal-advanced-search");
        modalContainer.classList.remove("advanced-search-modal-default");

        // change the button style
        button.classList.remove("index-advanced-search-button-initial");
        button.classList.remove("index-advanced-search-button");
        button.classList.add("button-rounded-top");

        // if the modal has been displayed, then store the state
        modalShown = true;
    });
});

function performSearchButton() {
    performSearchButtonShared(modalShown);
}

async function loadProvince(optionSelected){
    loadProvinceShared(optionSelected, "Index");
}