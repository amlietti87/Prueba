import {
    AfterViewInit,
    Component,
    ComponentFactoryResolver,
    ViewChild,
    ViewContainerRef,
    Input,
    Output,
    OnInit,
    Injector,
    ElementRef,
    ViewEncapsulation,

    QueryList
} from '@angular/core';
import { DetailModalComponent, IDetailComponent, DetailEmbeddedComponent } from '../../../../../shared/manager/detail.component';
import { SiniestrosDto, SiniestrosConsecuenciasDto } from '../../model/siniestro.model';
import { SiniestroService } from '../siniestro.service';
import { MatExpansionPanel, MatDatepickerInputEvent } from '@angular/material';
import { ViewMode, ItemDto, ItemDtoStr } from '../../../../../shared/model/base.model';
import { EmpleadosService } from '../../empleados/empleados.service';
import { EmpleadosDto } from '../../model/empleado.model';
import { PracticantesService } from '../../practicantes/practicantes.service';
import { CochesService } from '../../coches/coches.service';
import { LineaService } from '../../../planificacion/linea/linea.service';
import { ConsecuenciasDto } from '../../model/consecuencias.model';
import { DatosLugarComponent } from '../datoslugar/datoslugar.component';
import { NgForm } from '@angular/forms';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray } from '@angular/forms';
import { ConductorComponent } from '../conductor/conductor.component';
import { CocheComponent } from '../coche/coche.component';
//import { CausasSubCausasComponent } from '../causas/causas.component';
//import { ConsecuenciasCategoriasComponent } from '../consecuencias/consecuenciasacc.component';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { CochesDto } from '../../model/coche.model';
import { DATE } from 'ngx-bootstrap/chronos/units/constants';
//import { FactoresComponent } from '../factores/factores.component';
import { SucursalComboComponent } from '../../../planificacion/shared/sucursal-combo.component';
import { EmpresaComboComponent } from '../../../planificacion/shared/empresa-combo.component';
import { DetalleSiniestroComponent } from '../detallesiniestro/detallesiniestro.component';
import { InvolucradosDto, MuebleInmuebleDto } from '../../model/involucrados.model';
import { InvolucradosComponent } from '../involucrados/involucrados.component';
import { ConductorDto } from '../../model/conductor.model';
import { LesionadoDto } from '../../model/lesionado.model';
import { VehiculoDto } from '../../model/vehiculo.model';

import * as moment from 'moment';
import { ReclamosDto } from '../../model/reclamos.model';
import { environment } from '../../../../../../environments/environment';
import { TabsetComponent } from 'ngx-bootstrap';
import { CroquiComponent } from '../../../../../shared/croqui/croqui.component';
import { saveAs } from 'file-saver'
import { NotificationService } from '../../../../../shared/notification/notification.service';
import { ReclamosComponent } from '../../../reclamos/reclamos/reclamos.component';


declare let mApp: any;

@Component({
    selector: 'siniestro-tabs',
    templateUrl: './create-or-edit-siniestro.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./create-or-edit-siniestro.component.css']
})
export class SiniestroTabsComponent extends DetailEmbeddedComponent<SiniestrosDto> implements OnInit, IDetailComponent {
    protected cfr: ComponentFactoryResolver;
    HoraSiniestro: any;
    ConsecuenciasDefault: SiniestrosConsecuenciasDto[];
    @ViewChild('cochedanio') cochedanio: CocheComponent;
    @ViewChild('conductores') conductores: ConductorComponent;
    @ViewChild('detalle') detalle: DetalleSiniestroComponent;
    @ViewChild('involucradosC') involucradosC: InvolucradosComponent;
    @ViewChild('ReclamosC') ReclamosC: ReclamosComponent;
    @ViewChild('myPanel') myPanel: MatExpansionPanel;
    @ViewChild('panelCoche') panelCoche: MatExpansionPanel;
    @ViewChild('panelDatosLugar') panelDatosLugar: MatExpansionPanel;
    @ViewChild('panelCausas') panelCausas: MatExpansionPanel;
    @ViewChild('panelConsecuencias') panelConsecuencias: MatExpansionPanel;
    @ViewChild('panelFactores') panelFactores: MatExpansionPanel;
    @ViewChild('panelNormas') panelNormas: MatExpansionPanel;
    @ViewChild('panelDetalles') panelDetalles: MatExpansionPanel;
    @ViewChild('panelSeguro') panelSeguro: MatExpansionPanel;
    @ViewChild('panelConductor') panelConductor: MatExpansionPanel;

