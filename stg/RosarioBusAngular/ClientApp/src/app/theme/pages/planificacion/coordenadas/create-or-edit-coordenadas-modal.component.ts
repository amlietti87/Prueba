import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';

import { TipoDiaDto } from '../model/tipoDia.model';
import { CoordenadasService } from './coordenadas.service';
import { CoordenadasDto } from '../model/coordenadas.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { LocalidadesService } from '../../siniestros/localidades/localidad.service';
import { ViewMode, ItemDto } from '../../../../shared/model/base.model';


@Component({
    selector: 'createOrEditCoordenadaDtoModal',
    templateUrl: './create-or-edit-coordenadas-modal.component.html',

})
export class CreateOrEditCoordenadaModalComponent extends DetailAgregationComponent<CoordenadasDto> {


    constructor(
        protected dialogRef: MatDialogRef<CreateOrEditCoordenadaModalComponent>,
        injector: Injector,
        protected service: CoordenadasService,
        protected localidadservice: LocalidadesService,
        @Inject(MAT_DIALOG_DATA) public data: CoordenadasDto
    ) {
        super(dialogRef, service, injector, data);

        if (!data) {
            this.detail = new CoordenadasDto();
        }

        this.title = "Sector";
    }


    completedataBeforeShow(item: CoordenadasDto): any {


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

            }

        }

    }

    completedataBeforeSave(item: CoordenadasDto): any {

        if (item.selectLocalidades) {
            item.LocalidadId = item.selectLocalidades.Id;
        }
    }

}
