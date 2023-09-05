
import { DataService } from '../../../shared/common/services/data.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule, TooltipModule, TabsModule } from 'ngx-bootstrap';

import { Routes, RouterModule } from '@angular/router';
import { LayoutModule } from '../../layouts/layout.module';
import { DefaultComponent } from '../default/default.component';

import { UtilsModule } from '../../../shared/utils/utils.module';
import { NgxPermissionsGuard } from 'ngx-permissions';
import { AuthGuard } from "../../../auth/guards/auth.guard";

import { RbMapLineaPorGrupoFilter } from '../../../components/rbmaps/RbMapLineaPorGrupoFilter';
import { SharedModule } from '../../../shared/shared.module';
import { ConsecuenciasComponent } from './consecuencias/consecuencias.component';
import { ConsecuenciasService } from './consecuencias/consecuencias.service';
import { CausasComponent } from './causas/causas.component';
import { CausasService } from './causas/causas.service';
import { EstadosComponent } from './estados/estados.component';
import { AbogadosComponent } from './abogados/abogados.component';
import { TipoLesionadoService } from './tipolesionado/tipolesionado.service';
import { FactoresIntervinientesComponent } from './factores-intervinientes/factoresintervinientes.component';
import { FactoresIntervinientesService } from './factores-intervinientes/factoresintervinientes.service';
import { ProvinciasService } from './provincias/provincias.service';
import { CiaSegurosComponent } from './seguros/seguros.component';
import { CiaSegurosService } from './seguros/seguros.service';
import { TipoMuebleComponent } from './tipoMuebleInmueble/tipomueble.component';
import { TipoMuebleService } from './tipoMuebleInmueble/tipomueble.service';
import { PracticantesComponent } from './practicantes/practicantes.component';
import { PracticantesService } from './practicantes/practicantes.service';
import { TipoInvolucradoComponent } from './tipoinvolucrado/tipoinvolucrado.component';
import { JuzgadosComponent } from './juzgados/juzgados.component';
import { JuzgadosService } from './juzgados/juzgados.service';
import { SiniestroComponent } from './siniestro/siniestro.component';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { SortablejsModule } from 'angular-sortablejs/dist';
import { CreateOrEditConsecuenciasModalComponent } from './consecuencias/create-or-edit-consecuencias-modal.component';
import { CreateOrEditFactoresIntervinientesModalComponent } from './factores-intervinientes/create-or-edit-factoresintervinientes-modal.component';
import { CreateOrEditSegurosModalComponent } from './seguros/create-or-edit-seguros-modal.component';
import { CreateOrEditAbogadosModalComponent } from './abogados/create-or-edit-abogados-modal.component';
import { CreateOrEditTipoMuebleModalComponent } from './tipoMuebleInmueble/create-or-edit-tipomueble-modal.component';
import { CreateOrEditTipoLesionadoModalComponent } from './tipolesionado/create-or-edit-tipolesionado-modal.component';
import { CreateOrEditTipoInvolucradoModalComponent } from './tipoinvolucrado/create-or-edit-tipoinvolucrado-modal.component';
import { CreateOrEditPracticantesModalComponent } from './practicantes/create-or-edit-practicantes-modal.component';
import { CreateOrEditCausasModalComponent } from './causas/create-or-edit-causas-modal.component';
import { MatTabsModule, MatAccordion, MatExpansionModule, MatInputModule } from '@angular/material';
import { MatDialogModule, MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SiniestroTabsComponent } from './siniestro/tabs/create-or-edit-siniestro.component';
import { CustomFormsModule } from 'ng2-validation'
import { CategoriasService } from './consecuencias/categorias.service';
import { SubCausasService } from './causas/subcausas.service';
import { EmpleadosService } from './empleados/empleados.service';
import { TipoLesionadoComponent } from './tipolesionado/tipolesionado.component';
import { ConductorComponent } from './siniestro/conductor/conductor.component';
import { CocheComponent } from './siniestro/coche/coche.component';
import { EmpresaService } from '../planificacion/empresa/empresa.service';
import { SucursalService } from '../planificacion/sucursal/sucursal.service';
import { EmpPractComboComponent } from './shared/EmPract-combo.component';
import { PracticantesAutoCompleteComponent } from './shared/practicante-autocomplete.component';
import { LegajosEmpleadoService } from './legajoempleado/legajosempleado.service';
import { ConductasNormasComponent } from './normas/normas.component';
import { CreateOrEditConductasNormasModalComponent } from './normas/create-or-edit-normas-modal.component';
import { ConductasNormasService } from './normas/normas.service';
import { CochesAutoCompleteComponent } from './shared/coche-autocomplete.component';
import { CochesService } from './coches/coches.service';
import { BandaSiniestralComboComponent } from './shared/bandasiniestral-combo.component';
import { TipoDanioComboComponent } from './shared/tipodanio-combo.component';
import { TipoDanioService } from './tipodedanio/tipodanio.service';
import { BandaSiniestralService } from './bandasiniestral/bandasiniestral.service';
import { NgbTimepicker, NgbTimepickerModule, NgbAccordionModule } from '@ng-bootstrap/ng-bootstrap';
import { CausaComboComponent } from './shared/causa-combo.component';
import { SubCausaComboComponent } from './shared/subcausa-combo.component';
import { CategoriasComboComponent } from './shared/categorias-combo.component';
import { ConsecuenciasComboComponent } from './shared/consecuencia-combo.component';
import { FactoresComboComponent } from './shared/factores-combo.component';
import { NormasComponent } from './siniestro/normas/normas.component';
import { NormasComboComponent } from './shared/normas-combo.component';
import { SegurosComboComponent } from './shared/seguros-combo.component';
import { DatosLugarComponent } from './siniestro/datoslugar/datoslugar.component';
import { SiniestroMapsComponent } from '../../../components/rbmaps/siniestro.maps.component';
import { DescargoComboComponent } from './shared/descargo-combo.component';
import { ResponsableComboComponent } from './shared/responsable-combo.component';
import { CreateCausaDialogComponent } from './siniestro/causas/createcausadialog.component';
import { DetalleSiniestroComponent } from './siniestro/detallesiniestro/detallesiniestro.component';
import { InvolucradosComponent } from './siniestro/involucrados/involucrados.component';
import { AddConsecuenciaModalComponent } from './siniestro/detallesiniestro/add-consecuencia-modal.component';
import { InvolucradoDetailComponent } from './siniestro/involucrados/involucrado-detail-modal.component';