    ngOnInit() {

    }

    @ViewChild('SucursalCombo') SucursalCombo: SucursalComboComponent;
    @ViewChild('EmpresaCombo') EmpresaCombo: EmpresaComboComponent;

    @ViewChild('tabSet') tabSet: TabsetComponent;
    @ViewChild('Croqui') Croqui: CroquiComponent
    mapaCreado: boolean = false;

    collapsedHeight: string = "35px";
    expandedHeight: string = "35px";
    expanded: boolean = true;
    validateConsecuencias: boolean = true;
    @Input() viewMainTab: boolean = true;
    @Input() viewChildTab: boolean = true;
    GetAdjuntosSiniestros: string;
    appDeleteFileById: string;
    appUploadFiles: string;

    tabMode(m: boolean, c: boolean): void {
        this.viewChildTab = c
        this.viewMainTab = m

        if (this.viewChildTab && !this.viewMainTab) {
            $('#m_heder_portlet_tab_Ramal').click()
        }

        if (this.viewMainTab) {
            $('#m_heder_portlet_tab_Linea').click()
        }
    }

    @ViewChild('DatosLugares') DatosLugares: DatosLugarComponent;
    @ViewChild('tabContent', { read: ViewContainerRef })
    tabContent: ViewContainerRef;

    ngAfterViewInit(): void {
        mApp.initPortlets();
        this.initFirtTab();

    }

    initFirtTab(): void {
        $('#m_heder_portlet_tab_Linea').click();
    }

    constructor(injector: Injector,
        protected service: SiniestroService,
        protected empleadoservice: EmpleadosService,
        protected practicanteservice: PracticantesService,
        protected CochesService: CochesService,
        protected LineaService: LineaService,
        notificationService: NotificationService) {

        super(service, injector);

        this.detail = new SiniestrosDto();
        this.icon = "fa fa-car"
        this.title = "Siniestro";
        var el = injector.get(ElementRef);
        var selector = el.nativeElement.tagName;

        this.cfr = injector.get(ComponentFactoryResolver);
        //this.SetAllowPermission();
        let listsincon: Array<InvolucradosDto> = [];

        let listrec: Array<ReclamosDto> = [];
        this.detail.Reclamos = listrec;


        this.GetAdjuntosSiniestros = environment.siniestrosUrl + '/siniestros/GetAdjuntosSiniestros';
        this.appUploadFiles = environment.siniestrosUrl + '/siniestros/UploadFiles/?SiniestroId=';
        this.appDeleteFileById = environment.siniestrosUrl + '/Siniestros/DeleteFileById';


    }

    events: string[] = [];

    addEvent(type: string, event: MatDatepickerInputEvent<Date>) {
        this.events.push(`${type}: ${event.value}`);
    }

    public onDate(event): void {
        this.GetDiaEvent(event);
    }

    allowAddPracticante: boolean = false;
    //lengueta involucrados
    allowVisualizarInvolucrados: boolean = false;
    //lengueta reclamos
    allowVisualizarReclamos: boolean = false;
    //impresion siniestros
    allowImprimirSiniestro: boolean = false;
    //permiso eliminar
    allowDelete: boolean = false;

    //siniestros
    allowAdd: boolean = false;

    allowModify: boolean = false;

    SetAllowPermission() {
        this.allowAddPracticante = this.permission.isGranted('Siniestro.Practicante.Agregar');
        this.allowVisualizarInvolucrados = this.permission.isGranted('Siniestro.Involucrado.Visualizar');
        this.allowVisualizarReclamos = this.permission.isGranted('Siniestro.Reclamo.Visualizar');
        this.allowImprimirSiniestro = this.permission.isGranted('Siniestro.Siniestro.Imprimir');

        this.allowAdd = this.permission.isGranted('Siniestro.Siniestro.Agregar');
        this.allowModify = this.permission.isGranted('Siniestro.Siniestro.Modificar');
        this.allowDelete = this.permission.isGranted('Siniestro.Siniestro.Eliminar');
        if (!this.allowVisualizarInvolucrados) {
            var tabinv = this.tabSet.tabs.find(e => e.heading == "Involucrados");
            if (tabinv && tabinv != null) {
                this.tabSet.removeTab(tabinv);
            }
        }
        if (!this.allowVisualizarReclamos) {
            var tabinv = this.tabSet.tabs.find(e => e.heading == "Reclamos/Recuperos");
            if (tabinv && tabinv != null) {
                this.tabSet.removeTab(tabinv);
            }
        }
    }

