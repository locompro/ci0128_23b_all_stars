/**
 * Class representing a GoogleMap instance.
 */
class GoogleMap {
    /**
     * Create a GoogleMap.
     * @param {string} mapElementId - The ID of the HTML element where the map will be rendered.
     * @param {string} latitudeInputId - The ID of the input element for the latitude.
     * @param {string} longitudeInputId - The ID of the input element for the longitude.
     * @param {string} addressInputId - The ID of the input element for the address.
     */
    constructor(mapElementId, latitudeInputId, longitudeInputId, addressInputId) {
        this.mapElement = document.getElementById(mapElementId);
        this.latitudeInput = document.getElementById(latitudeInputId);
        this.longitudeInput = document.getElementById(longitudeInputId);
        this.addressInput = document.getElementById(addressInputId);
        this.geocoder = new google.maps.Geocoder();
        this.initMap();
    }
    /**
     * Initialize the map, setting the user's current location as the center.
     * If geolocation fails, use a default location.
     */
    initMap() {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition((position) => {
                const userLocation = {
                    lat: position.coords.latitude,
                    lng: position.coords.longitude
                };
                
                this.createMapAtLocation(userLocation);
            }, () => {
                this.useDefaultLocation();
            });
        } else {
            this.useDefaultLocation();
        }
    }
    /**
     * Create the map at the specified location and add a marker.
     * @param {Object} location - The location to center the map on.
     */
    createMapAtLocation(location) {
        this.map = new google.maps.Map(this.mapElement, {
            zoom: 8,
            center: location,
        });
        this.addMarker(location);
        this.updateLocationDetails(location);

    }
    /**
     * Set the map to a default location.
     */
    useDefaultLocation() {
        const defaultLocation = { lat: 9.7489, lng: -83.7534 };
        this.createMapAtLocation(defaultLocation);
    }
    /**
     * Add a draggable marker to the map at the specified location.
     * @param {Object} location - The location to add the marker to.
     */
    addMarker(location) {
        this.marker = new google.maps.Marker({
            position: location,
            map: this.map,
            draggable: true
        });

        this.marker.addListener('dragend', () => {
            this.updateLocationFromMarker();
        });
    }
    /**
     * Update the latitude and longitude input elements based on the provided location.
     * @param {Object} latLng - The latitude and longitude to update the inputs with.
     */
    updateLocationDetails(latLng) {
        // Check if latLng is a Google Maps LatLng object or a plain object
        const latitude = typeof latLng.lat === 'function' ? latLng.lat() : latLng.lat;
        const longitude = typeof latLng.lng === 'function' ? latLng.lng() : latLng.lng;

        this.latitudeInput.value = latitude;
        this.longitudeInput.value = longitude;
        this.getAddressFromLatLng({ lat: latitude, lng: longitude });
    }

    /**
     * Use the geocoder to get the address for a given latitude and longitude and update the address input.
     * @param {Object} latLng - The latitude and longitude to get the address for.
     */
    getAddressFromLatLng(latLng) {
        this.geocoder.geocode({ 'location': latLng }, (results, status) => {
            if (status === 'OK') {
                if (results[0]) {
                    this.addressInput.value = results[0].formatted_address;
                } else {
                    console.log('No results found');
                }
            } else {
                console.log('Geocoder failed due to: ' + status);
            }
        });
    }

    /**
     * Finds and displays the location based on the provided address string.
     * @param {string} address - The address to locate on the map.
     */
    findLocationByAddress(address) {
        this.geocoder.geocode({ 'address': address }, (results, status) => {
            if (status === 'OK') {
                const location = results[0].geometry.location;
                this.map.setCenter(location);
                this.marker.setPosition(location);
                this.updateLocationDetails(location);
            } else {
                console.log('Geocode was not successful for the following reason: ' + status);
            }
        });
    }
    /**
     * Update the location details based on the current position of the marker.
     */
    updateLocationFromMarker() {
        const latLng = this.marker.getPosition();
        this.updateLocationDetails(latLng);
    }
}

export { GoogleMap };