import { InformeComboComponent } from './shared/informe-combo.component';
import { MapaModalComponent } from './siniestro/datoslugar/mapa-modal.component';

import { SancionComboComponent } from './shared/sancion-combo.component';
import { SancionSugeridaService } from './sancionsugerida/sancion.service';
import { TipoLesionadoComboComponent } from './shared/tipolesionado.component';
import { DetalleLesionAgregationComponent } from './siniestro/involucrados/detallelesionagregation.component';
import { DetalleLesionDetailComponent } from './siniestro/involucrados/detallelesion-detail-modal.component';
import { TipoMuebleInmuebleComboComponent } from './shared/tipo-mueble-inmueble-combo.component';
import { CurrencyMaskModule } from "ng2-currency-mask";
import { CreateJuzgadosModalComponent } from './siniestro/reclamos/create-juzgados-modal.component';
import { ReclamosHistoricosService } from './reclamos/reclamoshistoricos.service';
import { CreatePracticantesModalComponent } from './siniestro/conductor/create-practicante-modal.component';
import { CroquiComponent } from '../../../shared/croqui/croqui.component';
import { CreateOrEditTipoElementosModalComponent } from './tipoelemento/create-or-edit-tipoelementos-modal.component';
import { TipoElementosComponent } from './tipoelemento/tipoelemento.component';
import { TipoElementoService } from './tipoelemento/tipoelemento.service';
import { ElementosService } from './elemento/elemento.service';
import { ElementosComponent } from './elemento/elemento.component';
import { CreateOrEditElementosModalComponent } from './elemento/create-or-edit-elemento-modal.component';
import { MultiSelectModule } from 'primeng/multiselect';
import { TipoElementoComboComponent } from './shared/tipoelemento.component';
import { CroquiService } from '../../../shared/croqui/croqui.service';
import { TipoDanioComponent } from './tipodedanio/tipodanio.component';
import { CreateOrEditTipoDanioModalComponent } from './tipodedanio/create-or-edit-tipodanio-modal.component';
import { SancionSugeridaComponent } from './sancionsugerida/sancion.component';
import { CreateOrEditSancionModalComponent } from './sancionsugerida/create-or-edit-sancion-modal.component';
import { TipoColisionComponent } from './tipocolision/tipocolision.component';
import { CreateOrEditTipoColisionModalComponent } from './tipocolision/create-or-edit-tipocolision-modal.component';
import { TipoColisionService } from './tipocolision/tipocolision.service';
import { TipoColisionComboComponent } from './shared/tipocolision-combo.component';