    DatosCabecera(datoscabecera: MatExpansionPanel) {
        this.detail.Hora = this.HoraSiniestro.hour + ":" + this.HoraSiniestro.minute + ":00";
        this.SucursalCombo.setDisabledState(true);
        this.EmpresaCombo.setDisabledState(true);
    }

    fireOnChange(newTime) {

        if (newTime) {
            this.detail.Hora = newTime.hour + ":" + newTime.minute + ":00";
        }
    }

    GetDia(detail: SiniestrosDto) {

        var weekday = new Array(7);
        weekday[0] = "Domingo";
        weekday[1] = "Lunes";
        weekday[2] = "Martes";
        weekday[3] = "Miércoles";
        weekday[4] = "Jueves";
        weekday[5] = "Viernes";
        weekday[6] = "Sábado";
        var date = new Date(this.detail.Fecha);
        detail.Dia = weekday[date.getDay()];

    }

    GetDiaEvent(fecha: Date) {

        var weekday = new Array(7);
        weekday[0] = "Domingo";
        weekday[1] = "Lunes";
        weekday[2] = "Martes";
        weekday[3] = "Miércoles";
        weekday[4] = "Jueves";
        weekday[5] = "Viernes";
        weekday[6] = "Sábado";
        this.detail.Dia = weekday[fecha.getDay()];

    }

    onPrint(row: SiniestrosDto): void {

        this.message.confirm('Si tiene cambios sin guardar no estarán en la impresión, ¿Desea imprimir de todos modos?', 'Confirmación', (a) => {
            if (a.value) {
                var name = "SiniestroNro-" + row.NroSiniestro + ".pdf";
                this.service.GenerateReportById(row.Id)
                    .subscribe(blob => {
                        saveAs(blob, name, {
                            type: 'text/plain;charset=windows-1252' // --> or whatever you need here
                        })
                    });
            }
        });

    }

    completedataBeforeSave(item: SiniestrosDto): any {


        if (item.EmpPractConId == 1) {
            if (item.selectEmpleados) {
                item.ConductorId = item.selectEmpleados.Id;
                item.EmpPract = 'E';
                item.PracticanteId = null;
                item.selectPracticantes = null;
            }
        }
        if (item.EmpPractConId == 2) {
            if (item.selectPracticantes) {
                item.PracticanteId = item.selectPracticantes.Id;
                item.EmpPract = 'P';
                item.ConductorId = null;
                item.selectEmpleados = null;
            }
        }
        if (item.DescargoConId == 1) {
            item.Descargo = true;
        }
        else {
            item.Descargo = false;
        }

        if (item.InformeConId == 1) {
            item.GenerarInforme = true;
        }
        else {
            item.GenerarInforme = false;
        }

        if (item.ResponsableConId == 1) {
            item.Responsable = true;
        }
        else {
            item.Responsable = false;
        }
        if (item.selectCoches) {
            item.CocheId = item.selectCoches.Id;
        }
        if (item.selectLineas) {
            item.CocheLineaId = item.selectLineas.Id;
        }
        if (item.Fecha) {
            this.GetDia(item);
        }

        item.SiniestrosConsecuencias = this.detalle.SiniestroConsecuencias;
        this.validateConsecuencias = true;
        if (item.SiniestrosConsecuencias) {
            if (item.SiniestrosConsecuencias.length > 0) {
                item.SiniestrosConsecuencias.forEach(siniestroConsecuencia => {
                    if (!siniestroConsecuencia.ConsecuenciaId || siniestroConsecuencia.ConsecuenciaId == 0) {
                        // Al menos una consecuencia no esta bien cargada
                        this.notificationService.warn("Al menos una fila de la grilla de consecuencias del siniestro no tiene definida la consecuencia");
                        this.validateConsecuencias = false;
                        return;
                    }
                    if (siniestroConsecuencia.Consecuencia.Adicional == true && ((siniestroConsecuencia.Observaciones == null || siniestroConsecuencia.Observaciones == ""))) {
                        this.notificationService.warn("La consecuencia " + siniestroConsecuencia.Consecuencia.Descripcion + " tiene Adicional requerido, por lo tanto debe agregar una observación.");
                        this.validateConsecuencias = false;
                        return;
                    }
                    if (siniestroConsecuencia.Categorias && siniestroConsecuencia.Categorias.length > 0 && (!siniestroConsecuencia.CategoriaId || siniestroConsecuencia.CategoriaId == null)) {
                        this.notificationService.warn("La consecuencia " + siniestroConsecuencia.Consecuencia.Descripcion + " tiene Categorias disponibles, por lo tanto debe seleccionar una de ellas.");
                        this.validateConsecuencias = false;
                        return;
                    }
                })
            } else {
                // No ingreso consecuencias
                this.notificationService.warn("Debe agregar al menos una consecuencia");
                this.validateConsecuencias = false;
                return;
            }
        } else {
            // No ingreso consecuencias
            this.notificationService.warn("Debe agregar al menos una consecuencia");
            this.validateConsecuencias = false;
            return;
        }

        if (this.viewMode == ViewMode.Add) {
            item.NroSiniestro = "0";
        }

    }

