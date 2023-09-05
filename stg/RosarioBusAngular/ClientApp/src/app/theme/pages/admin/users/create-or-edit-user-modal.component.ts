import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Injectable } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';

import { ViewMode, StatusResponse, ItemDto } from '../../../../shared/model/base.model';


import { DetailModalComponent } from '../../../../shared/manager/detail.component';
import { UserService } from '../../../../services/user.service';
import { DialogService, DialogComponent } from 'ng2-bootstrap-modal';
import * as _ from 'lodash';
import { elementAt } from 'rxjs/operators/elementAt';
import { NgForm } from '@angular/forms/src/directives/ng_form';
import { FilterPipe, SortByPipe } from '../../../../shared/utils/pipe/pipe'
import { UserRoleDto, UserDto } from '../model/user.model';
import { EmpleadosAutoCompleteComponent } from '../../siniestros/shared/empleado-autocomplete.component';
import { EmpleadosService } from '../../siniestros/empleados/empleados.service';

@Component({
    selector: 'createOrEditUserModal',
    templateUrl: './create-or-edit-user-modal.component.html',

})
export class CreateOrEditUserModalComponent extends DetailModalComponent<UserDto> {

    term: { isAssigned: true };
    roles: UserRoleDto[];
    profilePicture: string;
    allowInspector: boolean = false;
    allowEmpleado: boolean = false;
    allowTurno: boolean = false;
    selecEmpleado: ItemDto;
    allowBlanquearPassword: boolean = false;
    mode: string;
    modoInspector: boolean = false;
    allowModificarUsr: boolean = true;
    //variables creadas para comparar al cambiar grupo
    grupoId: number;
    turnoId: number;

    constructor(
        injector: Injector,
        protected service: UserService,
        private empleadoService: EmpleadosService
    ) {
        super(service, injector);
        this.detail = new UserDto();
        this.active = true;
        this.profilePicture = "";
        this.allowBlanquearPassword = this.permission.isGranted("Admin.User.BlanqueoPassword");
    }

    //PermiteLoginManual: boolean = false;
    @ViewChild('EmpleadoId') EmpleadoId: EmpleadosAutoCompleteComponent;

    ngAfterViewChecked(): void {

        //Temporary fix for: https://github.com/valor-software/ngx-bootstrap/issues/1508
        $('tabset ul.nav').addClass('m-tabs-line');
        $('tabset ul.nav li a.nav-link').addClass('m-tabs__link');
    }

    showNew(item: UserDto) {
        this.viewMode = ViewMode.Add;
        this.service.getById(0).subscribe(result => {
            this.viewMode = ViewMode.Add;
            this.showDto(result.DataObject);
        });
    }


    completedataBeforeShow(item: UserDto): any {

        if (this.mode == 'inspector') {
            this.modoInspector = true;
            this.allowModificarUsr = this.permission.isGranted("Inspectores.User.Modificar");
        }


        if (item.EmpleadoId != null) {
            var itemdto = new ItemDto();
            itemdto.Id = item.EmpleadoId;
            itemdto.Description = item.DescEmpleado;
            this.selecEmpleado = itemdto;
        }

        if (this.viewMode == ViewMode.Add) {
            this.roles = item.UserRoles || [];
            item.PermiteLoginManual = true;
        }
        else {

            if (item.UserRoles) {
                this.roles = item.UserRoles;
            }
        }

        this.grupoId = item.GruposInspectoresId;
        this.turnoId = item.TurnoId;

    }

    completedataBeforeSave(item: UserDto): any {
        this.detail.UserRoles = this.roles;
        if (this.selecEmpleado != null) {

            this.detail.EmpleadoId = this.selecEmpleado.Id;
            this.detail.DescEmpleado = this.selecEmpleado.Description.toString();
        }
        else {
            this.detail.EmpleadoId = undefined;
            this.detail.DescEmpleado = undefined;
        }

        if (this.detail.GruposInspectoresId == 0) {
            this.detail.GruposInspectoresId = undefined;
        }


    }

    getProfilePicture(profilePictureId: string): void {
        if (!profilePictureId) {
            this.profilePicture = '/assets/app/media/img/users/default-profile-picture.jpg';
        }
    }

    save(form: NgForm): void {
        var rolInspector = this.detail.UserRoles.find(r => r.RoleName == "Inspector");
        this.allowInspector = rolInspector.IsAssigned && (this.detail.GruposInspectoresId == null || this.detail.GruposInspectoresId.toString() == "");
        this.allowEmpleado = rolInspector.IsAssigned && this.selecEmpleado == null;
        this.allowTurno = rolInspector.IsAssigned && (this.detail.TurnoId == null || this.detail.TurnoId.toString() == "");

        if (this.allowInspector || this.allowEmpleado || this.allowTurno) {
            return;
        }

        if (this.detailForm && this.detailForm.form.invalid) {
            return;
        }

        this.saving = true;
        this.completedataBeforeSave(this.detail);

        if (!this.validateSave()) {
            this.saving = false;
            return;
        }

        this.SaveDetail();
    }

    OnChangeRole(event: any, role: any) {
        if (role.RoleName == 'Inspector' && !event) {
            this.allowInspector = false;
            this.allowEmpleado = false;
            this.allowTurno = false;
        }
    }

    OnChangeEmpleado(event: any, selecEmpl: any) {
        if (this.selecEmpleado == null) {
            this.allowEmpleado = true;
        }
    }

    getAssignedRoleCount(): number {
        return _.filter(this.roles, { IsAssigned: true }).length;
    }

    resetPassword(): void {
        this.saving = true;

        this.service.resetPassword(this.detail.Id)
            .finally(() => { this.saving = false; })
            .subscribe(resp => {
                if (resp.Status == StatusResponse.Ok) {
                    this.message.success("Se ha enviado un mail al Usuario.", "Blanqueo de contraseña");

                }
                else {
                    this.message.error("Ha ocurrido un error.", "Blanqueo de contraseña");
                }
            });
    }

    findUser(): void {
        this.service.findUser(this.detail.LogonName)
            .finally(() => { })
            .subscribe(resp => {
                if (resp.Status == StatusResponse.Ok) {
                    this.detail.DisplayName = resp.DataObject.DisplayName;
                    this.detail.Mail = resp.DataObject.Mail;
                    this.detail.UserPrincipalName = resp.DataObject.UserPrincipalName;
                    this.detail.NomUsuario = resp.DataObject.NomUsuario;
                    this.detail.LogonName = resp.DataObject.LogonName;

                } else {

                }

            });
    }

    OnChangeGrupos(event: any) {
        //Al cambiar grupo, limpia combo turnos.
        if (this.grupoId == parseInt(event)) {
            this.detail.TurnoId = this.turnoId;
        } else {
            this.detail.TurnoId = null;
        }

    }


    close(): void {
        this.detailForm.controls['EmpleadoId'].reset();
        this.allowEmpleado = false;
        this.allowInspector = false;
        this.allowTurno = false;
        super.close();
        this.modal.hide();
    }
}
