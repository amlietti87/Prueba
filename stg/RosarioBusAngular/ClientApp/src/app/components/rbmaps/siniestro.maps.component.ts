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
import { SiniestrosDto } from '../../theme/pages/siniestros/model/siniestro.model';


declare var GMaps: any;
declare var google: any;
declare var bootbox: any;
declare var $: any;
declare let mApp: any;

@Component({
    selector: 'app-siniestromaps',
    templateUrl: './siniestro.maps.component.html',
    styleUrls: ['./rbmaps.component.css']
})
export class SiniestroMapsComponent extends BaseMapsComponent implements OnInit {





    heatmap: any;
    markers: RbMapMarker[];
    constructor(injector: Injector,
        protected modalService: NgbModal,
        protected cdRef: ChangeDetectorRef, _service: RbMapServices
    ) {
        super(injector, cdRef, _service);
        this.DefaultIconID = 4;
        this.markers = [];
        this.loadOnInit = false;

    }


    ngOnInit() {
        super.ngOnInit();
    }

    AgregarMarcador(latlng, center): void {

        this.removeLayerPuntos();
        super.AgregarMarcador(latlng, center);
    }



    removeLayerPuntos(): void {
        for (var i = 0; i < this.map.markers.length; i++) {
            var index = this.map.markers.findIndex(e => e.id == this.map.markers[i].id);
            if (index >= 0) {
                this.map.markers[index].setMap(null);
                this.map.markers.splice(index, 1);
            }
        }
    }


    generateInstructions(data: any) {

        var direccion = "";
        var selft = this;
        GMaps.geocode({
            lat: data.Latitud,
            lng: data.Longitud,
            callback: function(results, status) {
                if (status == 'OK') {
                    if (results[0].formatted_address) {
                        data.Direccion = results[0].formatted_address;
                    }
                    else {
                        data.Direccion = "Error al cargar datos";
                    }
                    if (results[0].address_components) {
                        results[0].address_components.forEach(function(value, index) {
                            if (value.types) {
                                var resul = results[0].address_components[index].types.some(
                                    e => e === "locality"
                                );
                                if (resul) {
                                    data.Localidad = results[0].address_components[index].long_name;
                                }
                            }
                        });
                    }
                }
                else {
                    data.Direccion = "Error al cargar datos";
                }
            }
        });
        return data;
    }




    buscar_Gmaps(text, callback = null): void {
        var rbmaps = this;
        if (!callback) {
            callback = function(results, status) {
                if (status == 'OK') {
                    rbmaps.successfullyGeocodePorCruce(results);
                }
            };
        }


        GMaps.geocode({
            address: text,
            callback: callback
        });
    }





    successfullyGeocodePorCruce(results: any): void {
        console.log(results);

        var latlng = results[0].geometry.location;
        this.setCenter(latlng.lat(), latlng.lng());

        this.removeLayerPuntos();
        var marker = this.AgregarMarcador_lat_lng(latlng.lat(), latlng.lng(), true);
        this.AfterAddMaker.emit(marker);

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
            disableDefaultUI: true,
            zoomControl: true,
            fullscreenControl: true
        });




        this.initContextMenu(rbmaps);
    }


}

