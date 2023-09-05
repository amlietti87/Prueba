import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailEmbeddedComponent, DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { DetailComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { TipoLesionadoService } from './tipolesionado.service';
import { TipoLesionadoDto } from '../model/tipolesionado.model';
import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { ViewMode } from '../../../../shared/model/base.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditTipoLesionadoDtoModal',
    templateUrl: './create-or-edit-tipolesionado-modal.component.html',

})
export class CreateOrEditTipoLesionadoModalComponent extends DetailAgregationComponent<TipoLesionadoDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    constructor(protected dialogRef: MatDialogRef<CreateOrEditTipoLesionadoModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: TipoLesionadoDto,
        injector: Injector,
        protected service: TipoLesionadoService
    ) {

        super(dialogRef, service, injector, data);

        this.cfr = injector.get(ComponentFactoryResolver);
        this.IsInMaterialPopupMode = true;
        this.saveLocal = false;
    }




}

