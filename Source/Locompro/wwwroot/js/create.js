import { ModalManager } from "./modal-manager.js"

class StoreModalManager extends ModalManager {
    setupEvents() {
        super.setupEvents();
        
        this.#setupProvinceChange();

        this.modal.find("#partialStoreProvince").select2({
            placeholder: "Seleccionar",
            allowClear: true,
            dropdownParent: $('#addStoreModal .modal-content')
        });

        this.modal.find("#partialStoreCanton").select2({
            placeholder: "Seleccionar",
            allowClear: true,
            dropdownParent: $('#addStoreModal .modal-content')
        });
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

class ProductModalManager extends ModalManager {
    setupEvents() {
        super.setupEvents();
        this.modal.find("#partialProductCategory").select2({
            placeholder: "Seleccionar",
            allowClear: true,
            dropdownParent: $('#addProductModal .modal-content')
        });
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

    const productManager = new ProductModalManager(
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

    $('#mainStoreName').select2({
        ajax: {
            url: '/Submissions/Create',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                return {
                    handler: 'FetchStores',
                    partialName: params.term
                };
            },
            processResults: function(data) {
                return {
                    results: data
                };
            },
            cache: true
        },
        placeholder: 'Seleccionar',
        minimumInputLength: 1
    });

    $('#mainProductName').select2({
        ajax: {
            url: '/Submissions/Create',
            dataType: 'json',
            delay: 250,
            data: function(params) {
                return {
                    handler: 'FetchProducts',
                    partialName: params.term,
                    store: null
                };
            },
            processResults: function(data) {
                return {
                    results: data
                };
            },
            cache: true
        },
        placeholder: 'Seleccionar',
        minimumInputLength: 1
    });
});
