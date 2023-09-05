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

    QueryList,

    ChangeDetectorRef
} from '@angular/core';
import { DetailModalComponent, IDetailComponent, DetailEmbeddedComponent } from '../../../../../shared/manager/detail.component';
import { MatExpansionPanel, MatDatepickerInputEvent, MatTableDataSource } from '@angular/material';
import { ViewMode, ItemDto, ItemDtoStr } from '../../../../../shared/model/base.model';
import { NgForm, AbstractControl, ValidatorFn } from '@angular/forms';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray } from '@angular/forms';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { DATE } from 'ngx-bootstrap/chronos/units/constants';
import * as moment from 'moment';
import { environment } from '../../../../../../environments/environment';
import { TabsetComponent } from 'ngx-bootstrap';
import { DenunciasDto } from '../../model/denuncias.model';
import { DenunciasService } from '../denuncias.service';
import { HistorialReclamosEmpleadoDto, HistorialDenunciasEmpleadoDto } from '../../model/HistorialReclamosEmpleado';
import { DenunciaNotificacionesDto } from '../../model/denunciasnotificaciones.model';
import { MotivosNotificacionesDto } from '../../model/motivosnotificaciones.model';
import { MotivosNotificacionesService } from '../../motivosnotificaciones/motivosnotificaciones.service';
import { ReingresosDto } from '../../model/reingresos.model';
import { EmpresaComboComponent } from '../../../planificacion/shared/empresa-combo.component';
import { EmpleadosService } from '../../../siniestros/empleados/empleados.service';
import { EmpleadosDto, LegajosEmpleadoDto } from '../../../siniestros/model/empleado.model';
import { LegajosEmpleadoService } from '../../../siniestros/legajoempleado/legajosempleado.service';
import { EmpresaService } from '../../../planificacion/empresa/empresa.service';
import { EmpresaDto } from '../../../planificacion/model/empresa.model';
import { SucursalService } from '../../../planificacion/sucursal/sucursal.service';
import { SucursalDto } from '../../../planificacion/model/sucursal.model';
import { HistorialDenunciasDto } from '../../model/HistorialDenuncias';
import { forEach } from '@angular/router/src/utils/collection';
import { EstadosComboComponent } from '../../../siniestros/shared/estados-combo.component';
import { DenunciasEstadosComponent } from '../../estados/estados.component';
import { EstadosDenunciaComboComponent } from '../../shared/estadosdenuncia-combo.component';
import { SiniestroService } from '../../../siniestros/siniestro/siniestro.service';


declare let mApp: any;

@Component({
    selector: 'denuncias-tabs',
    templateUrl: './create-or-edit-denuncias.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./create-or-edit-denuncias.component.css']
})
export class DenunciasTabsComponent extends DetailEmbeddedComponent<DenunciasDto> implements OnInit, IDetailComponent {
    protected cfr: ComponentFactoryResolver;

    collapsedHeight: string = "35px";
    expandedHeight: string = "35px";
    @Input() viewMainTab: boolean = true;
    @Input() viewChildTab: boolean = true;
    expanded: boolean = true;
    events: string[] = [];

    @ViewChild('form') form;

    //ADJUNTOS
    GetAdjuntosDenuncias: string;
    appDeleteFileById: string;
    appUploadFiles: string;

    //HISTORIALES
    historialDenunciasEmpleado: HistorialDenunciasEmpleadoDto[] = [];
    historialReclamosEmpleado: HistorialReclamosEmpleadoDto[] = [];
    historialDenunciasPrestador: HistorialDenunciasDto[] = []

    //EMPLEADO
    empleadoExiste: boolean = true;
    CurrentEmpleado: EmpleadosDto;
    CurrentEmpleadoLegajo: LegajosEmpleadoDto;
    CurrentConductorEmpresa: EmpresaDto;
    CurrentUnidadDeNegocio: SucursalDto;

