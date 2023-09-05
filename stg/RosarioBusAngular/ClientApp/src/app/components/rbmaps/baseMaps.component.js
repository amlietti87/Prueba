"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
// import { RbMap } from '../rbmaps/RbMap';
// import { HEROES } from '../rbmaps/RbMaps';
var RbMapMarker_1 = require("../rbmaps/RbMapMarker");
var app_component_base_1 = require("../../shared/common/app-component-base");
var BaseMapsComponent = /** @class */ (function (_super) {
    __extends(BaseMapsComponent, _super);
    function BaseMapsComponent(injector, cdRef, _service) {
        var _this = _super.call(this, injector) || this;
        _this.cdRef = cdRef;
        _this.mapId = "map";
        _this.loadOnInit = true;
        _this.OnClickMarker = new core_1.EventEmitter();
        _this.AfterAddMaker = new core_1.EventEmitter();
        _this.isCreatedMap = false;
        _this.DefaultIconID = 9;
        _this.DefaultColor = "#000000";
        //this.service = injector.get();
        _this.service = _service;
        //this.puntos = [];
        _this.dataRecorrido = new RbMapMarker_1.DataRecorrido();
        return _this;
    }
    BaseMapsComponent.prototype.ngOnInit = function () {
        if (this.loadOnInit) {
            this.crearMapa(this.latitud, this.longitud);
        }
    };
    BaseMapsComponent.prototype.InitializeList = function () {
        this.removeMarkersAndPolylines(this);
    };
    BaseMapsComponent.prototype.removeMarkersAndPolylines = function (currentMap) {
        mApp.blockPage();
        if (currentMap.map) {
            currentMap.map.removeMarkers();
        }
        mApp.unblock();
    };
    BaseMapsComponent.prototype.crearMapa = function (latitud, longitud) {
        if (latitud === void 0) { latitud = 0; }
        if (longitud === void 0) { longitud = 0; }
        this.InitializeList();
        var rbmaps = this;
        var myStyles = [
            {
                featureType: "poi",
                elementType: "labels",
                stylers: [
                    { visibility: "off" }
                ]
            }
        ];
        this.map = new GMaps({
            div: '#' + this.mapId,
            lat: latitud,
            lng: longitud,
            click: function (e) {
                rbmaps.AgregarMarcador(e.latLng, true);
            },
            styles: myStyles,
            disableDefaultUI: true
        });
        this.initContextMenu(rbmaps);
    };
    BaseMapsComponent.prototype.buscar_Gmaps = function (text, callback) {
        if (callback === void 0) { callback = null; }
        var rbmaps = this;
        if (!callback) {
            callback = function (results, status) {
                if (status == 'OK') {
                    rbmaps.successfullyGeocode(results);
                }
            };
        }
        GMaps.geocode({
            address: text,
            callback: callback
        });
    };
    BaseMapsComponent.prototype.successfullyGeocode = function (results) {
        var latlng = results[0].geometry.location;
        this.setCenter(latlng.lat(), latlng.lng());
    };
    BaseMapsComponent.prototype.initContextMenu = function (rbmaps) {
        this.map.setContextMenu({
            control: 'marker',
            options: [{
                    title: 'Eliminar',
                    name: 'delete_marker',
                    action: function (e) {
                        rbmaps.EliminarItemByMarcador(e.marker, rbmaps);
                    }
                }, {
                    title: 'Center here',
                    name: 'center_here',
                    action: function (e) {
                        this.setCenter(e.latLng.lat(), e.latLng.lng());
                    }
                }]
        });
    };
    BaseMapsComponent.prototype.crearViewMapa = function (SucursalId, puntoDefault) {
        if (puntoDefault === void 0) { puntoDefault = null; }
        this.InitializeList();
        var rbmaps = this;
        var myStyles = [
            {
                featureType: "poi",
                elementType: "labels",
                stylers: [
                    { visibility: "off" }
                ]
            }
        ];
        if (puntoDefault && puntoDefault != null) {
            this.map = new GMaps({
                div: '#' + this.mapId,
                lat: puntoDefault.Lat,
                lng: puntoDefault.Long,
                styles: myStyles,
                disableDefaultUI: true
            });
        }
        else if (!SucursalId) {
            this.map = new GMaps({
                div: '#' + this.mapId,
                lat: -32.954517,
                lng: -60.655931,
                styles: myStyles,
                disableDefaultUI: true
            });
        }
        else if (SucursalId == 1) {
            this.map = new GMaps({
                div: '#' + this.mapId,
                lat: -32.954517,
                lng: -60.655931,
                styles: myStyles,
                disableDefaultUI: true
            });
        }
        else if (SucursalId == 2) {
            this.map = new GMaps({
                div: '#' + this.mapId,
                lat: -34.54325,
                lng: -58.71917,
                styles: myStyles,
                disableDefaultUI: true
            });
        }
        else if (SucursalId == 3) {
            this.map = new GMaps({
                div: '#' + this.mapId,
                lat: -31.61863,
                lng: -60.69809,
                styles: myStyles,
                disableDefaultUI: true
            });
        }
        else {
            this.map = new GMaps({
                div: '#' + this.mapId,
                lat: -32.954517,
                lng: -60.655931,
                styles: myStyles,
                disableDefaultUI: true
            });
        }
        this.map.map.data.setStyle(function (feature) {
            var color = 'gray';
            if (feature.getProperty('color')) {
                color = feature.getProperty('color');
            }
            var icono = feature.getProperty('icono');
            return /** @type {!google.maps.Data.StyleOptions} */ ({
                fillColor: color,
                strokeColor: color,
                strokeWeight: 3,
                icon: icono
            });
        });
    };
    BaseMapsComponent.prototype.AgregarMarcador = function (latlng, center) {
        var marker = this.AgregarMarcador_lat_lng(latlng.lat(), latlng.lng(), center);
        this.AfterAddMaker.emit(marker);
    };
    BaseMapsComponent.prototype.AgregarMarcador_lat_lng = function (lat, lng, center, callback) {
        var marker = new RbMapMarker_1.RbMapMarker(lat, lng);
        marker.icon.url = this.service.markerIcon(this.DefaultIconID);
        var rbm = this;
        marker.SetDragend(function (e) {
            rbm.MoverMarcador(this, e, rbm);
        });
        marker.SetClick(function (e) {
            rbm.ClickMarcador(this, e, rbm);
        });
        this.map.addMarker(marker);
        if (center) {
            this.map.setCenter(lat, lng);
        }
        return marker;
    };
    BaseMapsComponent.prototype.MoverMarcador = function (marcador, e, rbm) {
        marcador.lat = marcador.position.lat();
        marcador.lng = marcador.position.lng();
        this.AfterAddMaker.emit(marcador);
    };
    BaseMapsComponent.prototype.ClickMarcador = function (marcador, e, rbm) {
        //rbm.onClickMarker(marcador);
    };
    BaseMapsComponent.prototype.EliminarItemByMarcador = function (marcador, rbm) {
    };
    BaseMapsComponent.prototype.removeRutas = function (puntos) {
        puntos.forEach(function (p) { if (p.Polylines)
            p.Polylines.forEach(function (pol) { return pol.setMap(null); }); });
    };
    BaseMapsComponent.prototype.hacerRuta = function (puntoInicio, puntoFin, reemplazar, currentMap, callback) {
        var rbm = currentMap;
        //mApp.blockPage();
        var origen = [puntoInicio.Lat, puntoInicio.Long];
        var destino = [puntoFin.Lat, puntoFin.Long];
        rbm.map.travelRoute({
            origin: origen,
            destination: destino,
            travelMode: 'driving',
            end: function (e) {
                //console.log("end", e);
                mApp.unblock();
            },
            step: function (e, total) {
                rbm.agregarPolyline(puntoFin, e.path, rbm);
                if (!puntoFin.Steps)
                    puntoFin.Steps = [];
                puntoFin.Steps.push(e);
                //rbm.calcularDistanciaTotal();
                if (total == e.step_number) {
                    //if (reemplazar) rbm.reemplazarPunto(puntoNuevo);
                    //else rbm.agregarPunto(puntoNuevo, rbm);
                    if (callback)
                        callback();
                }
            }
        });
    };
    BaseMapsComponent.prototype.agregarPolyline = function (punto, path, rbm) {
        var p = rbm.map.drawPolyline({
            path: path,
            strokeColor: punto.Color || punto.LineColor,
            strokeOpacity: 0.6,
            strokeWeight: 6,
            click: function (e) {
                //// Insertar nuevo_marcador antes de marcador
                //// Mover nuevo_marcador
                rbm.AgregarMarcador_Click_Linea(e.latLng, punto, false);
            }
        });
        if (!punto.Polylines)
            punto.Polylines = [];
        punto.Polylines.push(p);
    };
    BaseMapsComponent.prototype.AgregarMarcador_Click_Linea = function (latlng, marcadorSiguiente, center) {
    };
    BaseMapsComponent.prototype.findMapMarker = function (id) {
        var index = this.map.markers.findIndex(function (fruit) { return fruit.details.id === id; });
        var marker = this.map.markers[index];
        return marker;
    };
    BaseMapsComponent.prototype.setCenter = function (lat, lng) {
        this.map.setCenter(lat, lng);
    };
    BaseMapsComponent.prototype.GenerarPuntosGeoJson = function (puntos) {
        var feacture = this.map.map.data.addGeoJson(puntos);
        return feacture;
        //map.data.loadGeoJson('https://storage.googleapis.com/mapsdevsite/json/google.json');
    };
    BaseMapsComponent.prototype.RemoveFeacture = function (key) {
        var self = this;
        this.map.map.data.forEach(function (e) {
            //e.setProperty('visible', false);
            //self.map.map.data.overrideStyle(e, { visible: 8 });
            if (e.getProperty('key') == key) {
                self.map.map.data.remove(e);
                //self.map.map.data.overrideStyle(e, { strokeWeight: 8 });
                //self.map.map.data.overrideStyle(e, { visibility: 'off' });
            }
        });
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], BaseMapsComponent.prototype, "loadOnInit", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], BaseMapsComponent.prototype, "OnClickMarker", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], BaseMapsComponent.prototype, "AfterAddMaker", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], BaseMapsComponent.prototype, "latitud", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], BaseMapsComponent.prototype, "longitud", void 0);
    return BaseMapsComponent;
}(app_component_base_1.AppComponentBase));
exports.BaseMapsComponent = BaseMapsComponent;
//# sourceMappingURL=baseMaps.component.js.map