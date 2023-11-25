﻿import { GoogleMap } from './google-map.js';
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
            console.log("cursor fue movido.")
            this.updateLocationFromMarker()
            this.reverseGeocode(this.marker.getPosition())
        })
    }
    /**
     * Adds change event listeners to province and canton input elements.
     */
    prepare() {
        this.provinceInput.change(() =>{
            console.log("provincia fue cambiada")
            this.onLocationChange()
        })
        this.cantonInput.change(() => {
            console.log("canton fue cambiado")
            this.onLocationChange()
        })
    }
    /**
     * Handles location change events for province and canton input elements.
     */
    onLocationChange() {
        this.updateMarkerLocation(this.provinceInput.value, this.cantonInput.value)
    }
    /**
     * Updates the marker location on the map based on the selected province and canton.
     * @param {string} province - The selected province.
     * @param {string} canton - The selected canton.
     */
    updateMarkerLocation(province, canton) {
        if (!province || !canton) return

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
            .catch(error => console.error('Error obteniendo datos de geocodificación inversa:', error))
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
     * @param {HTMLSelectElement} selectElement - The select element to update.
     * @param {string} value - The value to select in the select element.
     */
    updateSelect(selectElement, value) {
        const optionToSelect = Array.from(selectElement.options).find(option => option.text === value)
        if (optionToSelect) selectElement.value = optionToSelect.value
    }
    
}

export { StoreGoogleMap };