    //EDICION EN GRILLA
    displayedColumnsNotificacion = ['FechaNotificacion', 'MotivoNotificacion', 'ObservacionesNotificaciones', 'Acciones'];
    // displayedColumnsReingresos = ['FechaReingreso', 'AltaMedicaReingreso', 'FechaAltaMedicaReingreso', 'AltaLaboralReingreso', 'FechaAltaLaboralReingreso', 'FechaProbableAlta', 'Observacion', 'CantidadDias', 'Acciones'];
    datasourceNotificaciones: MatTableDataSource<DenunciaNotificacionesDto>;
    // datasourceReingresos: MatTableDataSource<ReingresosDto>;

    //PERMISOS DENUNCIAS
    allowAdd: boolean = false;
    allowModify: boolean = false;
    allowDelete: boolean = false;
    allowAnular: boolean = false;
    allowVisualizar: boolean = false;
    allowImprimir: boolean = false;
    allowExportar: boolean = false;
    allowImportar: boolean = false;
    allowAdjunto: boolean = false;

    //PERMISOS COMBOS
    allowAddPrestadores: boolean = false;
    allowAddEstados: boolean = false;
    allowAddContingencia: boolean = false;
    allowAddTratamiento: boolean = false;
    allowAddPatologias: boolean = false;
    allowAddMotivoAudiencia: boolean = false;
    allowAddMotivoNotificacion: boolean = false;

    SetAllowPermission() {

        this.allowAnular = this.permission.isGranted('ART.Denuncia.Anular');
        this.allowAdd = this.permission.isGranted('ART.Denuncia.Agregar');
        this.allowModify = this.permission.isGranted('ART.Denuncia.Modificar');
        this.allowDelete = this.permission.isGranted('ART.Denuncia.Eliminar');
        this.allowVisualizar = this.permission.isGranted('ART.Denuncia.Visualizar');
        this.allowImprimir = this.permission.isGranted('ART.Denuncia.Imprimir');
        this.allowExportar = this.permission.isGranted('ART.Denuncia.Exportar');
        this.allowImportar = this.permission.isGranted('ART.Denuncia.Importar');
        this.allowAdjunto = this.permission.isGranted('ART.Denuncia.Adjunto');
        this.allowAddPrestadores = this.permission.isGranted('ART.PrestadorMedico.Agregar');
        this.allowAddEstados = this.permission.isGranted('ART.EstadoDenuncia.Agregar');
        this.allowAddContingencia = this.permission.isGranted('ART.Contingencia.Agregar');
        this.allowAddTratamiento = this.permission.isGranted('ART.Tratamiento.Agregar');
        this.allowAddPatologias = this.permission.isGranted('ART.Patologia.Agregar');
        this.allowAddMotivoAudiencia = this.permission.isGranted('ART.MotivoAudiencia.Agregar');
        this.allowAddMotivoNotificacion = this.permission.isGranted('ART.MotivoNotificacion.Agregar');
    }


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
    @ViewChild('tabSet') tabSet: TabsetComponent;
    @ViewChild('tabContent', { read: ViewContainerRef })
    @ViewChild('EmpresaCombo') EmpresaCombo: EmpresaComboComponent;
    @ViewChild('EstadosCombo') EstadosComboComponent: EstadosDenunciaComboComponent;


    tabContent: ViewContainerRef;

    ngAfterViewInit(): void {
        mApp.initPortlets();
        this.initFirtTab();
    }

    initFirtTab(): void {
        $('#m_heder_portlet_tab_Linea').click();
    }

