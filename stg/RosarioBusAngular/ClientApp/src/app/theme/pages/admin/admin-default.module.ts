
import { NgModule, Injector } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';
import { AdminDefaultComponent } from './admin-default.component';
import { DefaultComponent } from '../default/default.component';
import { UsersComponent } from '../admin/users/users.component';
import { RolesComponent } from '../admin/roles/roles.component';
import { AuthGuard } from "../../../auth/guards/auth.guard";
import { RoleComboComponent } from "./shared/role-combo.component";
import { CreateOrEditUserModalComponent } from './users/create-or-edit-user-modal.component';
import { EditUserPermissionsModalComponent } from './users/edit-user-permissions-modal.component';
import { EditRolPermissionsModalComponent } from './roles/edit-rol-permissions-modal.component';
import { PermissionTreeComponent } from './shared/permission-tree.component';
import { RolesService } from './roles/roles.service';
import { CreateOrEditRolModalComponent } from './roles/create-or-edit-rol-modal.component';
import { SharedModule } from '../../../shared/shared.module';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { NgxPermissionsModule } from 'ngx-permissions';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { EditUserLineasModalComponent } from './users/edit-user-lineas-modal.component';
import { LineaAutoCompleteComponent } from '../planificacion/shared/linea-autocomplete.component';
import { ParametersComponent } from './parameters/parameters.component';
import { ParametersService } from './parameters/parameters.service';
import { CreateOrEditParametersModalComponent } from './parameters/create-or-edit-parameters-modal.component';
import { DataTypesComboComponent } from './parameters/dataType-combo.component';
import { DataTypeService } from './parameters/dataType.service';
import { TipoDniService } from '../siniestros/tipodni/tipodni.service';
import { TipoDniComponent } from './tipodni/tipodni.component';
import { LocalidadesComponent } from './localidades/localidades.component';
import { SectoresTarifariosComponent } from './sectores-tarifarios/sectores-tarifarios.component';
import { CreateOrEditSectoresTarifariosComponent } from './sectores-tarifarios/create-or-edit-sectores-tarifarios.component';
import { SectoresTarifariosService } from '../planificacion/sectoresTarifarios/sectoresTarifarios.service';
import { TalleresIvuComponent } from './talleresivu/talleresivu.component';
import { CreateOrEditTalleresIvuModalComponent } from './talleresivu/create-or-edit-talleresivu-modal.component';
import { PlaTalleresIvuService } from './talleresivu/talleresivu.service';
import { GalponComboComponent } from '../planificacion/shared/Galpon-combo.component';
import { GruposInspectoresComboComponent } from '../inspectores/shared/gruposinspectores-combo.component';
import { GruposInspectoresComponent } from '../inspectores/gruposinspectores/gruposinspectores.component';
import { GruposInspectoresService } from '../inspectores/gruposinspectores/gruposinspectores.service';
import { NotificationComponent } from '../../../shared/notification/notification.component';
import { NotificationComboComponent } from './shared/notification-combo.component';
import { EmpleadosService } from '../siniestros/empleados/empleados.service';
import { ZonasComponent } from '../inspectores/zonas/zonas.component';
import { ZonasService } from '../inspectores/zonas/zonas.service';
import { UserService } from './users/user.service';

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "index",
                component: AdminDefaultComponent
            },
            {
                path: "users",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: UsersComponent,
                data: {
                    permissions: {
                        only: 'Admin.User.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "sectores-tarifarios",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: SectoresTarifariosComponent,
                data: {
                    permissions: {
                        only: 'Admin.SectoresTarifarios.Administracion',
                        redirectTo: '/401'
                    }
                }
            },
            {
                path: "roles",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: RolesComponent,
                data: {
                    permissions: {
                        only: 'Admin.Rol.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "parametros",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: ParametersComponent,
                data: {
                    permissions: {
                        only: 'Admin.Parametros.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "tipodni",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TipoDniComponent,
                data: {
                    permissions: {
                        only: 'Admin.TipoDni.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "localidades",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: LocalidadesComponent,
                data: {
                    permissions: {
                        only: 'Admin.Localidad.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "talleresivu",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TalleresIvuComponent,
                data: {
                    permissions: {
                        only: 'Admin.TalleresIVU.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "",
                redirectTo: "index",
                pathMatch: "full"
            }
        ]
    }
];
@NgModule({
    imports: [
        FormsModule,
        CommonModule,
        RouterModule.forChild(routes),
        SharedModule,
        AutoCompleteModule,
    ], exports: [
        RouterModule
    ], declarations: [
        AdminDefaultComponent,
        RolesComponent,
        // RoleComboComponent,
        CreateOrEditRolModalComponent,
        EditRolPermissionsModalComponent,
        ParametersComponent,
        CreateOrEditParametersModalComponent,
        TipoDniComponent,
        LocalidadesComponent,
        SectoresTarifariosComponent,
        CreateOrEditSectoresTarifariosComponent,
        TalleresIvuComponent,
        CreateOrEditTalleresIvuModalComponent,
    ],
    entryComponents: [
        CreateOrEditRolModalComponent,
        EditRolPermissionsModalComponent,
        CreateOrEditParametersModalComponent,
        TipoDniComponent,
        LocalidadesComponent,
        CreateOrEditSectoresTarifariosComponent,
        TalleresIvuComponent,
        CreateOrEditTalleresIvuModalComponent,
    ],
    providers: [
        // RolesService,
        UserService,
        ParametersService,
        DataTypeService,
        TipoDniService,
        SectoresTarifariosService,
        PlaTalleresIvuService,
        EmpleadosService,
        ZonasService
    ],
})
export class AdminDefaultModule {
    constructor(private injector: Injector) {
        //LocatorService.injector = this.injector; 
    }
}