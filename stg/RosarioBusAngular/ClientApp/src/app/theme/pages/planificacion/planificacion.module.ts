
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
import { RecorridosComponent } from "./recorridos/recorridos.component";
import { AuthGuard } from "../../../auth/guards/auth.guard";

import { RbMapLineaPorGrupoFilter } from '../../../components/rbmaps/RbMapLineaPorGrupoFilter';
import { TallerMapsComponent } from '../../../components/rbmaps/taller.maps.component';
import { SharedModule } from '../../../shared/shared.module';

import { SucursalComponent } from './sucursal/sucursal.component';
import { CreateOrEditSucursalModalComponent } from './sucursal/create-or-edit-sucursal-modal.component';
import { SucursalService } from './sucursal/sucursal.service';
import { LineaComponent } from './linea/linea.component';
import { CreateOrEditLineaModalComponent } from './linea/create-or-edit-linea-modal.component';
import { LineaService } from './linea/linea.service';
import { TipoLineaComboComponent } from './shared/tipoLinea-combo.component';
import { ResponsableInformesComboComponent } from './shared/responsableInformes-combo.component';
import { HServicioComboComponent } from './shared/hservicio-combo.component';
import { TipoLineaService } from './tipoLinea/tipoLinea.service';
import { TipoDiaService } from './tipoDia/tipodia.service';
import { TipoParadaService } from './tipoParada/tipoparada.service';
import { GrupoLineasComboComponent } from './shared/grupolineas-combo.component';
import { GrupoLineasService } from './grupolineas/grupolineas.service';
import { EmpresaService } from './empresa/empresa.service';
import { TipoLineaComponent } from './tipoLinea/tipoLinea.component';
import { TipoDiaComponent } from './tipoDia/tipodia.component';
import { TipoParadaComponent } from './tipoParada/tipoparada.component';
import { CreateOrEditTipoLineaModalComponent } from './tipoLinea/create-or-edit-tipoLinea-modal.component';
import { CreateOrEditTipoDiaModalComponent } from './tipoDia/create-or-edit-tipodia-modal.component';
import { CreateOrEditTipoParadaModalComponent } from './tipoParada/create-or-edit-tipoparada-modal.component';
import { EmpresaComponent } from './empresa/empresa.component';
import { CreateOrEditEmpresaModalComponent } from './empresa/create-or-edit-empresa-modal.component';
import { CreateOrEditGrupoLineasModalComponent } from './grupolineas/create-or-edit-grupolineas-modal.component';
import { GrupoLineasComponent } from './grupolineas/grupolineas.component';
import { AutoCompleteModule } from 'primeng/autocomplete';
import { RamalColorComponent } from './ramalcolor/ramalcolor.component';
import { RamalColorService } from './ramalcolor/ramalcolor.service';
import { CreateOrEditRamaColorModalComponent } from './ramalcolor/create-or-edit-ramalcolor-modal.component';
import { BanderaComponent } from './bandera/bandera.component';
import { CreateOrEditBanderaModalComponent } from './bandera/create-or-edit-bandera-modal.component';
import { BanderaService } from './bandera/bandera.service';
import { CreateOrEditRutaModalComponent } from './ruta/create-or-edit-ruta-modal.component';
import { RutaService } from './ruta/ruta.service';
import { EstadoRutaService } from './estadoruta/estadoruta.service';
import { EstadoRutaComboComponent } from './shared/estadoruta-combo.component';
import { TiempoEsperadoDeCargaService } from './tipoParada/tiempoesperadodecarga.service';
import { TipoDiaComboComponent } from './shared/tipoDia-combo.component';
import { LineaAutoCompleteComponent } from './shared/linea-autocomplete.component';
import { PuntoService } from './punto/punto.service';
import { SectorViewComponent } from './sector/sector-view.component';
import { TallerComponent } from './taller/talleres-mapa.component';
import { TallerService } from './taller/taller.service';
import { CreateOrEditTallerModalComponent } from './taller/create-or-edit-taller-modal.component';
import { TipoBanderaService } from './tipoBandera/tipoBandera.service';
import { TipoBanderaComboComponent } from './shared/tipoBandera-combo.component';
import { CoordenadasService } from './coordenadas/coordenadas.service';
import { LineasComponent } from './lineas/lineas.component';
import { RamalColorComboComponent } from './shared/ramalColor-combo.component';
import { BanderaAutoCompleteComponent } from './shared/bandera-autocomplete.component';
import { GroupByPipe } from '../../../shared/utils/pipe/pipe';
import { ViewRutasComponent } from './lineas/view-rutas.component';
import { SortablejsModule } from 'angular-sortablejs/dist';
import { CodigoSectorComboComponent } from './shared/codigosector-combo.component';
import { BanderaTupService } from './banderaTup/banderaTup.service';
import { BanderaTupComboComponent } from './shared/banderaTup-combo.component';
import { SelectSectoresHorariosModalComponent } from './bandera/select-sectores-horarios-modal.component';
import { PickListModule } from 'primeng/picklist';
import { SentidoBanderaService } from './sentidoBandera/sentidoBandera.service';
import { SentidoBanderaComboComponent } from './shared/sentidoBandera-combo.component';
import { CreateOrEditSentidoBanderaModalComponent } from './sentidoBandera/create-or-edit-sentidobandera-modal.component';
import { SentidoBanderaComponent } from './sentidoBandera/sentidoBandera.component';
import { RamalColorAutoCompleteComponent } from './shared/ramalColor-autocomplete.component';
import { CoordenadasAutoCompleteComponent } from './shared/coordenada-autocomplete.component';
import { EstadoHorarioFechaComboComponent } from './shared/estadohorariofecha-combo.component';
import { EstadoHorarioFechaService } from './estadohorariofecha/estadohorariofecha.service';
import { CreateOrEditCoordenadaModalComponent } from './coordenadas/create-or-edit-coordenadas-modal.component';
import { CoordenadasComponent } from './coordenadas/coordenadas.component';
import { HFechasConfiService, PlaDistribucionDeCochesPorTipoDeDiaService } from './horariofecha/HFechasConfi.service';
import { SubGalponService, SubGalponSinCacheService } from './subgalpon/subgalpon.service';
import { SubgalponComboComponent } from './shared/subgalpon-combo.component';

