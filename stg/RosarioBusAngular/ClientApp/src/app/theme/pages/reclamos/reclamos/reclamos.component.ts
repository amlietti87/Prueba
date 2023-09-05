import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, SimpleChanges, Input, ChangeDetectorRef } from '@angular/core';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { Subscription } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { moment } from 'ngx-bootstrap/chronos/test/chain';
import { MatDatepickerInputEvent, MatDatepicker, MatDialogConfig, MatDialog } from '@angular/material';
import { getDate, isFirstDayOfWeek } from 'ngx-bootstrap/chronos/utils/date-getters';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { environment } from '../../../../../environments/environment';

import { ReclamosHistoricosService } from '../../siniestros/reclamos/reclamoshistoricos.service';
import { Paginator, DataTable } from 'primeng/primeng';
import { ReclamosGeneralComponent } from './create-or-edit-reclamos.component';
import { ViewMode, ItemDto } from '../../../../shared/model/base.model';
import { AdjuntoComponent } from '../../siniestros/siniestro/adjunto/adjunto.component';
import { ReclamosDto, ReclamosFilter, ReclamosHistoricosDto } from '../../siniestros/model/reclamos.model';
import { ReclamosService } from '../../siniestros/reclamos/reclamos.service';
import { SiniestrosDto } from '../../siniestros/model/siniestro.model';
import { TiposReclamoBase } from '../model/tiposreclamo.model';
import { AnularReclamoModalComponent } from '../anular/anular-modal.component';
import { EstadosReclamosModalComponent } from '../cambioestado/estados-modal.component';
import { SucursalService } from '../../planificacion/sucursal/sucursal.service';
import { SucursalDto } from '../../planificacion/model/sucursal.model';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';
import { stringify } from '@angular/compiler/src/util';
import { filter } from 'rxjs/operator/filter';
import { AuthService } from '../../../../auth/auth.service';
import { ReclamosImportadorComponent, ReclamoImportadorDTO } from './reclamos-importador/reclamos-importador.component';
import { SubEstadosDto } from '../../siniestros/model/estados.model';
import { SubEstadosService } from '../../siniestros/estados/subestados.service';

@Component({
    selector: 'reclamos-component',
    templateUrl: "./reclamos.component.html",
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./reclamos.component.css']
})
export class ReclamosComponent extends CrudComponent<ReclamosDto> implements OnInit, AfterViewInit {
    sub: Subscription;
    subQ: Subscription;
    customdetail: ReclamosGeneralComponent;
    filter: ReclamosFilter;

    //MultiSelect Sub-Estados
    listSubEstados: SubEstadosDto[] = [];
    selectedSubEstados: Array<SubEstadosDto> = [];

    //material dialog
    protected popupHeight: string = '';
    protected popupWidth: string = '80%';
    protected dialog: MatDialog;

    @Input() siniestro: boolean = false;

    _detailSiniestro: SiniestrosDto;
    @Input()
    get detailSiniestro(): SiniestrosDto {
        return this._detailSiniestro;
    }

    set detailSiniestro(value: SiniestrosDto) {
        this._detailSiniestro = value;
        if (value && value != null && this.filter && this.allowVisualizar) {
            this.filter.SiniestroId = value.Id;
            this.Search();
        }
    }

    //ViewChilds
    @ViewChild('ComboAnulado') ComboAnulado: YesNoAllComboComponent;

    constructor(protected serviceReclamos: ReclamosService,
        protected serviceReclamosHistoricos: ReclamosHistoricosService,
        private _activatedRoute: ActivatedRoute,
        injector: Injector,
        private authService: AuthService,
        private SubEstadoService: SubEstadosService,
        protected cdr: ChangeDetectorRef) {

        super(serviceReclamos, ReclamosGeneralComponent, injector);
        this.dialog = injector.get(MatDialog);
        this.isFirstTime = false;
        this.icon = "fa fa-car"
        this.title = "Reclamos";
        this.advancedFiltersAreShown = true;
        this.SetAllowPermission();
        this.loadInMaterialPopup = true;
        this.CargarSubEstados();
    }

    CargarSubEstados(): any {

        this.SubEstadoService.requestAllByFilter({ Anulado: false }).subscribe(result => {
            this.listSubEstados = result.DataObject.Items;
            this.listSubEstados.forEach((e) => { e.Descripcion = e.EstadoNombre + " " + e.Descripcion; });

        });
    }

