// TODO: Generify to use with product modal

var provinces = null;

$(document).ready(function() {
    // Bootstrap modal instance
    var addStoreModal = new bootstrap.Modal($("#addStoreModal")[0]);

    // Event fired just before the modal is shown
    $('#addStoreModal').on('show.bs.modal', function() {
        if (!provinces) {
            const provincesElement = $("#provincesData");
            provinces = JSON.parse(provincesElement.attr("data-provinces"));  // Initialize the global variable
        }
    });
    
    // Event fired just before the modal is hidden
    $('#addStoreModal').on('hide.bs.modal', function() {
        // Clear all input, select, and textarea elements within the modal
        $(this).find("input, select, textarea").each(function() {
            clearModalInputs($(this));
        });
    });

    // Hide the partial view and update main view
    $("#hideAddStoreBtn").click(function() {
        validateAndCopyValues(addStoreModal);
    });

    // Update cantons on province select
    $("#partialStoreProvince").change(function() {
        updateCantonDropdown($("#partialStoreProvince").val());
    });
});

function clearModalInputs(element) {
    if (element.is("select")) {
        element.prop("selectedIndex", 0);
    } else {
        element.val('');
    }

    updateCantonDropdown(null);
    
    // Trigger focusout event to re-run validation and clear messages
    element.trigger("focusout");

    // Manually reset the validation messages
    const fieldName = element.attr("name");
    $(`span[data-valmsg-for='${fieldName}']`).text("")
        .removeClass("field-validation-error")
        .addClass("field-validation-valid");
}

function validateAndCopyValues(addStoreModal) {
    let isValid = true;

    // Loop through all input, select, and textarea elements within the modal
    $("#addStoreModal").find("input, select, textarea").each(function () {
        // Trigger focusout event to simulate user interaction
        $(this).trigger("focusout");

        // Use jQuery Validation's valid method
        if (!$(this).valid()) {
            isValid = false;
        }
    });

    if (isValid) {
        // Copy the value from the partial to the main view
        var partialStoreName = $("#partialStoreName").val();
        $("#mainStoreName").val(partialStoreName);

        // Disable editing of the main view input box
        $("#mainStoreName").prop("disabled", true);

        // Hide the modal using Bootstrap's API
        addStoreModal.hide();
    }
}

function updateCantonDropdown(selectedProvinceName) {
    const $cantonDropdown = $("#partialStoreCanton");

    // Remove only the options that are not the default "Seleccionar" option
    $cantonDropdown.find("option").not(':first').remove();

    // If Province select is set to its placeholder, stop further execution.
    if (!selectedProvinceName) {
        return;
    }

    const selectedProvince = provinces.find(p => p.Name === selectedProvinceName);

    // Populate canton dropdown based on selected province
    if (selectedProvince) {
        const cantons = selectedProvince.Cantons;

        cantons.forEach(cantonObj => {
            const option = new Option(cantonObj.Name, cantonObj.Name);
            $cantonDropdown.append(option);
        });
    }
}  