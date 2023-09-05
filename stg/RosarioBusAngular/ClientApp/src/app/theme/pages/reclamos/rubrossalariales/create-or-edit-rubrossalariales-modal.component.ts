import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { RubrosSalarialesService } from './rubrossalariales.service';
import { RubrosSalarialesDto } from '../model/rubrossalariales.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditRubrosSalarialesDtoModal',
    templateUrl: './create-or-edit-rubrossalariales-modal.component.html',

})
export class CreateOrEditRubrosSalarialesModalComponent extends DetailAgregationComponent<RubrosSalarialesDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditRubrosSalarialesModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: RubrosSalarialesDto,
        injector: Injector,
        protected service: RubrosSalarialesService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;

    }
}

