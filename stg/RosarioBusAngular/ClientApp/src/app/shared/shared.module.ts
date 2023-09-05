import { PersTurnosService } from './../theme/pages/inspectores/turnos/persturnos.service';
import { PersTurnosComboComponent } from './../theme/pages/inspectores/shared/persturnos-combo.component';
import { CommonModule } from '@angular/common'
import { NgModule, LOCALE_ID } from '@angular/core';
import { UtilsModule } from './utils/utils.module';
import { ModalModule, TabsModule, BsDatepickerModule } from 'ngx-bootstrap';
import { LayoutModule } from '../theme/layouts/layout.module';
import { DataTableModule } from 'primeng/components/datatable/datatable';
import { PaginatorModule } from 'primeng/components/paginator/paginator';
import { CalendarModule } from 'primeng/calendar'
import { AutoCompleteModule } from 'primeng/autocomplete';
import { ColorPickerModule } from 'primeng/colorpicker';
import { TriStateCheckboxModule } from 'primeng/tristatecheckbox';
import { ToggleButtonModule } from 'primeng/togglebutton';
import { TableModule } from 'primeng/table';
import { AccordionModule } from 'primeng/accordion';
import { ListboxModule } from 'primeng/listbox';
import { FileUploadModule } from 'primeng/fileupload';
import { MultiSelectModule } from 'primeng/multiselect';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';
import { TextMaskModule } from 'angular2-text-mask';
import { NgbModule, NgbDatepickerI18n, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';

import {
    MatDatepickerModule,
    MatProgressSpinnerModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatInputModule,
    MatNativeDateModule,
    MatButtonToggleModule,
    MatCheckboxModule,
    MatPaginatorIntl,
    DateAdapter,
    MAT_DATE_FORMATS,
    MatAutocompleteModule,
    MatSelectModule,
    MatFormFieldModule,
    MatSlideToggleModule,
    MatListModule,
    MatSidenavModule,
    MAT_DATE_LOCALE,
    MatExpansionModule, MatProgressBarModule
} from '@angular/material';
import { I18n, CustomDatepickerI18n, NgbDateParserFormatterEs } from './helpers/CustomDatepickerI18n';
import { MY_DATE_FORMATS, CustomDateAdapter } from './constants/constants';
import { DataService } from './common/services/data.service';
import { NgxPermissionsModule } from 'ngx-permissions';
import { NgxPermissionsStore } from 'ngx-permissions';
import { NgxRolesStore } from 'ngx-permissions';
import { NgxPermissionsService } from 'ngx-permissions';
import { CrudComponent } from './manager/crud.component';
import { FilterPipe, SortByPipe, GroupByPipe } from './utils/pipe/pipe';
import { RbmapsComponent } from '../components/rbmaps/rbmaps.component';
import { CreateOrEditPuntoModalComponent } from '../components/punto/create-or-edit-punto-modal.component';
import { RbMapLineaPorGrupoFilter } from '../components/rbmaps/RbMapLineaPorGrupoFilter';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PuntoService } from '../theme/pages/planificacion/punto/punto.service';
import { BreadcrumbsNavComponent } from '../theme/layouts/breadcrumbs/breadcrumbs-nav.component';
import { BreadcrumbsService } from '../theme/layouts/breadcrumbs/breadcrumbs.service';
import { TipoParadaService } from '../theme/pages/planificacion/tipoParada/tipoparada.service';
import { TipoParadaComboComponent } from '../theme/pages/planificacion/shared/tipoParada-combo.component';
import { SectorService } from '../theme/pages/planificacion/sector/sector.service';
import { CoordenadasService } from '../theme/pages/planificacion/coordenadas/coordenadas.service';
import { SectoresTarifariosService } from '../theme/pages/planificacion/sectoresTarifarios/sectoresTarifarios.service';
import { RutasMapComponent } from '../components/rbmaps/rutas.map.component';
import { SortablejsModule } from 'angular-sortablejs/dist';
import { CustomFormsModule } from 'ng2-validation';
import { SucursalService } from '../theme/pages/planificacion/sucursal/sucursal.service';
import { SucursalComboComponent } from '../theme/pages/planificacion/shared/sucursal-combo.component';
import { LineaService } from '../theme/pages/planificacion/linea/linea.service';
import { LineaAutoCompleteComponent } from '../theme/pages/planificacion/shared/linea-autocomplete.component';
import { RbMapServices } from '../components/rbmaps/RbMapServices';
import { EmpresaComboComponent } from '../theme/pages/planificacion/shared/empresa-combo.component';
import { PlaLineaService } from '../theme/pages/planificacion/linea/pla-linea.service';
import { CoordenadasAutoCompleteComponent } from '../theme/pages/planificacion/shared/coordenada-autocomplete.component';
import { CroquiComponent } from './croqui/croqui.component';
import { CroquiService } from './croqui/croqui.service';
import { SectoresTarifariosAutoCompleteComponent } from '../theme/pages/planificacion/shared/sectoresTarifarios-autocomplete.component';
import { AdjuntoComponent } from '../theme/pages/siniestros/siniestro/adjunto/adjunto.component';
import { EmpleadosAutoCompleteComponent } from '../theme/pages/siniestros/shared/empleado-autocomplete.component';
import { EmpleadosService } from '../theme/pages/siniestros/empleados/empleados.service';
import { EmpresaService } from '../theme/pages/planificacion/empresa/empresa.service';
import { YesNoAllComboComponent } from './components/yesnoall-combo.component';
import { DataTypesComboComponent } from '../theme/pages/admin/parameters/dataType-combo.component';