    constructor(injector: Injector,
        protected service: DenunciasService,
        protected serviceEmpleado: EmpleadosService,
        protected serviceLegajoEmpleado: LegajosEmpleadoService,
        protected serviceEmpresa: EmpresaService,
        protected serviceSucursal: SucursalService,
        protected serviceSiniestro: SiniestroService,
        protected cdr: ChangeDetectorRef) {

        super(service, injector);
        this.detail = this.createDefaultDetail();

        this.icon = "fa fa-car"
        this.title = "Denuncia";
        var el = injector.get(ElementRef);
        var selector = el.nativeElement.tagName;

        this.cfr = injector.get(ComponentFactoryResolver);

        this.GetAdjuntosDenuncias = environment.artUrl + '/Denuncias/GetAdjuntosDenuncias';
        this.appUploadFiles = environment.artUrl + '/Denuncias/UploadFiles/?DenunciaId=';
        this.appDeleteFileById = environment.artUrl + '/Denuncias/DeleteFileById';
    }



    private HistorialDenunciaPorPrestador() {
        this.service.HistorialDenunciaPorPrestador(this.detail.EmpleadoId)
            .subscribe((t) => {
                this.historialDenunciasPrestador = t.DataObject;
            })
    }

    private HistorialReclamosEmpleado() {
        this.service.HistorialReclamosEmpleado(this.detail.EmpleadoId)
            .subscribe((t) => {
                this.historialReclamosEmpleado = t.DataObject;
            })
    }

    private HistorialDenunciasEmpleado() {
        this.service.HistorialDenunciasPorEstado(this.detail.EmpleadoId)
            .subscribe((t) => {
                this.historialDenunciasEmpleado = t.DataObject;
            })
    }


    private GetCurrentEmpleado(): void {

        if (this.detail.EmpleadoId) {
            this.serviceEmpleado.ExisteEmpleado(this.detail.EmpleadoId).subscribe((t) => {
                if (t.DataObject) {
                    this.empleadoExiste = true;
                    this.serviceEmpleado.getById(this.detail.EmpleadoId)
                        .subscribe((t) => {
                            this.CurrentEmpleado = t.DataObject;

                            if (this.CurrentEmpleado.cod_sucursal && this.CurrentEmpleado.cod_sucursal != null) {
                                this.serviceSucursal.getById(this.CurrentEmpleado.cod_sucursal)
                                    .subscribe((t) => {
                                        this.CurrentUnidadDeNegocio = t.DataObject;
                                        this.detail.SucursalId = this.CurrentUnidadDeNegocio.Id;
                                    })
                            }

                            this.serviceLegajoEmpleado.GetMaxById(this.detail.EmpleadoId)
                                .subscribe((t) => {
                                    this.CurrentEmpleadoLegajo = t.DataObject;
                                    if (this.detail.selectedEmpleado && !this.detail.ActualizarConductor && this.viewMode == ViewMode.Modify) {
                                        this.CurrentEmpleadoLegajo.LegajoSap = this.detail.EmpleadoLegajo;
                                        this.CurrentEmpleadoLegajo.FecIngreso = this.detail.EmpleadoFechaIngreso;
                                        this.CurrentEmpleadoLegajo.CodEmpresa = this.detail.EmpleadoEmpresaId;
                                        this.CurrentEmpleado.FecAntiguedad = this.detail.EmpleadoAntiguedad;
                                        this.CurrentEmpleado.Area = this.detail.EmpleadoArea;
                                    }
                                    else {
                                        this.detail.EmpleadoFechaIngreso = this.CurrentEmpleadoLegajo.FecIngreso;
                                        this.detail.EmpleadoLegajo = this.CurrentEmpleadoLegajo.LegajoSap;
                                        this.detail.EmpleadoEmpresaId = this.CurrentEmpleadoLegajo.CodEmpresa;
                                        this.detail.EmpleadoAntiguedad = this.CurrentEmpleado.FecAntiguedad;
                                        this.detail.EmpleadoArea = this.CurrentEmpleado.Area;
                                        this.detail.ActualizarConductor = false;
                                    }
                                    if (this.CurrentEmpleadoLegajo && this.CurrentEmpleadoLegajo.CodEmpresa && this.CurrentEmpleadoLegajo.CodEmpresa != null) {
                                        this.serviceEmpresa.getById(this.CurrentEmpleadoLegajo.CodEmpresa)
                                            .subscribe((t) => {
                                                this.CurrentConductorEmpresa = t.DataObject;
                                            })
                                    }
                                })

                        })

                }
                else {
                    this.empleadoExiste = false;
                }
            })
        }

    }