    //RECLAMOS
    allowAdd: boolean = false;
    allowModify: boolean = false;
    allowEliminar: boolean = false;
    allowVisualizar: boolean = false;
    allowAnular: boolean = false;
    allowAdjunto: boolean = false;
    allowCambioEstado: boolean = false;
    allowExportar: boolean;
    sucursalUserId: number;
    allowImportar: boolean;
    SetAllowPermission() {

        if (this.siniestro) {
            this.allowVisualizar = this.permission.isGranted('Siniestro.Reclamo.Visualizar');
            this.allowAdd = this.permission.isGranted('Siniestro.Reclamo.Agregar');
            this.allowEliminar = this.permission.isGranted('Siniestro.Reclamo.Eliminar');
            this.allowModify = this.permission.isGranted('Siniestro.Reclamo.Modificar');
            this.allowAnular = this.permission.isGranted('Siniestro.Reclamo.Anular');
            this.allowCambioEstado = this.permission.isGranted('Siniestro.Reclamo.CambiaEstado');
            this.allowAdjunto = this.permission.isGranted('Siniestro.Reclamo.Adjunto');
        }
        else {
            this.filter.SucursalId = this.sucursalUserId;
            this.allowVisualizar = this.permission.isGranted('Reclamo.Reclamo.Visualizar');
            this.allowAdd = this.permission.isGranted('Reclamo.Reclamo.Agregar');
            this.allowEliminar = this.permission.isGranted('Reclamo.Reclamo.Eliminar');
            this.allowModify = this.permission.isGranted('Reclamo.Reclamo.Modificar');
            this.allowAnular = this.permission.isGranted('Reclamo.Reclamo.Anular');
            this.allowCambioEstado = this.permission.isGranted('Reclamo.Reclamo.CambioEstado');
            this.allowAdjunto = this.permission.isGranted('Reclamo.Reclamo.Adjunto');
            this.allowExportar = this.permission.isGranted('Reclamo.Reclamo.Exportar');
            this.allowImportar = this.permission.isGranted('Reclamo.Reclamo.Importar');
        }
    }

    ngOnInit() {
        this.sucursalUserId = this.authService.GetUserData().sucursalId;

        super.ngOnInit();
        $('body').addClass("m-brand--minimize m-aside-left--minimize");
        this.sub = this._activatedRoute.params.subscribe(params => {
            this.paramsSubscribe(params);
        }
        );

        this.subQ = this._activatedRoute.queryParams.subscribe(params => {
            this.paramsSubscribe(params);
        });
    }

    ngOnDestroy() {
        super.ngOnDestroy();

        if (this.sub) {
            this.sub.unsubscribe();
            this.subQ.unsubscribe();
        }
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
        this.ComboAnulado.writeValue(this.filter.AnuladoCombo);
        this.ComboAnulado.refresh();
        this.cdr.detectChanges();
    }

    Search(event?: LazyLoadEventData) {
        if ((this.siniestro && this.filter && this.filter.SiniestroId && this.filter.SiniestroId != null) || this.siniestro == false) {
            super.Search(event);
        }
    }

    GetReporteExcel(): void {
        this.serviceReclamos.GetReporteExcel(this.filter);

    }

    onCreate() {
        super.onCreate();
        $('#scrollInv').addClass("smallsize");
    }

    paramsSubscribe(params: any) {
        this.breadcrumbsService.defaultBreadcrumbs(this.title);
        this.active = true;

        if (params.id) {
            this.onEditID(params.id);
        }
    }

    CloseChild(): void {
        var e = super.GetEditComponent();
        (e as ReclamosGeneralComponent).CloseChild();
        (e as ReclamosGeneralComponent).close();
    }



    getNewfilter(): ReclamosFilter {
        var f = new ReclamosFilter();
        f.AnuladoCombo = 2;
        return f;
    }

    completeFilter(filter: ReclamosFilter) {

        filter.SubEstadoReclamo = [];

        if (this.selectedSubEstados && this.selectedSubEstados.length >= 1) {
            this.selectedSubEstados.forEach(e => {
                filter.SubEstadoReclamo.push(e.Id);
            });
        }
    }


    GetEditComponentType(): Type<IDetailComponent> {
        return ReclamosGeneralComponent;
    }

