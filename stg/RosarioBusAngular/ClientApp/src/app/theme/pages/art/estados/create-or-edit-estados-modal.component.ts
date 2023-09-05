import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';;
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { EstadosDto } from '../model/estados.model';
import { DenunciasEstadosService } from './estados.service';


@Component({
    selector: 'createOrEditDenunciasEstadosDtoModal',
    templateUrl: './create-or-edit-estados-modal.component.html',

})
export class CreateOrEditDenunciasEstadosModalComponent extends DetailAgregationComponent<EstadosDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditDenunciasEstadosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: EstadosDto,
        injector: Injector,
        protected service: DenunciasEstadosService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;

    }
}

