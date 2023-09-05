import { Component, Input, Type, ViewChild, ChangeDetectionStrategy, ChangeDetectorRef, DebugElement, Injector, Output, EventEmitter, AfterViewInit } from '@angular/core';
import { SiniestrosDto } from '../../model/siniestro.model';
import { CochesDto } from '../../model/coche.model';
import { ItemDto, ItemDtoStr, ViewMode, ResponseModel, PaginListResultDto } from '../../../../../shared/model/base.model';
import { LineaDto } from '../../../planificacion/model/linea.model';
import { CausasService } from '../../causas/causas.service';
import { ActivatedRoute } from '@angular/router';
import { PermissionCheckerService } from '../../../../../shared/common/permission-checker.service';
import { IDetailComponent } from '../../../../../shared/manager/detail.component';
import { forEach } from '@angular/router/src/utils/collection';
import { NgModel } from '@angular/forms';
import { ConductasNormasService } from '../../normas/normas.service';
import { InvolucradosService } from '../../involucrados/involucrados.service';
import { InvolucradosDto, MuebleInmuebleDto, InvolucradosFilter } from '../../model/involucrados.model';
import { ConsecuenciasDto } from '../../model/consecuencias.model';
import { AgregationListComponent } from '../../../../../shared/manager/agregationlist.component';
import { InvolucradoDetailComponent } from './involucrado-detail-modal.component';
import { AddConsecuenciaModalComponent } from '../detallesiniestro/add-consecuencia-modal.component';
import { CreateOrEditSegurosModalComponent } from '../../seguros/create-or-edit-seguros-modal.component';
import { DataTable, Paginator } from 'primeng/primeng';
import { ConductorDto } from '../../model/conductor.model';
import { LesionadoDto } from '../../model/lesionado.model';
import { VehiculoDto } from '../../model/vehiculo.model';
import { MatDialogConfig } from '@angular/material';
import { AdjuntoComponent, LlamadorAdjuntos } from '../adjunto/adjunto.component';
import { environment } from '../../../../../../environments/environment';
import { LazyLoadEventData } from '../../../../../shared/helpers/PrimengDatatableHelper';

@Component({
    selector: 'involucrados',
    templateUrl: './involucrados.component.html'
})
export class InvolucradosComponent extends AgregationListComponent<InvolucradosDto> implements AfterViewInit {

    SiniestroId: number;

    @ViewChild('dt') dt: DataTable;
    filter: InvolucradosFilter;
    listFiltrada: InvolucradosDto[];

    @ViewChild('paginator') paginator: Paginator;

    _detail: SiniestrosDto;
    @Output() InvolucradosChange = new EventEmitter();
    defaultmaxenbd: number;

    @Input()
    get detail(): SiniestrosDto {

        return this._detail;
    }

    set detail(value: SiniestrosDto) {

        this._detail = value;
        //this.onSearch();
    }
    allowAdjuntos: boolean = false;
    allowInvAgregar: boolean = false;
    allowInvModificar: boolean = false;
    allowInvEliminar: boolean = false;
    allowAddTipoDni: boolean = false;
    allowInvAdjuntos: boolean = false;

    SetAllowPermission() {
        this.allowAdjuntos = this.permission.isGranted('Siniestro.Involucrado.Adjunto');
        this.allowInvAgregar = this.permission.isGranted('Siniestro.Involucrado.Modificar');
        this.allowInvModificar = this.permission.isGranted('Siniestro.Involucrado.Modificar');
        this.allowInvEliminar = this.permission.isGranted('Siniestro.Involucrado.Eliminar');
        this.allowInvAdjuntos = this.permission.isGranted('Siniestro.Involucrado.Adjunto');
        this.allowAddTipoDni = this.permission.isGranted('Admin.TipoDni.Agregar');
    }


    constructor(protected serviceInvolucrados: InvolucradosService,
        injector: Injector) {

        super(InvolucradoDetailComponent, injector);
        this.filter = this.getNewfilter();
        this.SetAllowPermission();
    }

    getNewfilter(): InvolucradosFilter {

        this.filter = new InvolucradosFilter();
        this.filter.SiniestroID = this.SiniestroId;
        return this.filter;
    }

    GetEditComponentType(): Type<IDetailComponent> {
        return InvolucradoDetailComponent
    }

