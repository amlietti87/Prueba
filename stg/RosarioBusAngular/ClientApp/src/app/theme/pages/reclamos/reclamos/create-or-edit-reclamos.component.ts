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

    ChangeDetectorRef,
    Inject
} from '@angular/core';
import { DetailModalComponent, IDetailComponent, DetailEmbeddedComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatExpansionPanel, MatDatepickerInputEvent, MatTableDataSource, MatDialogRef, MatDialog, MAT_DIALOG_DATA } from '@angular/material';
import { ViewMode, ItemDto, ItemDtoStr } from '../../../../shared/model/base.model';
import { NgForm } from '@angular/forms';
import { FormBuilder, FormGroup, Validators, FormControl, FormArray } from '@angular/forms';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { DATE } from 'ngx-bootstrap/chronos/units/constants';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { TabsetComponent } from 'ngx-bootstrap';
import { ReclamosHistoricosService } from '../../siniestros/reclamos/reclamoshistoricos.service';
import { ReclamosDto, ReclamoCuotasDto, ReclamosHistoricosDto, ReclamosFilter } from '../../siniestros/model/reclamos.model';
import { ReclamosService } from '../../siniestros/reclamos/reclamos.service';
import { InvolucradosComboComponent } from '../../siniestros/shared/involucrado-combo.component';
import { EstadosComboComponent } from '../../siniestros/shared/estados-combo.component';
import { EmpresaComboComponent } from '../../planificacion/shared/empresa-combo.component';
import { SiniestroService } from '../../siniestros/siniestro/siniestro.service';
import { TipoReclamoComboComponent } from '../shared/tiporeclamo-combo.component';
import { EmpleadosService } from '../../siniestros/empleados/empleados.service';
import { LegajosEmpleadoService } from '../../siniestros/legajoempleado/legajosempleado.service';
import { EmpleadosDto, LegajosEmpleadoDto } from '../../siniestros/model/empleado.model';
import { EmpresaDto } from '../../planificacion/model/empresa.model';
import { SucursalDto } from '../../planificacion/model/sucursal.model';
import { SucursalService } from '../../planificacion/sucursal/sucursal.service';
import { EmpresaService } from '../../planificacion/empresa/empresa.service';
import { TiposReclamoService } from '../tiposreclamo/tiposreclamo.service';
import { ViewRef_ } from '@angular/core/src/view';
import { TiposReclamoBase } from '../model/tiposreclamo.model';
import { ESTADOS_RUTAS } from '../../../../shared/constants/constants';
import { AuthService } from '../../../../auth/auth.service';
import { InvolucradosService } from '../../siniestros/involucrados/involucrados.service';
import { DatePickerWithKeyboard } from '../../../../shared/components/datepickerWithKeyboard.component';

declare let mApp: any;

@Component({
    selector: 'reclamos-general',
    templateUrl: './create-or-edit-reclamos.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./create-or-edit-reclamos.component.css']
})
export class ReclamosGeneralComponent extends DetailAgregationComponent<ReclamosDto> implements OnInit, IDetailComponent, AfterViewInit {


    protected cfr: ComponentFactoryResolver;


    collapsedHeight: string = "35px";
    expandedHeight: string = "35px";
    expanded: boolean = true;
    events: string[] = [];
    @Input() viewMainTab: boolean = true;
    @Input() viewChildTab: boolean = true;
    public dialog: MatDialog;
    protected detailElement: IDetailComponent;
    reclamosForm: FormGroup;


    // Adjuntos
    GetAdjuntosReclamos: string;
    appDeleteFileById: string;
    appUploadFiles: string;



    //Relamo Cuotas
    displayedColumnsCuotas = ['FechaVencimiento', 'MontoCuotas', 'Concepto', 'Acciones'];
    datasourceCuotas: MatTableDataSource<ReclamoCuotasDto>;

    // ViewChilds
    @ViewChild('tabSet') tabSet: TabsetComponent;
    @ViewChild('InvolucradoCombo') InvolucradoCombo: InvolucradosComboComponent;
    @ViewChild('EstadosCombo') EstadosCombo: EstadosComboComponent;
    @ViewChild('EmpresaCombo') EmpresaCombo: EmpresaComboComponent;
    @ViewChild('TipoReclamoCombo') TipoReclamoCombo: TipoReclamoComboComponent;
    @ViewChild('tabContent', { read: ViewContainerRef })
    @ViewChild('FechaPagoControl') FechaPagoControl: DatePickerWithKeyboard;

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