    addEvent(type: string, event: MatDatepickerInputEvent<Date>) {
        this.events.push(`${type}: ${event.value}`);
    }

    public onDate(event): void {

    }


    completedataBeforeSave(item: DenunciasDto): any {

    }

    AnuladoDisable() {

        (<any>Object).values(this.mainForm.controls).forEach(control => {
            control.disable();
        });
    }

    AnuladoEnable() {

        (<any>Object).values(this.mainForm.controls).forEach(control => {
            control.enable();
        });

        this.mainForm.get('Juicio').disable();
        this.mainForm.get('TieneReingresos').disable();
        this.mainForm.get('Anulado').disable();
    }

    completedataBeforeShow(item: DenunciasDto): any {
        this.SetAllowPermission();

        this.datasourceNotificaciones = new MatTableDataSource(item.DenunciaNotificaciones);
        if (item.DenunciaNotificaciones) {
            item.DenunciaNotificaciones.forEach(e => this.addNotificacion(e));
        }

        this.mainForm.get('Juicio').disable();
        this.mainForm.get('Anulado').disable();

        if (this.viewMode == ViewMode.Modify) {

            if (item.Anulado || (this.allowVisualizar && !this.allowModify)) {
                this.AnuladoDisable();
            }
            else {
                this.mainForm.get('EmpleadoId').enable();
            }
            this.mainForm.get('NroDenuncia').disable();
            this.mainForm.get('EmpresaId').disable();
            this.mainForm.get('PrestadorMedicoId').disable();

            this.GetCurrentEmpleado();
            this.HistorialDenunciaPorPrestador();
            this.HistorialReclamosEmpleado();
            this.HistorialDenunciasEmpleado();
        }
        else {
            this.detail = this.createDefaultDetail();


            this.mainForm.get('NroDenuncia').enable();
            this.mainForm.get('EmpresaId').enable();
            this.mainForm.get('EmpleadoId').enable();

            this.mainForm.get('EstadoId').setValue(this.LoadDefaultEstadoId());
            this.mainForm.get('PrestadorMedicoId').enable();
            this.mainForm.get('FechaAltaMedica').disable();
        }

        this.cdr.detectChanges();
    }


    ngAfterViewChecked(): void {
        $('tabset ul.nav').addClass('m-tabs-line');
        $('tabset ul.nav li a.nav-link').addClass('m-tabs__link');
    }
    createDefaultDetail(): any {
        var item = new DenunciasDto();
        item.Anulado = false;
        item.BajaServicio = true;
        item.AltaMedica = false;
        item.AltaLaboral = false;
        item.TieneReingresos = false;
        return item;
    }

    LoadDefaultEstadoId(): number {

        if (this.EstadosComboComponent) {
            if (this.EstadosComboComponent.data) {
                var defaultEstado = this.EstadosComboComponent.data.filter(e => e.Predeterminado);
                if (defaultEstado != null && defaultEstado.length > 0) {
                    return defaultEstado[0].Id;
                }
            }
            /*
            if (this.EstadosComboComponent.items) {
                var defaultEstado = this.EstadosComboComponent.items.filter(e => e.Predeterminado);
                if (defaultEstado != null && defaultEstado.length > 0) {
                    return defaultEstado[0].Id;
                }
            }
            */
        }
        return 0;
    }


