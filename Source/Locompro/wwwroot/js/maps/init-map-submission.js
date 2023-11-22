import { GoogleMap } from "./google-map.js";

function initMap() {
    new GoogleMap("StoreModalMap", "latitude", "longitude", "MapGeneratedAddress");
}

window.initMap = initMap;