    onAnularShow(item: ReclamosDto) {
        const dialogConfig = new MatDialogConfig<ReclamosDto>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        //this.CompleteBeforeChangeState(JSON.parse(JSON.stringify(item)) as DenunciasDto)

        dialogConfig.data = item;

        let dialogRef = this.dialog.open(AnularReclamoModalComponent, dialogConfig);

        this.detailElement = dialogRef.componentInstance;

        dialogRef.afterClosed().subscribe(
            data => {
                this.active = true;
            }
        );

    }


    onAdjuntarArchivosShow(item: ReclamosDto) {
        const dialogConfig = new MatDialogConfig<ReclamosDto>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        //dialogConfig.id = "1";
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        //dialogConfig.data = this.CompleteDataBeforeShow(JSON.parse(JSON.stringify(_detail)) as T);

        let dialogRef = this.dialog.open(AdjuntoComponent, dialogConfig);

        dialogRef.componentInstance.appGetAllFileByParent = environment.siniestrosUrl + '/Reclamos/GetAdjuntos';
        dialogRef.componentInstance.appUploadUrl = environment.siniestrosUrl + '/Reclamos/UploadFiles/?ReclamoId=' + item.Id;
        dialogRef.componentInstance.appRemoveFileUrl = environment.siniestrosUrl + '/Reclamos/DeleteFileById';
        dialogRef.componentInstance.allowClose = true;
        dialogRef.componentInstance.Parent = item.Id;
        dialogRef.componentInstance.AllowUploadFiles = this.allowAdjunto;
    }

    onEstadosShow(item: ReclamosDto) {
        const dialogConfig = new MatDialogConfig<ReclamosHistoricosDto>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;




        dialogConfig.data = this.CompleteBeforeChangeState(JSON.parse(JSON.stringify(item)) as ReclamosDto);

        let dialogRef = this.dialog.open(EstadosReclamosModalComponent, dialogConfig);

        this.detailElement = dialogRef.componentInstance;

        dialogRef.componentInstance.viewMode = ViewMode.Modify;

        dialogRef.afterClosed().subscribe(

            data => {

                this.active = true;

                if (data) {
                    this.Search();
                }
            }
        );

    }

    CompleteBeforeChangeState(_detail: ReclamosDto): ReclamosHistoricosDto {
        var item = new ReclamosHistoricosDto();
        item.Fecha = _detail.Fecha;
        item.InvolucradoId = _detail.InvolucradoId;
        item.ReclamoId = _detail.Id;
        item.EstadoId = null;
        item.SubEstadoId = null;
        item.AbogadoEmpresaId = _detail.AbogadoEmpresaId;
        item.AbogadoId = _detail.AbogadoId;
        item.Cuotas = _detail.Cuotas;
        item.FechaOfrecimiento = _detail.FechaOfrecimiento;
        item.FechaPago = _detail.FechaPago;
        item.JuntaMedica = _detail.JuntaMedica;
        item.JuzgadoId = _detail.JuzgadoId;
        item.MontoDemandado = _detail.MontoDemandado;
        item.MontoFranquicia = _detail.MontoFranquicia;
        item.MontoHonorariosAbogado = _detail.MontoHonorariosAbogado;
        item.MontoHonorariosMediador = _detail.MontoHonorariosMediador;
        item.MontoHonorariosPerito = _detail.MontoHonorariosPerito;
        item.MontoOfrecido = _detail.MontoOfrecido;
        item.MontoPagado = _detail.MontoPagado;
        item.MontoReconsideracion = _detail.MontoReconsideracion;
        item.NroExpediente = _detail.NroExpediente;
        item.ObsConvenioExtrajudicial = _detail.ObsConvenioExtrajudicial;
        item.Observaciones = _detail.Observaciones;
        item.PorcentajeIncapacidad = _detail.PorcentajeIncapacidad;
        item.Autos = _detail.Autos;

        //props nuevas
        item.EmpleadoAntiguedad = _detail.EmpleadoAntiguedad;
        item.EmpleadoArea = _detail.EmpleadoArea;
        item.EmpleadoEmpresaId = _detail.EmpleadoEmpresaId;
        item.EmpleadoFechaIngreso = _detail.EmpleadoFechaIngreso;
        item.EmpleadoId = _detail.EmpleadoId;
        item.EmpleadoLegajo = _detail.EmpleadoLegajo;
        item.EmpresaId = _detail.EmpresaId;
        item.DenunciaId = _detail.DenunciaId;
        item.TipoReclamoId = _detail.TipoReclamoId;
        item.SiniestroId = _detail.SiniestroId;
        item.Hechos = _detail.Hechos;
        item.TipoAcuerdoId = _detail.TipoAcuerdoId;
        item.RubroSalarialId = _detail.RubroSalarialId;
        item.SucursalId = _detail.SucursalId;
        item.EmpresaId = _detail.EmpresaId;
        item.CausaId = _detail.CausaId;
        item.MontoTasasJudiciales = _detail.MontoTasasJudiciales;
        return item;
    }

