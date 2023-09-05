import { Component, ViewChild, Injector } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';


import { PermissionTreeComponent } from '../shared/permission-tree.component';

import { UserService } from '../../../../services/user.service';

import * as _ from 'lodash';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { UpdatePermissionsInput } from '../model/permission.model';



@Component({
    selector: 'editUserPermissionsModal',
    templateUrl: './edit-user-permissions-modal.component.html'
})
export class EditUserPermissionsModalComponent extends AppComponentBase {

    @ViewChild('editModal') modal: ModalDirective;
    @ViewChild('permissionTree') permissionTree: PermissionTreeComponent;

    saving = false;
    resettingPermissions = false;

    userId: number;
    userName: string;

    constructor(
        injector: Injector,
        private _userService: UserService
    ) {
        super(injector);
    }

    show(userId: number, userName?: string): void {

        this.userId = userId;
        this.userName = userName;

        this._userService.getUserPermissionsForEdit(userId).subscribe(result => {
            this.permissionTree.editData = result.DataObject;
            this.modal.show();
        });
    }

    save(): void {
        let input = new UpdatePermissionsInput();

        input.Id = this.userId;
        input.GrantedPermissionNames = this.permissionTree.getGrantedPermissionNames();

        this.saving = true;
        this._userService.updateUserPermissions(input)
            .finally(() => { this.saving = false; })
            .subscribe(() => {
                this.notify.info("Guardado exitosamente");
                this.close();
            });
    }

    resetPermissions(): void {

        //let input = new EntityDtoOfInt64();

        //input.id = this.userId;

        //this.resettingPermissions = true;
        //this._userService.resetUserSpecificPermissions(input).subscribe(() => {
        //    this.notify.info(this.l('ResetSuccessfully'));
        //    this._userService.getUserPermissionsForEdit(this.userId).subscribe(result => {
        //        this.permissionTree.editData = result;
        //    });
        //}, undefined, () => {
        //    this.resettingPermissions = false;
        //});
    }

    close(): void {
        this.modal.hide();
    }
}