const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "consecuencias",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: ConsecuenciasComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.Consecuencia.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "factores-intervinientes",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: FactoresIntervinientesComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.FactoresIntervinientes.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "seguros",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: CiaSegurosComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.Seguro.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "abogados",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: AbogadosComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.Abogado.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "juzgados",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: JuzgadosComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.Juzgado.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "tipolesionado",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TipoLesionadoComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.TipoLesionado.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "tipoMuebleInmueble",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TipoMuebleComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.TipoMuebleInmueble.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "tipoInvolucrado",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TipoInvolucradoComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.TipoInvolucrado.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "practicantes",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: PracticantesComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.Practicante.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "causas",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: CausasComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.Causa.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "estados",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: EstadosComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.Estado.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "normas",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: ConductasNormasComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.Norma.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "tipoelemento",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TipoElementosComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.TipoElemento.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "elemento",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: ElementosComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.Elemento.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "siniestro",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: SiniestroComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.Siniestro.Visualizar',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "tipodedanio",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TipoDanioComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.TipoDanio.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "sancionsugerida",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: SancionSugeridaComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.SancionSugerida.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "tipocolision",
                // TODO: EH NgxPermissionsGuard,
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TipoColisionComponent,
                data: {
                    permissions: {
                        only: 'Siniestro.TipoColision.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            { path: 'croqui', component: CroquiComponent },

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
        MatTabsModule,
        MatExpansionModule,
        NgbTimepickerModule,
        MultiSelectModule,
        MatDialogModule,
        CurrencyMaskModule
    ], exports: [
        RouterModule
    ], declarations: [
        ConsecuenciasComponent,
        CausasComponent,
        CiaSegurosComponent,
        AbogadosComponent,
        JuzgadosComponent,
        FactoresIntervinientesComponent,
        SiniestroComponent,
        TipoLesionadoComponent,
        TipoMuebleComponent,
        TipoInvolucradoComponent,
        PracticantesComponent,
        EstadosComponent,
        CreateOrEditConsecuenciasModalComponent,
        CreateOrEditFactoresIntervinientesModalComponent,
        SiniestroTabsComponent,
        CreateOrEditSegurosModalComponent,
        InvolucradoDetailComponent,
        DetalleLesionDetailComponent,
        CreateOrEditAbogadosModalComponent,
        CreateOrEditTipoLesionadoModalComponent,
        CreateOrEditTipoMuebleModalComponent,
        CreateOrEditTipoInvolucradoModalComponent,
        CreateOrEditPracticantesModalComponent,
        CreateOrEditCausasModalComponent,
        TipoLesionadoComboComponent,
        TipoMuebleInmuebleComboComponent,
        ConductorComponent,
        CocheComponent,
        EmpPractComboComponent,
        PracticantesAutoCompleteComponent,
        ConductasNormasComponent,
        CreateOrEditConductasNormasModalComponent,
        CochesAutoCompleteComponent,
        BandaSiniestralComboComponent,
        TipoDanioComboComponent,
        CausaComboComponent,
        SubCausaComboComponent,
        CategoriasComboComponent,
        ConsecuenciasComboComponent,
        FactoresComboComponent,
        NormasComponent,
        NormasComboComponent,
        SegurosComboComponent,
        DetalleSiniestroComponent,
        DatosLugarComponent,
        SiniestroMapsComponent,
        DescargoComboComponent,
        ResponsableComboComponent,
        CreateCausaDialogComponent,
        AddConsecuenciaModalComponent,
        InvolucradosComponent,
        DetalleLesionAgregationComponent,
        InformeComboComponent,
        MapaModalComponent,
        SancionComboComponent,
        CreateJuzgadosModalComponent,
        CreatePracticantesModalComponent,
        CreateOrEditTipoElementosModalComponent,
        TipoElementosComponent,
        ElementosComponent,
        CreateOrEditElementosModalComponent,
        TipoElementoComboComponent,
        TipoDanioComponent,
        CreateOrEditTipoDanioModalComponent,
        SancionSugeridaComponent,
        CreateOrEditSancionModalComponent,
        TipoColisionComponent,
        CreateOrEditTipoColisionModalComponent,
        TipoColisionComboComponent
    ],
    entryComponents: [
        CreateOrEditConsecuenciasModalComponent,
        CreateOrEditFactoresIntervinientesModalComponent,
        CreateOrEditConductasNormasModalComponent,
        SiniestroTabsComponent,
        CreateOrEditSegurosModalComponent,
        InvolucradoDetailComponent,
        DetalleLesionDetailComponent,
        CreateOrEditAbogadosModalComponent,
        CreateOrEditTipoLesionadoModalComponent,
        CreateOrEditTipoMuebleModalComponent,
        CreateOrEditTipoInvolucradoModalComponent,
        CreateOrEditPracticantesModalComponent,
        CreateOrEditCausasModalComponent,
        TipoLesionadoComboComponent,
        TipoMuebleInmuebleComboComponent,
        ConductorComponent,
        CocheComponent,
        EmpPractComboComponent,
        PracticantesAutoCompleteComponent,
        CochesAutoCompleteComponent,
        BandaSiniestralComboComponent,
        TipoDanioComboComponent,
        CausaComboComponent,
        SubCausaComboComponent,
        CategoriasComboComponent,
        ConsecuenciasComboComponent,
        FactoresComboComponent,
        NormasComponent,
        NormasComboComponent,
        SegurosComboComponent,
        DetalleSiniestroComponent,
        DatosLugarComponent,
        SiniestroMapsComponent,
        DescargoComboComponent,
        ResponsableComboComponent,
        CreateCausaDialogComponent,
        AddConsecuenciaModalComponent,
        InvolucradosComponent,
        DetalleLesionAgregationComponent,
        InformeComboComponent,
        MapaModalComponent,
        SancionComboComponent,
        CreateJuzgadosModalComponent,
        CreatePracticantesModalComponent,
        CreateOrEditTipoElementosModalComponent,
        TipoElementosComponent,
        ElementosComponent,
        CreateOrEditElementosModalComponent,
        TipoElementoComboComponent,
        TipoDanioComponent,
        CreateOrEditTipoDanioModalComponent,
        SancionSugeridaComponent,
        CreateOrEditSancionModalComponent,
        TipoColisionComponent,
        CreateOrEditTipoColisionModalComponent,
        TipoColisionComboComponent
    ],
    providers: [
        ConsecuenciasService,
        CausasService,
        SubCausasService,
        FactoresIntervinientesService,
        CiaSegurosService,
        CategoriasService,
        JuzgadosService,
        TipoLesionadoService,
        TipoMuebleService,
        PracticantesService,
        EmpleadosService,
        EmpresaService,
        SucursalService,
        PracticantesService,
        LegajosEmpleadoService,
        ConductasNormasService,
        CochesService,
        TipoDanioService,
        BandaSiniestralService,
        SancionSugeridaService,
        ReclamosHistoricosService,
        TipoElementoService,
        ElementosService,
        TipoColisionService,
        { provide: MatDialogRef, useValue: {} },
        { provide: MAT_DIALOG_DATA, useValue: [] },
        ProvinciasService

    ],
})
export class SiniestrosModule {



}