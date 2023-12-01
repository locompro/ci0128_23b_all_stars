import { StoreGoogleMap } from './store-google-map.js'
import { GoogleMap } from './google-map.js'

// Mocking GoogleMap for dependency isolation
jest.mock('./google-map.js')

/**
 * Local mock implementation of jQuery
 */
const mockJQuery = jest.fn().mockImplementation(selector => {
    return {
        val: jest.fn().mockReturnValue(''),
        change: jest.fn(),
        find: jest.fn().mockReturnThis(),
        toArray: jest.fn().mockReturnThis(),
        trigger: jest.fn()
    }
})

// Replacing global jQuery with the mock version
global.$ = mockJQuery

// Mocking global fetch function
global.fetch = jest.fn(() =>
    Promise.resolve({
        json: () => Promise.resolve({ candidates: [{ location: { y: 35.6895, x: 139.6917 } }] })
    })
)

/**
 * Mocking the window.google object to simulate Google Maps API
 */
window.google = {
    maps: {
        Map: jest.fn(),
        Marker: jest.fn().mockImplementation(() => ({
            setPosition: jest.fn(),
            addListener: jest.fn()
        })),
        Geocoder: jest.fn().mockImplementation(() => ({
            geocode: jest.fn()
        }))
    }
}

/**
 * Test suite for StoreGoogleMap class
 */
describe('StoreGoogleMap', () => {
    let storeGoogleMap

    beforeEach(() => {
        GoogleMap.mockClear()
        mockJQuery.mockClear()
        fetch.mockClear()

        storeGoogleMap = new StoreGoogleMap('map', 'latitude', 'longitude', 'address', '#province', '#canton')
    })
    /**
     * 
     */
    it('should create an instance of GoogleMap', () => {
        expect(GoogleMap).toHaveBeenCalledWith('map', 'latitude', 'longitude', 'address')
    })
    
    /**
     * tests that the checkIfItWasAMarkerChange function returns the correct value when the counter flag is 0
     */
    it('should return false when counter flag is 0', () => {
        expect(storeGoogleMap.didMarkerChangeProvinceAndCanton).toBe(0)
        let result = storeGoogleMap.checkIfItWasAMarkerChange()
        expect(result).toBe(false)
    })
    /**
     * tests that the checkIfItWasAMarkerChange function returns the correct value when the counter flag is 2
     */
    it('should return true when counter flag is 2', () => {
        storeGoogleMap.didMarkerChangeProvinceAndCanton = 2
        expect(storeGoogleMap.didMarkerChangeProvinceAndCanton).toBe(2)
        let result = storeGoogleMap.checkIfItWasAMarkerChange()
        expect(result).toBe(true)
    })
    /**
     * tests that the checkIfItWasAMarkerChange function returns the correct value when the counter flag is 1
     */
    it('should return false when counter flag is 1', () => {
        storeGoogleMap.didMarkerChangeProvinceAndCanton = 1
        expect(storeGoogleMap.didMarkerChangeProvinceAndCanton).toBe(1)
        let result = storeGoogleMap.checkIfItWasAMarkerChange()
        expect(result).toBe(true)
    })
    /**
     * tests that the link is being constructed correctly given a coordinate
     */
    it('should return the correct link for the reverse geocoding', () => {
        let latLng = {lat: () => 35.6895, lng: () => 139.6917}
        let result = storeGoogleMap.constructReverseGeocodeUrl(latLng)
        let realResult = `https://geocode.arcgis.com/arcgis/rest/services/World/GeocodeServer/reverseGeocode?f=pjson&featureTypes=&location=${latLng.lng()},${latLng.lat()}`
        expect(result === realResult).toBe(true)
    })
    /**
     * tests that when calling reverseGeocode, fetch is called
     */
    it('should call fetch when calling reverseGeocode', () => {
        let latLng = {lat: () => 35.6895, lng: () => 139.6917}
        storeGoogleMap.reverseGeocode(latLng)
        expect(fetch).toHaveBeenCalled()
    })
})
