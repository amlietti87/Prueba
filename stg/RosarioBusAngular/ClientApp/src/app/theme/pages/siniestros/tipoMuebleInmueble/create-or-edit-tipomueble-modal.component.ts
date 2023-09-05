import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailEmbeddedComponent, DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { DetailComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { TipoMuebleService } from './tipomueble.service';
import { TipoMuebleDto } from '../model/tipomueble.model';
import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { ViewMode } from '../../../../shared/model/base.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditTipoMuebleDtoModal',
    templateUrl: './create-or-edit-tipomueble-modal.component.html',

})
export class CreateOrEditTipoMuebleModalComponent extends DetailAgregationComponent<TipoMuebleDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;
    //@ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    constructor(protected dialogRef: MatDialogRef<CreateOrEditTipoMuebleModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: TipoMuebleDto,
        injector: Injector,
        protected service: TipoMuebleService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
    }


}

