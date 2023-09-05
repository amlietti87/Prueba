import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;
import { DetailModalComponent } from '../../../../shared/manager/detail.component';
import { SentidoBanderaDto } from '../model/sentidoBandera.model';

import { BanderaDto } from '../model/bandera.model';
import { BanderaService } from '../bandera/bandera.service';


@Component({
    selector: 'create-or-edit-horariosPorSector.component',
    templateUrl: './create-or-edit-horariosPorSector.component.html',

})
export class CreateOrEditHorariosPorSectorComponent extends DetailModalComponent<BanderaDto> {


    constructor(
        injector: Injector,
        protected banderaService: BanderaService
    ) {
        super(banderaService, injector);
    }




}