    tabContent: ViewContainerRef;

    Involucrado: boolean = false;

    //EMPLEADO
    empleadoExiste: boolean = true;
    CurrentEmpleado: EmpleadosDto;
    CurrentEmpleadoLegajo: LegajosEmpleadoDto;
    CurrentConductorEmpresa: EmpresaDto;
    CurrentUnidadDeNegocio: SucursalDto;

    //PERMISOS
    allowAdd: boolean = false;
    allowModify: boolean = false;
    allowEliminar: boolean = false;
    allowVisualizar: boolean = false;
    allowAddEstados: boolean = false;
    allowAddAbogados: boolean = false;
    allowAddJuzgados: boolean = false;
    allowAddTipoAcuerdo: boolean = false;
    allowAddCausaReclamo: boolean = false;
    allowAddRubroSalarial: boolean = false;
    allowAnular: boolean = false;
    allowAdjunto: boolean = false;
    allowCambioEstado: boolean = false;


    sucursalUserId: number;


    SetAllowPermissionForSiniestro() {
        this.allowVisualizar = this.permission.isGranted('Siniestro.Reclamo.Visualizar');
        this.allowAdd = this.permission.isGranted('Siniestro.Reclamo.Agregar');
        this.allowEliminar = this.permission.isGranted('Siniestro.Reclamo.Eliminar');
        this.allowModify = this.permission.isGranted('Siniestro.Reclamo.Modificar');
        this.allowAnular = this.permission.isGranted('Siniestro.Reclamo.Anular');
        this.allowCambioEstado = this.permission.isGranted('Siniestro.Reclamo.CambiaEstado');
        this.allowAdjunto = this.permission.isGranted('Siniestro.Reclamo.Adjunto');
        this.SetAllowPermissionForAllowAdd();
    }

    SetAllowPermissionForGeneral() {
        this.allowVisualizar = this.permission.isGranted('Reclamo.Reclamo.Visualizar');
        this.allowAdd = this.permission.isGranted('Reclamo.Reclamo.Agregar');
        this.allowEliminar = this.permission.isGranted('Reclamo.Reclamo.Eliminar');
        this.allowModify = this.permission.isGranted('Reclamo.Reclamo.Modificar');
        this.allowAnular = this.permission.isGranted('Reclamo.Reclamo.Anular');
        this.allowCambioEstado = this.permission.isGranted('Reclamo.Reclamo.CambioEstado');
        this.allowAdjunto = this.permission.isGranted('Reclamo.Reclamo.Adjunto');
        this.SetAllowPermissionForAllowAdd();
    }

    SetAllowPermissionForAllowAdd() {
        this.allowAddEstados = this.permission.isGranted('Siniestro.Estado.Agregar');
        this.allowAddAbogados = this.permission.isGranted('Siniestro.Abogado.Agregar');
        this.allowAddJuzgados = this.permission.isGranted('Siniestro.Juzgado.Agregar');
        this.allowAddCausaReclamo = this.permission.isGranted('Reclamo.CausaReclamo.Agregar');
        this.allowAddTipoAcuerdo = this.permission.isGranted('Reclamo.TipoAcuerdo.Agregar');
        this.allowAddRubroSalarial = this.permission.isGranted('Reclamo.RubroSalarial.Agregar');
    }

    HideAllowAllHistoricoOrAnulado() {
        this.allowAddEstados = false;
        this.allowAddAbogados = false;
        this.allowAddJuzgados = false;
        this.allowAddCausaReclamo = false;
        this.allowAddTipoAcuerdo = false;
        this.allowAddRubroSalarial = false;
    }

    ngAfterViewInit(): void {
        mApp.initComponents();

        this.initFirtTab();
    }

    initFirtTab(): void {

        $('#m_heder_portlet_tab_Linea').click();

    }

