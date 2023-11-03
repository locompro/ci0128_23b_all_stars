/**
 * This class handles the behavior of province and canton dropdowns.
 */
export class ProvinceHandler {
    /**
     * Create a new ProvinceHandler instance.
     *
     * @param {string} provincesDropdownId - The ID of the provinces dropdown element.
     * @param {string} cantonsDropdownId - The ID of the cantons dropdown element.
     * @param {string} url - The URL to fetch cantons data.
     */
    constructor(provincesDropdownId, cantonsDropdownId, url) {
        this.provincesDropdownId = provincesDropdownId;
        this.cantonsDropdownId = cantonsDropdownId;
        this.url = url;
    }

    /**
     * Handle the change event of the provinces dropdown.
     * When a province is selected, it triggers a fetch request to get the corresponding cantons.
     * On success, it populates the cantons dropdown. On error, it calls the handleError method.
     */
    async handleProvinceChange() {
        try {
            let selectedProvince = $(this.provincesDropdownId).val();
            let response = await fetch(`${this.url}&province=${selectedProvince}`);
            if (!response.ok) {
                throw new Error('La respuesta de la red no fue Ok ' + response.statusText);
            }
            let data = await response.json();
            this.populateCantons(data);
        } catch (error) {
            this.handleError(error);
        }
    }

    /**
     * Handles errors that occur during the fetch request in handleProvinceChange.
     * Logs the error details to the console.
     *
     * @param {Error} error - The error thrown by the fetch request.
     */
    handleError(error) {
        console.error(`Request failed: ${error}`);
    }

    /**
     * Populate the cantons dropdown with the provided data.
     *
     * @param {Object[]} data - The cantons data.
     */
    populateCantons(data) {
        const cantonDropdown = $(this.cantonsDropdownId);
        cantonDropdown.empty();
        cantonDropdown.append('<option value="">Seleccionar</option>');
        $.each(data, (index, canton) => {
            cantonDropdown.append(`<option value="${canton.name}">${canton.name}</option>`);
        });
    }

}
