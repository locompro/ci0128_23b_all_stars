import {ModalManager} from "./modal-manager.js"

const select2SpanishLanguageSettings = {
    errorLoading: function () {
        return 'No se pudieron cargar los resultados.';
    },
    inputTooLong: function (args) {
        var remainingChars = args.input.length - args.maximum;
        return 'Por favor, elimine ' + remainingChars + ' caracter(es)';
    },
    inputTooShort: function (args) {
        var remainingChars = args.minimum - args.input.length;
        return 'Por favor, introduzca ' + remainingChars + ' o más caracteres';
    },
    loadingMore: function () {
        return 'Cargando más resultados...';
    },
    maximumSelected: function (args) {
        return 'Sólo puede seleccionar ' + args.maximum + ' elemento(s)';
    },
    noResults: function () {
        return 'No se encontraron resultados';
    },
    searching: function () {
        return 'Buscando...';
    }
};

class StoreModalManager extends ModalManager {
    setupEvents() {
        super.setupEvents();

        this.#setupProvinceChange();

        this.modal.find("#partialStoreProvince").select2({
            placeholder: "Seleccionar",
            allowClear: true,
            dropdownParent: $('#addStoreModal .modal-content'),
            language: select2SpanishLanguageSettings
        });

        this.modal.find("#partialStoreCanton").select2({
            placeholder: "Seleccionar",
            allowClear: true,
            dropdownParent: $('#addStoreModal .modal-content'),
            language: select2SpanishLanguageSettings
        });

        $("#partialStoreName").rules("add", {
            messages: {
                required: "Ingresar el nombre de la tienda."
            }
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
            dropdownParent: $('#addProductModal .modal-content'),
            language: select2SpanishLanguageSettings
        });
        
        $("#partialProductName").rules("add", {
            messages: {
                required: "Ingresar el nombre del producto."
            }
        });
    }

    addAndValidate() {
        super.addAndValidate();
        if(!this.shouldClearFlag) {
         $("#partialProductId").val(-1)   
        }
    }
}

$(document).ready(function () {
    const storeManager = new StoreModalManager(
        $("#addStoreModal"),
        $("#showAddStoreContainer"),
        $("#hideAddStoreBtn"),
        $("#addStoreBtn"),
        $("#mainStoreName"),
        $("#partialStoreName"),
        $("#provincesData")
    );
    storeManager.setupEvents();

    const productManager = new ProductModalManager(
        $("#addProductModal"),
        $("#showAddProductContainer"),
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
            delay: 100,
            data: function (params) {
                return {
                    handler: 'FetchStores',
                    partialName: params.term
                };
            },
            processResults: function (data) {
                return {
                    results: data
                };
            },
            cache: true
        },
        placeholder: 'Seleccionar',
        minimumInputLength: 1,
        language: select2SpanishLanguageSettings
    });

    $('#mainProductName').select2({
        ajax: {
            url: '/Submissions/Create',
            dataType: 'json',
            delay: 100,
            data: function (params) {
                return {
                    handler: 'FetchProducts',
                    partialName: params.term,
                    store: null
                };
            },
            processResults: function (data) {
                return {
                    results: data
                };
            },
            cache: true
        },
        placeholder: 'Seleccionar',
        minimumInputLength: 1,
        language: select2SpanishLanguageSettings
    });

    $("#addSubmissionBtn").click(() => {
        $("input, select, textarea").each((index, element) => {
            const $element = $(element);
            // Skip validation for disabled elements
            if (!$element.is(":disabled")) {
                $element.trigger("focusout");
                $element.valid();
            }
        });
    });
});

$(document).ready(function() {
    $('#file').on('change', function() {
        const fileInput = this;
        const errorElement = $('#fileError');
        const maxSize = 5000000; // 5MB in bytes

        errorElement.css('display', 'none');

        for (const element of fileInput.files) {
            const file = element;

            if (file.size > maxSize) {
                errorElement.html('Archivo "' + file.name + '" supera el tamaño máximo de 5MB.');
                errorElement.css('display', 'block');
                fileInput.value = '';
            }
        }
    });
});