    constructor(injector: Injector,
        protected service: ReclamosService,
        private serviceReclamosHistoricos: ReclamosHistoricosService,
        private siniestroService: SiniestroService,
        protected cdr: ChangeDetectorRef,
        private serviceEmpleado: EmpleadosService,
        private serviceSucursal: SucursalService,
        private serviceEmpresa: EmpresaService,
        private serviceLegajoEmpleado: LegajosEmpleadoService,
        private serviceTipoReclamo: TiposReclamoService,
        private serviceInvolucrados: InvolucradosService,
        private authService: AuthService,
        protected dialogRef: MatDialogRef<ReclamosGeneralComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ReclamosDto,
    ) {

        super(dialogRef, service, injector, data);
        this.detail = new ReclamosDto();
        this.icon = "fa fa-car"
        this.title = "Reclamo";
        var el = injector.get(ElementRef);
        var selector = el.nativeElement.tagName;

        this.cfr = injector.get(ComponentFactoryResolver);

        this.dialog = injector.get(MatDialog);

        this.GetAdjuntosReclamos = environment.siniestrosUrl + '/Reclamos/GetAdjuntos';
        this.appUploadFiles = environment.siniestrosUrl + '/Reclamos/UploadFiles/?ReclamoId=';
        this.appDeleteFileById = environment.siniestrosUrl + '/Reclamos/DeleteFileById';

        this.sucursalUserId = this.authService.GetUserData().sucursalId;


    }

    addEvent(type: string, event: MatDatepickerInputEvent<Date>) {
        this.events.push(`${type}: ${event.value}`);
    }

    public onDate(event): void {

    }


    detectChanges() {

        if (this.cdr !== null && this.cdr !== undefined && !(this.cdr as ViewRef_).destroyed) {
            this.cdr.detectChanges();
        }
    }