    completedataBeforeShow(item: SiniestrosDto): any {

        this.SetAllowPermission();
        this.validateConsecuencias = true;

        if (this.viewMode == ViewMode.Modify) {
            this.SucursalCombo.setDisabledState(true);
            this.EmpresaCombo.setDisabledState(true);
            if (item.Descargo) {
                item.DescargoConId = 1;
            }
            else {
                item.DescargoConId = 2;
            }

            if (item.GenerarInforme) {
                item.InformeConId = 1;

                if (this.detalle.InformeConId) {
                    this.detalle.InformeConId.setDisabledState(true);
                    this.detalle.disableInforme = true;
                }

                if (this.detalle.SancionSugeridaId) {
                    this.detalle.SancionSugeridaId.setDisabledState(true);
                    this.detalle.disableSancionSugerida = true;
                }
            }
            else {
                item.InformeConId = 2;
                if (this.detalle.InformeConId) {
                    this.detalle.InformeConId.setDisabledState(false);
                    this.detalle.disableInforme = false;
                }

                if (this.detalle.SancionSugeridaId) {
                    this.detalle.SancionSugeridaId.setDisabledState(false);
                    this.detalle.disableSancionSugerida = false;
                }
            }

            if (item.Responsable) {
                item.ResponsableConId = 1;
            }
            else {
                item.ResponsableConId = 2;
            }
            if (item.CocheLineaId) {
                this.LineaService.getById(item.CocheLineaId)
                    .subscribe((t) => {
                        var findLinea = new ItemDto();
                        findLinea.Id = item.CocheLineaId;
                        findLinea.Description = t.DataObject.DesLin;
                        item.selectLineas = findLinea;

                    })
            }


            if (this.allowVisualizarInvolucrados) {
                this.involucradosC.InitInvolucrado(this.detail);
            }

            if (this.allowVisualizarReclamos) {
                this.ReclamosC.filter.SiniestroId = this.detail.Id;
                this.ReclamosC.Search();
            }



        }
        if (this.viewMode == ViewMode.Modify && item.EmpPract == 'E' && item.ConductorId) {

            this.empleadoservice.ExisteEmpleado(item.ConductorId)
                .subscribe((t) => {

                    if (t.DataObject) {
                        if (this.conductores.ConductorId) {
                            this.conductores.ConductorId.setDisabledState(false);
                        }
                        if (this.conductores.EmpPractConId) {
                            this.conductores.EmpPractConId.setDisabledState(false);
                        }
                        this.conductores.empleadoExiste = true;
                        this.empleadoservice.getById(item.ConductorId)
                            .subscribe((t) => {

                                var findconductor = new ItemDto();
                                findconductor.Id = item.ConductorId;
                                findconductor.Description = t.DataObject.Apellido + ', ' + t.DataObject.Nombre;
                                item.selectEmpleados = findconductor;
                                item.EmpPractConId = 1;
                            })
                        this.empleadoservice.ExisteLegajoEmpleado(item.ConductorId).subscribe((t) => {
                            if (t.DataObject) {
                                if (this.detalle.InformeConId) {
                                    this.detalle.InformeConId.setDisabledState(false);
                                    this.detalle.disableInforme = false;
                                }

                                if (this.detalle.SancionSugeridaId) {
                                    this.detalle.SancionSugeridaId.setDisabledState(false);
                                    this.detalle.disableSancionSugerida = false;
                                }
                            }
                            else {
                                if (this.detalle.InformeConId) {
                                    if (!this.detalle.detail.ConductorLegajo) {
                                        this.detalle.InformeConId.setDisabledState(true);
                                        this.detalle.disableInforme = true;
                                    }
                                }

                                if (this.detalle.SancionSugeridaId) {
                                    this.detalle.SancionSugeridaId.setDisabledState(true);
                                    this.detalle.disableSancionSugerida = true;
                                }
                            }
                        })
                    }
                    else {
                        if (this.conductores.ConductorId) {
                            this.conductores.ConductorId.setDisabledState(true);
                        }

                        if (this.detalle.InformeConId) {
                            this.detalle.InformeConId.setDisabledState(true);
                            this.detalle.disableInforme = true;
                        }

                        if (this.detalle.SancionSugeridaId) {
                            this.detalle.SancionSugeridaId.setDisabledState(true);
                            this.detalle.disableSancionSugerida = true;
                        }

                        if (this.conductores.EmpPractConId) {
                            this.conductores.EmpPractConId.setDisabledState(true);
                        }

                        this.conductores.empleadoExiste = false;

                    }
                })



            if (item.CocheId) {
                this.cochedanio.CurrentCoche = new CochesDto();
                this.cochedanio.CurrentCoche.Id = this.detail.CocheId;
                this.cochedanio.CurrentCoche.Dominio = this.detail.CocheDominio + " ";
                this.cochedanio.CurrentCoche.Ficha = this.detail.CocheFicha;
                this.cochedanio.CurrentCoche.Interno = this.detail.CocheInterno;
                if (this.viewMode == ViewMode.Modify) {

                    this.CochesService.ExisteCoche(item.CocheId)
                        .subscribe((t) => {
                            if (t.DataObject) {
                            }
                            else {
                                this.cochedanio.autocompletecoche.setDisabledState(true);
                                var item2 = new ItemDtoStr();
                                item2.Description = " ";
                                item2.Id = item.CocheId;
                                this.cochedanio.autocompletecoche.value = item2;
                            }
                        })


                }

            }

        }
        else if (this.viewMode == ViewMode.Modify && item.EmpPract == 'P' && item.PracticanteId) {

            this.practicanteservice.getById(item.PracticanteId)
                .subscribe((t) => {
                    var findempleado = new ItemDto();
                    findempleado.Id = item.PracticanteId;
                    findempleado.Description = t.DataObject.ApellidoNombre;
                    item.selectPracticantes = findempleado;

                    item.EmpPractConId = 2;
                })

            if (item.CocheId) {
                this.CochesService.GetCocheById(item.CocheId, item.Fecha)
                    .subscribe((t) => {
                        if (t.DataObject) {
                            var findcoche = new ItemDtoStr();
                            findcoche.Id = item.CocheId;
                            findcoche.Description = t.DataObject.Dominio;
                            item.selectCoches = findcoche;
                        }

                    })
            }
        }

        if (this.viewMode == ViewMode.Modify && item.Hora) {
            this.HoraSiniestro = { hour: item.Hora.split(":")[0], minute: item.Hora.split(":")[1] };
        }
        if (this.viewMode == ViewMode.Add) {

            item.DescargoConId = 1;
            item.EmpPractConId = 1;
            item.InformeConId = 2;
            //this.ConsecuenciasDefault = [];
            //let lista = [...this.ConsecuenciasDefault];
            //lista.push(this.detalle.getNewItem(new SiniestrosConsecuenciasDto(), null));
            //this.ConsecuenciasDefault = lista;
            this.cochedanio.autocompletecoche.setDisabledState(false);



        }
        if (this.viewMode == ViewMode.Modify && item.SiniestrosConsecuencias) {

            //var array = [];
            //item.SiniestrosConsecuencias.forEach(function (value) {
            //    var itemdto = new ConsecuenciasDto();
            //    itemdto = value.Consecuencia;
            //    if (value.Categoria) {
            //        itemdto.CategoriaElegida = value.Categoria.Id;
            //        itemdto.CategoriaNombre = value.Categoria.Descripcion;
            //    }
            //    itemdto.Observaciones = value.Observaciones;
            //    itemdto.SinConsecuenciaId = value.Id;
            //    array.push(itemdto);
            //});

            //this.ConsecuenciasDefault = array;
        }

    }