import { CreateOrEditProvinciasModalComponent } from '../theme/pages/siniestros/provincias/create-or-edit-provincias-modal.component';
import { ProvinciasService } from '../theme/pages/siniestros/provincias/provincias.service';
import { LocalidadesService } from '../theme/pages/siniestros/localidades/localidad.service';
import { ProvinciasComboComponent } from '../theme/pages/siniestros/shared/provincias-combo.component';
import { CreateOrEditLocalidadesModalComponent } from '../theme/pages/admin/localidades/create-or-edit-localidades-modal.component';
import { LocalidadesAutoCompleteComponent } from '../theme/pages/siniestros/shared/localidad-autocomplete.component';
import { CreateOrEditTipoDniModalComponent } from '../theme/pages/admin/tipodni/create-or-edit-tipodni-modal.component';
import { EstadosComboComponent } from '../theme/pages/siniestros/shared/estados-combo.component';
import { SubEstadoComboComponent } from '../theme/pages/siniestros/shared/subestado-combo.component';
import { TipoDocIdComboComponent } from '../theme/pages/siniestros/shared/tipoDocId-combo.component';
import { TipoInvolucradoComboComponent } from '../theme/pages/siniestros/shared/tipoinvolucrado-combo .component';
import { InvolucradosComboComponent } from '../theme/pages/siniestros/shared/involucrado-combo.component';
import { AbogadosComboComponent } from '../theme/pages/siniestros/shared/abogados-combo.component';
import { JuzgadosComboComponent } from '../theme/pages/siniestros/shared/juzgado-combo.component';
import { ReclamosHistoricosService } from '../theme/pages/siniestros/reclamos/reclamoshistoricos.service';
import { ReclamosService } from '../theme/pages/siniestros/reclamos/reclamos.service';
import { EstadosService } from '../theme/pages/siniestros/estados/estados.service';
import { TipoDniService } from '../theme/pages/siniestros/tipodni/tipodni.service';
import { TipoInvolucradoService } from '../theme/pages/siniestros/tipoinvolucrado/tipoinvolucrado.service';
import { InvolucradosService } from '../theme/pages/siniestros/involucrados/involucrados.service';
import { SubEstadosService } from '../theme/pages/siniestros/estados/subestados.service';
import { AbogadosService } from '../theme/pages/siniestros/abogados/abogados.service';
import { SiniestroService } from '../theme/pages/siniestros/siniestro/siniestro.service';
import { SiniestrosComboComponent } from '../theme/pages/art/shared/siniestro-combo.component';
import { GalponService } from '../theme/pages/planificacion/galpon/galpon.service';
import { GalponComboComponent } from '../theme/pages/planificacion/shared/Galpon-combo.component';
import { ReclamosGeneralComponent } from '../theme/pages/reclamos/reclamos/create-or-edit-reclamos.component';
import { ReclamosComponent } from '../theme/pages/reclamos/reclamos/reclamos.component';
import { TipoReclamoComboComponent } from '../theme/pages/reclamos/shared/tiporeclamo-combo.component';
import { DenunciaAutocompleteComponent } from '../theme/pages/reclamos/shared/denuncia-autocomplete.component';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { CausaReclamoComboComponent } from '../theme/pages/reclamos/shared/causareclamo-combo.component';
import { TiposAcuerdoComboComponent } from '../theme/pages/reclamos/shared/tiposacuerdo-combo.component';
import { TiposAcuerdoService } from '../theme/pages/reclamos/tiposacuerdo/tiposacuerdo.service';
import { RubroSalarialComboComponent } from '../theme/pages/reclamos/shared/rubrosalarial-combo.component';
import { CreateOrEditEstadosModalComponent } from '../theme/pages/siniestros/estados/create-or-edit-estados-modal.component';
import { CreateAbogadosModalComponent } from '../theme/pages/siniestros/siniestro/reclamos/create-abogados-modal.component';
import { CreateOrEditTiposAcuerdoModalComponent } from '../theme/pages/reclamos/tiposacuerdo/create-or-edit-tiposacuerdo-modal.component';
import { CreateOrEditRubrosSalarialesModalComponent } from '../theme/pages/reclamos/rubrossalariales/create-or-edit-rubrossalariales-modal.component';
import { CreateOrEditCausasReclamoModalComponent } from '../theme/pages/reclamos/causasreclamo/create-or-edit-causasreclamo-modal.component';
import { CreateOrEditTiposReclamoModalComponent } from '../theme/pages/reclamos/tiposreclamo/create-or-edit-tiposreclamo-modal.component';
import { CausasReclamoService } from '../theme/pages/reclamos/causasreclamo/causasreclamo.service';
import { TiposReclamoService } from '../theme/pages/reclamos/tiposreclamo/tiposreclamo.service';
import { RubrosSalarialesService } from '../theme/pages/reclamos/rubrossalariales/rubrossalariales.service';
import { DenunciasService } from '../theme/pages/art/denuncias/denuncias.service';
import { AnularReclamoModalComponent } from '../theme/pages/reclamos/anular/anular-modal.component';
import { EstadosReclamosModalComponent } from '../theme/pages/reclamos/cambioestado/estados-modal.component';
import { CreateOrEditJuzgadosModalComponent } from '../theme/pages/siniestros/juzgados/create-or-edit-juzgados-modal.component';
import { ReclamosImportadorComponent } from '../theme/pages/reclamos/reclamos/reclamos-importador/reclamos-importador.component';
import { ParadaAutoCompleteComponent } from '../theme/pages/planificacion/shared/parada-autocomplete.component';
import { ParadaService } from '../theme/pages/planificacion/parada/parada.service';
import { SiniestroEmpleadoComboComponent } from '../theme/pages/art/shared/siniestro-combobyempleado.component';
import { DenunciasComboComponent } from '../theme/pages/art/shared/denuncias-combo.component';
import { TipoViajeComboComponent } from '../theme/pages/planificacion/shared/tipoviaje-combo.component';
import { TipoViajeService } from '../components/rbmaps/tipoviaje.service';
import { DatePickerWithKeyboard } from './components/datepickerWithKeyboard.component';
import { SignalRService } from '../services/signalr.service';
import { MonthComboComponent } from './components/month-combo.component';
import { GruposInspectoresComboComponent } from '../theme/pages/inspectores/shared/gruposinspectores-combo.component';
import { GruposInspectoresService } from '../theme/pages/inspectores/gruposinspectores/gruposinspectores.service';
import { CreateOrEditUserModalComponent } from '../theme/pages/admin/users/create-or-edit-user-modal.component';
import { EditUserPermissionsModalComponent } from '../theme/pages/admin/users/edit-user-permissions-modal.component';
import { PermissionTreeComponent } from '../theme/pages/admin/shared/permission-tree.component';
import { EditUserLineasModalComponent } from '../theme/pages/admin/users/edit-user-lineas-modal.component';
import { UsersComponent } from '../theme/pages/admin/users/users.component';
import { UserService } from '../auth/services';
import { RoleComboComponent } from '../theme/pages/admin/shared/role-combo.component';
import { RolesService } from '../theme/pages/admin/roles/roles.service';


