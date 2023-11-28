import { GoogleMap } from './google-map'
// Author: A. Badilla Olivas B80874
// Sprint 3
// Mock functions for the Google Maps API
const mockGeocode = jest.fn()
const mockSetCenter = jest.fn()
const mockAddListener = jest.fn()

// Mocking the DOM elements
const mockElements = {}

/**
 * Mock implementation of document.getElementById
 * Stores and retrieves mock elements based on their IDs.
 */
document.getElementById = jest.fn().mockImplementation(id => {
    if (!mockElements[id]) {
        mockElements[id] = {
            value: '',
            addEventListener: jest.fn()
        }
    }
    return mockElements[id]
})

/**
 * Mock of the window.google global object, simulating the Google Maps JavaScript API
 */
window.google = {
    maps: {
        Map: jest.fn().mockImplementation(() => ({
            setCenter: mockSetCenter
        })),
        Marker: jest.fn().mockImplementation(() => ({
            setPosition: jest.fn(),
            addListener: mockAddListener
        })),
        Geocoder: jest.fn().mockImplementation(() => ({
            geocode: mockGeocode
        })),
        LatLng: jest.fn()
    }
}

/**
 * Mock of navigator.geolocation, simulating geolocation functionality
 */
navigator.geolocation = {
    getCurrentPosition: jest.fn().mockImplementation((success, error) => {
        return success({ coords: { latitude: 35.6895, longitude: 139.6917 }})
    })
}

/**
 * Test suite for GoogleMap class
 */
describe('GoogleMap', () => {
    let googleMap

    beforeEach(() => {
        // Initialize GoogleMap instance before each test
        googleMap = new GoogleMap('map', 'latitude', 'longitude', 'address')
    })

    /**
     * Test if GoogleMap is initialized with the user's location
     */
    it('should initialize map with user location', () => {
        expect(window.google.maps.Map).toHaveBeenCalled()
        expect(navigator.geolocation.getCurrentPosition).toHaveBeenCalled()
    })

    /**
     * Test if GoogleMap can find and display a location based on an address
     */
    it('should find and display location by address', () => {
        const mockAddress = "Tokyo Tower, Japan"
        const mockLocation = { lat: 35.6586, lng: 139.7454 }

        mockGeocode.mockImplementation((request, callback) => {
            if (request.address === mockAddress) {
                callback([{ geometry: { location: mockLocation } }], 'OK')
            }
        })

        googleMap.findLocationByAddress(mockAddress)

        expect(mockGeocode).toHaveBeenCalledWith({ 'address': mockAddress }, expect.any(Function))
        expect(mockSetCenter).toHaveBeenCalledWith(mockLocation)
        expect(mockElements['latitude'].value).toBe(mockLocation.lat)
        expect(mockElements['longitude'].value).toBe(mockLocation.lng)
    })

    /**
     * Test if GoogleMap handles geocode failures gracefully
     */
    it('should handle geocode failures gracefully', () => {
        const mockAddress = "Invalid Address"

        mockGeocode.mockImplementation((request, callback) => {
            if (request.address === mockAddress) {
                callback([], 'ZERO_RESULTS')
            }
        })

        console.log = jest.fn()

        googleMap.findLocationByAddress(mockAddress)

        expect(mockGeocode).toHaveBeenCalledWith({ 'address': mockAddress }, expect.any(Function))
        expect(console.log).toHaveBeenCalledWith('Geocode was not successful for the following reason: ZERO_RESULTS')
    })

})
