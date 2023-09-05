import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailModalComponent } from '../../../../shared/manager/detail.component';
import { FDTiposDocumentosDto } from '../model/fdtiposdocumentos.model';
import { FDTiposDocumentosService } from '../services/fdtiposdocumentos.service';
import { ViewMode } from '../../../../shared/model/base.model';


@Component({
    selector: 'createOrEditTipoDocumentoDtoModal',
    templateUrl: './create-or-edit-tipoDocumento-modal.component.html',

})
export class CreateOrEditTipoDocumentoModalComponent extends DetailModalComponent<FDTiposDocumentosDto> {

    allowEliminarTipoDoc: boolean = false;

    constructor(
        injector: Injector,
        protected service: FDTiposDocumentosService

    ) {
        super(service, injector);
        this.detail = new FDTiposDocumentosDto();
        this.allowEliminarTipoDoc = this.permission.isGranted("FirmaDigital.TipoDocumento.Eliminar");
    }

}
