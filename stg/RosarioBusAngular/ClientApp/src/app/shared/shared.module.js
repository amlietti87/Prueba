"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var persturnos_service_1 = require("./../theme/pages/inspectores/turnos/persturnos.service");
var persturnos_combo_component_1 = require("./../theme/pages/inspectores/shared/persturnos-combo.component");
var common_1 = require("@angular/common");
var core_1 = require("@angular/core");
var utils_module_1 = require("./utils/utils.module");
var ngx_bootstrap_1 = require("ngx-bootstrap");
var layout_module_1 = require("../theme/layouts/layout.module");
var datatable_1 = require("primeng/components/datatable/datatable");
var paginator_1 = require("primeng/components/paginator/paginator");
var calendar_1 = require("primeng/calendar");
var autocomplete_1 = require("primeng/autocomplete");
var colorpicker_1 = require("primeng/colorpicker");
var tristatecheckbox_1 = require("primeng/tristatecheckbox");
var togglebutton_1 = require("primeng/togglebutton");
var table_1 = require("primeng/table");
var accordion_1 = require("primeng/accordion");
var listbox_1 = require("primeng/listbox");
var fileupload_1 = require("primeng/fileupload");
var multiselect_1 = require("primeng/multiselect");
var ngx_mat_select_search_1 = require("ngx-mat-select-search");
var angular2_text_mask_1 = require("angular2-text-mask");
var ng_bootstrap_1 = require("@ng-bootstrap/ng-bootstrap");
var material_1 = require("@angular/material");
var CustomDatepickerI18n_1 = require("./helpers/CustomDatepickerI18n");
var constants_1 = require("./constants/constants");
var ngx_permissions_1 = require("ngx-permissions");
var pipe_1 = require("./utils/pipe/pipe");
var rbmaps_component_1 = require("../components/rbmaps/rbmaps.component");
var create_or_edit_punto_modal_component_1 = require("../components/punto/create-or-edit-punto-modal.component");
var RbMapLineaPorGrupoFilter_1 = require("../components/rbmaps/RbMapLineaPorGrupoFilter");
var forms_1 = require("@angular/forms");
var punto_service_1 = require("../theme/pages/planificacion/punto/punto.service");
var tipoparada_service_1 = require("../theme/pages/planificacion/tipoParada/tipoparada.service");
var tipoParada_combo_component_1 = require("../theme/pages/planificacion/shared/tipoParada-combo.component");
var sector_service_1 = require("../theme/pages/planificacion/sector/sector.service");
var coordenadas_service_1 = require("../theme/pages/planificacion/coordenadas/coordenadas.service");
var sectoresTarifarios_service_1 = require("../theme/pages/planificacion/sectoresTarifarios/sectoresTarifarios.service");
var rutas_map_component_1 = require("../components/rbmaps/rutas.map.component");
var dist_1 = require("angular-sortablejs/dist");
var ng2_validation_1 = require("ng2-validation");
var sucursal_service_1 = require("../theme/pages/planificacion/sucursal/sucursal.service");
var sucursal_combo_component_1 = require("../theme/pages/planificacion/shared/sucursal-combo.component");
var linea_service_1 = require("../theme/pages/planificacion/linea/linea.service");
var linea_autocomplete_component_1 = require("../theme/pages/planificacion/shared/linea-autocomplete.component");
var RbMapServices_1 = require("../components/rbmaps/RbMapServices");
var empresa_combo_component_1 = require("../theme/pages/planificacion/shared/empresa-combo.component");
var pla_linea_service_1 = require("../theme/pages/planificacion/linea/pla-linea.service");
var coordenada_autocomplete_component_1 = require("../theme/pages/planificacion/shared/coordenada-autocomplete.component");
var croqui_component_1 = require("./croqui/croqui.component");
var croqui_service_1 = require("./croqui/croqui.service");
var sectoresTarifarios_autocomplete_component_1 = require("../theme/pages/planificacion/shared/sectoresTarifarios-autocomplete.component");
var adjunto_component_1 = require("../theme/pages/siniestros/siniestro/adjunto/adjunto.component");
var empleado_autocomplete_component_1 = require("../theme/pages/siniestros/shared/empleado-autocomplete.component");
var empleados_service_1 = require("../theme/pages/siniestros/empleados/empleados.service");
var empresa_service_1 = require("../theme/pages/planificacion/empresa/empresa.service");
var yesnoall_combo_component_1 = require("./components/yesnoall-combo.component");
var dataType_combo_component_1 = require("../theme/pages/admin/parameters/dataType-combo.component");
var create_or_edit_provincias_modal_component_1 = require("../theme/pages/siniestros/provincias/create-or-edit-provincias-modal.component");
var provincias_service_1 = require("../theme/pages/siniestros/provincias/provincias.service");
var localidad_service_1 = require("../theme/pages/siniestros/localidades/localidad.service");
var provincias_combo_component_1 = require("../theme/pages/siniestros/shared/provincias-combo.component");
var create_or_edit_localidades_modal_component_1 = require("../theme/pages/admin/localidades/create-or-edit-localidades-modal.component");
var localidad_autocomplete_component_1 = require("../theme/pages/siniestros/shared/localidad-autocomplete.component");
var create_or_edit_tipodni_modal_component_1 = require("../theme/pages/admin/tipodni/create-or-edit-tipodni-modal.component");
var estados_combo_component_1 = require("../theme/pages/siniestros/shared/estados-combo.component");
var subestado_combo_component_1 = require("../theme/pages/siniestros/shared/subestado-combo.component");
var tipoDocId_combo_component_1 = require("../theme/pages/siniestros/shared/tipoDocId-combo.component");
var tipoinvolucrado_combo__component_1 = require("../theme/pages/siniestros/shared/tipoinvolucrado-combo .component");
var involucrado_combo_component_1 = require("../theme/pages/siniestros/shared/involucrado-combo.component");
var abogados_combo_component_1 = require("../theme/pages/siniestros/shared/abogados-combo.component");
var juzgado_combo_component_1 = require("../theme/pages/siniestros/shared/juzgado-combo.component");
var reclamoshistoricos_service_1 = require("../theme/pages/siniestros/reclamos/reclamoshistoricos.service");
var reclamos_service_1 = require("../theme/pages/siniestros/reclamos/reclamos.service");
var estados_service_1 = require("../theme/pages/siniestros/estados/estados.service");
var tipodni_service_1 = require("../theme/pages/siniestros/tipodni/tipodni.service");
var tipoinvolucrado_service_1 = require("../theme/pages/siniestros/tipoinvolucrado/tipoinvolucrado.service");
var involucrados_service_1 = require("../theme/pages/siniestros/involucrados/involucrados.service");
var subestados_service_1 = require("../theme/pages/siniestros/estados/subestados.service");
var abogados_service_1 = require("../theme/pages/siniestros/abogados/abogados.service");
var siniestro_service_1 = require("../theme/pages/siniestros/siniestro/siniestro.service");
var siniestro_combo_component_1 = require("../theme/pages/art/shared/siniestro-combo.component");
var galpon_service_1 = require("../theme/pages/planificacion/galpon/galpon.service");
var Galpon_combo_component_1 = require("../theme/pages/planificacion/shared/Galpon-combo.component");
var create_or_edit_reclamos_component_1 = require("../theme/pages/reclamos/reclamos/create-or-edit-reclamos.component");
var reclamos_component_1 = require("../theme/pages/reclamos/reclamos/reclamos.component");
var tiporeclamo_combo_component_1 = require("../theme/pages/reclamos/shared/tiporeclamo-combo.component");
var denuncia_autocomplete_component_1 = require("../theme/pages/reclamos/shared/denuncia-autocomplete.component");
var ng2_currency_mask_1 = require("ng2-currency-mask");
var causareclamo_combo_component_1 = require("../theme/pages/reclamos/shared/causareclamo-combo.component");
var tiposacuerdo_combo_component_1 = require("../theme/pages/reclamos/shared/tiposacuerdo-combo.component");
var tiposacuerdo_service_1 = require("../theme/pages/reclamos/tiposacuerdo/tiposacuerdo.service");
var rubrosalarial_combo_component_1 = require("../theme/pages/reclamos/shared/rubrosalarial-combo.component");
var create_or_edit_estados_modal_component_1 = require("../theme/pages/siniestros/estados/create-or-edit-estados-modal.component");
var create_abogados_modal_component_1 = require("../theme/pages/siniestros/siniestro/reclamos/create-abogados-modal.component");
var create_or_edit_tiposacuerdo_modal_component_1 = require("../theme/pages/reclamos/tiposacuerdo/create-or-edit-tiposacuerdo-modal.component");
var create_or_edit_rubrossalariales_modal_component_1 = require("../theme/pages/reclamos/rubrossalariales/create-or-edit-rubrossalariales-modal.component");
var create_or_edit_causasreclamo_modal_component_1 = require("../theme/pages/reclamos/causasreclamo/create-or-edit-causasreclamo-modal.component");
var create_or_edit_tiposreclamo_modal_component_1 = require("../theme/pages/reclamos/tiposreclamo/create-or-edit-tiposreclamo-modal.component");
var causasreclamo_service_1 = require("../theme/pages/reclamos/causasreclamo/causasreclamo.service");
var tiposreclamo_service_1 = require("../theme/pages/reclamos/tiposreclamo/tiposreclamo.service");
var rubrossalariales_service_1 = require("../theme/pages/reclamos/rubrossalariales/rubrossalariales.service");
var denuncias_service_1 = require("../theme/pages/art/denuncias/denuncias.service");
var anular_modal_component_1 = require("../theme/pages/reclamos/anular/anular-modal.component");
var estados_modal_component_1 = require("../theme/pages/reclamos/cambioestado/estados-modal.component");
var create_or_edit_juzgados_modal_component_1 = require("../theme/pages/siniestros/juzgados/create-or-edit-juzgados-modal.component");
var reclamos_importador_component_1 = require("../theme/pages/reclamos/reclamos/reclamos-importador/reclamos-importador.component");
var parada_autocomplete_component_1 = require("../theme/pages/planificacion/shared/parada-autocomplete.component");
var parada_service_1 = require("../theme/pages/planificacion/parada/parada.service");
var siniestro_combobyempleado_component_1 = require("../theme/pages/art/shared/siniestro-combobyempleado.component");
var denuncias_combo_component_1 = require("../theme/pages/art/shared/denuncias-combo.component");
var tipoviaje_combo_component_1 = require("../theme/pages/planificacion/shared/tipoviaje-combo.component");
var tipoviaje_service_1 = require("../components/rbmaps/tipoviaje.service");
var datepickerWithKeyboard_component_1 = require("./components/datepickerWithKeyboard.component");
var signalr_service_1 = require("../services/signalr.service");
var month_combo_component_1 = require("./components/month-combo.component");
var gruposinspectores_combo_component_1 = require("../theme/pages/inspectores/shared/gruposinspectores-combo.component");
var gruposinspectores_service_1 = require("../theme/pages/inspectores/gruposinspectores/gruposinspectores.service");
var create_or_edit_user_modal_component_1 = require("../theme/pages/admin/users/create-or-edit-user-modal.component");
var edit_user_permissions_modal_component_1 = require("../theme/pages/admin/users/edit-user-permissions-modal.component");
var permission_tree_component_1 = require("../theme/pages/admin/shared/permission-tree.component");
var edit_user_lineas_modal_component_1 = require("../theme/pages/admin/users/edit-user-lineas-modal.component");
var users_component_1 = require("../theme/pages/admin/users/users.component");
var services_1 = require("../auth/services");
var role_combo_component_1 = require("../theme/pages/admin/shared/role-combo.component");
var roles_service_1 = require("../theme/pages/admin/roles/roles.service");
var SharedModule = /** @class */ (function () {
    function SharedModule() {
    }
    SharedModule = __decorate([
        core_1.NgModule({
            entryComponents: [
                create_or_edit_localidades_modal_component_1.CreateOrEditLocalidadesModalComponent,
                create_or_edit_provincias_modal_component_1.CreateOrEditProvinciasModalComponent,
                create_or_edit_tipodni_modal_component_1.CreateOrEditTipoDniModalComponent,
                create_or_edit_reclamos_component_1.ReclamosGeneralComponent,
                create_or_edit_estados_modal_component_1.CreateOrEditEstadosModalComponent,
                create_abogados_modal_component_1.CreateAbogadosModalComponent,
                create_or_edit_tiposacuerdo_modal_component_1.CreateOrEditTiposAcuerdoModalComponent,
                create_or_edit_rubrossalariales_modal_component_1.CreateOrEditRubrosSalarialesModalComponent,
                create_or_edit_causasreclamo_modal_component_1.CreateOrEditCausasReclamoModalComponent,
                create_or_edit_tiposreclamo_modal_component_1.CreateOrEditTiposReclamoModalComponent,
                create_abogados_modal_component_1.CreateAbogadosModalComponent,
                anular_modal_component_1.AnularReclamoModalComponent,
                estados_modal_component_1.EstadosReclamosModalComponent,
                adjunto_component_1.AdjuntoComponent,
                create_or_edit_juzgados_modal_component_1.CreateOrEditJuzgadosModalComponent,
                reclamos_importador_component_1.ReclamosImportadorComponent,
                create_or_edit_user_modal_component_1.CreateOrEditUserModalComponent,
                edit_user_permissions_modal_component_1.EditUserPermissionsModalComponent,
                permission_tree_component_1.PermissionTreeComponent,
                edit_user_lineas_modal_component_1.EditUserLineasModalComponent,
            ],
            declarations: [
                users_component_1.UsersComponent,
                create_or_edit_user_modal_component_1.CreateOrEditUserModalComponent,
                edit_user_permissions_modal_component_1.EditUserPermissionsModalComponent,
                permission_tree_component_1.PermissionTreeComponent,
                edit_user_lineas_modal_component_1.EditUserLineasModalComponent,
                pipe_1.SortByPipe,
                pipe_1.FilterPipe,
                rbmaps_component_1.RbmapsComponent,
                rutas_map_component_1.RutasMapComponent,
                RbMapLineaPorGrupoFilter_1.RbMapLineaPorGrupoFilter,
                create_or_edit_punto_modal_component_1.CreateOrEditPuntoModalComponent,
                tipoParada_combo_component_1.TipoParadaComboComponent,
                sucursal_combo_component_1.SucursalComboComponent,
                dataType_combo_component_1.DataTypesComboComponent,
                empresa_combo_component_1.EmpresaComboComponent,
                linea_autocomplete_component_1.LineaAutoCompleteComponent,
                coordenada_autocomplete_component_1.CoordenadasAutoCompleteComponent,
                parada_autocomplete_component_1.ParadaAutoCompleteComponent,
                croqui_component_1.CroquiComponent,
                sectoresTarifarios_autocomplete_component_1.SectoresTarifariosAutoCompleteComponent,
                localidad_autocomplete_component_1.LocalidadesAutoCompleteComponent,
                create_or_edit_localidades_modal_component_1.CreateOrEditLocalidadesModalComponent,
                provincias_combo_component_1.ProvinciasComboComponent,
                create_or_edit_provincias_modal_component_1.CreateOrEditProvinciasModalComponent,
                create_or_edit_tipodni_modal_component_1.CreateOrEditTipoDniModalComponent,
                adjunto_component_1.AdjuntoComponent,
                empleado_autocomplete_component_1.EmpleadosAutoCompleteComponent,
                yesnoall_combo_component_1.YesNoAllComboComponent,
                month_combo_component_1.MonthComboComponent,
                estados_combo_component_1.EstadosComboComponent,
                subestado_combo_component_1.SubEstadoComboComponent,
                tipoDocId_combo_component_1.TipoDocIdComboComponent,
                tipoinvolucrado_combo__component_1.TipoInvolucradoComboComponent,
                involucrado_combo_component_1.InvolucradosComboComponent,
                abogados_combo_component_1.AbogadosComboComponent,
                juzgado_combo_component_1.JuzgadosComboComponent,
                siniestro_combo_component_1.SiniestrosComboComponent,
                Galpon_combo_component_1.GalponComboComponent,
                create_or_edit_reclamos_component_1.ReclamosGeneralComponent,
                reclamos_component_1.ReclamosComponent,
                tiporeclamo_combo_component_1.TipoReclamoComboComponent,
                denuncia_autocomplete_component_1.DenunciaAutocompleteComponent,
                causareclamo_combo_component_1.CausaReclamoComboComponent,
                tiposacuerdo_combo_component_1.TiposAcuerdoComboComponent,
                rubrosalarial_combo_component_1.RubroSalarialComboComponent,
                create_or_edit_estados_modal_component_1.CreateOrEditEstadosModalComponent,
                create_abogados_modal_component_1.CreateAbogadosModalComponent,
                create_or_edit_tiposacuerdo_modal_component_1.CreateOrEditTiposAcuerdoModalComponent,
                create_or_edit_rubrossalariales_modal_component_1.CreateOrEditRubrosSalarialesModalComponent,
                create_or_edit_causasreclamo_modal_component_1.CreateOrEditCausasReclamoModalComponent,
                create_or_edit_tiposreclamo_modal_component_1.CreateOrEditTiposReclamoModalComponent,
                create_abogados_modal_component_1.CreateAbogadosModalComponent,
                anular_modal_component_1.AnularReclamoModalComponent,
                estados_modal_component_1.EstadosReclamosModalComponent,
                create_or_edit_juzgados_modal_component_1.CreateOrEditJuzgadosModalComponent,
                reclamos_importador_component_1.ReclamosImportadorComponent,
                siniestro_combobyempleado_component_1.SiniestroEmpleadoComboComponent,
                denuncias_combo_component_1.DenunciasComboComponent,
                gruposinspectores_combo_component_1.GruposInspectoresComboComponent,
                persturnos_combo_component_1.PersTurnosComboComponent,
                role_combo_component_1.RoleComboComponent,
                tipoviaje_combo_component_1.TipoViajeComboComponent,
                datepickerWithKeyboard_component_1.DatePickerWithKeyboard
            ],
            imports: [
                //CommonModule,
                forms_1.FormsModule,
                ng2_validation_1.CustomFormsModule,
                common_1.CommonModule,
                dist_1.SortablejsModule.forRoot({ animation: 150 }),
                utils_module_1.UtilsModule,
                ngx_bootstrap_1.ModalModule.forRoot(),
                ngx_bootstrap_1.TabsModule.forRoot(),
                ngx_bootstrap_1.BsDatepickerModule.forRoot(),
                tristatecheckbox_1.TriStateCheckboxModule,
                togglebutton_1.ToggleButtonModule,
                ngx_permissions_1.NgxPermissionsModule,
                material_1.MatDatepickerModule,
                material_1.MatProgressSpinnerModule,
                material_1.MatTableModule,
                material_1.MatPaginatorModule,
                material_1.MatInputModule,
                material_1.MatSortModule,
                material_1.MatNativeDateModule,
                material_1.MatButtonToggleModule,
                material_1.MatCheckboxModule,
                material_1.MatAutocompleteModule,
                material_1.MatSelectModule,
                material_1.MatSlideToggleModule,
                material_1.MatListModule,
                material_1.MatSidenavModule,
                layout_module_1.LayoutModule,
                utils_module_1.UtilsModule,
                datatable_1.DataTableModule,
                paginator_1.PaginatorModule,
                autocomplete_1.AutoCompleteModule,
                accordion_1.AccordionModule,
                calendar_1.CalendarModule,
                ng_bootstrap_1.NgbModule.forRoot(),
                colorpicker_1.ColorPickerModule,
                table_1.TableModule,
                listbox_1.ListboxModule,
                fileupload_1.FileUploadModule,
                forms_1.ReactiveFormsModule,
                multiselect_1.MultiSelectModule,
                material_1.MatExpansionModule,
                material_1.MatProgressBarModule,
                ngx_mat_select_search_1.NgxMatSelectSearchModule,
                material_1.MatFormFieldModule,
                ng2_currency_mask_1.CurrencyMaskModule,
                angular2_text_mask_1.TextMaskModule
            ],
            exports: [
                utils_module_1.UtilsModule,
                ng2_validation_1.CustomFormsModule,
                ngx_bootstrap_1.ModalModule,
                ngx_bootstrap_1.TabsModule,
                ngx_bootstrap_1.BsDatepickerModule,
                material_1.MatDatepickerModule,
                material_1.MatProgressSpinnerModule,
                material_1.MatTableModule,
                material_1.MatPaginatorModule,
                material_1.MatInputModule,
                material_1.MatSortModule,
                material_1.MatNativeDateModule,
                material_1.MatButtonToggleModule,
                material_1.MatCheckboxModule,
                material_1.MatAutocompleteModule,
                material_1.MatSelectModule,
                material_1.MatSlideToggleModule,
                material_1.MatSidenavModule,
                material_1.MatListModule,
                layout_module_1.LayoutModule,
                utils_module_1.UtilsModule,
                datatable_1.DataTableModule,
                paginator_1.PaginatorModule,
                autocomplete_1.AutoCompleteModule,
                accordion_1.AccordionModule,
                calendar_1.CalendarModule,
                ng_bootstrap_1.NgbModule,
                ngx_permissions_1.NgxPermissionsModule,
                rbmaps_component_1.RbmapsComponent,
                rutas_map_component_1.RutasMapComponent,
                ng2_currency_mask_1.CurrencyMaskModule,
                RbMapLineaPorGrupoFilter_1.RbMapLineaPorGrupoFilter,
                create_or_edit_punto_modal_component_1.CreateOrEditPuntoModalComponent,
                create_or_edit_localidades_modal_component_1.CreateOrEditLocalidadesModalComponent,
                tipoParada_combo_component_1.TipoParadaComboComponent,
                colorpicker_1.ColorPickerModule,
                table_1.TableModule,
                tristatecheckbox_1.TriStateCheckboxModule,
                togglebutton_1.ToggleButtonModule,
                sucursal_combo_component_1.SucursalComboComponent,
                dataType_combo_component_1.DataTypesComboComponent,
                linea_autocomplete_component_1.LineaAutoCompleteComponent,
                listbox_1.ListboxModule,
                fileupload_1.FileUploadModule,
                empresa_combo_component_1.EmpresaComboComponent,
                forms_1.ReactiveFormsModule,
                coordenada_autocomplete_component_1.CoordenadasAutoCompleteComponent,
                parada_autocomplete_component_1.ParadaAutoCompleteComponent,
                croqui_component_1.CroquiComponent,
                multiselect_1.MultiSelectModule,
                material_1.MatExpansionModule,
                material_1.MatProgressBarModule,
                sectoresTarifarios_autocomplete_component_1.SectoresTarifariosAutoCompleteComponent,
                empleado_autocomplete_component_1.EmpleadosAutoCompleteComponent,
                adjunto_component_1.AdjuntoComponent,
                yesnoall_combo_component_1.YesNoAllComboComponent,
                month_combo_component_1.MonthComboComponent,
                localidad_autocomplete_component_1.LocalidadesAutoCompleteComponent,
                ngx_mat_select_search_1.NgxMatSelectSearchModule,
                material_1.MatFormFieldModule,
                create_or_edit_tipodni_modal_component_1.CreateOrEditTipoDniModalComponent,
                estados_combo_component_1.EstadosComboComponent,
                subestado_combo_component_1.SubEstadoComboComponent,
                tipoDocId_combo_component_1.TipoDocIdComboComponent,
                tipoinvolucrado_combo__component_1.TipoInvolucradoComboComponent,
                involucrado_combo_component_1.InvolucradosComboComponent,
                abogados_combo_component_1.AbogadosComboComponent,
                juzgado_combo_component_1.JuzgadosComboComponent,
                siniestro_combo_component_1.SiniestrosComboComponent,
                Galpon_combo_component_1.GalponComboComponent,
                create_or_edit_reclamos_component_1.ReclamosGeneralComponent,
                reclamos_component_1.ReclamosComponent,
                tiporeclamo_combo_component_1.TipoReclamoComboComponent,
                denuncia_autocomplete_component_1.DenunciaAutocompleteComponent,
                causareclamo_combo_component_1.CausaReclamoComboComponent,
                tiposacuerdo_combo_component_1.TiposAcuerdoComboComponent,
                rubrosalarial_combo_component_1.RubroSalarialComboComponent,
                create_or_edit_estados_modal_component_1.CreateOrEditEstadosModalComponent,
                create_abogados_modal_component_1.CreateAbogadosModalComponent,
                create_or_edit_tiposacuerdo_modal_component_1.CreateOrEditTiposAcuerdoModalComponent,
                create_or_edit_rubrossalariales_modal_component_1.CreateOrEditRubrosSalarialesModalComponent,
                create_or_edit_causasreclamo_modal_component_1.CreateOrEditCausasReclamoModalComponent,
                create_or_edit_tiposreclamo_modal_component_1.CreateOrEditTiposReclamoModalComponent,
                create_abogados_modal_component_1.CreateAbogadosModalComponent,
                anular_modal_component_1.AnularReclamoModalComponent,
                estados_modal_component_1.EstadosReclamosModalComponent,
                create_or_edit_juzgados_modal_component_1.CreateOrEditJuzgadosModalComponent,
                siniestro_combobyempleado_component_1.SiniestroEmpleadoComboComponent,
                denuncias_combo_component_1.DenunciasComboComponent,
                datepickerWithKeyboard_component_1.DatePickerWithKeyboard,
                tipoviaje_combo_component_1.TipoViajeComboComponent,
                gruposinspectores_combo_component_1.GruposInspectoresComboComponent,
                persturnos_combo_component_1.PersTurnosComboComponent,
                role_combo_component_1.RoleComboComponent,
                users_component_1.UsersComponent,
                create_or_edit_user_modal_component_1.CreateOrEditUserModalComponent,
                edit_user_permissions_modal_component_1.EditUserPermissionsModalComponent,
                permission_tree_component_1.PermissionTreeComponent,
                edit_user_lineas_modal_component_1.EditUserLineasModalComponent
            ],
            providers: [
                CustomDatepickerI18n_1.I18n, { provide: ng_bootstrap_1.NgbDatepickerI18n, useClass: CustomDatepickerI18n_1.CustomDatepickerI18n },
                {
                    provide: ng_bootstrap_1.NgbDateParserFormatter,
                    useFactory: function () { return new CustomDatepickerI18n_1.NgbDateParserFormatterEs(); }
                },
                // { provide: MAT_DATE_FORMATS, useValue: MY_DATE_FORMATS },
                // { provide: MAT_DATE_LOCALE, useValue: 'es-ar' },
                { provide: material_1.MAT_DATE_FORMATS, useValue: constants_1.MY_DATE_FORMATS },
                { provide: material_1.DateAdapter, useClass: constants_1.CustomDateAdapter },
                { provide: material_1.MAT_DATE_LOCALE, useValue: 'es-ar' },
                // { provide: LOCALE_ID, useValue: 'es-ar' },
                punto_service_1.PuntoService,
                tipoparada_service_1.TipoParadaService,
                sector_service_1.SectorService,
                coordenadas_service_1.CoordenadasService,
                parada_service_1.ParadaService,
                sectoresTarifarios_service_1.SectoresTarifariosService,
                sucursal_service_1.SucursalService,
                linea_service_1.LineaService,
                pla_linea_service_1.PlaLineaService,
                RbMapServices_1.RbMapServices,
                croqui_service_1.CroquiService,
                empleados_service_1.EmpleadosService,
                empresa_service_1.EmpresaService,
                provincias_service_1.ProvinciasService,
                localidad_service_1.LocalidadesService,
                reclamoshistoricos_service_1.ReclamosHistoricosService,
                reclamos_service_1.ReclamosService,
                reclamoshistoricos_service_1.ReclamosHistoricosService,
                estados_service_1.EstadosService,
                tipodni_service_1.TipoDniService,
                tipoinvolucrado_service_1.TipoInvolucradoService,
                involucrados_service_1.InvolucradosService,
                subestados_service_1.SubEstadosService,
                abogados_service_1.AbogadosService,
                siniestro_service_1.SiniestroService,
                galpon_service_1.GalponService,
                tiposacuerdo_service_1.TiposAcuerdoService,
                tiposreclamo_service_1.TiposReclamoService,
                causasreclamo_service_1.CausasReclamoService,
                rubrossalariales_service_1.RubrosSalarialesService,
                denuncias_service_1.DenunciasService,
                tipoviaje_service_1.TipoViajeService,
                signalr_service_1.SignalRService,
                tipoviaje_service_1.TipoViajeService,
                gruposinspectores_service_1.GruposInspectoresService,
                persturnos_service_1.PersTurnosService,
                services_1.UserService,
                roles_service_1.RolesService
            ],
        })
    ], SharedModule);
    return SharedModule;
}());
exports.SharedModule = SharedModule;
//# sourceMappingURL=shared.module.js.map