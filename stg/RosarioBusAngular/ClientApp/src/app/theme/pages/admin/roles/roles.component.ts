import { Component, Type, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Helpers } from '../../../../helpers';
import { ScriptLoaderService } from '../../../../services/script-loader.service';
import { AppComponentBase } from '../../../../shared/common/app-component-base';

import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';


import { CreateOrEditRolModalComponent } from './create-or-edit-rol-modal.component';


import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { RolDto } from '../model/rol.model';
import { RolesService } from './roles.service';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { EditRolPermissionsModalComponent } from './edit-rol-permissions-modal.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';


@Component({

    templateUrl: "./roles.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class RolesComponent extends CrudComponent<RolDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditRolModalComponent
    }


    @ViewChild('editRolPermissionsModal') editRolPermissionsModal: EditRolPermissionsModalComponent;

    constructor(injector: Injector, protected _RolesService: RolesService) {
        super(_RolesService, CreateOrEditRolModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Roles"
        this.moduleName = "Administración";
        this.icon = "flaticon-settings";
        //this.icon = "fa fa-save";
        this.showbreadcum = false;
    }

    ngAfterViewInit() {

        super.ngAfterViewInit();
    }


    allowEditPermisos: boolean = false;
    SetAllowPermission() {
        super.SetAllowPermission();
        this.allowEditPermisos = this.permission.isGranted('Admin.Rol.Permisos');


    }

    getDescription(item: RolDto): string {
        return item.DisplayName;

    }
    getNewItem(item: RolDto): RolDto {
        return new RolDto(item);
    }

    onEditUserPermissions(item: RolDto) {
        this.editRolPermissionsModal.show(item.Id, item.DisplayName);
    }





}