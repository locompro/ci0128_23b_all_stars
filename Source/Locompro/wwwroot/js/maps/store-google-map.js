import { GoogleMap } from './google-map.js';
/**
 * Class representing a StoreGoogleMap instance which extends GoogleMap.
 * It adds functionality to update the map marker based on selected province and canton.
 */
class StoreGoogleMap extends GoogleMap {
    /**
     * Create a StoreGoogleMap.
     * @param {string} mapElementId - The ID of the HTML element where the map will be rendered.
     * @param {string} latitudeInputId - The ID of the input element for the latitude.
     * @param {string} longitudeInputId - The ID of the input element for the longitude.
     * @param {string} addressInputId - The ID of the input element for the address.
     * @param {string} provinceInputId - The ID of the input element for the province.
     * @param {string} cantonInputId - The ID of the input element for the canton.
     */
    constructor(mapElementId, latitudeInputId, longitudeInputId, addressInputId, provinceInputId, cantonInputId) {
        super(mapElementId, latitudeInputId, longitudeInputId, addressInputId)
        this.provinceInput = $(provinceInputId)
        this.cantonInput = $(cantonInputId)
        this.didMarkerChangeProvinceAndCanton = 0
    }

    /**
     * sets a counter flag to signal that the marker changed the province and canton
     */
    signalThatMarkerChangedProvinceAndCanton() {
        this.didMarkerChangeProvinceAndCanton = 2
    }
    /**
     * 
     * @param location
     */
    addMarker(location) {
        this.marker = new google.maps.Marker({
            position: location,
            map: this.map,
            draggable: true
        })

        this.marker.addListener('dragend', () => {
            this.signalThatMarkerChangedProvinceAndCanton()
            
            this.updateLocationFromMarker()
            this.reverseGeocode(this.marker.getPosition())
            
        })
    }
    /**
     * Adds change event listeners to province and canton input elements.
     */
    prepare() {
        this.provinceInput.change(() => this.onLocationChange())
        this.cantonInput.change(() => this.onLocationChange())
    }

    /**
     * checks if the marker was the reason for the change in province and canton
     * this is important so that the marker doesn't get updated when it is the one that changed the province and canton
     * @returns {boolean}
     */
    checkIfItWasAMarkerChange() {
        if (this.didMarkerChangeProvinceAndCanton) {
            this.didMarkerChangeProvinceAndCanton -= 1
            return true
        }
        return false
    }
    /**
     * Handles location change events for province and canton input elements.
     */
    onLocationChange() {
        if (this.checkIfItWasAMarkerChange()) return
        
        const provinceValue = this.provinceInput.val();
        const cantonValue = this.cantonInput.val();
        
        if (!provinceValue || !cantonValue) return
        
        this.updateMarkerLocation(provinceValue, cantonValue);
    }
    /**
     * Updates the marker location on the map based on the selected province and canton.
     * @param {string} province - The selected province.
     * @param {string} canton - The selected canton.
     */
    updateMarkerLocation(province, canton) {
        const geocodeUrl = this.constructGeocodeUrl(canton, province)
        this.fetchLocationAndUpdateMap(geocodeUrl)
    }
    /**
     * Constructs the geocode URL for a given canton and province.
     * @param {string} canton - The selected canton.
     * @param {string} province - The selected province.
     * @returns {string} The constructed geocode URL.
     */
    constructGeocodeUrl(canton, province) {
        return 'https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/findAddressCandidates?singleLine='
            + encodeURIComponent(canton) + ','
            + encodeURIComponent(province) + '&f=pjson'
    }
    /**
     * Fetches the location data from the geocode URL and updates the map accordingly.
     * @param {string} geocodeUrl - The geocode URL to fetch location data from.
     */
    fetchLocationAndUpdateMap(geocodeUrl) {
        fetch(geocodeUrl)
            .then(response => response.json())
            .then(data => {
                if (data.candidates.length > 0) {
                    const location = data.candidates[0].location
                    this.setMapCenter(location)
                    this.updateLocationInputs(location)
                }
            })
            .catch(error => console.error('Error obteniendo datos de geocodificación:', error))
    }
    /**
     * Sets the center of the map to a given location and updates the marker position.
     * @param {Object} location - The location to set as the map center.
     */
    setMapCenter(location) {
        const newCenter = { lat: location.y, lng: location.x }
        this.map.setCenter(newCenter)
        this.marker.setPosition(newCenter)
    }
    /**
     * Updates the latitude and longitude input elements based on the given location.
     * @param {Object} location - The location to update the inputs with.
     */
    updateLocationInputs(location) {
        this.updateLocationDetails({ lat: location.y, lng: location.x })
    }
    /**
     * Performs reverse geocoding for a given latitude and longitude.
     * @param {Object} latLng - The latitude and longitude to reverse geocode.
     */
    reverseGeocode(latLng) {
        const reverseGeocodeUrl = this.constructReverseGeocodeUrl(latLng)

        fetch(reverseGeocodeUrl)
            .then(response => response.json())
            .then(data => {
                if (data.address) {
                    this.updateSelect(this.provinceInput, data.address.Region)
                    this.updateSelect(this.cantonInput, data.address.Subregion)
                }
            })
            .catch(error => console.error('Error en geocodificación inversa:', error))
    }
    /**
     * Constructs the reverse geocode URL for a given latitude and longitude.
     * @param {Object} latLng - The latitude and longitude to use for constructing the URL.
     * @returns {string} The constructed reverse geocode URL.
     */
    constructReverseGeocodeUrl(latLng) {
        return `https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/reverseGeocode?f=pjson&featureTypes=&location=${latLng.lng()},${latLng.lat()}`
    }
    /**
     * Updates the select element to select the option that matches the given value.
     * @param {Object} $selectElement - The select element as a jQuery object.
     * @param {string} value - The value to select in the select element.
     */
    updateSelect($selectElement, value) {
        const optionToSelect = $selectElement.find('option').toArray().find(option => option.text === value)

        if (optionToSelect) {
            $selectElement.val(optionToSelect.value).trigger('change')
        }
    }


}

export { StoreGoogleMap }
