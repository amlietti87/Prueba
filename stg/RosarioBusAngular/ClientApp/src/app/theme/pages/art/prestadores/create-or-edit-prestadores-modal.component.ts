import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { PrestadoresMedicosService } from './prestadores.service';
import { PrestadoresMedicosDto } from '../model/prestadoresmedicos.model';


@Component({
    selector: 'createOrEditPrestadoresDtoModal',
    templateUrl: './create-or-edit-prestadores-modal.component.html',

})
export class CreateOrEditPrestadoresMedicosModalComponent extends DetailAgregationComponent<PrestadoresMedicosDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditPrestadoresMedicosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: PrestadoresMedicosDto,
        injector: Injector,
        protected service: PrestadoresMedicosService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;

    }
}

