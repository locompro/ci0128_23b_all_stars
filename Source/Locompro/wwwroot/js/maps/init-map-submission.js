import { StoreGoogleMap } from './store-google-map.js';
/**
 * Initializes the Google Map.
 * This function creates a new instance of the StoreGoogleMap class with specified parameters.
 * It is intended to be used as a callback for the Google Maps API once it's loaded.
 */

function initMap() {
    const map = new StoreGoogleMap(
        "StoreModalMap",
        "latitude",
        "longitude",
        "MapGeneratedAddress",
        "#partialStoreProvince",
        "#partialStoreCanton")
    map.prepare()
}

// Make initMap globally accessible
window.initMap = initMap

/**
 * Dynamically loads the Google Maps API script.
 * Inserts a script tag with the Google Maps API URL into the document head.
 *
 * @param {string} apiKey - The API key for Google Maps.
 */
function loadGoogleMapsApi(apiKey) {
    const script = document.createElement('script')
    // Remove quotation marks from the API key
    apiKey = apiKey.replace(/"/g, "");
    script.src = `https://maps.googleapis.com/maps/api/js?key=${apiKey}&callback=initMap`
    script.async = true
    script.defer = true
    document.head.appendChild(script)
}

/**
 * Fetches the Google Maps API key from the backend.
 * @returns {Promise<string>} A promise that resolves to the API key.
 */
function fetchApiKey() {
    return fetch('/Submissions/Create?handler=GoogleMapsApiKey')
        .then(response => {
            if (!response.ok) {
                throw new Error(`Problemas con la respuesta de la red: ${response.statusText}`)
            }
            return response.text()
        })
}

/**
 * Handles the successful retrieval of the API key.
 * @param {string} apiKey - The fetched API key.
 */
function handleApiKeySuccess(apiKey) {
    if (apiKey) {
        loadGoogleMapsApi(apiKey);
    } else {
        console.error('No se pudo obtener la clave de la API de Google Maps')
    }
}

/**
 * Handles any errors that occur during the API key fetching process.
 * @param {Error} error - The error that occurred.
 */
function handleApiKeyError(error) {
    console.error('Error fetching Google Maps API key:', error)
}

/**
 * Initiates the process of fetching the API key and then loading the Google Maps API.
 */
function fetchApiKeyAndLoadMap() {
    fetchApiKey()
        .then(handleApiKeySuccess)
        .catch(handleApiKeyError)
}

// Load the Google Maps API after fetching the API key, depending on document readiness
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', fetchApiKeyAndLoadMap)
} else {
    fetchApiKeyAndLoadMap()
}
