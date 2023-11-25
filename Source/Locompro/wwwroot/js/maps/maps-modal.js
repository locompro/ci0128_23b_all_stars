import {GoogleMap} from "./google-map.js";

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
            
           getAdvancedSearchMapsModal("mapModal")
        }
    });
    
    const config = { childList: true};
    modalObserver.observe(modalContainer, config);
});

function getAdvancedSearchMapsModal(elementId) {
    let cantonId = "provinceDropdown";
    let provinceId = "cantonDropdown";

    var mapModal = new MapsModal(elementId, "Costa Rica", provinceId, cantonId);

    window.GetModalMap = () => { return mapModal; };

    mapModal.setMapModalCreationListener().then(() => {});
}

class MapsModal {
    constructor(mapModalElementId, country, provinceId, cantonId) {
        this.country = country;
        this.mapModalElement = document.getElementById(mapModalElementId);
        this.provinceInput = document.getElementById(provinceId);
        this.cantonInput = document.getElementById(cantonId);
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
        this.map = new GoogleMap("StoreModalMap", "latitude", "longitude", "MapGeneratedAddress");
        
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
}

window.initMap = () => {
    let mapModal = window.GetModalMap();
    if (mapModal !== undefined) {
        mapModal.initiateMap();
    }
} 