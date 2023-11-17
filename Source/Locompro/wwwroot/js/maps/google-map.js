export class GoogleMap {
    constructor(mapElementId, latitudeInputId, longitudeInputId, addressInputId) {
        this.mapElement = document.getElementById(mapElementId);
        this.latitudeInput = document.getElementById(latitudeInputId);
        this.longitudeInput = document.getElementById(longitudeInputId);
        this.addressInput = document.getElementById(addressInputId);
        this.geocoder = new google.maps.Geocoder();
        this.initMap();
    }

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

    createMapAtLocation(location) {
        this.map = new google.maps.Map(this.mapElement, {
            zoom: 8,
            center: location,
        });
        this.addMarker(location);
        this.updateLocationDetails(location);

    }

    useDefaultLocation() {
        const defaultLocation = { lat: 9.7489, lng: -83.7534 };
        this.createMapAtLocation(defaultLocation);
    }

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

    updateLocationDetails(latLng) {
        // Check if latLng is a Google Maps LatLng object or a plain object
        const latitude = typeof latLng.lat === 'function' ? latLng.lat() : latLng.lat;
        const longitude = typeof latLng.lng === 'function' ? latLng.lng() : latLng.lng;

        this.latitudeInput.value = latitude;
        this.longitudeInput.value = longitude;
        this.getAddressFromLatLng({ lat: latitude, lng: longitude });
    }


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

    updateLocationFromMarker() {
        const latLng = this.marker.getPosition();
        this.updateLocationDetails(latLng);
    }
}
