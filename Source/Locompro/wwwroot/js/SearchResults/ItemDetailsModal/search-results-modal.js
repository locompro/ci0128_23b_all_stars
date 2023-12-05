/**
 * The SearchResultsModal class is responsible for managing the modal pop-up that displays detailed information about a selected item from the search results.
 */
class SearchResultsModal {
    /**
     * The constructor initializes the modal with detailed information about the product selected from the search results.
     * It sets up the DOM elements that will display the product information, and creates a new SearchResultsPictureContainer to manage product images.
     *
     * @param searchResults An array of search result items.
     * @param itemSelected The index of the selected item within the search results array.
     * @param reportedSubmissions
     */
    constructor(searchResults, itemSelected, reportedSubmissions = []) {
        this.generateTable();
        
        // DOM element references for displaying product information
        this.modalProductName = document.getElementById("modalProductName");
        this.modalStoreName = document.getElementById("modalStoreName");
        this.modalModel = document.getElementById("modalModel");
        this.modalBrand = document.getElementById("modalBrand");
        this.bookmarkIcon = document.getElementById('bookmarkIcon');
        this.iconText = document.getElementById('iconText');

        this.submissionsTable = document.getElementById("ItemModalSubmissionsTable");

        // The search results array and the index of the selected item
        this.searchResults = searchResults;
        this.itemSelected = itemSelected;

        // Creating a picture container for the selected item's images
        // Creating a picture container for the selected item's images
        this.pictureContainer =
            new SearchResultsPictureContainer(
                this.searchResults[this.itemSelected].ProductId,
                this.searchResults[this.itemSelected].Store,
                "SearchResults");

        this.submissionsRatings = [];
        this.reportedSubmissions = reportedSubmissions;

        $('#descriptionModal').on('show.bs.modal', function () {
            // Use a short delay to apply the style change to the backdrop
            setTimeout(() => {
                $('.modal-backdrop').last().css('opacity', '0');
            }, 0);
        });

        // Populate the modal with the selected item's details
        this.populateModal();
        
        try {
            if ($.fn.DataTable.isDataTable('#SubmissionsPerItem')) {
                $('#SubmissionsPerItem').DataTable().clear().destroy();
            }

            new DataTable('#SubmissionsPerItem', { 
                info: false,
                paging: false,
                searching: false,
                scrollCollapse: true,
                scrollY: '50vh',
                "columnDefs": [{
                    "targets": [3, 5],
                    "orderable": false,
                }],
                "bDestroy": true
            });
        } catch (e) {
            console.error('An error occurred while initializing DataTable:', e);
            alert('An error occurred while initializing the DataTable. Please try again.');
        }
    }
    
    generateTable() {
        // Define the table HTML
        let tableHTML = `
        <table id="SubmissionsPerItem" class="display compact mb-0 pe-2" style="width:100%">
            <caption></caption>
            <thead>
                <tr>
                    <th>Usuario&nbsp;</th>
                    <th class="text-center">Fecha&nbsp;</th>
                    <th>Precio&nbsp;</th>
                    <th>Descripción&nbsp;</th>
                    <th>Calificación</th>
                    <th class="text-center">Reportar</th>
                </tr>
            </thead>
            <tbody id="ItemModalSubmissionsTable" data-is-user-authenticated="@isLoggedIn">
            </tbody>
        </table>`;

        // Select the container div
        let containerDiv = document.getElementById('submissionsModalTableContainer');

        // Append the table to the div
        containerDiv.innerHTML = tableHTML;
    }

