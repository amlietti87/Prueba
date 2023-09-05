import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { TipoInvolucradoService } from './tipoinvolucrado.service';
import { TipoInvolucradoDto } from '../model/tipoinvolucrado.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditTipoInvolucradoDtoModal',
    templateUrl: './create-or-edit-tipoinvolucrado-modal.component.html',

})
export class CreateOrEditTipoInvolucradoModalComponent extends DetailAgregationComponent<TipoInvolucradoDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditTipoInvolucradoModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: TipoInvolucradoDto,
        injector: Injector,
        protected service: TipoInvolucradoService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;
    }


}

