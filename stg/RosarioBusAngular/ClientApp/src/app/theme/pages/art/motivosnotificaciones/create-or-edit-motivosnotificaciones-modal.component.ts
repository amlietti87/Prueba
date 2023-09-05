import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';;
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DenunciasEstadosDto } from '../model/denunciasestados.model';
import { MotivosNotificacionesDto } from '../model/motivosnotificaciones.model';
import { MotivosNotificacionesService } from './motivosnotificaciones.service';

@Component({
    selector: 'createOrEditMotivosNotificacionesDtoModal',
    templateUrl: './create-or-edit-motivosnotificaciones-modal.component.html',

})
export class CreateOrEditMotivosNotificacionesModalComponent extends DetailAgregationComponent<MotivosNotificacionesDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditMotivosNotificacionesModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: MotivosNotificacionesDto,
        injector: Injector,
        protected service: MotivosNotificacionesService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;

    }
}

