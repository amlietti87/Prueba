import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
import { RolDto } from '../model/rol.model';
import { DetailModalComponent } from '../../../../shared/manager/detail.component';
import { RolesService } from './roles.service';

@Component({
    selector: 'createOrEditRolModal',
    templateUrl: './create-or-edit-rol-modal.component.html',

})
export class CreateOrEditRolModalComponent extends DetailModalComponent<RolDto> {


    constructor(
        injector: Injector,
        protected service: RolesService
    ) {
        super(service, injector);
        this.detail = new RolDto();
    }

    ngAfterViewChecked(): void {

    }


    completedataBeforeShow(item: RolDto): any {
        super.completedataBeforeShow(item);
        if (!item.Id) item.CaducarSesionInactividad = true;
    }


}
