import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';;
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DenunciasEstadosDto } from '../model/denunciasestados.model';
import { MotivosAudienciasService } from './motivosaudiencias.service';
import { MotivosAudienciasDto } from '../model/motivosaudencias.model';

@Component({
    selector: 'createOrEditMotivosAudienciasDtoModal',
    templateUrl: './create-or-edit-motivosaudiencias-modal.component.html',

})
export class CreateOrEditMotivosAudienciasModalComponent extends DetailAgregationComponent<MotivosAudienciasDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditMotivosAudienciasModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: MotivosAudienciasDto,
        injector: Injector,
        protected service: MotivosAudienciasService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;

    }
}

