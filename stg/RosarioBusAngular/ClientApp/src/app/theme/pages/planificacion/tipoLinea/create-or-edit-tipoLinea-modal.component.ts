import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailModalComponent } from '../../../../shared/manager/detail.component';
import { TipoLineaService } from './tipoLinea.service';
import { TipoLineaDto } from '../model/tipoLinea.model';


@Component({
    selector: 'createOrEditTipoLineaDtoModal',
    templateUrl: './create-or-edit-tipoLinea-modal.component.html',

})
export class CreateOrEditTipoLineaModalComponent extends DetailModalComponent<TipoLineaDto> {


    constructor(
        injector: Injector,
        protected service: TipoLineaService
    ) {
        super(service, injector);
        this.detail = new TipoLineaDto();
    }




}
