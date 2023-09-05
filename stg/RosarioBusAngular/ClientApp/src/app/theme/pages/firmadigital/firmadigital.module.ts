

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
import { CanDeactivateGuard } from '../../../app.component';
import { SelectButtonModule } from 'primeng/selectbutton';
import { MatDialogModule, MatTabsModule, MatExpansionModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { BandejaEmpleadorComponent } from './bdempleador/bandejaempleador.component';
import { BandejaEmpleadoComponent } from './bdempleado/bandejaempleado.component';
import { TipoDocumentoComponent } from './fdtipodocumento/tipodocumento.component';
import { DocumentosProcesadosService } from './services/documentosprocesados.service';
import { FDTipoDocumentoComboComponent } from './shared/tipodocumento-combo.component';
import { FDTiposDocumentosService } from './services/fdtiposdocumentos.service';
import { FDEstadosComboComponent } from './shared/fdestados-combo.component';
import { FDEstadosService } from './services/fdestados.service';
import { FDAccionesGrillaComboComponent } from './shared/fdaccionesgrilla-combo.component';
import { FdAccionesPermitidasService } from './services/fdaccionespermitidas.service';
import { FdAccionesService } from './services/fdacciones.service';
import { CreateOrEditTipoDocumentoModalComponent } from './fdtipodocumento/create-or-edit-tipoDocumento-modal.component';
import { AbrirArchivosModalComponent } from './abrirarchivo-modal/abrirarchivo-modal.component';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { ErrorResponseAplicarAccionModalComponent } from './errorResponse/errorResponse.component';
import { SeleccionarCorreoModalComponent } from './seleccionarcorreo-modal/seleccionarcorreo-modal.component';
import { ImportarDocumentosModalComponent } from './importardocumentos/importardocumentos-modal.component';
import { ImportarDocumentosTriggerComponent } from './importardocumentos/importardocumentosTrigger.component';
import { RevisarErroresComponent } from './revisarerrores/revisarerrores.component';
import { DocumentosErrorService } from './services/documentosconerror.service';
import { RechazarDocumentoComponent } from './rechazar/rechazar-documento.component';
import { VerArchivosModalComponent } from './verarchivo-modal/verarchivo-modal.component';
import { AdministrarCertificadoComponent } from './admincertificados/administrar-certificado.component';
import { FdCertificadosService } from './services/fdcertificados.service';
import { HistorialCertificadosComponent } from './admincertificados/historial-certificados.component';
import { SubirCertificadoComponent } from './admincertificados/subir-certificado.component';
import { MiCertificadoComponent } from './micertificado/mi-certificado.component';
import { CertificadoEmailComponent } from './micertificado/certificado-email.component';
import { EsperaFirmadorComponent } from './documento-progreso/espera-firmador.component';

const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "bdempleador",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: BandejaEmpleadorComponent,
                data: {
                    permissions: {
                        only: 'FirmaDigital.BD-Empleador.Visualizar',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "bdempleado",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: BandejaEmpleadoComponent,
                data: {
                    permissions: {
                        only: 'FirmaDigital.BD-Empleado.Visualizar',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "fdtipodocumento",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TipoDocumentoComponent,
                data: {
                    permissions: {
                        only: 'FirmaDigital.TipoDocumento.Administracion',
                        redirectTo: '/401'
                    }
                }
            },
            {
                path: "importardocumentos",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: ImportarDocumentosTriggerComponent,
                data: {
                    permissions: {
                        only: 'FirmaDigital.Archivos.ImportarDocumentos',
                        redirectTo: '/401'
                    }
                }
            },
            {
                path: "revisarerrores",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: RevisarErroresComponent,
                data: {
                    permissions: {
                        only: 'FirmaDigital.Archivos.RevisarErrores',
                        redirectTo: '/401'
                    }
                }
            },
            {
                path: "admincertificados",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: AdministrarCertificadoComponent,
                data: {
                    permissions: {
                        only: 'FirmaDigital.AdministrarCertificados.Visualizar',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "micertificado",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: MiCertificadoComponent,
                data: {
                    permissions: {
                        only: 'FirmaDigital.MiCertificado.Visualizar',
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
        MatDialogModule,
        MatTabsModule,
        MatExpansionModule,
        PdfViewerModule
    ], exports: [
        RouterModule
    ], declarations: [
        BandejaEmpleadorComponent,
        BandejaEmpleadoComponent,
        TipoDocumentoComponent,
        CreateOrEditTipoDocumentoModalComponent,
        FDTipoDocumentoComboComponent,
        FDEstadosComboComponent,
        FDAccionesGrillaComboComponent,
        AbrirArchivosModalComponent,
        EsperaFirmadorComponent,
        VerArchivosModalComponent,
        RechazarDocumentoComponent,
        ErrorResponseAplicarAccionModalComponent,
        SeleccionarCorreoModalComponent,
        ImportarDocumentosTriggerComponent,
        ImportarDocumentosModalComponent,
        RevisarErroresComponent,
        AdministrarCertificadoComponent,
        HistorialCertificadosComponent,
        SubirCertificadoComponent,
        MiCertificadoComponent,
        CertificadoEmailComponent
    ],
    entryComponents: [
        BandejaEmpleadorComponent,
        BandejaEmpleadoComponent,
        FDTipoDocumentoComboComponent,
        FDAccionesGrillaComboComponent,
        FDEstadosComboComponent,
        TipoDocumentoComponent,
        CreateOrEditTipoDocumentoModalComponent,
        AbrirArchivosModalComponent,
        EsperaFirmadorComponent,
        VerArchivosModalComponent,
        RechazarDocumentoComponent,
        ErrorResponseAplicarAccionModalComponent,
        SeleccionarCorreoModalComponent,
        ImportarDocumentosTriggerComponent,
        ImportarDocumentosModalComponent,
        RevisarErroresComponent,
        AdministrarCertificadoComponent,
        HistorialCertificadosComponent,
        SubirCertificadoComponent,
        MiCertificadoComponent,
        CertificadoEmailComponent
    ],
    providers: [
        DocumentosProcesadosService,
        FDTiposDocumentosService,
        FDEstadosService,
        FdAccionesService,
        FdAccionesPermitidasService,
        DocumentosErrorService,
        FdCertificadosService,
        { provide: MatDialogRef, useValue: {} },
        { provide: MAT_DIALOG_DATA, useValue: [] }
    ],
})
export class FirmaDigitalModule {



}