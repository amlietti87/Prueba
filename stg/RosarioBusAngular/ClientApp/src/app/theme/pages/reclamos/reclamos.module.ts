
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
//import { AdjuntoComponent } from '../siniestros/siniestro/adjunto/adjunto.component';
import { MatDialogModule, MatTabsModule, MatExpansionModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { CausasReclamoComponent } from './causasreclamo/causasreclamo.component';
import { TiposAcuerdoComponent } from './tiposacuerdo/tiposacuerdo.component';
import { RubrosSalarialesComponent } from './rubrossalariales/rubrossalariales.component';
import { TiposReclamoComponent } from './tiposreclamo/tiposreclamo.component';
import { ReclamosComponent } from './reclamos/reclamos.component';
import { JuzgadosService } from '../siniestros/juzgados/juzgados.service';
import { TipoReclamoComboComponent } from './shared/tiporeclamo-combo.component';
import { LegajosEmpleadoService } from '../siniestros/legajoempleado/legajosempleado.service';


const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "reclamos",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: ReclamosComponent,
                data: {
                    permissions: {
                        only: 'Reclamo.Reclamo.Visualizar',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "tiposreclamo",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TiposReclamoComponent,
                data: {
                    permissions: {
                        only: 'Reclamo.TipoReclamo.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "causasreclamo",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: CausasReclamoComponent,
                data: {
                    permissions: {
                        only: 'Reclamo.CausaReclamo.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "tiposacuerdo",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TiposAcuerdoComponent,
                data: {
                    permissions: {
                        only: 'Reclamo.TipoAcuerdo.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "rubrossalariales",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: RubrosSalarialesComponent,
                data: {
                    permissions: {
                        only: 'Reclamo.RubroSalarial.Administracion',
                        redirectTo: '/401'
                    }
                }
            }
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
        MatExpansionModule
    ], exports: [
        RouterModule
    ], declarations: [
        TiposReclamoComponent,
        CausasReclamoComponent,
        TiposAcuerdoComponent,
        RubrosSalarialesComponent
    ],
    entryComponents: [
        TiposReclamoComponent,
        CausasReclamoComponent,
        TiposAcuerdoComponent,
        RubrosSalarialesComponent
    ],
    providers: [
        JuzgadosService,
        LegajosEmpleadoService,
        { provide: MatDialogRef, useValue: {} },
        { provide: MAT_DIALOG_DATA, useValue: [] }
    ],
})
export class ReclamosModule {



}