    ngAfterViewChecked(): void {
        $('tabset ul.nav').addClass('m-tabs-line');
        $('tabset ul.nav li a.nav-link').addClass('m-tabs__link');
    }

    createDefaultDetail(): any {
        this.detail = new SiniestrosDto();
    }

    createForm() {
        this.mainForm = this.detailFB.group({
            Id: [this.detail.Id, null],
            SucursalId: [this.detail.SucursalId, Validators.required],
            EmpresaId: [this.detail.EmpresaId, Validators.required],
            Fecha: [this.detail.Fecha, Validators.required],
            HoraSiniestro: [this.HoraSiniestro, Validators.required]
        });

        this.formControlValueChanged();
    }

    formControlValueChanged() {

    }

    scrollToElement(): void {
        $('#porletSiniestro')[0].scrollTo(0, 0);
        this.tabSet.tabs.forEach(e => e.active = false);
        this.tabSet.tabs[0].active = true;
    }

    close(): void {
        this.breadcrumbsService.RemoveItem(this.getSelector());
        this.scrollToElement();
        $('#fullscreentools')[0].click();
        super.close();
        if (this.detailForm) {
            this.detailForm.resetForm();
        }

        if (this.mainForm) {
            this.mainForm.reset();
        }

        if (this.conductores.conductorForm) {
            this.conductores.conductorForm.reset();
        }
        if (this.cochedanio.cocheForm) {
            this.cochedanio.cocheForm.reset();
        }

        if (this.DatosLugares.lugarForm) {
            this.DatosLugares.lugarForm.reset();
        }

        if (this.detalle.detalleSiniestroForm1) {
            this.detalle.detalleSiniestroForm1.reset();
        }

        if (this.detalle.detalleSiniestroForm2) {
            this.detalle.detalleSiniestroForm2.reset();
        }

        this.SucursalCombo.setDisabledState(false);
        this.EmpresaCombo.setDisabledState(false);

        if (this.detalle.InformeConId) {
            this.detalle.InformeConId.setDisabledState(false);
            this.detalle.disableInforme = false;
        }

        if (this.detalle.SancionSugeridaId) {
            this.detalle.SancionSugeridaId.setDisabledState(false);
            this.detalle.disableSancionSugerida = false;
        }

        if (this.detalle.SiniestroConsecuencias) {
            this.detalle.SiniestroConsecuencias = null;
        }
        this.ConsecuenciasDefault = null;
        this.cochedanio.CurrentCoche = null;
        this.cochedanio.CurrentLinea = null;
        this.conductores.CurrentConductor = null;
        this.conductores.CurrentConductorEmpresa = null;
        this.conductores.CurrentConductorLegajo = null;
        this.conductores.CurrentHistorial = null;
        this.conductores.CurrentPracticante = null;

        if (this.allowVisualizarReclamos && this.ReclamosC) {

            this.ReclamosC.filter = this.ReclamosC.getNewfilter();
            this.ReclamosC.primengDatatableHelper.records = [];
            this.ReclamosC.primengDatatableHelper.totalRecordsCount = 0;
        }
        if (this.allowVisualizarInvolucrados) {
            this.involucradosC.getNewfilter();
        }

        $('body').removeClass("smallsize");
    }

