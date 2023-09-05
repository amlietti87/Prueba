import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailEmbeddedComponent, DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { DetailComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { TipoElementoService } from './tipoelemento.service';
import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { ViewMode } from '../../../../shared/model/base.model';
import { TipoElementoDto } from '../model/tipoelemento.model';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
    selector: 'createOrEditTipoElementoDtoModal',
    templateUrl: './create-or-edit-tipoelementos-modal.component.html',

})
export class CreateOrEditTipoElementosModalComponent extends DetailAgregationComponent<TipoElementoDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    constructor(protected dialogRef: MatDialogRef<CreateOrEditTipoElementosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: TipoElementoDto,
        injector: Injector,
        protected service: TipoElementoService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);

    }


}

