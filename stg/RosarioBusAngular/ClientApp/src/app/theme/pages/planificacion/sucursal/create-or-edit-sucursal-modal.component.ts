import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailEmbeddedComponent, DetailModalComponent } from '../../../../shared/manager/detail.component';
import { SucursalService } from './sucursal.service';
import { SucursalDto } from '../model/sucursal.model';


@Component({
    selector: 'createOrEditSucursalModal',
    templateUrl: './create-or-edit-sucursal-modal.component.html',

})
export class CreateOrEditSucursalModalComponent extends DetailModalComponent<SucursalDto> {


    constructor(
        injector: Injector,
        protected service: SucursalService
    ) {
        super(service, injector);
        this.detail = new SucursalDto();
    }




}
