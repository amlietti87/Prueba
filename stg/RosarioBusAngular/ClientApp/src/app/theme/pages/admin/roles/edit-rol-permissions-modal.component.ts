import { Component, ViewChild, Injector, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';


import { PermissionTreeComponent } from '../shared/permission-tree.component';

import { RolesService } from './roles.service';

import * as _ from 'lodash';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { UpdatePermissionsInput } from '../model/permission.model';


@Component({
    selector: 'editRolPermissionsModal',
    templateUrl: './edit-rol-permissions-modal.component.html'
})
export class EditRolPermissionsModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('editModal') modal: ModalDirective;
    @ViewChild('permissionTree') permissionTree: PermissionTreeComponent;

    saving = false;
    resettingPermissions = false;

    rolId: number;
    rolName: string;
    isLoading: boolean = false;
    constructor(
        injector: Injector,
        private _service: RolesService
    ) {
        super(injector);
    }

    ngOnInit() {
    }


    show(rolId: number, Name?: string): void {

        this.rolId = rolId;
        this.rolName = Name;

        this.isLoading = true;
        this._service.getRolePermissionsForEdit(rolId).subscribe(result => {
            this.permissionTree.editData = result.DataObject; this.modal.show();
            this.modal.show();
            this.isLoading = false;
        });
    }

    save(): void {
        let input = new UpdatePermissionsInput();

        input.Id = this.rolId;
        input.GrantedPermissionNames = this.permissionTree.getGrantedPermissionNames();

        this.saving = true;
        this._service.updateRolePermissions(input)
            .finally(() => { this.saving = false; })
            .subscribe(() => {
                this.notify.info("Guardado exitosamente");
                this.close();
            });
    }



    close(): void {
        this.modal.hide();
    }
}
