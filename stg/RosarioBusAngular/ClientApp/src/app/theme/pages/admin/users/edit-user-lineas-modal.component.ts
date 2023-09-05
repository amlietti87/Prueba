import { Component, ViewChild, Injector, AfterViewInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';


import { PermissionTreeComponent } from '../shared/permission-tree.component';


import * as _ from 'lodash';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { UpdatePermissionsInput } from '../model/permission.model';
import { UpdateLineasForEdit } from '../model/usuariolineas.model';
import { UserDto } from '../model/user.model';
import { ItemDto } from '../../../../shared/model/base.model';
import { LineaService } from '../../planificacion/linea/linea.service';
import { LineaFilter } from '../../planificacion/model/linea.model';
import { UserService } from '../../../../services/user.service';




@Component({
    selector: 'editUserLineasModal',
    templateUrl: './edit-user-lineas-modal.component.html'
})
export class EditUserLineasModalComponent extends AppComponentBase {
    // ngAfterViewInit(): void { implements AfterViewInit
    //     if (this.mode == 'inspector') {
    //         this.modoInspector = true;
    //         this.allowModificarUsr = this.permission.isGranted("Inspectores.User.Modificar");
    //     }
    //     console.log("MODE::::::::",this.user);

    // }

    @ViewChild('editModal') modal: ModalDirective;


    model: UpdateLineasForEdit = new UpdateLineasForEdit();

    saving: boolean = false;

    user: UserDto;
    userId: number;
    userName: string;
    SucursalId: number = 0;
    selectItem: ItemDto;
    mode: string;
    allowModificarUsr: boolean = true;
    modoInspector: boolean = false;

    constructor(
        injector: Injector,
        private _userService: UserService,
        private _lineaService: LineaService
    ) {
        super(injector);
    }

    show(_user: UserDto): void {

        this.user = _user;
        this.userId = this.user.Id;
        this.userName = this.user.NomUsuario;
        this.SucursalId = this.user.SucursalId;
        this._userService.GetUserLineasForEdit(this.userId).subscribe(result => {
            // this.permissionTree.editData = result.DataObject;
            this.model = result.DataObject;
            if (this.mode == 'inspector') {
                this.modoInspector = true;
                this.allowModificarUsr = this.permission.isGranted("Inspectores.User.Modificar");
            }
            console.log("MODE::::::::", this.user);

            this.modal.show();
        });

    }


    ngOnInit() {

    }

    save(): void {
        let input = new UpdateLineasForEdit();

        this.saving = true;
        this._userService.SetUserLineasForEdit(this.model)
            .finally(() => { this.saving = false; })
            .subscribe(() => {
                this.notify.info("Guardado exitosamente");
                this.close();
            });
    }


    addLineaSucursal() {
        var self = this;
        if (this.SucursalId) {
            var filtro = new LineaFilter();
            filtro.SucursalId = this.SucursalId;
            this._lineaService.FindItemsAsync(filtro).subscribe((result) => {

                var msg = [];
                result.DataObject.forEach(item => {

                    if (self.model.Lineas.some(x => x.Id == item.Id)) {
                        msg.push(item.Description);
                    }

                    self.addAndValidateLinea(item, false);
                });
                if (msg.length > 0) {
                    this.notificationService.warn(msg.join(', '), "Algunas Lineas ya fueron agregadas");
                }



            });

        }
    }



    addLinea(item: ItemDto): void {
        this.addAndValidateLinea(item, true);
    }


    addAndValidateLinea(item: ItemDto, showwarn: boolean) {
        if (item.Id) {
            if (!this.model.Lineas.some(x => x.Id == item.Id)) {

                item.IsSelected = true;
                this.model.Lineas.push(item);

            }
            else {
                var c = this.model.Lineas.find(x => x.Id == item.Id);
                if (showwarn) {
                    this.notificationService.warn("La linea ya fue agregada", c.Description);
                }

                c.animate = true;

                setTimeout(() => {
                    c.animate = false;
                }, 1000);
            }

            this.selectItem = null;

        }
    }

    removeLinea(item: ItemDto) {

        var index = this.model.Lineas.findIndex(x => x.Id == item.Id);
        if (index >= 0) {
            this.model.Lineas.splice(index, 1);
        }

    }


    close(): void {
        this.modal.hide();
    }
}