    showNew(item: SiniestrosDto) {

        if (this.detailForm) {
            this.detailForm.resetForm();
        }

        if (this.mainForm) {
            this.mainForm.reset();
        }

        if (this.conductores.conductorForm) {
            this.conductores.conductorForm.reset();
        }
        if (this.cochedanio.cocheForm) {
            this.cochedanio.cocheForm.reset();
        }

        if (this.DatosLugares.lugarForm) {
            this.DatosLugares.lugarForm.reset();
        }

        if (this.detalle.detalleSiniestroForm1) {
            this.detalle.detalleSiniestroForm1.reset();
        }

        if (this.detalle.detalleSiniestroForm2) {
            this.detalle.detalleSiniestroForm2.reset();
        }

        this.conductores.empleadoExiste = true;
        this.SucursalCombo.setDisabledState(false);
        this.EmpresaCombo.setDisabledState(false);
        if (this.detalle.InformeConId) {
            this.detalle.InformeConId.setDisabledState(false);
            this.detalle.disableInforme = false;
        }
        if (this.detalle.SancionSugeridaId) {
            this.detalle.SancionSugeridaId.setDisabledState(false);
            this.detalle.disableSancionSugerida = false;
        }
        this.viewMode = ViewMode.Add;
        this.showDto(item);
    }

