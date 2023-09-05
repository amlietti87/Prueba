import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, AfterViewInit, ViewChildren } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailModalComponent } from '../../../../shared/manager/detail.component';
import { TipoDiaService } from './tipodia.service';
import { TipoDiaDto } from '../model/tipoDia.model';
import { DescripcionTipoDiaPredictivoComponent } from '../shared/descripcionTipoDia-predictivo.component';


@Component({
    selector: 'createOrEditTipoDiaDtoModal',
    templateUrl: './create-or-edit-tipodia-modal.component.html',

})
export class CreateOrEditTipoDiaModalComponent extends DetailModalComponent<TipoDiaDto> {

    @ViewChild('predictivo')
    set predictivo(componente: DescripcionTipoDiaPredictivoComponent) {
        if (componente) {
            componente.Refrescar();
        }
    };

    constructor(
        injector: Injector,
        protected service: TipoDiaService
    ) {
        super(service, injector);
        this.detail = new TipoDiaDto();
    }

    completedataBeforeShow(item: TipoDiaDto): any {
        if (this.predictivo)
            this.predictivo.Refrescar();
    }


}
