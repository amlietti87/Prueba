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
    selector: 'select-marker-maps',
    templateUrl: './selectmarker.maps.component.html',
    styleUrls: ['./rbmaps.component.css']
})
export class SelectMarkerMapsComponent extends BaseMapsComponent implements OnInit {


    heatmap: any;
    markers: RbMapMarker[];
    constructor(injector: Injector,
        protected cdRef: ChangeDetectorRef, _service: RbMapServices
    ) {
        super(injector, cdRef, _service);
        this.mapId = "SelectMarkerMapr ";
        this.loadOnInit = false;
        this.DefaultIconID = 4;
        this.markers = [];

    }


    ngOnInit() {
        super.ngOnInit();
    }


    AgregarMarcador(latlng, center): void {
        this.removeLayerPuntos();
        super.AgregarMarcador(latlng, center);
    }

    successfullyGeocode(results: any): void {
        super.successfullyGeocode(results);
        var latlng = results[0].geometry.location;
        this.AgregarMarcador(latlng, true);
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


}