    onFilterChange(event, col) {

    }

    ngAfterViewInit() {
    }

    onDelete(item: InvolucradosDto, bAvoidConfirmation?: boolean) {

        if (!this.allowInvEliminar) {
            this.notificationService.warn("No posee permisos para quitar Involucrados");
            return;
        }

        var strindto = this.getDescription(item);
        //var aa = this.getNewItem(item);
        //var stringentity = aa.getDescription();
        if (!bAvoidConfirmation) {

            this.message.confirm('¿Está seguro de que desea eliminar el involucrado: ' + item.NroInvolucrado + '?', strindto || 'Confirmación', (a) => {

                //this.isshowalgo = !this.isshowalgo;
                if (a.value) {
                    this.deleteItem(item);
                }

            });
        } else {
            this.deleteItem(item);
        }
    }

    RefreshWithFilters() {

        this.filter.SiniestroID = this.SiniestroId;
        this.onSearch();
    }

    getNewItem(): InvolucradosDto {
        var item = new InvolucradosDto();
        item.SiniestroId = this.detail.Id;
        item.DetalleLesion = [];
        var maxenlist = Math.max.apply(Math, this.list.map(function(o) { return o.NroInvolucradoPuro; }))
        if (this.detail && this.detail.Id != 0) {
            if (this.defaultmaxenbd > maxenlist) {
                item.NroInvolucradoPuro = this.defaultmaxenbd + 1;
            }
            else {
                item.NroInvolucradoPuro = maxenlist + 1;
            }
            item.NroInvolucrado = this.detail.NroSiniestro + "/" + item.NroInvolucradoPuro;
        }
        return item;
    }

    onCreate() {
        ;
        if (!this.allowInvAgregar) {
            return;
        }
        //this.active = false;
        this.viewMode = ViewMode.Add;
        let item = this.getNewItem();
        item.Id = Math.min.apply(Math, this.list.map(function(o) { return o.Id; })) - 1;
        if (item.Id > 0)
            item.Id = -1;

        this.OpenCustomizedWidthDialog(item, '99%');
        this.primengDatatableHelper.records = [...this.list];
    }

    CompleteDataBeforeShow(_detail: InvolucradosDto): InvolucradosDto {
        //if (_detail.Conductor == null) {
        //    _detail.Conductor = new ConductorDto(null);
        //}

        //if (_detail.Lesionado == null) {
        //    _detail.Lesionado = new LesionadoDto(null);
        //}

        //if (_detail.MuebleInmueble == null) {
        //    _detail.MuebleInmueble = new MuebleInmuebleDto(null);
        //}

        //if (_detail.Vehiculo == null) {
        //    _detail.Vehiculo = new VehiculoDto(null);
        //}


        return _detail;
    }

    onEdit(row: InvolucradosDto) {
        this.viewMode = ViewMode.Modify;
        this.Opendialog(row);
    }

    CompleteDataBeforeUpdateItem(item: InvolucradosDto): any {
        var detailComp = this.detailElement as InvolucradoDetailComponent;


        //if (item.TieneConductor) {
        //    item.ConductorNombre = (item.Conductor.ApellidoNombre || '') + ' ' + (item.Conductor.NroDoc || '');
        //}
        //if (item.TieneVehiculo) {
        //    item.VehiculoNombre = (item.Vehiculo.Marca || '') + ' ' + (item.Vehiculo.Modelo || '') + ' ' + (item.Vehiculo.Dominio || '');
        //}

        //if (item.TieneMuebleInmueble) {
        //    //involucradoentitiy.MuebleInmueble.TipoInmueble.Descripcion, involucradoentitiy.MuebleInmueble.Lugar, localidad
        //    item.MuebleInmuebleNombre = (item.MuebleInmueble.Lugar || '') + ' ' + item.MuebleInmueble.selectLocalidades != null ? (item.MuebleInmueble.selectLocalidades.Description || '') : "";
        //}

        //if (item.TieneLesionado) {
        //    //involucradoentitiy.MuebleInmueble.TipoInmueble.Descripcion, involucradoentitiy.MuebleInmueble.Lugar, localidad

        //    item.Lesionado.TipoLesionadoDescripcion = detailComp.TipoLesionadoCombo.items.find(e => e.Id == item.Lesionado.TipoLesionadoId).Descripcion || '';

        //    item.LesionadoNombre = item.Lesionado.TipoLesionadoDescripcion || '';

        //}


    }

