import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Inject, ChangeDetectorRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';

import { TipoDiaDto } from '../model/tipoDia.model';
import { ParadaService } from './parada.service';
import { ParadaDto } from '../model/parada.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { LocalidadesService } from '../../siniestros/localidades/localidad.service';
import { ViewMode, ItemDto } from '../../../../shared/model/base.model';
import { SelectMarkerMapsComponent } from '../../../../components/rbmaps/selectmarker.maps.component';
import { Validators } from '@angular/forms';


@Component({
    selector: 'createOrEditParadaDtoModal',
    templateUrl: './create-or-edit-parada-modal.component.html',

})
export class CreateOrEditParadaModalComponent extends DetailAgregationComponent<ParadaDto> {

    @ViewChild('mapaComponents') mapaComponents: SelectMarkerMapsComponent;
    codigoparada: any;
    cantChange: boolean;
    addEstacion: boolean;
    constructor(
        protected dialogRef: MatDialogRef<CreateOrEditParadaModalComponent>,
        injector: Injector,
        protected service: ParadaService,
        protected localidadservice: LocalidadesService,
        private cdr: ChangeDetectorRef,
        @Inject(MAT_DIALOG_DATA) public data: ParadaDto
    ) {
        super(dialogRef, service, injector, data);

        if (!data) {
            this.detail = new ParadaDto();
        }

        this.title = "Parada";
    }


    ngOnInit() {
        super.ngOnInit();
    }

    inicializarMapa() {

        var long: number;
        var lat: number;
        long = -60.655931;
        lat = -32.954517;
        this.mapaComponents.crearMapa(lat, long);

        if (this.data && this.data.Long && this.data.Lat) {
            this.mapaComponents.removeLayerPuntos();
            var marker = this.mapaComponents.AgregarMarcador_lat_lng(this.data.Lat, this.data.Long, true);
            this.mapaComponents.AfterAddMaker.emit(marker);
            setTimeout(() => { this.mapaComponents.setCenter(this.data.Lat, this.data.Long); }, 100);
        }
        else {
            this.mapaComponents.setCenter(lat, long);
        }
        this.mapaComponents.AfterAddMaker.subscribe(e => {
            this.data.Lat = e.lat;
            this.data.Long = e.lng;
        }
        );

    }

    cambiotipoparada() {

        if (this.detail.LocationType != null) {

            if (this.detail.LocationType == 2) {
                this.detailForm.controls['ParentStationId'].clearValidators();
                this.detailForm.controls['ParentStationId'].setValidators(Validators.required);
                this.addEstacion = true;
            }
            else {
                this.detailForm.controls['ParentStationId'].clearValidators();
                this.detailForm.controls['ParentStationId'].setValidators(null);
                this.addEstacion = false;
            }
        }
        this.cdr.detectChanges();
    }
    searchGmap(): void {

        var Calle = this.detail.Calle;
        var Cruce = this.detail.Cruce;
        var localidad = this.detail.selectLocalidades ? this.detail.selectLocalidades.Description : "";
        localidad = (localidad.split(' - ')[0]).trim();
        var buscarpo = `${Calle} y ${Cruce} ,  ${localidad}`;
        this.mapaComponents.buscar_Gmaps(buscarpo);
    }

    completedataBeforeShow(item: ParadaDto): any {
        this.addEstacion = false;
        this.cantChange = false;
        if (this.viewMode == ViewMode.Modify) {

            if (item.LocalidadId) {
                if (item.Localidad) {
                    var findlocalidad = new ItemDto();
                    findlocalidad.Id = item.LocalidadId;
                    findlocalidad.Description = item.Localidad;
                    item.selectLocalidades = findlocalidad;
                }
                else {
                    this.localidadservice.getById(item.LocalidadId)
                        //.finally(() => { this.isTableLoading = false; })
                        .subscribe((t) => {
                            var findlocalidad = new ItemDto();
                            findlocalidad.Id = item.LocalidadId;
                            findlocalidad.Description = t.DataObject.DscLocalidad + ' - ' + t.DataObject.CodPostal;
                            item.selectLocalidades = findlocalidad;
                        })
                }
                if (item.LocationType == 1) {
                    var f = { ParentStationId: item.Id }
                    this.service.requestAllByFilter(f)
                        .subscribe(e => {

                            var paradas = e.DataObject.Items;
                            if (paradas.length != 0) {
                                this.cantChange = true;
                            }
                        })
                }
                if (item.LocationType == 2) {
                    this.addEstacion = true;
                }
                this.cambiotipoparada();
            }
            else {
                this.detail.LocationType = 0
            }

        }
        this.inicializarMapa();

    }

    validateSave(): boolean {
        if (!this.detail.Lat) {
            this.message.error("Falta marcar un Punto en el mapa", "Localizacion es requeria");
            return false;
        }
        if (this.detail.LocationType == 2 && this.detail.ParentStation == null) {
            this.message.error("Falta agregar la estacion a la cual corresponde la entrada o salida", "Estacion es requeria");
            return false;
        }
        return true;
    }


    completedataBeforeSave(item: ParadaDto): any {

        if (item.ParentStation != null) {
            item.ParentStationId = item.ParentStation.Id;
        }
        if (item.selectLocalidades) {
            item.LocalidadId = item.selectLocalidades.Id;
        }
    }

}
