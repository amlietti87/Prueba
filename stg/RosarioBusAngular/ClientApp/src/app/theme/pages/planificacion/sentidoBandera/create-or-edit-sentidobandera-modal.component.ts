import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;
import { DetailModalComponent } from '../../../../shared/manager/detail.component';
import { SentidoBanderaDto } from '../model/sentidoBandera.model';
import { SentidoBanderaService } from './sentidoBandera.service';


@Component({
    selector: 'createOrEditSentidoBanderaDtoModal',
    templateUrl: './create-or-edit-sentidobandera-modal.component.html',

})
export class CreateOrEditSentidoBanderaModalComponent extends DetailModalComponent<SentidoBanderaDto> {


    constructor(
        injector: Injector,
        protected service: SentidoBanderaService
    ) {
        super(service, injector);
        this.detail = new SentidoBanderaDto();
    }


    completedataBeforeShow(item: SentidoBanderaDto): any {
        if (item.Color && item.Color != null) {
            item.Color = '#' + item.Color;
        }
    }

    completedataBeforeSave(item: SentidoBanderaDto): any {
        if (item.Color && item.Color != null) {
            item.Color = item.Color.split('#')[1];
        }
    }

}
