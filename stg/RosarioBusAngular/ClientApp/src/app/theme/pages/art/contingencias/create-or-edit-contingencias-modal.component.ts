import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { ContingenciasService } from './contingencias.service';
import { ContingenciasDto } from '../model/contingencias.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditContingenciasDtoModal',
    templateUrl: './create-or-edit-contingencias-modal.component.html',

})
export class CreateOrEditContingenciasModalComponent extends DetailAgregationComponent<ContingenciasDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditContingenciasModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ContingenciasDto,
        injector: Injector,
        protected service: ContingenciasService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;

    }
}

