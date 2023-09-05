import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailEmbeddedComponent, DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { DetailComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { JuzgadosService } from './juzgados.service';
import { JuzgadosDto } from '../model/juzgados.model';
import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { ViewMode, ItemDto } from '../../../../shared/model/base.model';
import { LocalidadesService } from '../localidades/localidad.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
    selector: 'createOrEditJuzgadosDtoModal',
    templateUrl: './create-or-edit-juzgados-modal.component.html',

})
export class CreateOrEditJuzgadosModalComponent extends DetailAgregationComponent<JuzgadosDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    constructor(protected dialogRef: MatDialogRef<CreateOrEditJuzgadosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: JuzgadosDto,
        injector: Injector,
        protected service: JuzgadosService,
        protected localidadservice: LocalidadesService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;

        this.SetAllowPermission();

    }


    allowAddLocalidades: boolean = false;

    SetAllowPermission() {
        this.allowAddLocalidades = this.permission.isGranted('Admin.Localidad.Agregar');
    }

    completedataBeforeSave(item: JuzgadosDto): any {

        if (item.selectLocalidades) {
            item.LocalidadId = item.selectLocalidades.Id;
        }

    }

    completedataBeforeShow(item: JuzgadosDto): any {
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

}

