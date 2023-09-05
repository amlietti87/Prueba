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
import { TallerDto } from '../../theme/pages/planificacion/model/taller.model';
import { BaseMapsComponent } from './baseMaps.component';
import { forEach } from '@angular/router/src/utils/collection';
import { RutaDto } from '../../theme/pages/planificacion/model/ruta.model';
import { PuntoService } from '../../theme/pages/planificacion/punto/punto.service';
import { ConfigurationService } from '../../shared/common/services/configuration.service';


declare var GMaps: any;
declare var google: any;
declare var bootbox: any;
declare var $: any;
declare let mApp: any;

@Component({
    selector: 'app-tallermaps',
    templateUrl: './taller.maps.component.html',
    styleUrls: ['./rbmaps.component.css']
})
export class TallerMapsComponent extends BaseMapsComponent implements OnInit {




    @Input() marcadores: Array<TallerDto>;
    @Output() OnClickMarcador: EventEmitter<TallerDto> = new EventEmitter<TallerDto>();
    @Output() OnDeleteMarcador: EventEmitter<TallerDto> = new EventEmitter<TallerDto>();
    @Input() Sucursalid: number;
    @Input() Sucursal: string;

    heatmap: any;
    markers: RbMapMarker[];
    constructor(injector: Injector,
        protected modalService: NgbModal,
        protected cdRef: ChangeDetectorRef, _service: RbMapServices
    ) {
        super(injector, cdRef, _service);
        this.DefaultIconID = 10;
        this.markers = [];

    }


    ngOnInit() {
        super.ngOnInit();
    }

    setTalleres(talleres: TallerDto[]) {

        this.marcadores = talleres;

        try {
            var p = JSON.stringify(this.marcadores);
            this.OriginalData = { marcadores: p };
        } catch (e) {
        }

        this.marcadores.forEach(item => {
            this.AgregarMarcadorDto(item);
        });

        //this.test();

    }


    addLayerMaestros(puntos: PuntoDto[]) {

        this.addLayerPuntos(puntos);
    }


    private addLayerPuntos(puntos: PuntoDto[]): void {


        for (var i = 0; i < puntos.length; i++) {
            var marker = this.getMarkerFromRutaDTO(puntos[i]);

            this.markers.push(marker);

            this.map.addMarker(marker);
        }
    }


    private getMarkerFromRutaDTO(punto: PuntoDto): any {

        var marker = new RbMapMarker(punto.Lat, punto.Long, punto.Id);
        marker.SetDraggable(false);
        var rbm = this;
        marker.icon.url = this.service.markerIcon(4);

        return marker;
    }

    private addLayerPuntosGeoJson(puntos: PuntoDto[]): void {
        //this.map.

        var json = {
            "id": "talleres",
            "type": "FeatureCollection",
            "features": [{
                "id": "talleres",
                "type": "Feature",
                "geometry": { "type": "MultiPoint", "coordinates": [] },
                "properties": {
                    "title": "",
                    "icon": "http://maps.google.com/mapfiles/dir_0.png",
                }
            }]
        };


        for (var i = 0; i < puntos.length; i++) {
            var p = [puntos[i].Long, puntos[i].Lat, 0, puntos[0].Id];

            json.features[0].geometry.coordinates.push(p);
        }


        this.map.buildFromGeoJson({ name: "talleres", geoJson: json });

    }

    toogleLayer(puntos: PuntoDto[]) {

        var rbm = this;
        if (this.markers.length == 0) {
            this.addLayerPuntos(puntos);
        }
        else {
            this.removeLayerPuntos();
        }
    }

    removeLayerPuntos(): void {
        for (var i = 0; i < this.markers.length; i++) {
            var index = this.map.markers.findIndex(e => e.id == this.markers[i].id);
            if (index >= 0) {
                this.map.markers[index].setMap(null);
                this.map.markers.splice(index, 1);
            }
        }
        this.markers = [];
    }



    AgregarMarcadorDto(marcador: TallerDto): void {
        var marker = this.getMarkerFromDTO(marcador);

        this.map.addMarker(marker);
    }


    getMarkerFromDTO(marcador: TallerDto): RbMapMarker {
        var marker = new RbMapMarker(marcador.Lat, marcador.Long, marcador.Id);
        var rbm = this;


        if (marcador.isNew) {
            marker.SetDragend(function(e) {
                rbm.MoverMarcador(this, e, rbm);
            });
        }
        else {
            marker.SetDraggable(false);
        }

        marker.SetClick(function(e) {
            rbm.ClickMarcador(this, e, rbm);
        });

        marker.icon.url = this.service.markerIcon(10);

        return marker

    }


    MoverMarcador(marcador: RbMapMarker, data: any, rbm: TallerMapsComponent) {
        // console.log("marcador", marcador);
        // console.log("data", data);

        var encontrado = false;
        for (let i = 0; i < rbm.marcadores.length && !encontrado; i++) {
            const element = rbm.marcadores[i];
            if (element.Id == marcador.id) {

                element.Lat = data.latLng.lat();
                element.Long = data.latLng.lng();
            }
        }

    }


    ClickMarcador(marcador: RbMapMarker, e: any, rbm: TallerMapsComponent) {

        this.onClickMarker(marcador);
    }



    private onClickMarker(marker: RbMapMarker) {

        this.OnClickMarker.emit(marker);

        this.marcadores.forEach(m => {
            this.refreshIcon(m.Id, 10);
        });

        this.refreshIcon(marker.id, 11);


        var marcadorDto = this.marcadores.filter(p => p.Id === marker.id)[0];
        this.OnClickMarcador.emit(marcadorDto);


    }

    refreshIcon(id: any, icon: number) {
        this.findMapMarker(id).setIcon(this.service.markerIcon(icon));
    }


    setSucursal(sucursalid: any, sucursal: any): any {
        this.Sucursalid = sucursalid;
        this.Sucursal = sucursal;


        if (this.Sucursalid == 2) {
            this.setCenter(-34.54325, -58.71917);
        }
        else if (this.Sucursalid == 3) {
            this.setCenter(-31.61863, -60.69809);
        }
    }


    EliminarItemByMarcador(marcador: RbMapMarker, rbm: BaseMapsComponent) {
        var taller = this.marcadores.filter(p => p.Id === marcador.id)[0];

        if (taller) {
            //emir delete 
            this.OnDeleteMarcador.emit(taller);
        }
    }


    callbackEliminarMarcador(taller: TallerDto) {

        if (taller) {

            var marcador = this.map.markers.find(e => e.id == taller.Id);

            this.marcadores.splice(this.marcadores.findIndex(e => e.Id == marcador.id), 1);
            var index = this.map.markers.indexOf(marcador);

            if (index > -1) {
                this.map.markers[index].setMap(null);
                this.map.markers.splice(index, 1);
            }

            this.cdRef.detectChanges();
        }


    }

    fitBounds(puntos: PuntoDto[]): void {

        var bounds = new google.maps.LatLngBounds();

        puntos.forEach(p => {
            bounds.extend(new google.maps.LatLng(p.Lat, p.Long));
        });


        this.map.fitBounds(bounds);

    }

}