    /**
     * This method populates the modal with the selected item's details, including product name, store name, model, brand, and submissions.
     * It also initializes the picture container with the item's images.
     */
    populateModal() {
        // Setting the text content for the product details in the modal
        this.modalProductName.innerHTML = this.searchResults[this.itemSelected].Name;
        this.modalStoreName.innerHTML = this.searchResults[this.itemSelected].Store;
        this.modalModel.innerHTML = this.searchResults[this.itemSelected].Model;
        this.modalBrand.innerHTML = this.searchResults[this.itemSelected].Brand;
        this.bookmarkIcon = document.getElementById('bookmarkIcon');
        this.iconText = document.getElementById('iconText');

        if (this.bookmarkIcon && this.iconText) {
            this.bookmarkIcon.addEventListener('click', () => {
                this.triggerBookmarkButton(this.searchResults[this.itemSelected].ProductId);
            });
        }
        // Building the picture container with the product images
        this.pictureContainer.buildPictureContainer();
        
        let tableWithAuth = document.getElementById("submissionsModalTableContainer");
        this.isUserLoggedIn = tableWithAuth.getAttribute('data-is-user-authenticated') === 'True';

        // Populating the submissions table with entries
        for (const submission of this.searchResults[this.itemSelected].Submissions) {
            const row = this.submissionsTable.insertRow();

            // Prepare "Mis Contribuciones" button
            const contributionsButton = document.createElement('a');
            contributionsButton.className = 'btn btn-primary text-center border rounded-pill flex-shrink-1 justify-content-xxl-start';
            contributionsButton.href = '/Account/Contributions?query=' + submission.UserId; // Set the appropriate URL
            contributionsButton.id = 'Contributions';
            let userIcon = document.createElement('i');
            userIcon.classList.add('fa', 'fa-user');

            // Append the icon to the contributions button
            contributionsButton.appendChild(userIcon);
            contributionsButton.style.marginLeft = '15px';

            // Create a cell and append both the contributions button and the user profile
            const combinedCell = row.insertCell(0);
            combinedCell.appendChild(contributionsButton);
            combinedCell.innerHTML += "   " + submission.Username; // Add a space between the button and username

            // Inserting and formatting the date cell
            const dateCell = row.insertCell(1);
            dateCell.innerHTML = submission.EntryTime;
            dateCell.style.textAlign = 'center';

            // Inserting the price cell
            const priceCell = row.insertCell(2);
            priceCell.innerHTML = submission.FormattedPrice;

            // Inserting the description cell
            const descriptionCell = row.insertCell(3);
            descriptionCell.innerHTML = submission.Description;

            // Inserting the rating cell
            const ratingCell = row.insertCell(4);
            ratingCell.innerHTML += '<span style="display: none;">' + submission.Rating;
            this.submissionsRatings.push(new SearchResultsSubmissionRating(submission, ratingCell));
            this.submissionsRatings[this.submissionsRatings.length - 1].buildRating(this.isUserLoggedIn);
            
            const ratingsAmount = document.createElement('div');
            ratingsAmount.style.display = 'inline-block';
            ratingsAmount.style.paddingLeft = '10px';
            ratingsAmount.style.verticalAlign = '2px';
            ratingsAmount.style.textAlign = 'right';
            ratingsAmount.innerHTML = '(' + submission.NumberOfRatings + ')';
            ratingCell.appendChild(ratingsAmount);

            // Prepare report button
            const reportButton = document.createElement('button');
            reportButton.className = 'btn btn-primary';
            reportButton.type = 'submit';

            // Create the icon element
            let icon = document.createElement('i');
            icon.classList.add('fa', 'fa-flag');

            // Append the icon to the button
            reportButton.appendChild(icon);
            
            let isModerated = submission["Status"] === 1;
            let isSubmissionReported = this.isSubmissionReported(submission)
            
            if (!isModerated && this.isUserLoggedIn && !isSubmissionReported) {
                const submissionId = submission.UserId + submission.NonFormatedEntryTime;

                reportButton.setAttribute('data-id', submissionId);
                reportButton.setAttribute('data-bs-toggle', 'modal');
                reportButton.setAttribute('data-bs-target', '#descriptionModal');

                reportButton.addEventListener('click', () => {
                    const reportForm = document.getElementById('reportForm');
                    reportForm.reset();

                    const isLoggedInElement = document.getElementById('isLoggedIn');
                    const isLoggedIn = isLoggedInElement.getAttribute('data') === 'True';

                    if (!isLoggedIn) {
                        window.location.href = '/Account/Login'; // Redirect to the login page if not logged in
                        return; // Exit the function to prevent the rest of the code from running
                    }

                    document.querySelector('input[name="UserReportVm.SubmissionUserId"]').value = submission.UserId;
                    document.querySelector('input[name="UserReportVm.SubmissionEntryTime"]').value = submission.NonFormatedEntryTime;
                });
            } else {
                reportButton.disabled = true;
            }

            // Inserting the report button cell
            const reportCell = row.insertCell(5);
            reportCell.style.textAlign = 'center';
            reportCell.appendChild(reportButton);
        }
        
    }
    
    isSubmissionReported(submission) {
        for (let sub of this.reportedSubmissions) {
            if (submission.UserId === sub.UserId && submission.NonFormatedEntryTime === sub.NonFormatedEntryTime) {
                return true;
            }
        }
        return false;
    }


    /**
     * This method controls the navigation between different slides (images) in the modal.
     *
     * @param slideIndex The index of the slide to navigate to.
     */
    plusSlides(slideIndex) {
        // Delegating the slide navigation to the picture container's method
        this.pictureContainer.plusSlides(slideIndex);
    }

    triggerBookmarkButton(productId) {
        this.AddToShoppingList(productId);

        // Set initial opacity to 0
        this.iconText.style.opacity = '0';

        // Set display property to inline
        this.iconText.style.display = 'inline';

        // Set transition duration (e.g., 0.5 seconds)
        this.iconText.style.transition = 'opacity 0.5s';

        // Toggle animation of icon
        this.bookmarkIcon.classList.remove('fa-regular');
        this.bookmarkIcon.classList.add('fa-solid');
        setTimeout(() => {
            this.bookmarkIcon.classList.remove('fa-solid');
            this.bookmarkIcon.classList.add('fa-regular');
        }, 1600);

        // After a delay (e.g., 100 milliseconds), set opacity to 1
        setTimeout(() => {
            this.iconText.style.opacity = '1';

            // After another delay (e.g., 1500 milliseconds), set opacity back to 0
            setTimeout(() => {
                this.iconText.style.opacity = '0';
            }, 1500);
        }, 100);
    }


    /**
     * This method triggers a fetch to add a product to the users's shopping list
     *
     * @param productId The id of the product to add
     */
    AddToShoppingList(productSendId) {
        let url = window.location.pathname;
        url += "?handler=AddToShoppingList";

        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: productSendId
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
            });
    }
}