@NgModule({
    entryComponents:
    [
        CreateOrEditLocalidadesModalComponent,
        CreateOrEditProvinciasModalComponent,
        CreateOrEditTipoDniModalComponent,
        ReclamosGeneralComponent,
        CreateOrEditEstadosModalComponent,
        CreateAbogadosModalComponent,
        CreateOrEditTiposAcuerdoModalComponent,
        CreateOrEditRubrosSalarialesModalComponent,
        CreateOrEditCausasReclamoModalComponent,
        CreateOrEditTiposReclamoModalComponent,
        CreateAbogadosModalComponent,
        AnularReclamoModalComponent,
        EstadosReclamosModalComponent,
        AdjuntoComponent,
        CreateOrEditJuzgadosModalComponent,
        ReclamosImportadorComponent,
        CreateOrEditUserModalComponent,
        EditUserPermissionsModalComponent,
        PermissionTreeComponent,
        EditUserLineasModalComponent,
    ],
    declarations: [
        UsersComponent,
        CreateOrEditUserModalComponent,
        EditUserPermissionsModalComponent,
        PermissionTreeComponent,
        EditUserLineasModalComponent,
        SortByPipe,
        FilterPipe,
        RbmapsComponent,
        RutasMapComponent,
        RbMapLineaPorGrupoFilter,
        CreateOrEditPuntoModalComponent,
        TipoParadaComboComponent,
        SucursalComboComponent,
        DataTypesComboComponent,
        EmpresaComboComponent,
        LineaAutoCompleteComponent,
        CoordenadasAutoCompleteComponent,
        ParadaAutoCompleteComponent,
        CroquiComponent,
        SectoresTarifariosAutoCompleteComponent,
        LocalidadesAutoCompleteComponent,
        CreateOrEditLocalidadesModalComponent,
        ProvinciasComboComponent,
        CreateOrEditProvinciasModalComponent,
        CreateOrEditTipoDniModalComponent,
        AdjuntoComponent,
        EmpleadosAutoCompleteComponent,
        YesNoAllComboComponent,
        MonthComboComponent,
        EstadosComboComponent,
        SubEstadoComboComponent,
        TipoDocIdComboComponent,
        TipoInvolucradoComboComponent,
        InvolucradosComboComponent,
        AbogadosComboComponent,
        JuzgadosComboComponent,
        SiniestrosComboComponent,
        GalponComboComponent,
        ReclamosGeneralComponent,
        ReclamosComponent,
        TipoReclamoComboComponent,
        DenunciaAutocompleteComponent,
        CausaReclamoComboComponent,
        TiposAcuerdoComboComponent,
        RubroSalarialComboComponent,
        CreateOrEditEstadosModalComponent,
        CreateAbogadosModalComponent,
        CreateOrEditTiposAcuerdoModalComponent,
        CreateOrEditRubrosSalarialesModalComponent,
        CreateOrEditCausasReclamoModalComponent,
        CreateOrEditTiposReclamoModalComponent,
        CreateAbogadosModalComponent,
        AnularReclamoModalComponent,
        EstadosReclamosModalComponent,
        CreateOrEditJuzgadosModalComponent,
        ReclamosImportadorComponent,
        SiniestroEmpleadoComboComponent,
        DenunciasComboComponent,
        GruposInspectoresComboComponent,
        PersTurnosComboComponent,
        RoleComboComponent,
        TipoViajeComboComponent,
        DatePickerWithKeyboard
    ],
    imports: [
        //CommonModule,
        FormsModule,
        CustomFormsModule,
        CommonModule,
        SortablejsModule.forRoot({ animation: 150 }),
        UtilsModule,
        ModalModule.forRoot(),
        TabsModule.forRoot(),
        BsDatepickerModule.forRoot(),
        TriStateCheckboxModule,
        ToggleButtonModule,
        NgxPermissionsModule,
        MatDatepickerModule,
        MatProgressSpinnerModule,
        MatTableModule,
        MatPaginatorModule,
        MatInputModule,
        MatSortModule,
        MatNativeDateModule,
        MatButtonToggleModule,
        MatCheckboxModule,
        MatAutocompleteModule,
        MatSelectModule,
        MatSlideToggleModule,
        MatListModule,
        MatSidenavModule,
        LayoutModule,
        UtilsModule,
        DataTableModule,
        PaginatorModule,
        AutoCompleteModule,
        AccordionModule,
        CalendarModule,
        NgbModule.forRoot(),
        ColorPickerModule,
        TableModule,
        ListboxModule,
        FileUploadModule,
        ReactiveFormsModule,
        MultiSelectModule,
        MatExpansionModule,
        MatProgressBarModule,
        NgxMatSelectSearchModule,
        MatFormFieldModule,
        CurrencyMaskModule,
        TextMaskModule
    ],
    exports: [
        UtilsModule,
        CustomFormsModule,
        ModalModule,
        TabsModule,
        BsDatepickerModule,
        MatDatepickerModule,
        MatProgressSpinnerModule,
        MatTableModule,
        MatPaginatorModule,
        MatInputModule,
        MatSortModule,
        MatNativeDateModule,
        MatButtonToggleModule,
        MatCheckboxModule,
        MatAutocompleteModule,
        MatSelectModule,
        MatSlideToggleModule,
        MatSidenavModule,
        MatListModule,
        LayoutModule,
        UtilsModule,
        DataTableModule,
        PaginatorModule,
        AutoCompleteModule,
        AccordionModule,
        CalendarModule,
        NgbModule,
        NgxPermissionsModule,
        RbmapsComponent,
        RutasMapComponent,
        CurrencyMaskModule,
        RbMapLineaPorGrupoFilter,
        CreateOrEditPuntoModalComponent,
        CreateOrEditLocalidadesModalComponent,
        TipoParadaComboComponent,
        ColorPickerModule,
        TableModule,
        TriStateCheckboxModule,
        ToggleButtonModule,
        SucursalComboComponent,
        DataTypesComboComponent,
        LineaAutoCompleteComponent,
        ListboxModule,
        FileUploadModule,
        EmpresaComboComponent,
        ReactiveFormsModule,
        CoordenadasAutoCompleteComponent,
        ParadaAutoCompleteComponent,
        CroquiComponent,
        MultiSelectModule,
        MatExpansionModule,
        MatProgressBarModule,
        SectoresTarifariosAutoCompleteComponent,
        EmpleadosAutoCompleteComponent,
        AdjuntoComponent,
        YesNoAllComboComponent,
        MonthComboComponent,
        LocalidadesAutoCompleteComponent,
        NgxMatSelectSearchModule,
        MatFormFieldModule,
        CreateOrEditTipoDniModalComponent,
        EstadosComboComponent,
        SubEstadoComboComponent,
        TipoDocIdComboComponent,
        TipoInvolucradoComboComponent,
        InvolucradosComboComponent,
        AbogadosComboComponent,
        JuzgadosComboComponent,
        SiniestrosComboComponent,
        GalponComboComponent,
        ReclamosGeneralComponent,
        ReclamosComponent,
        TipoReclamoComboComponent,
        DenunciaAutocompleteComponent,
        CausaReclamoComboComponent,
        TiposAcuerdoComboComponent,
        RubroSalarialComboComponent,
        CreateOrEditEstadosModalComponent,
        CreateAbogadosModalComponent,
        CreateOrEditTiposAcuerdoModalComponent,
        CreateOrEditRubrosSalarialesModalComponent,
        CreateOrEditCausasReclamoModalComponent,
        CreateOrEditTiposReclamoModalComponent,
        CreateAbogadosModalComponent,
        AnularReclamoModalComponent,
        EstadosReclamosModalComponent,
        CreateOrEditJuzgadosModalComponent,
        SiniestroEmpleadoComboComponent,
        DenunciasComboComponent,
        DatePickerWithKeyboard,
        TipoViajeComboComponent,
        GruposInspectoresComboComponent,
        PersTurnosComboComponent,
        RoleComboComponent,
        UsersComponent,
        CreateOrEditUserModalComponent,
        EditUserPermissionsModalComponent,
        PermissionTreeComponent,
        EditUserLineasModalComponent
    ],
    providers: [

        I18n, { provide: NgbDatepickerI18n, useClass: CustomDatepickerI18n },
        {
            provide: NgbDateParserFormatter,
            useFactory: () => { return new NgbDateParserFormatterEs() }
        },
        // { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
        // { provide: MAT_DATE_LOCALE, useValue: 'es-ar' },
        { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
        { provide: DateAdapter, useClass: CustomDateAdapter },
        { provide: MAT_DATE_LOCALE, useValue: 'es-ar' },
        // { provide: LOCALE_ID, useValue: 'es-ar' },
        PuntoService,
        TipoParadaService,
        SectorService,
        CoordenadasService,
        ParadaService,
        SectoresTarifariosService,
        SucursalService,
        LineaService,
        PlaLineaService,
        RbMapServices,
        CroquiService,
        EmpleadosService,
        EmpresaService,
        ProvinciasService,
        LocalidadesService,
        ReclamosHistoricosService,
        ReclamosService,
        ReclamosHistoricosService,
        EstadosService,
        TipoDniService,
        TipoInvolucradoService,
        InvolucradosService,
        SubEstadosService,
        AbogadosService,
        SiniestroService,
        GalponService,
        TiposAcuerdoService,
        TiposReclamoService,
        CausasReclamoService,
        RubrosSalarialesService,
        DenunciasService,
        TipoViajeService,
        SignalRService,

        TipoViajeService,
        GruposInspectoresService,
        PersTurnosService,
        UserService,
        RolesService
    ],
    // entryComponents: [CreateOrEditUserModalComponent],

})
export class SharedModule { }