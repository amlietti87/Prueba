import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;
import { DetailModalComponent } from '../../../../shared/manager/detail.component';
import { SentidoBanderaDto } from '../model/sentidoBandera.model';
import { HHorariosConfiDto } from '../model/hhorariosconfi.model';
import { HHorariosConfiService } from './hhorariosconfi.service';




@Component({
    selector: 'create-or-edit-detalleSalidasYRelevos.component',
    templateUrl: './create-or-edit-detalleSalidasYRelevos.component.html',

})
export class CreateOrEditDetalleSalidasYRelevos extends DetailModalComponent<HHorariosConfiDto> {


    constructor(
        injector: Injector,
        protected _service: HHorariosConfiService
    ) {
        super(_service, injector);
    }




}