    getNewItem(): ReclamosDto {
        var item = new ReclamosDto();
        item.AccessFromSiniestros = this.siniestro;
        if (this.siniestro) {

            item.SiniestroId = this.detailSiniestro.Id;
            item.selectedSiniestro = new ItemDto();
            item.selectedSiniestro.Id = item.SiniestroId;
            item.selectedSiniestro.Description = this.detailSiniestro.NroSiniestro + " - " + this.detailSiniestro.Sucursal.Description.trim() + " - " + new Date(this.detailSiniestro.Fecha.toString()).toLocaleDateString() + " - " + this.detailSiniestro.Hora;
            item.TipoReclamoId = TiposReclamoBase.Siniestro;
            item.EmpresaId = this.detailSiniestro.EmpresaId;
            item.SucursalId = this.detailSiniestro.SucursalId;
            return item;
        }
        else {
            return item;
        }
    }

    onEdit(row: ReclamosDto) {

        super.onEdit(row);
        $('#scrollInv').addClass("smallsize");
    }

    onEditID(id: any) {
        if (!this.allowVisualizar) {
            return;
        }
        this.active = false;
        if (this.loadInMaterialPopup) {
            this.service.getById(id).subscribe((e) => {
                this.Opendialog(e.DataObject, ViewMode.Modify);
            }
            );

        }
        else {
            this.GetEditComponent().show(id);
        }

    }

    BorrarFiltros() {
        this.filter = this.getNewfilter();
        this.selectedSubEstados = [];
    }

    Opendialog(_detail: ReclamosDto, viewMode: ViewMode) {

        var popupHeight: string = '';
        var popupWidth: string = '95%';

        var dialog = this.injector.get(MatDialog);

        const dialogConfig = new MatDialogConfig<ReclamosDto>();

        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.width = popupWidth;
        dialogConfig.height = popupHeight;



        _detail.AccessFromSiniestros = this.siniestro;
        dialogConfig.data = _detail;

        let dialogRef = dialog.open(this.GetEditComponentType(), dialogConfig);

        this.detailElement = dialogRef.componentInstance;

        dialogRef.componentInstance.viewMode = viewMode;

        (dialogRef.componentInstance as ReclamosGeneralComponent).saveLocal = false;

        dialogRef.afterClosed().subscribe(


            data => {
                this.active = true;
                this.onSearch();
                $('.custom-combo').on('show.bs.dropdown', function() {
                    $('.dropdown-menu .show').removeClass('smallsize');
                })
            }
        );

    }

    onDelete(item: ReclamosDto) {

        if (!this.allowEliminar) {
            this.notificationService.warn("No posee permisos para quitar reclamos");
            return;
        }

        var strindto = this.getDescription(item);

        var Fecha = moment(item.Fecha).format('DD/MM/YYYY');
        this.message.confirm('¿Está seguro de que desea eliminar el registro: ' + item.Involucrado.NroInvolucrado + ' - ' + Fecha + '?', strindto || 'Confirmación', (a) => {

            if (a.value) {
                this.serviceReclamos.delete(item.Id)
                    .subscribe(() => {
                        this.Search();
                        this.notify.success(this.l('Registro eliminado correctamente'));
                    });
            }

        });

    }

    ImportarDesdeExcel(): void {
        const dialogConfig = new MatDialogConfig<ReclamoImportadorDTO>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = true;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        let dialogRef = this.dialog.open(ReclamosImportadorComponent, dialogConfig);

        //this.detailElement = dialogRef.componentInstance;
        dialogRef.afterClosed().subscribe(
            data => {

                this.active = true;
            }
        );

    }

    onEstadoReclamoSelected(event: any): void {
        this.filter.SubEstadoReclamo = null;
        this.selectedSubEstados = null;
    }

}