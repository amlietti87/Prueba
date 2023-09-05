import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailModalComponent } from '../../../../shared/manager/detail.component';
import { EmpresaService } from './empresa.service';
import { EmpresaDto } from '../model/empresa.model';


@Component({
    selector: 'createOrEditEmpresaDtoModal',
    templateUrl: './create-or-edit-empresa-modal.component.html',

})
export class CreateOrEditEmpresaModalComponent extends DetailModalComponent<EmpresaDto> {


    constructor(
        injector: Injector,
        protected service: EmpresaService
    ) {
        super(service, injector);
        this.detail = new EmpresaDto();
    }




}