    createForm() {
        if (!this.detail) {
            this.detail = this.createDefaultDetail();

        }
        this.mainForm = this.detailFB.group({
            NroDenuncia: [this.detail.NroDenuncia, Validators.required],
            EmpresaId: [this.detail.EmpresaId, Validators.required],
            EmpleadoId: [this.detail.selectedEmpleado, Validators.required],
            EstadoId: [this.detail.EstadoId, Validators.required],
            FechaOcurrencia: [this.detail.FechaOcurrencia, Validators.required],
            FechaRecepcionDenuncia: [this.detail.FechaRecepcionDenuncia, Validators.required],
            ContingenciaId: [this.detail.ContingenciaId, null],
            PatologiaId: [this.detail.PatologiaId, null],
            BajaServicio: [this.detail.BajaServicio, null],
            FechaBajaServicio: [this.detail.FechaBajaServicio, Validators.required],
            TratamientoId: [this.detail.TratamientoId, null],
            PorcentajeIncapacidad: [this.detail.PorcentajeIncapacidad, Validators.max(100)],
            AltaMedica: [this.detail.AltaMedica, null],
            FechaAltaMedica: [this.detail.FechaAltaMedica, Validators.required],
            FechaUltimoControl: [this.detail.FechaUltimoControl, null],
            FechaProximaConsulta: [this.detail.FechaProximaConsulta, null],
            FechaAudienciaMedica: [this.detail.FechaAudienciaMedica, null],
            MotivoAudienciaId: [this.detail.MotivoAudienciaId, null],
            AltaLaboral: [this.detail.AltaLaboral, null],
            FechaAltaLaboral: [this.detail.FechaAltaLaboral, Validators.required],
            CantidadDiasBaja: [this.detail.CantidadDiasBaja, null],
            SiniestroId: [this.detail.SiniestroId, null],
            Juicio: [this.detail.Juicio, Validators.required],
            Observaciones: [this.detail.Observaciones, null],
            Diagnostico: [this.detail.Diagnostico, null],
            TieneReingresos: [this.detail.TieneReingresos, null],
            PrestadorMedicoId: [this.detail.PrestadorMedicoId, Validators.required],
            Anulado: [this.detail.Anulado, Validators.required],
            Notificaciones: this.detailFB.array([]),/*
            Reingresos: this.detailFB.array([]),*/
            DenunciaIdOrigen: [this.detail.DenunciaIdOrigen, null],
            FechaProbableAlta: [this.detail.FechaProbableAlta, null]
        });
        this.mainForm.get('Juicio').disable();
        this.mainForm.get('Anulado').disable();
        this.mainForm.get('TieneReingresos').disable();

        this.formControlValueChanged();
    }

