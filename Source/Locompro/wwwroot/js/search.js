const invalidSimpleSearch = "/SearchResults/SearchResults?query=";

const invalidAdvancedSearch =
    "/SearchResults/SearchResults?"
    + "query="
    + "&province=Todos"
    + "&canton=Todos"
    + "&minValue=0"
    + "&maxValue=0"
    + "&category=Todos"
    + "&model="
    + "&brand=";

// activated when province is changed in the dropdown menu
async function loadProvinceShared(optionSelected, sourceName) {
    var content = optionSelected.value;

    try {
        // send server side the province selected by client and wait for response
        const response = await fetch(sourceName + "?handler=UpdateProvince&province=" + content, {
            method: 'GET', // use get method
            headers: {
                'Accept': 'application/json', // acepting json files
            },
        });

        if (response.ok) {
        console.log("ok");
        // get the cantons that were sent
        loadCantons(response);
        } else {
            console.log("not ok");
        }
    } catch (error) {
        console.log("Error");
    }
}

// get the cantons sent by the server side
async function loadCantons(response) {
    try {
        // wait for cantons json file to be received
        const data = await response.json();
    
        // Clear existing options in the optgroup
        const optgroup = document.getElementById('cantonDropdown');
        optgroup.innerHTML = '';
    
        // Populate with new options based on fetched data
        data.forEach(function (canton) {
            const option = document.createElement('option');
            option.textContent = canton.Name;
            optgroup.appendChild(option);
        });
    } catch (error) {
    console.error('Error loading cantons:', error);
    }
}

function performSearchButtonShared(modalShownParam) {
    // if advanced search is not open it is normal search
    var redirect = "/SearchResults/SearchResults?query=";

    var searchValue = document.getElementById("searchBox").value.valueOf() ;

    if (!modalShownParam) {
        // get value
        redirect += searchValue;

        if (redirect.localeCompare(invalidSimpleSearch) === 0) {
            return;
        }
    } else {
        var provinceValue = document.getElementById("provinceDropdown").value;
        var cantonValue = document.getElementById("cantonDropdown").value;
        var minValue = /*document.getElementById("minValue").value*/ 0;
        var maxValue = /*document.getElementById("maxValue").value*/ 0;
        var categoryValue = document.getElementById("categoryDropdown").value;

        try {
            var modelValue = document.getElementById("modelInput").value;
            var brandValue = document.getElementById("brandInput").value;
        } catch (error) {
        }

        redirect += searchValue
        + "&province=" + provinceValue
        + "&canton=" + cantonValue
        + "&minValue=" + minValue
        + "&maxValue=" + maxValue
        + "&category=" + categoryValue
        + "&model=" + modelValue
        + "&brand=" + brandValue;
    
        if (redirect.localeCompare(invalidAdvancedSearch) === 0) {
            return;
        }
    }

    window.location.href = redirect;

    /*
    const dataToSend = {
        query: searchValue,
        province: provinceValue,
        canton: cantonValue,
        minValue: minValue,
        maxValue: maxValue,
        category: categoryValue,
        model: modelValue
    };

    var url = window.location.pathname;
    var handler = "?handler=SendSearchParameters";
    var location = url + handler;

    $.ajax({
        contentType: 'application/json',
        type: 'POST',
        url: location,
        data: JSON.stringify(dataToSend),
        headers: {
            "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
        },
        success: function () {
            window.location.href = "/SearchResults/SearchResults";
        },
        error: function () {
            alert("AJAX Request Failed, another failure...");
        }
    });
    */
}


// makes sure the min field is less than the max field
function validatePriceInput(button) {
    // get the fields
    var minButton = document.getElementById("minValue");
    var maxButton = document.getElementById("maxValue");

    // if the field is the min field
    if (button === minButton) {
        // check if the min field is greater or equal than the max field
        if (parseInt(minButton.value, 10) >= parseInt(maxButton.value, 10)) {
            // if no then make it less than the max field
            minButton.value = parseInt(maxButton.value, 10) - 1;
            // ensure the value is not negative
            if (minButton.value < 0) {
                minButton.value = 0;
            }
        }
    }

    // field is the max field
    if (button === maxButton) {
        // if the value is less or equal than the min field
        if (parseInt(minButton.value, 10) >= parseInt(maxButton.value, 10)) {
            // make it greater than the min field
            maxButton.value = parseInt(minButton.value, 10) + 1;
        }
    }
}

// script to search on enter press
document.getElementById("searchBox")
    .addEventListener("keyup", function (event) {
    event.preventDefault();
    if (event.keyCode === 13) {
        document.getElementById("searchButton").click();
    }
});