    completedataBeforeSave(item: ReclamosDto): any {


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

                                    this.serviceEmpresa.getById(this.CurrentEmpleadoLegajo.CodEmpresa)
                                        .subscribe((t) => {
                                            this.CurrentConductorEmpresa = t.DataObject;
                                        })
                                })

                        })

                }
                else {
                    this.empleadoExiste = false;
                }
            })
        }

    }

    EnableAll(): any {

        (<any>Object).values(this.reclamosForm.controls).forEach(control => {
            control.enable();
        });

    }


    AnularEnAnulado(): any {

        (<any>Object).values(this.reclamosForm.controls).forEach(control => {
            control.disable();
        });

        this.HideAllowAllHistoricoOrAnulado();

    }


    AnularEnModificar(): any {
        this.reclamosForm.get('TipoReclamoId').disable();
        this.reclamosForm.get('EstadoId').disable();
        if (this.Involucrado) {
            this.reclamosForm.get('SiniestroId').disable();
        }
        else {
            this.reclamosForm.get('SiniestroEmpleado').disable();
        }
        this.reclamosForm.get('InvolucradoId').disable();
        this.reclamosForm.get('EmpleadoId').disable();
    }

    AnularEnHistorico(): any {

        (<any>Object).values(this.reclamosForm.controls).forEach(control => {
            control.disable();
        });

        this.HideAllowAllHistoricoOrAnulado();
    }

    onView(row: ReclamosHistoricosDto) {

        row.SiniestroId = this.detail.Id;
        this.Opendialog(row);

    }

    Opendialog(_detail) {

        const dialogConfig = this.dialogRef._containerInstance._config;
        dialogConfig.data = this.CompleteDataBeforeShow(JSON.parse(JSON.stringify(_detail)) as ReclamosHistoricosDto);
        let dialogRef = this.dialog.open(ReclamosGeneralComponent, dialogConfig);
        this.detailElement = dialogRef.componentInstance;
        dialogRef.componentInstance.viewMode = ViewMode.Historico;
    }

    CompleteDataBeforeShow(_detail: ReclamosHistoricosDto): ReclamosHistoricosDto {

        return _detail;
    }


    completedataBeforeShow(item: ReclamosDto): any {
        var self = item.JudicialSelected;

        $('.custom-combo').on('show.bs.dropdown', function() {
            $('.dropdown-menu .show').addClass('smallsize');
        })

        if (item.AccessFromSiniestros) {
            this.SetAllowPermissionForSiniestro();
        }
        else {
            if (this.sucursalUserId != null) {
                this.detail.SucursalId = this.sucursalUserId;
                this.reclamosForm.get("SucursalId").disable();
            }
            this.SetAllowPermissionForGeneral();
        }

        this.reclamosForm.get('Anulado').disable();
        this.reclamosForm.get('MotivoAnulado').disable();

        this.datasourceCuotas = new MatTableDataSource(item.ReclamoCuotas);
        if (item.ReclamoCuotas) {
            item.ReclamoCuotas.forEach(e => this.addCuotas(e));
        }

        if (this.viewMode == ViewMode.Modify) {
            var f = {
                ReclamoId: item.Id
            };

            this.datasourceCuotas = new MatTableDataSource(item.ReclamoCuotas);

            if (item.ReclamoCuotas) {
                item.ReclamoCuotas.forEach(e => this.addCuotas(e));
            }

            if (item.Anulado) {
                this.AnularEnAnulado();
            }
            else {
                this.AnularEnModificar();
            }

            this.serviceReclamosHistoricos.requestAllByFilter(f).subscribe(x => {
                item.itemsHistorial = [];
                item.itemsHistorial = x.DataObject.Items;
                if (item.TipoReclamoId == TiposReclamoBase.Siniestro && item.EnableADet == true && this.permission.isGranted('Reclamo.Involucrado.EditADeterminar') && item.itemsHistorial.length == 0) {
                    this.reclamosForm.get('InvolucradoId').enable();
                    this.reclamosForm.get('InvolucradoId').updateValueAndValidity();
                    this.detectChanges();
                }

            });




            this.reclamosForm.get('SucursalId').disable();
            this.reclamosForm.get('EmpresaId').disable();

        }
        else if (this.viewMode == ViewMode.Add) {

            item.Fecha = new Date();
            if (this.detail.TipoReclamoId == TiposReclamoBase.Siniestro) {
                this.reclamosForm.get('TipoReclamoId').disable();
                this.reclamosForm.get('SiniestroId').disable();
            }
        }
        else if (this.viewMode == ViewMode.Historico) {
            this.AnularEnHistorico();
        }



        item.JudicialSelected = self;
        this.detectChanges();

    }

    addCuotas(item: ReclamoCuotasDto): void {

        let cuotas = this.reclamosForm.get('Cuotas') as FormArray;

        cuotas.push(this.detailFB.group({
            FechaVencimiento: [item.FechaVencimiento, null],
            MontoCuotas: [item.Monto, null],
            Concepto: [item.Concepto, null],
        }));
    }

    addNewReclamoCuotas(): void {
        if (!this.detail.ReclamoCuotas) {
            this.detail.ReclamoCuotas = [];
        }

        var obj: ReclamoCuotasDto = this.getNewReclamoCuota(new ReclamoCuotasDto(), this.detail.ReclamoCuotas.length * -1);
        this.detail.ReclamoCuotas.push(obj);
        this.detail.ReclamoCuotas = [...this.detail.ReclamoCuotas];


        this.datasourceCuotas.data = this.detail.ReclamoCuotas;

        this.addCuotas(obj);

        this.detectChanges();
    }

    getNewReclamoCuota(item: ReclamoCuotasDto, id: number): ReclamoCuotasDto {
        var item = new ReclamoCuotasDto(item);
        item.ReclamoId = this.detail.Id;
        item.Id = id;
        item.FechaVencimiento = null;
        item.Monto = null;
        item.Concepto = null;
        return item;

    }

    onDeleteCuotas(item: ReclamoCuotasDto): void {
        var index = this.detail.ReclamoCuotas.indexOf(item);

        this.detail.ReclamoCuotas.splice(index, 1);

        this.datasourceCuotas.data = this.detail.ReclamoCuotas;

        this.removeCuotas(index);

    }

    removeCuotas(index): void {

        let cuotas = this.reclamosForm.get('Cuotas') as FormArray;

        cuotas.removeAt(index);
    }

    createForm() {

        if (!this.detail) {
            this.detail = this.createDefaultDetail();
        }

        this.reclamosForm = this.detailFB.group({
            TipoReclamoId: [this.detail.TipoReclamoId, Validators.required],
            InvolucradoId: [this.detail.InvolucradoId, Validators.required],
            EstadoId: [this.detail.EstadoId, Validators.required],
            SubEstadoId: [this.detail.SubEstadoId, Validators.required],
            Fecha: [this.detail.Fecha, Validators.required],
            EmpresaId: [this.detail.EmpresaId, Validators.required],
            SucursalId: [this.detail.SucursalId, Validators.required],
            SiniestroId: [this.detail.selectedSiniestro, null],
            SiniestroEmpleado: [this.detail.SiniestroId, null],
            EmpleadoId: [this.detail.selectedEmpleado, null],
            CausaReclamoId: [this.detail.CausaId, null],
            TipoAcuerdoId: [this.detail.TipoAcuerdoId, null],
            RubroSalarialId: [this.detail.RubroSalarialId, null],
            DenunciaId: [this.detail.DenunciaId, null],
            MontoDemandado: [this.detail.MontoDemandado, null],
            Hechos: [this.detail.Hechos, null],
            FechaOfrecimiento: [this.detail.FechaOfrecimiento, null],
            MontoOfrecido: [this.detail.MontoOfrecido, null],
            MontoReconsideracion: [this.detail.MontoReconsideracion, null],
            FechaPago: [this.detail.FechaPago, null],
            MontoPagado: [this.detail.MontoPagado, null],
            MotivoAnulado: [this.detail.MotivoAnulado, null],
            CuotasCheck: [this.detail.Cuotas, null],
            MontoFranquicia: [this.detail.MontoFranquicia, null],
            AbogadoId: [this.detail.AbogadoId, null],
            MontoHonorariosAbogado: [this.detail.MontoHonorariosAbogado, null],
            MontoHonorariosMediador: [this.detail.MontoHonorariosMediador, null],
            MontoHonorariosPerito: [this.detail.MontoHonorariosPerito, null],
            MontoTasasJudiciales: [this.detail.MontoTasasJudiciales, null],
            JuntaMedica: [this.detail.JuntaMedica, null],
            PorcentajeIncapacidad: [this.detail.PorcentajeIncapacidad, Validators.max(100)],
            Observaciones: [this.detail.Observaciones, null],
            ObsConvenioExtrajudicial: [this.detail.ObsConvenioExtrajudicial, null],
            Autos: [this.detail.Autos, null],
            NroExpediente: [this.detail.NroExpediente, null],
            JuzgadoId: [this.detail.JuzgadoId, null],
            AbogadoEmpresaId: [this.detail.AbogadoEmpresaId, null],
            Anulado: [this.detail.Anulado, null],
            Cuotas: this.detailFB.array([]),
        });

        this.reclamosForm.get('Anulado').disable();
        this.reclamosForm.get('MotivoAnulado').disable();
        this.formControlValueChanged();
    }

    formControlValueChanged() {

        this.reclamosForm.get('CuotasCheck').valueChanges.subscribe(
            (CuotasCheck: boolean) => {

                if (!CuotasCheck) {
                    this.detail.Cuotas = null;
                }

            });

        this.reclamosForm.get('TipoReclamoId').valueChanges.subscribe(
            (TipoReclamoId: number) => {

                if (TipoReclamoId && TipoReclamoId != null) {
                    this.serviceTipoReclamo.getById(TipoReclamoId).subscribe(e => {

                        if (e.DataObject) {

                            var tiporec = e.DataObject;

                            this.reclamosForm.get('EmpleadoId').clearValidators();
                            this.reclamosForm.get('SiniestroId').clearValidators();
                            this.reclamosForm.get('InvolucradoId').clearValidators();

                            this.reclamosForm.get('SiniestroId').setValue(this.detail.selectedSiniestro);

                            this.reclamosForm.get('DenunciaId').setValue(this.detail.DenunciaId);
                            this.reclamosForm.get('EmpleadoId').setValue(this.detail.selectedEmpleado);

                            if (tiporec.Involucrado) {

                                this.Involucrado = true;

                                this.reclamosForm.get('EmpleadoId').setValidators(null);

                                this.reclamosForm.get('SiniestroId').setValidators([Validators.required]);
                                this.reclamosForm.get('InvolucradoId').setValidators([Validators.required]);

                                this.detail.EmpleadoId = null;
                                this.detail.selectedEmpleado = null;

                                this.reclamosForm.get('SucursalId').disable();
                                this.reclamosForm.get('EmpresaId').disable();

                            }
                            else {
                                this.Involucrado = false;

                                this.reclamosForm.get('EmpleadoId').setValidators([Validators.required]);
                                this.reclamosForm.get('SiniestroId').setValidators(null);
                                this.reclamosForm.get('InvolucradoId').setValidators(null);

                                if (this.detail.selectedSiniestro && this.detail.selectedSiniestro != null) {
                                    this.detail.selectedSiniestro = null;
                                }

                                if (this.viewMode != ViewMode.Modify && this.viewMode != ViewMode.Historico) {
                                    this.reclamosForm.get('SucursalId').enable();
                                    this.reclamosForm.get('EmpresaId').enable();
                                }

                                this.detail.InvolucradoId = null;
                            }

                            this.reclamosForm.get('EmpleadoId').updateValueAndValidity();
                            this.reclamosForm.get('SiniestroId').updateValueAndValidity();
                            this.reclamosForm.get('SiniestroEmpleado').updateValueAndValidity();
                            this.reclamosForm.get('InvolucradoId').updateValueAndValidity();

                            this.detectChanges();
                        }
                    });

                }

            });

        this.reclamosForm.get('EstadoId').valueChanges.subscribe(
            (EstadoId: number) => {

                if (EstadoId && this.EstadosCombo && this.EstadosCombo.items.length > 0) {

                    var busqueda = this.EstadosCombo.items.find(en => en.Id == EstadoId);
                    if (busqueda && busqueda != null) {
                        this.detail.JudicialSelected = busqueda.Judicial;
                    }
                    else {
                        this.detail.JudicialSelected = false;
                    }
                    this.detectChanges();
                }

            });

        this.reclamosForm.get('EmpleadoId').valueChanges.subscribe(
            (EmpleadoId: ItemDto) => {

                if (EmpleadoId && EmpleadoId != null) {
                    this.detail.EmpleadoId = EmpleadoId.Id;
                    if (!this.enableonclose) {
                        this.GetCurrentEmpleado();
                    }
                }
                else {
                    this.detail.EmpleadoId = null;
                }

            });

        this.reclamosForm.get('SiniestroId').valueChanges.subscribe(
            (SiniestroId: ItemDto) => {

                if (!(!this.Involucrado && this.detail.SiniestroId && this.detail.SiniestroId != null && SiniestroId == null)) {
                    if (SiniestroId && SiniestroId != null) {
                        this.detail.SiniestroId = SiniestroId.Id;
                    }
                    else if (this.detail.SiniestroId) {
                        this.detail.SiniestroId = null;
                    }
                    this.detectChanges();
                }
            });


        this.reclamosForm.get('InvolucradoId').valueChanges.subscribe(
            (InvolucradoId: number) => {
                if (InvolucradoId && InvolucradoId != null) {
                    this.siniestroService.getById(this.detail.SiniestroId).subscribe(e => {
                        if (e.DataObject) {
                            this.detail.EmpresaId = e.DataObject.EmpresaId;
                            this.detail.SucursalId = e.DataObject.SucursalId;
                        }
                    });
                }

            });
    }

    OnChangeEmpleado($event) {
        if ($event && $event != null) {
            this.detail.ActualizarConductor = true;
        }
    }

    InvolucradoChange(value: any): void {

        if (value && !(this.detail.Id && this.detail.Id != null && this.detail.Id != 0)) {
            var f = {};
            if (this.detail.EstadoId != null) {
                var estado = this.EstadosCombo.items.filter(e => e.Id == this.detail.EstadoId)[0];
                if (!estado.OrdenCambioEstado || estado.OrdenCambioEstado == null) {
                    f = {
                        InvolucradoId: value
                    }
                }
                else {
                    f = {
                        InvolucradoId: value,
                        EstadoId: this.detail.EstadoId
                    }
                }
            }
            else {
                f = {
                    InvolucradoId: value,
                    EstadoId: this.detail.EstadoId
                }
            }

            var self = this;
            this.service.requestAllByFilter(f)
                .subscribe((t) => {
                    if (t.DataObject) {
                        debugger;
                        if (t.DataObject.Items.length > 0) {
                            if (this.detail.EstadoId && this.detail.EstadoId != null
                                && this.EstadosCombo.items.filter(e => e.Id == this.detail.EstadoId)[0].OrdenCambioEstado
                                && this.EstadosCombo.items.filter(e => e.Id == this.detail.EstadoId)[0].OrdenCambioEstado != null
                            ) {
                                self.message.error('Este involucrado ya tiene un reclamo/recupero asociado con el mismo estado. No se puede seleccionar', 'Error');
                                this.reclamosForm.get('InvolucradoId').setValue(null);
                            }
                            else {
                                self.message.confirm('Este involucrado ya tiene un reclamo/recupero asociado a este siniestro. Desea continuar?', 'Confirmaci&oacute;n', (a) => {
                                    if (!(a.value)) {
                                        this.reclamosForm.get('InvolucradoId').setValue(null);
                                    }
                                });
                            }
                            this.reclamosForm.get('InvolucradoId').updateValueAndValidity();
                        }
                        else {
                            if (this.detail.EstadoId && this.detail.EstadoId != null) {
                                this.CheckNuevoReclamoNoNecesario();
                            }
                        }
                    }
                })
        }

    }

    OnEstadoChange(value: any) {

        if (value && this.viewMode == ViewMode.Add) {
            this.reclamosForm.get('EstadoId').disable();
            this.cdr.detectChanges();
            this.allowAddEstados = false;
            this.reclamosForm.get('SubEstadoId').setValue(null);
        }
        if (value && this.detail.InvolucradoId && value != null && this.detail.InvolucradoId != null && !(this.detail.Id && this.detail.Id != null && this.detail.Id != 0)) {
            var f = {
                InvolucradoId: this.detail.InvolucradoId,
                EstadoId: value
            }
            var self = this;
            this.service.requestAllByFilter(f)
                .subscribe((t) => {
                    if (t.DataObject) {
                        if (t.DataObject.Items.length > 0) {
                            self.message.error('Este involucrado ya tiene un reclamo/recupero asociado con el mismo estado. No se puede seleccionar', 'Error');
                            this.reclamosForm.get('EstadoId').setValue(null);
                            this.reclamosForm.get('EstadoId').updateValueAndValidity();
                        }
                        else {
                            this.CheckNuevoReclamoNoNecesario();
                        }
                    }
                    else {
                        this.reclamosForm.get('EstadoId').disable();
                        this.cdr.detectChanges();
                        this.allowAddEstados = false;
                    }
                })
        }
    }

    validateEstadoEInvolucrado: boolean = true;

    CheckNuevoReclamoNoNecesario() {
        var f = {
            InvolucradoId: this.detail.InvolucradoId,
            EstadoId: this.detail.EstadoId
        }
        var self = this;
        this.service.CheckNuevoReclamoNoNecesario(f)
            .subscribe((t) => {

                if (t == false) {
                    self.message.error('Debe cambiar estado porque solo se pueden insertar reclamos en recupero para este involucrado.', 'Error');
                    this.reclamosForm.get('InvolucradoId').setValue(null);
                    this.reclamosForm.get('InvolucradoId').updateValueAndValidity();
                    this.reclamosForm.get('EstadoId').setValue(null);
                    this.reclamosForm.get('EstadoId').updateValueAndValidity();
                    this.validateEstadoEInvolucrado = false;
                }
                else {

                    var estado = this.EstadosCombo.items.find(e => e.Id == this.detail.EstadoId);
                    if (estado.OrdenCambioEstado == null) {

                        var filterreclamo = new ReclamosFilter();
                        filterreclamo.InvolucradoId = this.detail.InvolucradoId;

                        this.service.requestAllByFilter(filterreclamo).subscribe(f => {

                            if (f.DataObject && f.DataObject.Items && f.DataObject.Items.length > 0) {
                                self.message.confirm('Este involucrado ya tiene un reclamo/recupero asociado a este siniestro. Desea continuar?', 'Confirmaci&oacute;n', (a) => {
                                    if (!(a.value)) {
                                        this.reclamosForm.get('InvolucradoId').setValue(null);
                                    }
                                    else {
                                        this.reclamosForm.get('EstadoId').disable();
                                        this.cdr.detectChanges();
                                        this.allowAddEstados = false;
                                    }
                                });
                            }
                            else {
                                this.reclamosForm.get('EstadoId').disable();
                                this.cdr.detectChanges();
                                this.allowAddEstados = false;
                            }

                        });

                    }
                    this.validateEstadoEInvolucrado = true;
                }
            });
    }

    ngAfterViewChecked(): void {

        $('tabset ul.nav').addClass('m-tabs-line');
        $('tabset ul.nav li a.nav-link').addClass('m-tabs__link');

    }

    createDefaultDetail(): any {

        var item = new ReclamosDto();
        item.Anulado = false;
        return item;
    }



    scrollToElement(): void {


        this.tabSet.tabs.forEach(e => e.active = false);
        this.tabSet.tabs[0].active = true;

    }

    enableonclose: boolean = false;

    close(): void {

        if (this.datasourceCuotas && this.datasourceCuotas.data) {
            this.datasourceCuotas.data = null;
        }

        this.scrollToElement();


        this.CurrentConductorEmpresa = null;
        this.CurrentEmpleado = null;
        this.CurrentEmpleadoLegajo = null;
        this.CurrentUnidadDeNegocio = null;

        this.active = false;
        this.viewMode = ViewMode.Undefined;
        this.modalClose.emit(true);
        this.dialogRef.close(false);

        if (this.reclamosForm) {
            this.enableonclose = true;

            this.EnableAll();

            let cuotas = this.reclamosForm.get('Cuotas') as FormArray;
            cuotas.controls = [];


            this.reclamosForm.reset();
        }

        $('#scrollInv').removeClass("smallsize");
        $('.dropdown-menu .show').removeClass('smallsize');
    }



    showNew(item: ReclamosDto) {


        if (this.reclamosForm) {
            this.reclamosForm.reset();
        }

        this.viewMode = ViewMode.Add;
        this.showDto(item);
        this.detectChanges();
    }

    formatDate(date) {

        return moment(date).utc().format('DD/MM/YYYY HH:mm');

    }

    save(): void {

        var self = this;

        if (this.reclamosForm && this.reclamosForm.invalid) {
            this.markFormGroupTouched(this.reclamosForm);
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

        if (!this.validateEstadoEInvolucrado) {
            self.message.error('Debe cambiar estado. No puede insertar un nuevo reclamo.');
            return;
        }

        this.service.createOrUpdate(this.detail, this.viewMode)
            .finally(() => { this.saving = false; })
            .subscribe((t) => {

                if (this.viewMode = ViewMode.Add) {
                    this.detail.Id = t.DataObject;
                }

                this.notify.info('Guardado exitosamente');
                this.closeOnSave = true;
                if (this.closeOnSave) {
                    this.close();
                };
                this.affterSave(this.detail);
                this.modalSave.emit(null);
            })
    }


    Validates(item: ReclamosDto): boolean {

        var cuotas = this.ValidateReclamosCuotas(item);

        if (!cuotas) {
            return false;
        }

        if (!this.FechaPagoControl.validateFechaPago) {
            this.notificationService.error("No puede guardar el reclamo porque ingresó una Fecha de Pago incorrecta", "Error");
            return false;
        }

        return true;
    }

    ValidateReclamosCuotas(reclamo: ReclamosDto): boolean {
        if (!reclamo.Cuotas) {
            reclamo.ReclamoCuotas = null;
            return true;
        }
        else {
            for (var i = 0; i < reclamo.ReclamoCuotas.length; i++) {
                var reclamoActual = reclamo.ReclamoCuotas[i];
                if (!((reclamoActual.Concepto != null && reclamoActual.Concepto != "") || reclamoActual.Monto != null || reclamoActual.FechaVencimiento != null)) {
                    var x = i++;
                    this.notificationService.warn("La cuota n&uacute;mero: " + x + " es inv&aacute;lida");
                    return false;
                }
                if (reclamoActual.Monto == null) {
                    reclamoActual.Monto = 0;
                }
                reclamo.ReclamoCuotas[i] = reclamoActual;
            }
            return true;
        }
    }

}