import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailEmbeddedComponent, DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { DetailComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { CiaSegurosService } from './seguros.service';
import { CiaSegurosDto } from '../model/seguros.model';
import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { ViewMode, ItemDto } from '../../../../shared/model/base.model';
import { LocalidadesService } from '../localidades/localidad.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
    selector: 'createOrEditSegurosDtoModal',
    templateUrl: './create-or-edit-seguros-modal.component.html',

})
export class CreateOrEditSegurosModalComponent extends DetailAgregationComponent<CiaSegurosDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    constructor(
        protected dialogRef: MatDialogRef<CreateOrEditSegurosModalComponent>,
        injector: Injector,
        protected service: CiaSegurosService,
        protected localidadservice: LocalidadesService,
        @Inject(MAT_DIALOG_DATA) public data: CiaSegurosDto
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;

        this.SetAllowPermission();
    }


    allowAddLocalidades: boolean = false;

    SetAllowPermission() {
        this.allowAddLocalidades = this.permission.isGranted('Admin.Localidad.Agregar');
    }

    save(form: NgForm): void {
        super.save(form);
    }



    completedataBeforeShow(item: CiaSegurosDto): any {

        if (this.viewMode == ViewMode.Modify) {

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

    completedataBeforeSave(item: CiaSegurosDto): any {

        if (item.selectLocalidades) {
            item.LocalidadId = item.selectLocalidades.Id;
        }
    }



}

