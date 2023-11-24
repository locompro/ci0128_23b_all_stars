import { StoreGoogleMap } from "./store-google-map.js";

function initMap() {
    new StoreGoogleMap("StoreModalMap", "latitude", "longitude", "MapGeneratedAddress", "#partialStoreProvince","#partialStoreCanton")
}

window.initMap = initMap;