    InitInvolucrado(detail: SiniestrosDto): any {

        this.initList(detail.Id);
    }

    private initList(idSiniestro: number) {

        this.list = [];
        this.active = false;
        this.primengDatatableHelper.defaultRecordsCountPerPage = 250;

        var filter: InvolucradosFilter = new InvolucradosFilter();
        filter.SiniestroID = idSiniestro;
        this.SiniestroId = idSiniestro;
        this.serviceInvolucrados.search(filter)
            .finally(() => {
                this.active = true;
            })
            .subscribe(e => {
                this.list = e.DataObject.Items;
                this.primengDatatableHelper.records = e.DataObject.Items;
                this.primengDatatableHelper.totalRecordsCount = e.DataObject.TotalCount;
                this.primengDatatableHelper.hideLoadingIndicator()
                if (this.list.length > 0) {
                    this.defaultmaxenbd = Math.max.apply(Math, this.list.map(function(o) { return o.NroInvolucradoPuro; }))
                }
                else {
                    this.defaultmaxenbd = 0;
                }
            });
    }

    UpdateItem(item: InvolucradosDto) {
        this.initList(item.SiniestroId);
    }

    deleteItem(item: InvolucradosDto) {
        this.active = false;
        this.serviceInvolucrados.delete(item.Id)
            .finally(() =>
                this.active = true
            )
            .subscribe(() => {

                let _list = [...this.list];
                var index = this.list.findIndex(e => e == item);
                _list.splice(index, 1);
                this.list = _list;
                this.primengDatatableHelper.records = [...this._list];
                this.primengDatatableHelper.totalRecordsCount = this._list.length;

                this.notify.success(this.l('Registro eliminado correctamente'));
            });
    }

    onAdjuntarArchivosShow(item: InvolucradosDto) {
        const dialogConfig = new MatDialogConfig<InvolucradosDto>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        //dialogConfig.id = "1";
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        //dialogConfig.data = this.CompleteDataBeforeShow(JSON.parse(JSON.stringify(_detail)) as T);

        let dialogRef = this.dialog.open(AdjuntoComponent, dialogConfig);

        dialogRef.componentInstance.appGetAllFileByParent = environment.siniestrosUrl + '/Involucrados/GetAdjuntos';
        dialogRef.componentInstance.appUploadUrl = environment.siniestrosUrl + '/Involucrados/UploadFiles/?InvolucradoId=' + item.Id;
        dialogRef.componentInstance.appRemoveFileUrl = environment.siniestrosUrl + '/Involucrados/DeleteFileById';
        dialogRef.componentInstance.allowClose = true;
        dialogRef.componentInstance.Parent = item.Id;
        dialogRef.componentInstance.AllowUploadFiles = this.allowInvAdjuntos;
    }


    onSearch(event?: LazyLoadEventData) {
        if (event == null) {
            if (this.paginator) {
                this.paginator.changePage(0);
            }
        }
        this.Search(event);
    }

    Search(event?: LazyLoadEventData) {

        if (this.isFirstTime == false) {
            this.isFirstTime = true;
            return;
        }

        if (!this.filter) { this.filter = this.getNewfilter(); }

        this.filter.SiniestroID = this.SiniestroId;

        this.isTableLoading = true;
        this.primengDatatableHelper.showLoadingIndicator();

        this.filter.Sort = this.primengDatatableHelper.getSorting(this.dt);
        this.filter.Page = this.primengDatatableHelper.getPageIndex(this.paginator, event);
        this.filter.PageSize = this.primengDatatableHelper.getPageSize(this.paginator, event);

        this.serviceInvolucrados.search(this.filter)
            .finally(() => {
                this.isTableLoading = false;
            })
            .subscribe((result: ResponseModel<PaginListResultDto<InvolucradosDto>>) => {
                this.list = result.DataObject.Items;
                this.primengDatatableHelper.records = result.DataObject.Items
                this.primengDatatableHelper.totalRecordsCount = result.DataObject.TotalCount;
                this.primengDatatableHelper.hideLoadingIndicator()
            });
    }




}