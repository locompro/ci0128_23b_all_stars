class SearchResultsModal {
    constructor(searchResults, itemSelected) {
        this.modalProductName = document.getElementById("modalProductName");
        this.modalStoreName = document.getElementById("modalStoreName");
        this.modalModel = document.getElementById("modalModel");
        this.modalBrand = document.getElementById("modalBrand");
        this.submissionsTable = document.getElementById("ItemModalSubmissionsTable");
        this.searchResults = searchResults;
        this.itemSelected = itemSelected;
        
        this.populateModal();
    }

    populateModal() {
        this.modalProductName.innerHTML = this.searchResults[this.itemSelected].Name;
        this.modalStoreName.innerHTML = this.searchResults[this.itemSelected].Store;
        this.modalModel.innerHTML = "Modelo: " + this.searchResults[this.itemSelected].Model;
        this.modalBrand.innerHTML = "Marca: " + this.searchResults[this.itemSelected].Brand;
        
        let picturesContainer = document.getElementById("picturesContainer");
        
        loadPictures(picturesContainer, this.searchResults[this.itemSelected].Name, this.searchResults[this.itemSelected].Store);
        /*
        new SearchResultsPictureContainer(
            this.searchResults[this.itemSelected].Name,
            this.searchResults[this.itemSelected].Store,
            currentSlideIndex); */
        
        for (const submission of this.searchResults[this.itemSelected].Submissions) {
            const row = this.submissionsTable.insertRow();

            const dateCell = row.insertCell(0);
            dateCell.innerHTML = submission.DateTime;
            dateCell.classList.add("text-center");

            const priceCell = row.insertCell(1);
            priceCell.innerHTML = submission.Price;

            const descriptionCell = row.insertCell(2);
            descriptionCell.innerHTML = submission.Description;
        }
    }
}