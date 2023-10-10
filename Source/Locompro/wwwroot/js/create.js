class ModalManager {
    constructor(modal, showButton, hideButton, addButton, mainInput, partialInput, dataElement) {
        this.modal = modal;
        this.showButton = showButton;
        this.hideButton = hideButton;
        this.addButton = addButton;
        this.mainInput = mainInput;
        this.partialInput = partialInput;
        this.shouldClearFlag = true;
        this.dataElement = dataElement;
        this.data = null;
    }

    setupEvents() {
        this.modal.on('show.bs.modal', () => this.#loadData());
        this.modal.on('hidden.bs.modal', () => this.#clearOrHide());
        this.addButton.click(() => this.#addAndValidate());
    }

    #loadData() {
        if (!this.data) {
            this.data = JSON.parse(this.dataElement.attr("data"));
        }
    }

    #clearOrHide() {
        if (this.shouldClearFlag) {
            this.#clearModalInputs();
        } else {
            this.showButton.hide();
        }
    }

    #addAndValidate() {
        this.shouldClearFlag = this.#validateAndCopyValues();
    }

    #clearModalInputs() {
        this.modal.find("input, select, textarea").each((index, element) => {
            const $element = $(element);
            if ($element.is("select")) {
                $element.prop("selectedIndex", 0);
            } else {
                $element.val('');
            }
            $element.trigger("focusout");
            const fieldName = $element.attr("name");
            $(`span[data-valmsg-for='${fieldName}']`).text("")
                .removeClass("field-validation-error")
                .addClass("field-validation-valid");
        });
    }

    #validateAndCopyValues() {
        let isValid = true;
        this.modal.find("input, select, textarea").each((index, element) => {
            const $element = $(element);
            $element.trigger("focusout");
            if (!$element.valid()) {
                isValid = false;
            }
        });

        if (isValid) {
            const partialVal = this.partialInput.val();
            this.mainInput.val(partialVal);
            this.mainInput.prop("disabled", true);
            this.modal.modal('hide');
            return false;
        }

        return true;
    }
}

class StoreModalManager extends ModalManager {
    setupEvents() {
        super.setupEvents();
        this.#setupProvinceChange();
    }

    #setupProvinceChange() {
        this.modal.find("#partialStoreProvince").change(() => {
            const selectedProvinceName = this.modal.find("#partialStoreProvince").val();
            this.#updateCantonDropdown(selectedProvinceName);
        });
    }

    #updateCantonDropdown(selectedProvinceName) {
        const $cantonDropdown = this.modal.find("#partialStoreCanton");
        $cantonDropdown.find("option").not(':first').remove();
        if (!selectedProvinceName) {
            return;
        }
        const selectedProvince = this.data.find(p => p.Name === selectedProvinceName);
        if (selectedProvince) {
            const cantons = selectedProvince.Cantons;
            cantons.forEach(cantonObj => {
                const option = new Option(cantonObj.Name, cantonObj.Name);
                $cantonDropdown.append(option);
            });
        }
    }
}

$(document).ready(function () {
    const storeManager = new StoreModalManager(
        $("#addStoreModal"),
        $("#showAddStoreBtn"),
        $("#hideAddStoreBtn"),
        $("#addStoreBtn"),
        $("#mainStoreName"),
        $("#partialStoreName"),
        $("#provincesData")
    );
    storeManager.setupEvents();

    const productManager = new ModalManager(
        $("#addProductModal"),
        $("#showAddProductBtn"),
        $("#hideAddProductBtn"),
        $("#addProductBtn"),
        $("#mainProductName"),
        $("#partialProductName"),
        $("#categoriesData")
    );
    productManager.setupEvents();

    // Attach global event listeners for validation
    $("input, select, textarea").on('input', function () {
        $(this).valid();
    });
});