import { HServiciosService } from './hservicio/servicio.service';
import { MediasVueltasService } from './mediasvueltas/mediasvueltas.service';
import { HHorariosConfiService } from './HHorariosConfi/hhorariosconfi.service';
import { BanderaComboComponent } from './shared/bandera-combo.component';
import { CreateOrEditHFechasConfiServiciosComponent } from './horariofecha/tabsitem/create-or-edit-hfechasconfi-servicios.component';

import { HTposHorasService } from './htposhoras/htposhoras.service';
import { HTposHorasComboComponent } from './shared/htphoras-combo.component';
import { MinutosPorSectorComponent } from './horariofecha/tabsitem/minutos-por-sector.component';
import { HMinxtipoService } from './hminxtipo/hminxtipo.service';
import { NgxMaskModule } from 'ngx-mask'
import { ImportarHorarioFechaComponent } from './horariofecha/importador/importar-horariofecha.component';
import { BanderaCartelService } from './banderacartel/banderacartel.service';
import { CurrencyMaskModule } from "ng2-currency-mask";

import { HFechasConfiComboComponent } from './shared/hfechas_confi-combo.component';

import { MvueltasViewComponent } from './horariofecha/mvueltas/mvueltas-view.component';
import { HorariosPorSectorComponent } from './sabana/horariosPorSector.component';
import { CreateOrEditHorariosPorSectorComponent } from './sabana/create-or-edit-horariosPorSector.component';
import { ServicioNewComponent } from './horariofecha/tabsitem/servicio-new.component';
import { PlaLineaAutoCompleteComponent } from './shared/plalinea-autocomplete.component';
import { CanDeactivateGuard } from '../../../app.component';
import { SelectButtonModule } from 'primeng/selectbutton';
import { ImportarMinutosPorSectorComponent } from './horariofecha/tabsitem/importador/importar-minutosporsector.component';
import { CopiarMinutosPorSectorComponent } from './horariofecha/copiarMinutos/copiar-minutosporsector.component';
import { SubGalponComponent } from './subgalpon/subgalpon.component';
import { CreateOrEditSubGalponModalComponent } from './subgalpon/create-or-edit-subgalpon-modal.component';
import { GruposService } from './grupos/grupo.service';
import { GrupoComboComponent } from './shared/grupo-combo.component';
import { PlanCamService } from './PlaCam/PlanCam.service';
import { PlanCamComboComponent } from './shared/plancam-combo.component';
import { HChoxserComponent } from './horariofecha/tabsitem/hChoxser.component';
import { HChoxserService } from './HChoxser/hChoxser.service';
import { ImportarDuracionComponent } from './horariofecha/tabsitem/importador/importar-duracion.component';
import { AddConfiguModalComponent } from './subgalpon/add-configu-modal.component';
import { OrigenPredictivoComponent } from './shared/origen-predictivo.component';
import { DestinoPredictivoComponent } from './shared/destino-predictivo.component';
import { DetalleSalidasYRelevosComponent } from './HHorariosConfi/detalleSalidasYRelevos.component';
import { CreateOrEditDetalleSalidasYRelevos } from './HHorariosConfi/create-or-edit-detalleSalidasYRelevos.component';
import { HorarioPasajerosComponent } from './HHorariosConfi/horarioPasajeros.component';
import { DistribucionCochesComponent } from './HHorariosConfi/distribucionCoches.component';
import { CreateOrEditHFechasConfiModalComponent } from './horariofecha/hfechasconfig/create-or-edit-hfechasconfi-modal.component';
import { HFechasConfiComponent } from './horariofecha/hfechasconfi.component';
import { HFechasConfiLineaComponent } from './horariofecha/hfechasconfig/hfechasconfi-linea.component';
import { DistribucionDeCochesDetailComponent } from './horariofecha/distribucion/distribuciondecoches-detail.component';
import { DistribucionDeCochesAgregation } from './horariofecha/distribucion/distribuciondecoches-agregation.component';
import { TabsDetalleHorariofechaComponent } from './horariofecha/servicios/tabsdetalle-horariofecha.component';
import { AsignacionBanderaHbasec } from './horariofecha/hfechasconfig/asignacion-bandera-hbasec.component';
import { AsignacionLineaLineaHoraria } from './horariofecha/asignacion-linea-lineahoraria.component';
import { HfechasConfiNewComponent } from './horariofecha/hfechasconfig/hfechasconfi-new.component';
import { SentidoBanderaSubeComboComponent } from './shared/sentidobanderasube-combo.component';
import { PlaSentidoBanderaSubeService } from './PlaSentidoBanderaSube/PlaSentidoBanderaSube.service';
import { HBanderasColoresService } from './hbanderascolores/hbanderascolores.service';
import { ParadaComponent } from './parada/parada.component';
import { ParadaService } from './parada/parada.service';
import { CreateOrEditParadaModalComponent } from './parada/create-or-edit-parada-modal.component';
import { SelectMarkerMapsComponent } from '../../../components/rbmaps/selectmarker.maps.component';
import { ExportarMinutosPorSectorComponent } from './horariofecha/tabsitem/exportador/exportar-minutosporsector.component';
import { ExportarExcelComponent } from './horariofecha/tabsitem/exportar-excel.component';
import { ResponsableInformesService } from './responsableInformes/ResponsableInformes.service';
import { ExportarKmlModalComponent } from './exportarkml/exportarkml-modal.component';
import { DescripcionTipoDiaPredictivoComponent } from './shared/descripcionTipoDia-predictivo.component';
import { ReportePasajerosComponent } from './HHorariosConfi/reportePasajeros.component';
import { RamalColorComboComunComponent } from './shared/ramalColor-comboComun.component';
import { InputSwitchModule, RadioButtonModule } from 'primeng/primeng';
import { ExportarsabanaComponent } from './sabana/exportarsabana.component';
import { ChangeRouteComponent } from './horariofecha/hfechasconfig/change-route.component';
import { StopTypeComboComponent } from './shared/stoptype-combo.component';