    formControlValueChanged() {
        this.mainForm.get('AltaMedica').valueChanges.subscribe(
            (AltaMedica: boolean) => {
                this.mainForm.get('FechaAltaMedica').clearValidators();
                if (AltaMedica) {
                    this.mainForm.get('FechaAltaMedica').enable();
                    this.mainForm.get('FechaAltaMedica').setValidators([Validators.required]);
                    this.mainForm.get('FechaAltaMedica').updateValueAndValidity();
                }
                else {
                    this.mainForm.get('FechaAltaMedica').disable();
                    this.detail.FechaAltaMedica = null;
                }

                var anulado = this.mainForm.get('Anulado');
                if (anulado && anulado.value) {
                    this.mainForm.get('FechaAltaMedica').disable();
                }
            });

        this.mainForm.get('AltaLaboral').valueChanges.subscribe(
            (AltaLaboral: boolean) => {
                this.mainForm.get('FechaAltaLaboral').clearValidators();
                if (AltaLaboral) {
                    this.mainForm.get('FechaAltaLaboral').enable();
                    this.mainForm.get('FechaAltaLaboral').setValidators([Validators.required]);
                    this.mainForm.get('FechaAltaLaboral').updateValueAndValidity();
                }
                else {
                    this.mainForm.get('FechaAltaLaboral').disable();
                    this.detail.FechaAltaLaboral = null;
                }

                var anulado = this.mainForm.get('Anulado');
                if (anulado && anulado.value) {
                    this.mainForm.get('FechaAltaLaboral').disable();
                }

            });

        this.mainForm.get('FechaOcurrencia').valueChanges.subscribe(
            (FechaOcurrencia: Date) => {

                if (this.detail.BajaServicio && this.detail.BajaServicio == true) {
                    this.detail.FechaBajaServicio = FechaOcurrencia;
                }

            });

        var self = this;

        this.mainForm.get('EmpleadoId').valueChanges.subscribe(
            (EmpleadoId: ItemDto) => {
                if (EmpleadoId && EmpleadoId != null) {
                    self.detail.EmpleadoId = EmpleadoId.Id;
                    if (self.detail.SiniestroId && self.detail.SiniestroId != null) {
                        this.serviceSiniestro.getById(self.detail.SiniestroId)
                            .subscribe((t) => {

                                if (!(t.DataObject && t.DataObject.ConductorId == self.detail.EmpleadoId)) {
                                    self.detail.SiniestroId = null;
                                    this.cdr.detectChanges();
                                }

                            })
                    }
                }

            });
        this.mainForm.get('SiniestroId').valueChanges.subscribe(
            (SiniestroId: ItemDto) => {
                //if (SiniestroId && SiniestroId != null) {
                //    self.detail.SiniestroId = SiniestroId.Id;
                //}
                //else {
                //    self.detail.SiniestroId = null;
                //}
            });

        this.mainForm.get('BajaServicio').valueChanges.subscribe(
            (BajaServicio: boolean) => {
                this.mainForm.get('FechaBajaServicio').clearValidators();
                if (BajaServicio) {
                    this.mainForm.get('FechaBajaServicio').enable();
                    this.mainForm.get('FechaBajaServicio').setValidators([Validators.required]);
                    this.mainForm.get('FechaBajaServicio').updateValueAndValidity();
                }
                else {
                    this.mainForm.get('FechaBajaServicio').disable();
                    this.detail.FechaBajaServicio = null;
                }

                var anulado = this.mainForm.get('Anulado');
                if (anulado && anulado.value) {
                    this.mainForm.get('FechaBajaServicio').disable();
                }
                this.cdr.detectChanges();
            });

    }


    OnChangeEmpleado($event) {
        if ($event && $event != null) {
            this.detail.EmpleadoId = $event.Id;
            this.detail.ActualizarConductor = true;
            this.GetCurrentEmpleado();
            this.HistorialDenunciaPorPrestador();
            this.HistorialReclamosEmpleado();
            this.HistorialDenunciasEmpleado();
        }
        else {
            this.detail.EmpleadoId = null;
            this.detail.EmpleadoAntiguedad = null;
            this.detail.EmpleadoArea = null;
            this.detail.EmpleadoEmpresaId = null;
            this.detail.EmpleadoFechaIngreso = null;
            this.detail.EmpleadoLegajo = null;


            this.CurrentConductorEmpresa = null;
            this.CurrentEmpleado = null;
            this.CurrentEmpleadoLegajo = null;
            this.CurrentUnidadDeNegocio = null;
            this.historialDenunciasPrestador = null;
            this.historialReclamosEmpleado = null;
            this.historialDenunciasEmpleado = null;
        }
    }

    scrollToElement(): void {
        $('#porletDenuncia')[0].scrollTo(0, 0);
        this.tabSet.tabs.forEach(e => e.active = false);
        this.tabSet.tabs[0].active = true;
    }

    close(): void {
        if (this.datasourceNotificaciones && this.datasourceNotificaciones.data) {
            this.datasourceNotificaciones.data = null;
        }

        this.AnuladoEnable();


        this.CurrentConductorEmpresa = null;
        this.CurrentEmpleado = null;
        this.CurrentEmpleadoLegajo = null;
        this.CurrentUnidadDeNegocio = null;
        this.historialDenunciasPrestador = null;
        this.historialReclamosEmpleado = null;
        this.historialDenunciasEmpleado = null;


        this.scrollToElement();

        $('#fullscreentools')[0].click();

        if (this.mainForm) {

            let notificaciones = this.mainForm.get('Notificaciones') as FormArray;

            notificaciones.controls = [];

            this.form.resetForm();
        }

        $('body').removeClass("smallsize");

        super.close();
    }


