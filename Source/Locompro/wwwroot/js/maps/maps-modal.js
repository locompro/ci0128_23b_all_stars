import {GoogleMap} from "./google-map.js";

const country = "Costa Rica";
const latitudeElementId = "latitude";
const longitudeElementId = "longitude";
const addressInputId = "MapGeneratedAddress";
const cantonDropdownId = "cantonDropdown";
const provinceDropdownId = "provinceDropdown";
const distanceElementId = "distanceRangeSlider";
const elementId = "mapModal";

document.addEventListener("DOMContentLoaded", function () {
    const modalContainer = document.getElementById("modalContainer");
    let hasAdded = false;
    let somethingChanged = true;
    
    // observe the modal container for changes
    const modalObserver = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            // we only need to know if something has been added, not what has been added
            if (mutation.type === 'childList' &&
                mutation.addedNodes.length > 0 &&
                mutation.removedNodes.length === 0) {
                // so we just set a value once something has changed
                somethingChanged = true;
            }
        });
        
        if (somethingChanged) {
            // if something had been added, then we are removing
            if (hasAdded) {
                // reset the value to nothing has been added
                hasAdded = false;
                // skip
                return;
            }
            
            // otherwise, if false, nothing has been added previously and we are adding
            hasAdded = true;
            
           getAdvancedSearchMapsModal(elementId)
        }
    });
    
    const config = { childList: true};
    modalObserver.observe(modalContainer, config);
});

function getAdvancedSearchMapsModal(elementId) {
    let mapModal = new MapsModal(elementId, country, provinceDropdownId, cantonDropdownId, distanceElementId,
        latitudeElementId, longitudeElementId, addressInputId);

    window.GetModalMap = () => {
        return mapModal;
    };

    mapModal.setMapModalCreationListener().then(() => {});
}

class MapsModal {
    constructor(mapModalElementId, country, provinceId, cantonId,
                distanceElement, latitudeElementId, longitudeElementId, addressInputId) {
        this.country = country;
        this.mapModalElement = document.getElementById(mapModalElementId);
        this.provinceInput = document.getElementById(provinceId);
        this.cantonInput = document.getElementById(cantonId);
        this.distanceElement = document.getElementById(distanceElement);
        
        this.latitudeElementId = latitudeElementId;
        this.longitudeElementId = longitudeElementId;
        this.addressInputId = addressInputId;
        
        this.apiAlreadyLoaded = false;
        this.apiKey = "";

        let path = window.location.pathname;
        let isIndexPage = path === '/' || path.endsWith('/index.html') || path.endsWith('/index.htm');
        if (isIndexPage) {
            sessionStorage.removeItem("mapApiAlreadyLoaded")
        }
    }
    
    async getApiKey() {
        let apiKey = "";
        
        let path = window.location.pathname;
        path += "?handler=ReturnMapsApiKey";
        
        let response = await fetch(path, {
           method: 'GET',
              headers: {
                'Accept': 'application/json'
              }
        });
        
        if (response.ok) {
            let data = await response.json();
            apiKey = JSON.stringify(data);
        }
        
        return apiKey;
    }
    
    async setMapModalCreationListener() {
        this.apiKey = await this.getApiKey();
        
        let modal = this;
        
        this.mapModalElement.addEventListener("shown.bs.modal", function () {
            let sessionApiAlreadyLoaded = sessionStorage.getItem("mapApiAlreadyLoaded");
            
            if (sessionApiAlreadyLoaded === null) {
                sessionStorage.setItem("mapApiAlreadyLoaded", this.apiAlreadyLoaded);
            } else {
                this.apiAlreadyLoaded =
                    this.apiAlreadyLoaded === true || sessionApiAlreadyLoaded === "true";
                sessionStorage.setItem("mapApiAlreadyLoaded", this.apiAlreadyLoaded);
            }
            
            if (this.apiAlreadyLoaded) {
                modal.setLocationOnMap();
                return;
            }
            
            const script = document.createElement('script');

            let apiKey = modal.apiKey.toString().replace(/['"]+/g, ''); // Remove quotation marks
            script.src = "https://maps.googleapis.com/maps/api/js?key=" + apiKey + "&callback=initMap";
            
            document.body.appendChild(script);
            
            this.apiAlreadyLoaded = true;
        });
    }
    
    initiateMap() {
        this.map = new GoogleMap(
            "StoreModalMap",
            this.latitudeElementId,
            this.longitudeElementId,
            this.addressInputId,);
        
        this.setMapModalCreationListener().then(() => {});
    }
    
    setLocationOnMap() {
        if (!this.hasPreviousAddress()) {
            return;
        }

        let address = this.getLocationAddress();
        
        this.map.findLocationByAddress(address);
    }
    
    hasPreviousAddress() {
        let canton = this.cantonInput.value;
        let province = this.provinceInput.value;
        
        return province !== "Todos" || canton !== "Todos";
    }
    
    getLocationAddress() {
        let canton = this.cantonInput.value;
        let province = this.provinceInput.value;
        
        let hasCanton = canton !== "Todos";
        let hasProvince = province !== "Todos";
        
        let address = this.country + ", ";
        
        address += hasCanton? canton : "";
       
        if (!hasCanton) {
            address = hasProvince? province : "";
        } else {
            address += hasProvince? ", " + province : "";
        }
        
        return address;
    }

    clearLocation() {
        this.provinceInput.disabled = "";
        this.cantonInput.disabled = "";
        
        document.getElementById(this.latitudeElementId).value = 0;
        document.getElementById(this.longitudeElementId).value = 0;
        document.getElementById(this.addressInputId).value = "";
        this.distanceElement.value = 25;
    }
    
    submitLocation() {
        this.provinceInput.disabled = "disabled";
        this.cantonInput.disabled = "disabled";
        this.provinceInput.value = "Todos";
        this.cantonInput.value = "Todos";
    }
}

window.initMap = () => {
    let mapModal = window.GetModalMap();
    if (mapModal !== undefined) {
        mapModal.initiateMap();
    }
} 