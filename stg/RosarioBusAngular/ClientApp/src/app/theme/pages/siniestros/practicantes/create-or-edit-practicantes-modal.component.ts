import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailEmbeddedComponent, DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { DetailComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { PracticantesService } from './practicantes.service';
import { PracticantesDto } from '../model/practicantes.model';
import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { ViewMode, ItemDto } from '../../../../shared/model/base.model';
import { LocalidadesService } from '../localidades/localidad.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditPracticantesDtoModal',
    templateUrl: './create-or-edit-practicantes-modal.component.html',
    styleUrls: ['./create-or-edit-practicantes-modal.component.css']
})
export class CreateOrEditPracticantesModalComponent extends DetailAgregationComponent<PracticantesDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    constructor(
        protected dialogRef: MatDialogRef<CreateOrEditPracticantesModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: PracticantesDto,
        injector: Injector,
        protected service: PracticantesService,
        protected localidadservice: LocalidadesService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;
        this.SetAllowPermission();
    }

    allowAddLocalidades: boolean = false;
    allowAddTipoDni: boolean = false;

    SetAllowPermission() {
        this.allowAddLocalidades = this.permission.isGranted('Admin.Localidad.Agregar');
        this.allowAddTipoDni = this.permission.isGranted('Admin.TipoDni.Agregar');
    }

    completedataBeforeShow(item: PracticantesDto): any {
        if (this.viewMode == ViewMode.Modify && item.LocalidadId && item.LocalidadId != 0) {

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

    completedataBeforeSave(item: PracticantesDto): any {
        if (item.selectLocalidades) {
            item.LocalidadId = item.selectLocalidades.Id;
        }
    }

}

