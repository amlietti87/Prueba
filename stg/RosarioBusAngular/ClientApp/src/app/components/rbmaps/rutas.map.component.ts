import { Component, Input, OnInit, group, ViewChild, ChangeDetectorRef, Injector, EventEmitter, Output, DoCheck, SimpleChanges, SimpleChange, OnChanges, IterableDiffers, KeyValueDiffer, KeyValueDiffers } from '@angular/core';

// import { RbMap } from '../rbmaps/RbMap';
// import { HEROES } from '../rbmaps/RbMaps';

import { RbMapMarker, CustomMarker, PuntoInfo, DataRecorrido } from '../rbmaps/RbMapMarker';
import { RbMapGrupo } from '../rbmaps/RbMapGrupo';
import { RbMapLinea } from '../rbmaps/RbMapLinea';

import { RbMapServicesGrupo } from './RbMapServicesGrupo';
import { RbMapServicesLinea } from './RbMapServicesLinea';
import { RbMapServices } from './RbMapServices';
import { AppComponentBase } from '../../shared/common/app-component-base';
import { PuntoDto } from '../../theme/pages/planificacion/model/punto.model';
import { CreateOrEditPuntoModalComponent } from '../punto/create-or-edit-punto-modal.component';
import { ViewMode } from '../../shared/model/base.model';
import { retry } from 'rxjs/operators/retry';
import { elementAt } from 'rxjs/operator/elementAt';
import { NgbModal, ModalDismissReasons, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { SectorDto } from '../../theme/pages/planificacion/model/sector.model';
import { BaseMapsComponent, IBaseMapsComponent } from './baseMaps.component';
import { ConfigurationService } from '../../shared/common/services/configuration.service';


declare var GMaps: any;
declare var google: any;
declare var bootbox: any;
declare var $: any;
declare let mApp: any;

@Component({
    selector: 'app-rutasmaps',
    templateUrl: './rutas.map.component.html',
    styleUrls: ['./rbmaps.component.css']
})

export class RutasMapComponent extends BaseMapsComponent implements OnInit {



    @Output() SaveRuta: EventEmitter<any[]> = new EventEmitter<any[]>();
    @Output() SavePunto: EventEmitter<any> = new EventEmitter<any>();

    @Input() puntosdto: Array<PuntoDto>;
    sectoresdto: SectorDto[];

    @Input() maxMarker: number;
    dataRecorrido: DataRecorrido;
    OriginalData: any;



    constructor(injector: Injector,
        protected cdRef: ChangeDetectorRef, _service: RbMapServices) {
        super(injector, cdRef, _service);
        //this.service = new RbMapServices();
        this.dataRecorrido = new DataRecorrido();
    }

    ngOnInit() {



        if (this.loadOnInit) {
            this.crearMapa();
        }
    }

    InitializeList(): void {
        this.removeMarkersAndPolylines(this);
        this.puntosdto = [];
    }


    setRuta(puntosdto: PuntoDto[], sectoresdto: SectorDto[], paretId: number) {

        var This = this;

        console.log("sectoresdto", sectoresdto);

        this.puntosdto = puntosdto;
        this.sectoresdto = sectoresdto;
        this.paretId = paretId;
        this.findOrdenSectores(this.sectoresdto);

        try {
            var p = JSON.stringify(puntosdto);
            var s = JSON.stringify(sectoresdto);
            this.OriginalData = { puntosdto: p, sectoresdto: s };
        } catch (e) {
            console.log(puntosdto);
        }

        puntosdto.forEach(item => {
            This.AgregarMarcadorDto(item);
        });


        if (this.puntosdto.length > 0) {
            var up = this.puntosdto[this.puntosdto.length - 1];
            this.goTo(up.Lat, up.Long);
        }
    }




    private pintarPolylines(punto: PuntoDto): void {

        if (punto.Polylines) {
            punto.Polylines.forEach(po => {
                if (po) {
                    po.setOptions({ strokeColor: punto.Color || punto.LineColor });
                }
            })
        }

    }




    private findOrdenSectores(sectores: SectorDto[]) {
        var This = this;
        sectores.forEach(sector => {
            var puntoInicio = This.puntosdto.filter(p => p.Id == sector.PuntoInicioId)[0];
            var puntoFin = This.puntosdto.filter(p => p.Id == sector.PuntoFinId)[0];

            sector.OrdenInicio = puntoInicio.Orden;
            sector.OrdenFin = puntoFin.Orden;

            sector.Descripcion = (puntoInicio.Abreviacion || sector.OrdenInicio) + '-' + (puntoFin.Abreviacion || sector.OrdenFin);


        });
    }





    private BuscarCambioSectorAnterior(puntoDto: PuntoDto): PuntoDto {
        var p = this.puntosdto.findIndex(e => e.Id == puntoDto.Id);
        for (var i = p - 1; i != 0; i--) {
            if (this.puntosdto[i].Id != puntoDto.Id && this.puntosdto[i].EsCambioSector) {
                return this.puntosdto[i];
            }
        }
        return this.puntosdto[0];
    }

    private BuscarCambioSectorPosterior(puntoDto: PuntoDto): PuntoDto {
        var p = this.puntosdto.findIndex(e => e.Id == puntoDto.Id);
        for (var i = p + 1; i < this.puntosdto.length - 1; i++) {
            if (this.puntosdto[i].Id != puntoDto.Id && this.puntosdto[i].EsCambioSector) {
                return this.puntosdto[i];
            }
        }
        return this.puntosdto[this.puntosdto.length - 1];
    }


    buscar_Gmaps(text): void {
        var rbmaps = this;
        GMaps.geocode({
            address: text,
            callback: function(results, status) {
                if (status == 'OK') {
                    var latlng = results[0].geometry.location;
                    rbmaps.goTo(latlng.lat(), latlng.lng());
                }
            }
        });
    }

    goTo(lat, lng): void {
        this.map.setCenter(lat, lng);
    }


    removePolylineArray(plArray) {
        if (plArray && plArray.length > 0) {
            plArray.forEach(element => {
                if (element) element.setMap(null);
            });

            plArray = [];
        }
    }



    AgregarMarcador(latlng, center): void {


    }


    refreshOrden(): void {
        var i = 1;
        this.puntosdto.forEach(item => {
            item.Orden = i++;
        });

        this.findOrdenSectores(this.sectoresdto);
    }

    refreshIcon(punto: PuntoDto) {
        this.findMapMarker(punto.Id).setIcon(this.service.markerIconFromDto(punto));
    }

    getMarkerFromDTO(puntodto: PuntoDto): RbMapMarker {
        var marker = new RbMapMarker(puntodto.Lat, puntodto.Long, puntodto.Id);
        var rbm = this;


        marker.draggable = false;
        //marker.SetDragend(function (e) {
        //    rbm.MoverMarcador(this, e, rbm);
        //});

        //marker.SetClick(function (e) {
        //    rbm.ClickMarcador(this, e, rbm);
        //});

        marker.details.id = puntodto.Id.toString();
        marker.details.tipo = puntodto.TipoParadaId;
        //marker.details.info = puntodto.Descripcion;

        marker.icon.url = this.service.markerIconFromDto(puntodto);

        return marker;

    }

    setDraggable(marker: any, puntoDto: PuntoDto) {

        if (puntoDto.EsCambioSector || puntoDto.EsPuntoInicio || puntoDto.EsPuntoTermino) {
            //console.log(marker);
            if (puntoDto.CodigoNombre && puntoDto.Abreviacion) {
                marker.setDraggable(false);
            }
        }
        else {
            marker.setDraggable(true);
        }


    }

    findPuntoDtoColor(punto: PuntoDto): void {
        var color = this.DefaultColor;
        var sector = this.sectoresdto.find(s => s.OrdenInicio < punto.Orden && punto.Orden <= s.OrdenFin);

        //if (sectores) {
        //    var sector = sectores[0];
        if (sector) color = sector.Color;
        //}

        punto.LineColor = color;
    }


    parsePuntoDto(punto: PuntoDto): void {
        if (!punto.Steps) {
            var data = JSON.parse(punto.Data);
            punto.Steps = data.steps;
            punto.Instructions = data.instructions;
        }
    }

    parsePuntosDto(puntosdto: PuntoDto[]): void {
        puntosdto.forEach(item => {
            this.parsePuntoDto(item);
        });
    }


    AgregarMarcadorDto(punto: PuntoDto, callback?: any): void {

        this.parsePuntoDto(punto);

        this.findPuntoDtoColor(punto);

        var marker = this.getMarkerFromDTO(punto)

        this.map.addMarker(marker);

        this.hacerRutaDto(punto, this, callback);
    }


    private BuscarSectoractual(punto: PuntoDto): SectorDto {

        var pa = this.BuscarCambioSectorAnterior(punto);
        var pp = this.BuscarCambioSectorPosterior(punto);
        var sectoractual = this.sectoresdto.find(e => e.PuntoInicioId == pa.Id && e.PuntoFinId == pp.Id);
        return sectoractual;
    }


    hacerRuta(puntoUltimo: PuntoDto, puntoNuevo: PuntoDto, reemplazar: boolean, currentMap: IBaseMapsComponent, callback?: any) {


        var rbm = currentMap as RutasMapComponent;

        mApp.blockPage();

        if (reemplazar) { // quitamos polyline
            var encontrado = false;
            for (let i = 0; i < rbm.puntosdto.length && !encontrado; i++) {
                const element = rbm.puntosdto[i];
                if (element.Id == puntoNuevo.Id) {
                    encontrado = true;
                    for (let ip = 0; ip < element.Polylines.length; ip++) {
                        element.Polylines[ip].setMap(null);
                    }
                    element.Polylines = [];
                    element.Steps = [];
                }
            }
        }

        var origen = [puntoUltimo.Lat, puntoUltimo.Long];
        var destino = [puntoNuevo.Lat, puntoNuevo.Long];


        rbm.map.travelRoute({
            origin: origen,
            destination: destino,
            travelMode: 'driving',
            end: function(e) {

                mApp.unblock();
            },
            step: function(e, total) {
                rbm.agregarPolyline(puntoNuevo, e.path, rbm);

                puntoNuevo.Steps.push(e);

                if (total == e.step_number) {
                    if (callback) callback();
                }
            }
        });
    }


    agregarPunto(punto: PuntoDto) {
        this.puntosdto.push(punto);
        this.cdRef.detectChanges();
    }



    hacerRutaDto(punto: PuntoDto, rbm: RutasMapComponent, callback?: any) {

        mApp.blockPage();

        if (punto.Steps) {
            punto.Steps.forEach(step => {
                rbm.agregarPolyline(punto, step.path, rbm);
            });
        }


        mApp.unblock();

        if (callback) callback();
    }


    rehacerLineas(element: PuntoDto, i: number, rbm: RutasMapComponent) {
        if (i > 0) { // NO ES EL PRIMERO
            var punto_anterior = rbm.puntosdto[i - 1];
            // var lat = punto_anterior.Lat;
            // var lng = punto_anterior.Long;
            rbm.hacerRuta(punto_anterior, element, true, rbm, function() {
                var totalPuntos = rbm.puntosdto.length;
                if (i < totalPuntos - 1) { // NO ES EL ULTIMO
                    var punto_sig = rbm.puntosdto[i + 1];
                    // var lat = element.Lat;
                    // var lng = element.Long;
                    rbm.hacerRuta(element, punto_sig, true, rbm);
                }
            });
        }
        else {
            var totalPuntos = rbm.puntosdto.length;
            if (i < totalPuntos - 1) { // NO ES EL ULTIMO
                var punto_sig = rbm.puntosdto[i + 1];
                // var lat = element.Lat;
                // var lng = element.Long;
                rbm.hacerRuta(element, punto_sig, true, rbm);
            }
        }

    }


    getRuta(origen: any, destino: any, rbm: RutasMapComponent, salida: Date, callback1?: any) {
        rbm.map.getRoutes({
            origin: origen,
            destination: destino,
            travelMode: 'driving',
            drivingOptions: {
                departureTime: salida,
                //trafficModel: google.maps.TrafficModel.PESSIMISTIC
            },
            callback: function(data, status) {
                console.log("getRoutes " + salida, data[0].legs);
                //console.log("status ------------- ", status);
                //https://developers.google.com/maps/documentation/javascript/reference/3/#DirectionsLeg ---> duration_in_traffic
                //Only available to Premium Plan customers when drivingOptions is defined when making the request.

                if (callback1) callback1();
            }
        });
    }




    removeMarkersAndPolylines(currentMap: IBaseMapsComponent) {

        var rbm = currentMap as RutasMapComponent;
        super.removeMarkersAndPolylines(currentMap);

        if (rbm.puntosdto)
            rbm.puntosdto.forEach(punto => {
                rbm.removePolylineArray(punto.Polylines);
            });
    }


    removeMarkersAndPolylinesByPuntoDto(currentMap: IBaseMapsComponent, puntos: Array<PuntoDto>) {

        var rbm = currentMap as RutasMapComponent;



        if (rbm.map) {


            puntos.forEach(puntoDto => {
                var index = rbm.map.markers.findIndex(e => e.id == puntoDto.Id);
                if (index >= 0) {
                    rbm.map.markers[index].setMap(null);
                    rbm.map.markers.splice(index, 1);
                }
            });

        }



        if (puntos)
            puntos.forEach(punto => {
                rbm.removePolylineArray(punto.Polylines);
            });






    }


}
