import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { FactoresIntervinientesService } from './factoresintervinientes.service';
import { FactoresIntervinientesDto } from '../model/factoresintervinientes.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditFactoresIntervinientesDtoModal',
    templateUrl: './create-or-edit-factoresintervinientes-modal.component.html',

})
export class CreateOrEditFactoresIntervinientesModalComponent extends DetailAgregationComponent<FactoresIntervinientesDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditFactoresIntervinientesModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: FactoresIntervinientesDto,
        injector: Injector,
        protected service: FactoresIntervinientesService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;

    }
}