    showNew(item: DenunciasDto) {

        this.mainForm.reset();

        this.viewMode = ViewMode.Add;
        this.showDto(item);

    }

    showDto(item: DenunciasDto) {
        super.showDto(item);
    }

    show(id: any) {

        this.service.getById(id).subscribe(result => {
            this.viewMode = ViewMode.Modify;
            var item = result.DataObject;
            this.showDto(item);
        });
    }



    AddDenunciaNotificacionEmpty(): void {
        if (!this.detail.DenunciaNotificaciones)
            this.detail.DenunciaNotificaciones = [];

        var obj: DenunciaNotificacionesDto = this.getNewDenunciaNotificacion(new DenunciaNotificacionesDto(), this.detail.DenunciaNotificaciones.length * -1);
        this.detail.DenunciaNotificaciones.push(obj);
        this.detail.DenunciaNotificaciones = [...this.detail.DenunciaNotificaciones];


        this.datasourceNotificaciones.data = this.detail.DenunciaNotificaciones;

        this.addNotificacion(obj);

        this.cdr.detectChanges();
    }

    addNotificacion(item: DenunciaNotificacionesDto): void {

        let notificaciones = this.mainForm.get('Notificaciones') as FormArray;

        notificaciones.push(this.detailFB.group({
            FechaNotificacion: [item.Fecha, Validators.required],
            ObservacionesNotificaciones: [item.Observaciones, null],
            MotivoNotificacion: [item.MotivoId, Validators.required],
        }));
    }

    addReingreso(item: ReingresosDto): void {

        let reingresos = this.mainForm.get('Reingresos') as FormArray;

        reingresos.push(this.detailFB.group({
            FechaReingreso: [item.FechaReingreso, Validators.required],
            AltaMedicaReingreso: [item.AltaMedica, null],
            FechaAltaMedicaReingreso: [item.FechaAltaMedica, null],
            AltaLaboralReingreso: [item.AltaLaboral, null],
            FechaAltaLaboralReingreso: [item.FechaAltaLaboral, null],
            FechaProbableAlta: [item.FechaProbableAlta, null],
            CantidadDias: [item.CantidadDias, null],
            Observacion: [item.Observacion, null],
        }));

        var val = reingresos.controls[reingresos.controls.length - 1];

        val.get('AltaMedicaReingreso').valueChanges.subscribe(
            (AltaMedicaReingreso: boolean) => {

                val.get('FechaAltaMedicaReingreso').clearValidators();
                if (AltaMedicaReingreso) {
                    val.get('FechaAltaMedicaReingreso').enable();
                    val.get('FechaAltaMedicaReingreso').setValidators([Validators.required]);
                }
                else {
                    val.get('FechaAltaMedicaReingreso').setValue(null);
                    val.get('FechaAltaMedicaReingreso').disable();
                }
                val.get('FechaAltaMedicaReingreso').updateValueAndValidity();
            });

        val.get('AltaLaboralReingreso').valueChanges.subscribe(
            (AltaLaboralReingreso: boolean) => {

                val.get('FechaAltaLaboralReingreso').clearValidators();
                if (AltaLaboralReingreso) {
                    val.get('FechaAltaLaboralReingreso').enable();
                    val.get('FechaAltaLaboralReingreso').setValidators([Validators.required]);
                }
                else {
                    val.get('FechaAltaLaboralReingreso').setValue(null);
                    val.get('FechaAltaLaboralReingreso').disable();
                }
                val.get('FechaAltaLaboralReingreso').updateValueAndValidity();
            });

    }



