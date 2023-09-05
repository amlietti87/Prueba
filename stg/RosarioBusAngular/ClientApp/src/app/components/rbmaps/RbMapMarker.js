"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var RbMapServices_1 = require("../rbmaps/RbMapServices");
var locator_service_1 = require("../../shared/common/services/locator.service");
var RbMapMarker = /** @class */ (function () {
    function RbMapMarker(lat, lng, id) {
        this.service = locator_service_1.LocatorService.injector.get(RbMapServices_1.RbMapServices);
        var tipo = 1;
        var tipoIcon = RbMapServices_1.MapIcons.BluePin;
        this.draggable = true;
        this.id = id || RbMapServices_1.RbMapServices.guid();
        this.lat = lat;
        this.lng = lng;
        var icon_url = this.service.markerIcon(tipoIcon);
        var icon_size = new google.maps.Size(24, 24);
        this.icon = new RbMapMarkerIcon(icon_url, icon_size);
        this.details = new RbMapMarkerDetail(this.id, tipo);
    }
    RbMapMarker.prototype.SetDragend = function (callback) {
        this.dragend = callback;
    };
    RbMapMarker.prototype.SetClick = function (callback) {
        this.click = callback;
    };
    RbMapMarker.prototype.SetSaved = function (saved) {
        this.lat = saved.lat;
        this.lng = saved.lng;
        this.details.id = saved.id;
        this.details.tipo = saved.tipo;
        this.details.info = saved.info;
        this.icon.url = this.service.markerIcon(saved.tipo);
    };
    RbMapMarker.prototype.SetDraggable = function (draggable) {
        this.draggable = draggable;
    };
    return RbMapMarker;
}());
exports.RbMapMarker = RbMapMarker;
var RbMapMarkerIcon = /** @class */ (function () {
    function RbMapMarkerIcon(url, size) {
        this.url = url;
        this.size = size;
    }
    return RbMapMarkerIcon;
}());
exports.RbMapMarkerIcon = RbMapMarkerIcon;
var RbMapMarkerDetail = /** @class */ (function () {
    function RbMapMarkerDetail(id, tipo) {
        this.id = id;
        this.tipo = tipo;
    }
    RbMapMarkerDetail.prototype.SetInfo = function (info) {
        this.info = info;
    };
    return RbMapMarkerDetail;
}());
exports.RbMapMarkerDetail = RbMapMarkerDetail;
var CustomMarker = /** @class */ (function () {
    function CustomMarker() {
        this.id = RbMapServices_1.RbMapServices.guid();
        this.info = "";
        this.tipo = 1;
    }
    return CustomMarker;
}());
exports.CustomMarker = CustomMarker;
var PuntoInfo = /** @class */ (function () {
    function PuntoInfo(marcador) {
        this.marcador = marcador;
        this.steps = [];
        this.polylines = [];
    }
    return PuntoInfo;
}());
exports.PuntoInfo = PuntoInfo;
var DataRecorrido = /** @class */ (function () {
    function DataRecorrido() {
        this._distancia = 0;
        this._tiempo = 0;
    }
    DataRecorrido.prototype.SumarTiempo = function (tiempo) {
        this._tiempo += tiempo;
    };
    DataRecorrido.prototype.RestarTiempo = function (tiempo) {
        this._tiempo -= tiempo;
    };
    DataRecorrido.prototype.SumarDistancia = function (distancia) {
        this._distancia += distancia;
    };
    DataRecorrido.prototype.RestarDistancia = function (distancia) {
        this._distancia -= distancia;
    };
    return DataRecorrido;
}());
exports.DataRecorrido = DataRecorrido;
//# sourceMappingURL=RbMapMarker.js.map