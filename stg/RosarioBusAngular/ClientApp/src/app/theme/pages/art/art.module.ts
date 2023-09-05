
import { DataService } from '../../../shared/common/services/data.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModalModule, TooltipModule, TabsModule } from 'ngx-bootstrap';
import { Routes, RouterModule } from '@angular/router';
import { LayoutModule } from '../../layouts/layout.module';
import { DefaultComponent } from '../default/default.component';
import { UtilsModule } from '../../../shared/utils/utils.module';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { AuthGuard } from "../../../auth/guards/auth.guard";
import { RbMapLineaPorGrupoFilter } from '../../../components/rbmaps/RbMapLineaPorGrupoFilter';
import { TallerMapsComponent } from '../../../components/rbmaps/taller.maps.component';
import { SharedModule } from '../../../shared/shared.module';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { GroupByPipe } from '../../../shared/utils/pipe/pipe';
import { SortablejsModule } from 'angular-sortablejs/dist';
import { PickListModule } from 'primeng/picklist';
import { NgxMaskModule } from 'ngx-mask'
import { CurrencyMaskModule } from "ng2-currency-mask";
import { CanDeactivateGuard } from '../../../app.component';
import { SelectButtonModule } from 'primeng/selectbutton';
import { ContingenciasComponent } from './contingencias/contingencias.component';
import { CreateOrEditContingenciasModalComponent } from './contingencias/create-or-edit-contingencias-modal.component';
import { ContingenciasService } from './contingencias/contingencias.service';
import { DenunciasEstadosComponent } from './estados/estados.component';
import { CreateOrEditDenunciasEstadosModalComponent } from './estados/create-or-edit-estados-modal.component';
import { DenunciasEstadosService } from './estados/estados.service';
import { MotivosAudienciasComponent } from './motivosaudiencias/motivosaudiencias.component';
import { CreateOrEditMotivosAudienciasModalComponent } from './motivosaudiencias/create-or-edit-motivosaudiencias-modal.component';
import { MotivosAudienciasService } from './motivosaudiencias/motivosaudiencias.service';
import { MotivosNotificacionesComponent } from './motivosnotificaciones/motivosnotificaciones.component';
import { MotivosNotificacionesService } from './motivosnotificaciones/motivosnotificaciones.service';
import { CreateOrEditMotivosNotificacionesModalComponent } from './motivosnotificaciones/create-or-edit-motivosnotificaciones-modal.component';
import { PatologiasComponent } from './patologias/patologias.component';
import { PatologiasService } from './patologias/patologias.service';
import { CreateOrEditPatologiasModalComponent } from './patologias/create-or-edit-patologias-modal.component';
import { PrestadoresMedicosComponent } from './prestadores/prestadores.component';
import { CreateOrEditPrestadoresMedicosModalComponent } from './prestadores/create-or-edit-prestadores-modal.component';
import { PrestadoresMedicosService } from './prestadores/prestadores.service';
import { TratamientosComponent } from './tratamientos/tratamientos.component';
import { CreateOrEditTratamientosModalComponent } from './tratamientos/create-or-edit-tratamientos-modal.component';
import { TratamientosService } from './tratamientos/tratamientos.service';
import { DenunciasComponent } from './denuncias/denuncias.component';
import { DenunciasTabsComponent } from './denuncias/tabs/create-or-edit-denuncias.component';
//import { AdjuntoComponent } from '../siniestros/siniestro/adjunto/adjunto.component';
import { MatDialogModule, MatTabsModule, MatExpansionModule, MatDialogRef, MAT_DIALOG_DATA, MatTableModule, MatIconModule } from '@angular/material';
import { ContingenciasComboComponent } from './shared/contingencias-combo.component';
import { PatologiasComboComponent } from './shared/patologias-combo.component';
import { EstadosDenunciaComboComponent } from './shared/estadosdenuncia-combo.component';
import { TratamientosComboComponent } from './shared/tratamientos-combo.component';
import { MotivosAudienciaComboComponent } from './shared/motivosaudiencia-combo.component';
import { PrestadoresMedicosComboComponent } from './shared/prestadores-combo.component';
import { MotivosNotificacionComboComponent } from './shared/motivosnotificacion-combo.component';
import { LegajosEmpleadoService } from '../siniestros/legajoempleado/legajosempleado.service';
import { CantidadDiasDirective } from './shared/cantidad-dias.directive';
import { AnularDenunciaModalComponent } from './denuncias/anular/anular-modal.component';
import { DenunciasImportadorComponent } from './denuncias/denuncias-importador/denuncias-importador.component';
import { SucursalComboDenunciaComponent } from './shared/sucursal-combo-denuncia.component';
import { EmpresaComboDenunciaComponent } from './shared/empresa-combo-denuncia.component';

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "contingencias",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: ContingenciasComponent,
                data: {
                    permissions: {
                        only: 'ART.Contingencia.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "estados",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: DenunciasEstadosComponent,
                data: {
                    permissions: {
                        only: 'ART.EstadoDenuncia.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "motivosaudiencias",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: MotivosAudienciasComponent,
                data: {
                    permissions: {
                        only: 'ART.MotivoAudiencia.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "motivosnotificaciones",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: MotivosNotificacionesComponent,
                data: {
                    permissions: {
                        only: 'ART.MotivoNotificacion.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "patologias",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: PatologiasComponent,
                data: {
                    permissions: {
                        only: 'ART.Patologia.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "prestadores",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: PrestadoresMedicosComponent,
                data: {
                    permissions: {
                        only: 'ART.PrestadorMedico.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "tratamientos",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TratamientosComponent,
                data: {
                    permissions: {
                        only: 'ART.Tratamiento.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "denuncias",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: DenunciasComponent,
                data: {
                    permissions: {
                        only: 'ART.Denuncia.Visualizar',
                        redirectTo: '/401'
                    }
                }

            },
        ]
    }
];
@NgModule({
    imports: [
        FormsModule,
        CommonModule,
        RouterModule.forChild(routes),
        ModalModule.forRoot(),
        TabsModule.forRoot(),
        LayoutModule,
        UtilsModule,
        SharedModule,
        AutoCompleteModule,
        SortablejsModule,
        PickListModule,
        NgxMaskModule.forRoot(),
        CurrencyMaskModule,
        MatDialogModule,
        MatTabsModule,
        MatExpansionModule,
        MatTableModule,
        MatIconModule
    ], exports: [
        RouterModule
    ], declarations: [
        ContingenciasComponent,
        CreateOrEditContingenciasModalComponent,
        DenunciasEstadosComponent,
        CreateOrEditDenunciasEstadosModalComponent,
        MotivosAudienciasComponent,
        CreateOrEditMotivosAudienciasModalComponent,
        MotivosNotificacionesComponent,
        CreateOrEditMotivosNotificacionesModalComponent,
        CreateOrEditPatologiasModalComponent,
        PatologiasComponent,
        PrestadoresMedicosComponent,
        CreateOrEditPrestadoresMedicosModalComponent,
        TratamientosComponent,
        CreateOrEditTratamientosModalComponent,
        DenunciasComponent,
        DenunciasTabsComponent,
        ContingenciasComboComponent,
        PatologiasComboComponent,
        EstadosDenunciaComboComponent,
        TratamientosComboComponent,
        MotivosAudienciaComboComponent,
        PrestadoresMedicosComboComponent,
        MotivosNotificacionComboComponent,
        CantidadDiasDirective,
        AnularDenunciaModalComponent,
        DenunciasImportadorComponent,
        SucursalComboDenunciaComponent,
        EmpresaComboDenunciaComponent
    ],
    entryComponents: [
        ContingenciasComponent,
        CreateOrEditContingenciasModalComponent,
        DenunciasEstadosComponent,
        CreateOrEditDenunciasEstadosModalComponent,
        MotivosAudienciasComponent,
        CreateOrEditMotivosAudienciasModalComponent,
        MotivosNotificacionesComponent,
        CreateOrEditMotivosNotificacionesModalComponent,
        CreateOrEditPatologiasModalComponent,
        PatologiasComponent,
        PrestadoresMedicosComponent,
        CreateOrEditPrestadoresMedicosModalComponent,
        TratamientosComponent,
        CreateOrEditTratamientosModalComponent,
        DenunciasComponent,
        DenunciasTabsComponent,
        ContingenciasComboComponent,
        PatologiasComboComponent,
        EstadosDenunciaComboComponent,
        TratamientosComboComponent,
        MotivosAudienciaComboComponent,
        PrestadoresMedicosComboComponent,
        MotivosNotificacionComboComponent,
        AnularDenunciaModalComponent,
        DenunciasImportadorComponent,
        SucursalComboDenunciaComponent,
        EmpresaComboDenunciaComponent
    ],
    providers: [
        ContingenciasService,
        DenunciasEstadosService,
        MotivosAudienciasService,
        MotivosNotificacionesService,
        PatologiasService,
        PrestadoresMedicosService,
        TratamientosService,
        { provide: MatDialogRef, useValue: {} },
        { provide: MAT_DIALOG_DATA, useValue: [] },
        DenunciasEstadosService,
        MotivosAudienciasService,
        PrestadoresMedicosService,
        LegajosEmpleadoService
    ],
})
export class ARTModule {



}