    onDeleteNotificacion(item: DenunciaNotificacionesDto): void {
        var index = this.detail.DenunciaNotificaciones.indexOf(item);

        this.detail.DenunciaNotificaciones.splice(index, 1);

        this.datasourceNotificaciones.data = this.detail.DenunciaNotificaciones;

        this.removeNotificacion(index);

    }

    //onDeleteReingreso(item: ReingresosDto): void {

    //    var index = this.detail.Reingresos.indexOf(item);

    //    this.detail.Reingresos.splice(index, 1);

    //    this.datasourceReingresos.data = this.detail.Reingresos;

    //    this.removeReingreso(index);

    //    this.CalcularTotalDias();

    //}

    removeNotificacion(index): void {

        let notificaciones = this.mainForm.get('Notificaciones') as FormArray;

        notificaciones.removeAt(index);
    }

    removeReingreso(index): void {

        let reingresos = this.mainForm.get('Reingresos') as FormArray;

        reingresos.removeAt(index);
    }


    getNewDenunciaNotificacion(item: DenunciaNotificacionesDto, id: number): DenunciaNotificacionesDto {
        item.Id = id;
        item.Observaciones = null;
        return item;
    }

    getNewMotivoNotificacion(item: MotivosNotificacionesDto, id: number): MotivosNotificacionesDto {
        item.Id = id;
        return item;
    }


    //AddReingresoEmpty(): void {
    //    if (!this.detail.Reingresos)
    //        this.detail.Reingresos = [];

    //    var obj: ReingresosDto = this.getNewReingreso(new ReingresosDto(), this.detail.Reingresos.length * -1);
    //    this.detail.Reingresos.push(obj);
    //    this.detail.Reingresos = [...this.detail.Reingresos];

    //    this.datasourceReingresos.data = this.detail.Reingresos;

    //    this.addReingreso(obj);

    //    this.CalcularTotalDias();

    //    this.cdr.detectChanges();
    //}
    //getNewReingreso(item: ReingresosDto, id: number): ReingresosDto {
    //    item.Id = id;
    //    item.Observacion = null;
    //    item.AltaLaboral = false;
    //    item.AltaMedica = false;
    //    item.CantidadDias = 0;
    //    return item;
    //}

    //private getName(control: AbstractControl): string | null {
    //    let group = <FormGroup>control.parent;

    //    if (!group) {
    //        return null;
    //    }

    //    let name: string;

    //    Object.keys(group.controls).forEach(key => {
    //        let childControl = group.get(key);

    //        if (childControl !== control) {
    //            return;
    //        }

    //        name = key;
    //    });

    //    return name;
    //}

    //alerta(formGroup: FormGroup) {
    //    (<any>Object).values(this.mainForm.controls).forEach(control => {

    //        if (control.status == 'INVALID') {
    //            window.alert(this.getName(control));
    //        }
    //    });

    //}


    save(): void {

        var self = this;

        if (this.mainForm && this.mainForm.invalid) {
            this.markFormGroupTouched(this.mainForm);

            //this.alerta(this.mainForm);
            return;
        }

        this.saving = true;

        this.completedataBeforeSave(this.detail);


        //validaciones
        var valida = this.Validates(this.detail);
        if (!valida) {
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
                this.closeOnSave = false;
                if (this.closeOnSave) {
                    this.close();
                };
                this.affterSave(this.detail);
                this.modalSave.emit(null);
            })
    }

    Validates(item: DenunciasDto): boolean {

        if (!(item.selectedEmpleado && item.selectedEmpleado != null)) {
            this.notificationService.warn("El Empleado es requerido");
            return false;
        }

        if (!(item.SucursalId && item.SucursalId != null)) {
            this.notificationService.warn("El empleado no tiene Unidad de Negocio");
            return false;
        }



        return true;
    }


    formatDate(date) {
        return moment(date).utc().format('DD/MM/YYYY HH:mm');
    }


}