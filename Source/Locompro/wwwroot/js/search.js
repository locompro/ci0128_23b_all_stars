const invalidSimpleSearch = "/SearchResults/SearchResults?query=";

const invalidAdvancedSearch =
    "/SearchResults/SearchResults?"
    + "query="
    + "&province=Todos"
    + "&canton=Todos"
    + "&minValue=0"
    + "&maxValue=0"
    + "&category="
    + "&model="
    + "&brand=";

// activated when province is changed in the dropdown menu
async function loadProvinceShared(optionSelected, sourceName) {
    let content = optionSelected.value;

    try {
        // send server side the province selected by client and wait for response
        const response = await fetch(sourceName + "?handler=UpdateProvince&province=" + content, {
            method: 'GET', // use get method
            headers: {
                'Accept': 'application/json', // acepting json files
            },
        });

        if (response.ok) {
            // get the cantons that were sent
            await loadCantons(response);
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
    try {
        searchResultsPage.requestSent = true;
    } catch (error) {
    }

    const dataToSend = getDataToSend(modalShownParam);

    if (dataToSend === null) {
        return;
    }

    sendSearchRequest(dataToSend);
}

function getDataToSend(modalShownParam) {
    let redirect = "/SearchResults/SearchResults?query=";
    let searchValue = document.getElementById("searchBox").value.valueOf();

    let provinceValue, cantonValue, minValue, maxValue, categoryValue, modelValue, brandValue = "";

    if (!modalShownParam) {
        if (searchValue.localeCompare("") === 0) {
            return null;
        }
    } else {
        provinceValue = document.getElementById("provinceDropdown").value;
        cantonValue = document.getElementById("cantonDropdown").value;
        minValue = document.getElementById("minValue").value;
        maxValue = document.getElementById("maxValue").value;
        categoryValue = document.getElementById("categoryDropdown").value;
        modelValue = document.getElementById("modelDropdown").value;
        brandValue = document.getElementById("brandDropdown").value;

        redirect += searchValue
            + "&province=" + provinceValue
            + "&canton=" + cantonValue
            + "&minValue=" + minValue
            + "&maxValue=" + maxValue
            + "&category=" + categoryValue
            + "&model=" + modelValue
            + "&brand=" + brandValue;
        
        console.log(redirect  + "\n" + invalidAdvancedSearch);  

        if (redirect.localeCompare(invalidAdvancedSearch) === 0) {
            return null;
        }
    }

    return {
        ProductName: searchValue,
        ProvinceSelected: provinceValue,
        CantonSelected: cantonValue,
        MinPrice: minValue,
        MaxPrice: maxValue,
        ModelSelected: modelValue,
        BrandSelected: brandValue,
        CategorySelected: categoryValue
    };
}

function sendSearchRequest(dataToSend) {
    let url = window.location.pathname;
    let handler = '?handler=ReturnResults';
    let location = url + handler;

    fetch(location, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(dataToSend) // Data to be sent in JSON format
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok.');
            }
            // Redirect if the response is successful
            window.location.href = "/SearchResults/SearchResults";
        })
        .catch(error => {
            // Handle errors
            console.error('Fetch error:', error);
        });
}


// makes sure the min field is less than the max field
function validatePriceInput(button) {
    // get the fields
    let minButton = document.getElementById("minValue");
    let maxButton = document.getElementById("maxValue");

    // if the field is the min field
    if (button === minButton) {
        if (minButton.value < 0) {
            minButton.value = 0;
        }
        // check if the min field is greater or equal than the max field
        if (parseInt(minButton.value, 10) >= parseInt(maxButton.value, 10)) {
            // make it greater than the min field
            maxButton.value = parseInt(minButton.value, 10) + 1;
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
document.addEventListener("keyup", function (event) {
    event.preventDefault();
    if (event.key === "Enter") { // Use event.key instead of event.keyCode
        document.getElementById("searchButton").click();
    }
});
