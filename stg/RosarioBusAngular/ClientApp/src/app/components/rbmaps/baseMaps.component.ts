import { Input, OnInit, ChangeDetectorRef, Injector, EventEmitter, Output } from '@angular/core';

// import { RbMap } from '../rbmaps/RbMap';
// import { HEROES } from '../rbmaps/RbMaps';

import { RbMapMarker, DataRecorrido } from '../rbmaps/RbMapMarker';
import { RbMapServices } from './RbMapServices';
import { AppComponentBase } from '../../shared/common/app-component-base';
import { PuntoDto } from '../../theme/pages/planificacion/model/punto.model';



declare var GMaps: any;
declare var google: any;
declare var bootbox: any;
declare var $: any;
declare let mApp: any;




export interface IBaseMapsComponent {
    map: any;
    AgregarMarcador_Click_Linea(latlng, marcadorSiguiente: PuntoDto, center: boolean)
}


export abstract class BaseMapsComponent extends AppComponentBase implements IBaseMapsComponent, OnInit {

    mapId: string = "map";
    @Input() loadOnInit: boolean = true;
    @Output() OnClickMarker: EventEmitter<RbMapMarker> = new EventEmitter<RbMapMarker>();
    @Output() AfterAddMaker: EventEmitter<RbMapMarker> = new EventEmitter<RbMapMarker>();

    paretId: number;
    map: any;
    isCreatedMap: boolean = false;

    service: RbMapServices;

    dataRecorrido: DataRecorrido;
    OriginalData: any;
    DefaultIconID: number = 9;
    protected DefaultColor: string = "#000000";

    @Input() latitud: number;
    @Input() longitud: number;

    constructor(injector: Injector,
        protected cdRef: ChangeDetectorRef, _service: RbMapServices) {
        super(injector);

        //this.service = injector.get();
        this.service = _service;

        //this.puntos = [];
        this.dataRecorrido = new DataRecorrido();
    }

    ngOnInit() {
        if (this.loadOnInit) {
            this.crearMapa(this.latitud, this.longitud);
        }
    }

    InitializeList(): void {
        this.removeMarkersAndPolylines(this);
    }

    removeMarkersAndPolylines(currentMap: IBaseMapsComponent) {

        mApp.blockPage();

        if (currentMap.map) {
            currentMap.map.removeMarkers();
        }

        mApp.unblock();
    }

