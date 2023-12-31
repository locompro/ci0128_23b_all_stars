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
    + "&brand="
    + "&latitude=0"
    + "&longitude=0"
    + "&distance=25"
    + "&mapGeneratedAddress=";

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
        const optgroup = document.getElementById("cantonDropdown");
        optgroup.innerHTML = "";
        
        // Populate with new options based on fetched data
        data.forEach(function (canton) {
            const option = document.createElement("option");
            let cantonToAdd = canton.Name;
            
            option.value = cantonToAdd;
            option.innerHTML = cantonToAdd;
            
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

    let provinceValue, cantonValue, minValue, maxValue, categoryValue, modelValue, brandValue,
        distance, latitude, longitude, mapGeneratedAddress = "";
    
    if (!modalShownParam) {
        if (searchValue.localeCompare("") === 0) {
            return null;
        }
    } else {
        provinceValue = document.getElementById("provinceDropdown").value.valueOf();
        cantonValue = document.getElementById("cantonDropdown").value.valueOf();
        minValue = document.getElementById("minValue").value.valueOf();
        maxValue = document.getElementById("maxValue").value.valueOf();
        categoryValue = document.getElementById("categoryDropdown").value.valueOf();
        modelValue = document.getElementById("modelDropdown").value.valueOf();
        brandValue = document.getElementById("brandDropdown").value.valueOf();
        
        if (document.getElementById("latitude") !== null) {
            latitude = document.getElementById("latitude").value.valueOf();
            longitude = document.getElementById("longitude").value.valueOf();
            distance = document.getElementById("distanceRangeSlider").value.valueOf();
            mapGeneratedAddress = document.getElementById("MapGeneratedAddress").value.valueOf();
        } else {
            latitude = 0;
            longitude = 0;
            distance = 0;
            mapGeneratedAddress = "";
        }

        redirect += searchValue
            + "&province=" + provinceValue
            + "&canton=" + cantonValue
            + "&minValue=" + minValue
            + "&maxValue=" + maxValue
            + "&category=" + categoryValue
            + "&model=" + modelValue
            + "&brand=" + brandValue
            + "&latitude=" + latitude
            + "&longitude=" + longitude
            + "&distance=" + distance
            + "&mapGeneratedAddress=" + mapGeneratedAddress;

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
        CategorySelected: categoryValue,
        Latitude: latitude,
        Longitude: longitude,
        Distance: distance,
        MapGeneratedAddress: mapGeneratedAddress
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
        // check if the min field is greater or equal than the max field
        if (parseInt(minButton.value, 10) >= parseInt(maxButton.value, 10)) {
            // if no then make it less than the max field
            maxButton.value = parseInt(minButton.value, 10) + 1;
        }
    }
    
    // field is the max field
    if (button === maxButton) {
        // if the value is less or equal than the min field
        if (parseInt(maxButton.value, 10) <= parseInt(minButton.value, 10)) {
            // make it greater than the min field
            minButton.value = parseInt(maxButton.value, 10) - 1;
        }
    }
    
    if (minButton.value < 0) {
        minButton.value = 0;
    }
    
    if (maxButton.value < 0) {
        maxButton.value = 0;
    }
}

// script to search on enter press
document.addEventListener("keyup", function (event) {
    event.preventDefault();
    if (event.key === "Enter") { // Use event.key instead of event.keyCode
        document.getElementById("searchButton").click();
    }
});

function updateDistanceDisplay(slider) {
    let distance = slider.value;
    const displayElement = document.getElementById("mapDistanceSelected");

    displayElement.innerHTML = distance + " km";
    displayElement.value = distance;
}