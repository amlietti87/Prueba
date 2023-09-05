GMaps.prototype.buildFromGeoJson = function (options) {

    if (!this.geoJsonList) this.geoJsonList = {};

    this.geoJsonList[options.name] = this.map.data.addGeoJson(options.geoJson);
    this.map.data.setStyle(function (feature) {
        var icon = null;
        if (feature.getProperty('icon')) {
            icon = feature.getProperty('icon');
        }
        return /** @type {google.maps.Data.StyleOptions} */ ({
            icon: icon
        });
    });


    return this.geoJsonList[options.name];
};

GMaps.prototype.removeToMapa = function (data) {
    data.setMap(null);
};


GMaps.prototype.addToMapa = function (data) {
    data.setMap(this.map);
};

GMaps.prototype.toogleGeoJsonData = function (options) {

    var f = this.map.data.getFeatureById(options.name);
    if (f) {
                this.map.data.remove(f);
            }
    else {
        f = this.geoJsonList[options.name][0];
        this.map.data.add(f);
    }
    

    
};


