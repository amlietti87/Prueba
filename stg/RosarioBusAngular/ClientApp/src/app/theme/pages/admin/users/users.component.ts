import { ViewMode } from './../../../../shared/model/base.model';
import { Component, Type, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Input } from '@angular/core';
import { BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { UserService } from './user.service';
import { CreateOrEditUserModalComponent } from './create-or-edit-user-modal.component';
declare let mApp: any;
import { EditUserPermissionsModalComponent } from './edit-user-permissions-modal.component';
import { UserDto, UserFilter } from '../model/user.model';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { EditUserLineasModalComponent } from './edit-user-lineas-modal.component';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: "./users.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class UsersComponent extends BaseCrudComponent<UserDto, UserFilter> implements OnInit {
    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditUserModalComponent
    }

    @ViewChild('editUserPermissionsModal') editUserPermissionsModal: EditUserPermissionsModalComponent;
    @ViewChild('editUserLineasModal') editUserlineasModal: EditUserLineasModalComponent;
    id: number;
    RoleId: number;
    sub: Subscription;
    mode: string;
    cambiarModo: boolean = false;

    constructor(injector: Injector, protected _userService: UserService, private _activatedRoute: ActivatedRoute) {
        super(_userService, CreateOrEditUserModalComponent, injector);
        this.title = "Usuarios"
        this.moduleName = "Administración General";
        this.icon = "flaticon-users";
        this.showbreadcum = false;
    }

    allowEditPermisos: boolean = false;
    SetAllowPermission() {
        super.SetAllowPermission();
        this.allowEditPermisos = this.permission.isGranted('Admin.User.Permisos');

    }

    getNewfilter(): UserFilter {
        return new UserFilter();
    }

    ngOnInit() {
        super.ngOnInit();
        this.sub = this._activatedRoute.params.subscribe(params => {
            this.paramsSubscribe(params);
        });
    }

    paramsSubscribe(params: any) {
        this.breadcrumbsService.defaultBreadcrumbs(this.title);

        if (params.mode) {
            this.mode = params.mode;
            if (this.mode == 'inspector') {
                this.cambiarModo = true;
            }
        }
    }

    onEditModoInspector(user) {
        if (user.EsInspector) {
            (this.GetEditComponent() as CreateOrEditUserModalComponent).mode = this.mode;
            // super.onEdit(user);
            this.onEdit(user);

        } else {
            this.notify.error('Usuario no Inspector', '')
        }
    }

    getNewItem(item: UserDto): UserDto {
        return new UserDto(item);
    }

    getDescription(item: UserDto): string {
        return item.NomUsuario;
    }

    onEditUserPermissions(user: UserDto) {
        this.editUserPermissionsModal.show(user.Id, user.NomUsuario)
    }

    onEditUserLineasModoInsp(user: UserDto) {
        if (user.EsInspector) {
            this.editUserlineasModal.show(user);

            (this.editUserlineasModal).mode = this.mode;
        } else {
            this.notify.error('Usuario no Inspector', '')
        }
    }

    onEditUserLineas(user: UserDto) {
        this.editUserlineasModal.show(user)
    }

    onEdit(row: UserDto) {
        this.onEditID(row.Id);
    }

    onEditID(id: any) {

        this.active = false;

        if (this.loadInMaterialPopup) {
            this.service.getById(id).subscribe(e => this.Opendialog(e.DataObject, ViewMode.Modify));
        }
        else {
            this.GetEditComponent().show(id);
        }
    }

}