    crearMapa(latitud: number = 0, longitud: number = 0): void {
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
            click: function(e) {
                rbmaps.AgregarMarcador(e.latLng, true);
            },
            styles: myStyles,
            disableDefaultUI: true
        });




        this.initContextMenu(rbmaps);
    }



    buscar_Gmaps(text, callback = null): void {
        var rbmaps = this;
        if (!callback) {
            callback = function(results, status) {
                if (status == 'OK') {
                    rbmaps.successfullyGeocode(results);
                }
            };
        }


        GMaps.geocode({
            address: text,
            callback: callback
        });
    }


    successfullyGeocode(results: any): void {
        var latlng = results[0].geometry.location;
        this.setCenter(latlng.lat(), latlng.lng());
    }


    protected initContextMenu(rbmaps: BaseMapsComponent) {
        this.map.setContextMenu({
            control: 'marker',
            options: [{
                title: 'Eliminar',
                name: 'delete_marker',
                action: function(e) {
                    rbmaps.EliminarItemByMarcador(e.marker, rbmaps);
                }
            }, {
                title: 'Center here',
                name: 'center_here',
                action: function(e) {
                    this.setCenter(e.latLng.lat(), e.latLng.lng());
                }
            }]
        });
    }

    crearViewMapa(SucursalId?: number, puntoDefault: PuntoDto = null): void {
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

        this.map.map.data.setStyle(function(feature) {


            var color = 'gray';

            if (feature.getProperty('color')) {
                color = feature.getProperty('color');
            }

            var icono = feature.getProperty('icono');


            return /** @type {!google.maps.Data.StyleOptions} */({
                fillColor: color,
                strokeColor: color,
                strokeWeight: 3,
                icon: icono
            });
        });

    }



    AgregarMarcador(latlng, center): void {

        var marker = this.AgregarMarcador_lat_lng(latlng.lat(), latlng.lng(), center);
        this.AfterAddMaker.emit(marker);
    }

    AgregarMarcador_lat_lng(lat: any, lng: any, center, callback?: any): RbMapMarker {

        var marker = new RbMapMarker(lat, lng);
        marker.icon.url = this.service.markerIcon(this.DefaultIconID);
        var rbm = this;

        marker.SetDragend(function(e) {
            rbm.MoverMarcador(this, e, rbm);
        });

        marker.SetClick(function(e) {
            rbm.ClickMarcador(this, e, rbm);
        })

        this.map.addMarker(marker);

        if (center) {
            this.map.setCenter(lat, lng);
        }

        return marker;
    }

    MoverMarcador(marcador: RbMapMarker, e: any, rbm: IBaseMapsComponent) {
        marcador.lat = marcador.position.lat();
        marcador.lng = marcador.position.lng();
        this.AfterAddMaker.emit(marcador);
    }



    ClickMarcador(marcador: RbMapMarker, e: any, rbm: IBaseMapsComponent) {

        //rbm.onClickMarker(marcador);
    }

    EliminarItemByMarcador(marcador: RbMapMarker, rbm: BaseMapsComponent) {

    }

    removeRutas(puntos: PuntoDto[]) {
        puntos.forEach(p => { if (p.Polylines) p.Polylines.forEach(pol => pol.setMap(null)); });
    }

    hacerRuta(puntoInicio: PuntoDto, puntoFin: PuntoDto, reemplazar: boolean, currentMap: IBaseMapsComponent, callback?: any) {

        var rbm = currentMap as BaseMapsComponent;

        //mApp.blockPage();



        var origen = [puntoInicio.Lat, puntoInicio.Long];
        var destino = [puntoFin.Lat, puntoFin.Long];


        rbm.map.travelRoute({
            origin: origen,
            destination: destino,
            travelMode: 'driving',
            end: function(e) {
                //console.log("end", e);
                mApp.unblock();

            },
            step: function(e, total) {
                rbm.agregarPolyline(puntoFin, e.path, rbm);

                if (!puntoFin.Steps)
                    puntoFin.Steps = [];

                puntoFin.Steps.push(e);
                //rbm.calcularDistanciaTotal();

                if (total == e.step_number) {
                    //if (reemplazar) rbm.reemplazarPunto(puntoNuevo);
                    //else rbm.agregarPunto(puntoNuevo, rbm);

                    if (callback) callback();



                }
            }
        });
    }

    agregarPolyline(punto: PuntoDto, path: any, rbm: IBaseMapsComponent) {
        var p = rbm.map.drawPolyline({
            path: path,
            strokeColor: punto.Color || punto.LineColor, //'#131540',
            strokeOpacity: 0.6,
            strokeWeight: 6,
            click: function(e) {
                //// Insertar nuevo_marcador antes de marcador
                //// Mover nuevo_marcador
                rbm.AgregarMarcador_Click_Linea(e.latLng, punto, false);
            }
        });

        if (!punto.Polylines) punto.Polylines = [];

        punto.Polylines.push(p);
    }


    AgregarMarcador_Click_Linea(latlng, marcadorSiguiente: PuntoDto, center: boolean): void {

    }


    protected findMapMarker(id: any): any {
        const index = this.map.markers.findIndex(fruit => fruit.details.id === id);
        var marker = this.map.markers[index];
        return marker;
    }

    setCenter(lat, lng): void {
        this.map.setCenter(lat, lng);
    }

    GenerarPuntosGeoJson(puntos: GeoJSON.FeatureCollection): any {

        var feacture = this.map.map.data.addGeoJson(puntos);

        return feacture;

        //map.data.loadGeoJson('https://storage.googleapis.com/mapsdevsite/json/google.json');
    }



    RemoveFeacture(key: string) {
        var self = this;

        this.map.map.data.forEach(e => {
            //e.setProperty('visible', false);
            //self.map.map.data.overrideStyle(e, { visible: 8 });

            if (e.getProperty('key') == key) {
                self.map.map.data.remove(e);
                //self.map.map.data.overrideStyle(e, { strokeWeight: 8 });
                //self.map.map.data.overrideStyle(e, { visibility: 'off' });
            }

        });


    }

}