const routes: Routes = [
    {
        path: "",
        component: DefaultComponent,
        canActivate: [AuthGuard],
        children: [
            {
                path: "recorridos/:id",
                component: CreateOrEditRutaModalComponent
            },
            {
                path: "",
                redirectTo: "recorridos",
                pathMatch: "full"
            },
            {
                path: "linea/:sucursalid",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: LineaComponent,
                data: {
                    permissions: {
                        only: 'Planificacion.Linea.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "lineas/:sucursalid",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: LineasComponent,
                data: {
                    permissions: {
                        only: 'Planificacion.Linea.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "unidadnegocio",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: SucursalComponent,
                data: {
                    permissions: {
                        only: 'Admin.UnidadNegocio.Administracion',
                        redirectTo: '/401'
                    }
                }

            },

            {
                path: "ramalcolor",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: RamalColorComponent,
                data: {
                    permissions: {
                        only: 'Planificacion.RamaColor.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "ramalcolor/:Id",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: RamalColorComponent,
                data: {
                    permissions: {
                        only: 'Planificacion.RamaColor.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "tipolinea",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TipoLineaComponent,
                data: {
                    permissions: {
                        only: 'Admin.TipoDeLinea.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "empresa",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: EmpresaComponent,
                data: {
                    permissions: {
                        only: 'Admin.Empresa.Administracion',
                        redirectTo: '/401'
                    }
                }

            },

            {
                path: "grupolineas/:sucursalid",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: GrupoLineasComponent,
                data: {
                    permissions: {
                        only: 'Planificacion.GrupoLinea.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "tipodia",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TipoDiaComponent,
                data: {
                    permissions: {
                        only: 'Admin.TipoDeDias.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "bandera/:sucursalid",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: BanderaComponent,
                data: {
                    permissions: {
                        only: 'Planificacion.Bandera.Administracion',
                        redirectTo: '/401'
                    }
                }
            },

            {
                path: "sentidobandera",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: SentidoBanderaComponent,
                data: {
                    permissions: {
                        only: 'Admin.SentidoBandera.Administracion',
                        redirectTo: '/401'
                    }
                }

            },

            {
                path: "tipoparada",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TipoParadaComponent,
                data: {
                    permissions: {
                        only: 'Admin.TipoParada.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {
                path: "talleres/:sucursalid",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: TallerComponent,
                data: {
                    permissions: {
                        only: 'Planificacion.RamaColor.Administracion',
                        redirectTo: '/401'
                    }
                }

            },
            {

                path: "horariofecha",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: HFechasConfiComponent,
                data: {
                    permissions: {
                        only: 'Horarios.FechaHorario.EntrarHorario',
                        redirectTo: '/401'
                    }
                }
            },
            {

                path: "sabana",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: HorariosPorSectorComponent,
                data: {
                    permissions: {
                        only: 'Horarios.Sabana.Administracion',
                        redirectTo: '/401'
                    }
                }
            },
            {

                path: "detalleSalidasYRelevos",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: DetalleSalidasYRelevosComponent,
                data: {
                    permissions: {
                        only: 'Horarios.Sabana.Administracion',
                        redirectTo: '/401'
                    }
                }
            },
            {

                path: "reportePasajeros",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: ReportePasajerosComponent,
                data: {
                    permissions: {
                        only: 'Horarios.Sabana.ParadasPasajeros',
                        redirectTo: '/401'
                    }
                }
            },
            {

                path: "distribucionCoches",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: DistribucionCochesComponent,
                data: {
                    permissions: {
                        only: 'Horarios.Sabana.Administracion',
                        redirectTo: '/401'
                    }
                }
            },

            {

                path: "horarioPasajeros",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: HorarioPasajerosComponent,
                data: {
                    permissions: {
                        only: 'Horarios.Sabana.Administracion',
                        redirectTo: '/401'
                    }
                }
            },
            {

                path: "horariolinea/:lineaid",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: HFechasConfiLineaComponent,
                canDeactivate: [CanDeactivateGuard],
                data: {
                    permissions: {
                        only: 'Horarios.FechaHorario.EntrarHorario',
                        redirectTo: '/401'
                    }
                }
            },
            {

                path: "horariolinea/:lineaid/:id",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: HFechasConfiLineaComponent,
                data: {
                    permissions: {
                        only: 'Horarios.FechaHorario.EntrarHorario',
                        redirectTo: '/401'
                    }
                }
            },
            {

                path: "coordenadas",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: CoordenadasComponent,
                data: {
                    permissions: {
                        only: 'Planificacion.RamaColor.Administracion',
                        redirectTo: '/401'
                    }
                }
            },
            {

                path: "paradas",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: ParadaComponent,
                data: {
                    permissions: {
                        only: 'Admin.Paradas.Administracion',
                        redirectTo: '/401'
                    }
                }
            },
            {

                path: "subgalpon",
                canActivate: [AuthGuard, NgxPermissionsGuard],
                component: SubGalponComponent,
                data: {
                    permissions: {
                        only: 'Planificacion.RamaColor.Administracion',
                        redirectTo: '/401'
                    }
                }
            },
            {
                path: "sectorView",
                canActivate: [AuthGuard],
                component: SectorViewComponent
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
        SelectButtonModule,
        InputSwitchModule,
        RadioButtonModule
    ], exports: [
        RouterModule
    ], declarations: [
        RecorridosComponent,
        RamalColorComponent,
        GrupoLineasComponent,
        GrupoLineasComboComponent,
        CreateOrEditGrupoLineasModalComponent,
        TipoLineaComboComponent,
        ResponsableInformesComboComponent,
        HServicioComboComponent,
        TipoDiaComboComponent,
        SucursalComponent,
        CreateOrEditSucursalModalComponent,
        LineaComponent,
        CreateOrEditLineaModalComponent,
        TipoLineaComponent,
        TipoDiaComponent,
        TipoParadaComponent,
        CreateOrEditTipoLineaModalComponent,
        EmpresaComponent,
        CreateOrEditEmpresaModalComponent,
        CreateOrEditTipoDiaModalComponent,
        CreateOrEditRamaColorModalComponent,
        BanderaComponent,
        CreateOrEditBanderaModalComponent,
        CreateOrEditRutaModalComponent,
        CreateOrEditTipoParadaModalComponent,
        EstadoRutaComboComponent,
        CreateOrEditSucursalModalComponent,
        CreateOrEditLineaModalComponent,
        CreateOrEditTipoLineaModalComponent,
        CreateOrEditEmpresaModalComponent,
        CreateOrEditGrupoLineasModalComponent,
        CreateOrEditTipoDiaModalComponent,
        CreateOrEditRamaColorModalComponent,
        SelectSectoresHorariosModalComponent,
        CreateOrEditRutaModalComponent,
        CreateOrEditTipoParadaModalComponent,
        SectorViewComponent,
        TallerComponent,
        CreateOrEditTallerModalComponent,
        TallerMapsComponent,
        SelectMarkerMapsComponent,
        TipoBanderaComboComponent,
        LineasComponent,
        RamalColorComboComponent,
        BanderaAutoCompleteComponent,
        BanderaTupComboComponent,
        GroupByPipe,
        ViewRutasComponent,
        CodigoSectorComboComponent,
        SentidoBanderaComboComponent,
        SentidoBanderaComponent,
        CreateOrEditSentidoBanderaModalComponent,
        RamalColorAutoCompleteComponent,
        CreateOrEditHFechasConfiModalComponent,
        HFechasConfiComponent,
        HorariosPorSectorComponent,
        DetalleSalidasYRelevosComponent,
        DistribucionCochesComponent,
        HorarioPasajerosComponent,
        EstadoHorarioFechaComboComponent,
        CreateOrEditCoordenadaModalComponent,
        CoordenadasComponent,
        ParadaComponent,
        HFechasConfiLineaComponent,
        DistribucionDeCochesAgregation,
        DistribucionDeCochesDetailComponent,
        CreateOrEditHFechasConfiServiciosComponent,
        SubgalponComboComponent,
        BanderaComboComponent,
        TabsDetalleHorariofechaComponent,
        HTposHorasComboComponent,
        HFechasConfiComboComponent,
        MinutosPorSectorComponent,
        HChoxserComponent,
        ImportarHorarioFechaComponent,
        AsignacionBanderaHbasec,
        AsignacionLineaLineaHoraria,
        MvueltasViewComponent,
        HfechasConfiNewComponent,
        ServicioNewComponent,
        ExportarExcelComponent,
        CreateOrEditHorariosPorSectorComponent,
        CreateOrEditDetalleSalidasYRelevos,
        PlaLineaAutoCompleteComponent,
        ImportarMinutosPorSectorComponent,
        ExportarMinutosPorSectorComponent,
        ImportarDuracionComponent,
        CopiarMinutosPorSectorComponent,
        SubGalponComponent,
        CreateOrEditSubGalponModalComponent,
        GrupoComboComponent,
        SentidoBanderaSubeComboComponent,
        PlanCamComboComponent,
        AddConfiguModalComponent,
        OrigenPredictivoComponent,
        DestinoPredictivoComponent,
        CreateOrEditParadaModalComponent,
        ExportarKmlModalComponent,
        DescripcionTipoDiaPredictivoComponent,
        ReportePasajerosComponent,
        RamalColorComboComunComponent,
        ExportarsabanaComponent,
        ChangeRouteComponent,
        StopTypeComboComponent
    ],
    entryComponents: [
        MvueltasViewComponent,
        CreateOrEditSucursalModalComponent,
        CreateOrEditLineaModalComponent,
        CreateOrEditTipoLineaModalComponent,
        CreateOrEditEmpresaModalComponent,
        CreateOrEditGrupoLineasModalComponent,
        CreateOrEditTipoDiaModalComponent,
        CreateOrEditRamaColorModalComponent,
        CreateOrEditBanderaModalComponent,
        SelectSectoresHorariosModalComponent,
        CreateOrEditRutaModalComponent,
        CreateOrEditTipoParadaModalComponent,
        CreateOrEditTallerModalComponent,
        SectorViewComponent,
        TipoBanderaComboComponent,
        RamalColorComboComponent,
        BanderaAutoCompleteComponent,
        BanderaTupComboComponent,
        ViewRutasComponent,
        CodigoSectorComboComponent,
        SentidoBanderaComboComponent,
        CreateOrEditSentidoBanderaModalComponent,
        RamalColorAutoCompleteComponent,
        CreateOrEditHFechasConfiModalComponent,
        CreateOrEditCoordenadaModalComponent,
        CreateOrEditParadaModalComponent,
        DistribucionDeCochesDetailComponent,
        CreateOrEditHFechasConfiServiciosComponent,
        TabsDetalleHorariofechaComponent,
        MinutosPorSectorComponent,
        HChoxserComponent,
        ImportarHorarioFechaComponent,
        HfechasConfiNewComponent,
        ServicioNewComponent,
        ExportarExcelComponent,
        AsignacionLineaLineaHoraria,
        CreateOrEditHorariosPorSectorComponent,
        CreateOrEditDetalleSalidasYRelevos,
        PlaLineaAutoCompleteComponent,
        ImportarMinutosPorSectorComponent,
        ExportarMinutosPorSectorComponent,
        ImportarDuracionComponent,
        CopiarMinutosPorSectorComponent,
        SubGalponComponent,
        CreateOrEditSubGalponModalComponent,
        GrupoComboComponent,
        SentidoBanderaSubeComboComponent,
        PlanCamComboComponent,
        AddConfiguModalComponent,
        OrigenPredictivoComponent,
        DestinoPredictivoComponent,
        ResponsableInformesComboComponent,
        ExportarKmlModalComponent,
        DescripcionTipoDiaPredictivoComponent,
        ReportePasajerosComponent,
        RamalColorComboComunComponent,
        ExportarsabanaComponent,
        ChangeRouteComponent,
        StopTypeComboComponent

    ],
    providers: [
        RamalColorService,
        GrupoLineasService,
        TipoLineaService,
        EmpresaService,
        TipoDiaService,
        BanderaService,
        BanderaTupService,
        RutaService,
        EstadoRutaService,
        TiempoEsperadoDeCargaService,
        TipoParadaService,
        PuntoService,
        TallerService,
        TipoBanderaService,
        CoordenadasService,
        ParadaService,
        SentidoBanderaService,
        HFechasConfiService,
        EstadoHorarioFechaService,
        PlaDistribucionDeCochesPorTipoDeDiaService,
        SubGalponService,
        HServiciosService,
        MediasVueltasService,
        HHorariosConfiService,
        HTposHorasService,
        HMinxtipoService,
        BanderaCartelService,
        SubGalponSinCacheService,
        GruposService,
        PlanCamService,
        HChoxserService,
        PlaSentidoBanderaSubeService,
        HBanderasColoresService,
        ResponsableInformesService
    ],
})
export class PlanificacionModule {



}