import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { TiposAcuerdoService } from './tiposacuerdo.service';
import { TiposAcuerdoDto } from '../model/tiposacuerdo.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditTiposAcuerdoDtoModal',
    templateUrl: './create-or-edit-tiposacuerdo-modal.component.html',

})
export class CreateOrEditTiposAcuerdoModalComponent extends DetailAgregationComponent<TiposAcuerdoDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditTiposAcuerdoModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: TiposAcuerdoDto,
        injector: Injector,
        protected service: TiposAcuerdoService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;

    }
}

