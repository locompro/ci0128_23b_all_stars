/**
 * A class that manages modals in the profile page.
 * @export
 * @class ProfileModalManager
 */
export class ProfileModalManager {
    /**
     * Creates an instance of ProfileModalManager.
     * @param {string} id - The ID of the modal element.
     * @param {string} errorsListId - The ID of the element where errors are displayed.
     * @param {string} errorsDataId - The ID of the element that holds error data.
     * @memberof ProfileModalManager
     */
    constructor(id, errorsListId, errorsDataId) {
        this.id = id;
        this.errorsListId = errorsListId;
        this.errorsDataId = errorsDataId;
    }

    /**
     * Checks if the modal has errors by checking if the element that holds error data has the 'data-has-errors' attribute set to true.
     * @returns {boolean} - true if there are errors; otherwise, false.
     * @memberof ProfileModalManager
     */
    hasErrors() {
        return $(this.errorsDataId).data('has-errors');
    }

    /**
     * Shows the modal if it has errors.
     * @returns {ProfileModalManager} - The current instance for chaining.
     * @memberof ProfileModalManager
     */
    showModal() {
        if (this.hasErrors()) {
            $(this.id).modal('show');
        }
        return this;
    }

    /**
     * Clears the errors when the modal is closed. As it can be close with the close button or the reset button,
     * it sets up an event listener for both.
     * @returns {ProfileModalManager} - The current instance for chaining.
     * @memberof ProfileModalManager
     */
    clearErrors() {
        $(this.id + ' .btn-close').on('click', () => {
            $(this.errorsListId).empty();
        });

        $(this.id + ' button[type="reset"]').on('click', () => {
            $(this.errorsListId).empty();
        });

        return this;
    }

}