    showDto(item: SiniestrosDto) {
        super.showDto(item);
    }

    save(): void {

        var self = this;
        if (this.detailForm && this.detailForm.form.invalid) {
            return;
        }

        if (this.mainForm && this.mainForm.invalid) {
            this.markFormGroupTouched(this.mainForm);
            return;
        }


        if (this.cochedanio.cocheForm && this.cochedanio.cocheForm.invalid) {
            this.markFormGroupTouched(this.cochedanio.cocheForm);
            return;
        }

        this.conductores.conductorForm.get('ConductorId').updateValueAndValidity();
        this.conductores.conductorForm.get('PracticanteId').updateValueAndValidity();

        if (this.conductores.conductorForm && this.conductores.conductorForm.invalid) {
            this.markFormGroupTouched(this.conductores.conductorForm);
            return;
        }

        if (this.DatosLugares.lugarForm && this.DatosLugares.lugarForm.invalid) {
            this.markFormGroupTouched(this.DatosLugares.lugarForm);
            return;
        }

        if (this.detail.EmpPractConId == 2) {
            if (this.detalle.detalleSiniestroForm1) {
                this.detalle.detalleSiniestroForm1.get('InformeConId').clearValidators();
                this.detalle.detalleSiniestroForm1.get('InformeConId').reset();
            }
        }

        if (this.detalle.detalleSiniestroForm1 && this.detalle.detalleSiniestroForm1.invalid) {
            this.markFormGroupTouched(this.detalle.detalleSiniestroForm1);
            return;
        }

        if (this.detalle.detalleSiniestroForm2 && this.detalle.detalleSiniestroForm2.invalid) {
            if (this.detalle.detail.CocheDominio != null) {
                if (!this.detalle.detail.CocheFicha) {
                    this.markFormGroupTouched(this.detalle.detalleSiniestroForm2);
                    return;
                }
            }
        }

        this.saving = true;
        this.completedataBeforeSave(this.detail);


        if (!this.validateSave()) {
            this.saving = false;
            return;
        }

        if (!this.validateConsecuencias) {
            this.saving = false;
            return;
        }

        this.service.createOrUpdate(this.detail, this.viewMode)
            .finally(() => { this.saving = false; })
            .subscribe((t) => {

                if (this.viewMode = ViewMode.Add) {
                    this.detail.Id = t.DataObject;
                }

                this.notify.info('Guardado exitosamente');

                if (t.Messages && t.Messages.length > 0) {
                    this.notify.info(t.Messages.join(','));
                }

                this.closeOnSave = false;
                if (this.closeOnSave) {
                    this.close();
                };
                this.affterSave(this.detail);
                this.modalSave.emit(null);
            })
    }

    delete(id: number) {
        console.log(id);
        this.message.confirm('¿Está seguro que desea borrar el siniestro?', "Eliminar Siniestro", (a) => {
            if (a.value) {
                this.removing = true;
                this.service.delete(id)
                    .finally(() => this.removing = false)
                    .subscribe((t) => {
                        this.notify.info('Eliminado exitosamente');
                        this.closeOnSave = true;
                        if (this.closeOnSave) {
                            this.close();
                        };
                    })
            }
        });
    }

    formatDate(date) {
        return moment(date).utc().format('DD/MM/YYYY HH:mm');
    }

}