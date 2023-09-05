import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit, Input, ComponentFactoryResolver, ViewEncapsulation } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import * as _ from 'lodash';
declare let mApp: any;
import { Paginator } from 'primeng/components/paginator/paginator';
import { RouterModule, Routes, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { DetailEmbeddedComponent, IDetailComponent } from '../../../../../shared/manager/detail.component';
import { HFechasConfiDto } from '../../model/HFechasConfi.model';
import { HServiciosFilter, HServiciosDto, ExportarExcelDto } from '../../model/hServicios.model';
import { MediasVueltasDto, MediasVueltasFilter } from '../../model/mediasvueltas.model';
import { HHorariosConfiDto, HHorariosConfiFilter } from '../../model/hhorariosconfi.model';
import { SubGalponDto } from '../../model/subgalpon.model';
import { TipoDiaDto } from '../../model/tipoDia.model';
import { SubgalponComboComponent } from '../../shared/subgalpon-combo.component';
import { TipoDiaComboComponent } from '../../shared/tipoDia-combo.component';
import { HFechasConfiService } from '../HFechasConfi.service';
import { HServiciosService } from '../../hservicio/servicio.service';
import { MediasVueltasService } from '../../mediasvueltas/mediasvueltas.service';
import { HHorariosConfiService } from '../../HHorariosConfi/hhorariosconfi.service';
import { HTposHorasService } from '../../htposhoras/htposhoras.service';
import { HTposHorasDto } from '../../model/htposhoras.model';
import { BanderaService } from '../../bandera/bandera.service';
import { BanderaFilter, BanderaDto } from '../../model/bandera.model';
import { ItemDto, ViewMode } from '../../../../../shared/model/base.model';
import { SelectItem } from 'primeng/primeng';
import * as moment from 'moment';
import { Table } from 'primeng/table';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { ServicioNewComponent } from './servicio-new.component';
import { Service } from '../../../../../shared/common/services/crud.service';
import { Subject } from 'rxjs';
import { ExportarExcelComponent } from './exportar-excel.component';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';

//import * as deepEqual from 'deep-equal'
import { HChoxserService } from '../../HChoxser/hChoxser.service';


@Component({
    selector: 'create-or-edit-hfechasconfi-servicios',
    templateUrl: './create-or-edit-hfechasconfi-servicios.component.html',
    styleUrls: ['./create-or-edit-hfechasconfi-servicios.component.css'],
    encapsulation: ViewEncapsulation.None

})
export class CreateOrEditHFechasConfiServiciosComponent extends AppComponentBase implements OnInit {



    EditEnabled: boolean = true;
    allowExportarExcel: Boolean = true;
    existsDurations: Boolean = false;

    isLoadingMediaVueltas: boolean;
    isLoading: boolean;
    detail: HFechasConfiDto;

    filterValue: HServiciosFilter = new HServiciosFilter();
    viewMode: ViewMode = ViewMode.Add;
    active: boolean;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();
    saving: boolean;
    closeOnSave: boolean;



    @Input()
    get filter(): HServiciosFilter {
        return this.filterValue;
    }

    @Output() filterChange = new EventEmitter<HServiciosFilter>();
    set filter(val: HServiciosFilter) {
        this.filterValue = val;
        if (this.filterChange)
            this.filterChange.emit(this.filterValue);
    }

    servicios: HServiciosDto[];
    currentService: HServiciosDto;


    mediaVueltas: MediasVueltasDto[];
    mediaVueltasOrdenadas: MediasVueltasDto[];
    mediaVueltasOriginal: MediasVueltasDto[];

    hHorariosConfiDto: HHorariosConfiDto;
    subGalponDtoSelectItem: SelectItem[];
    subGalponDto: SubGalponDto[];
    tipoDiaDto: TipoDiaDto[];
    htposhoras: HTposHorasDto[];
    banderasDto: BanderaDto[];
    banderasDtoSelectItem: SelectItem[];
    htposhorasSelectItem: SelectItem[];
    autoLoad: boolean = false;

    allowAltaServicio: boolean = false;
    allowEliminarServicios: boolean = false;
    allowEliminarDuracion: boolean = false;
    allowGuardarServicios: boolean = false;

    @ViewChild('CodSubg') CodSubgCombo: SubgalponComboComponent;
    @ViewChild('CodTdia') CodTdia: TipoDiaComboComponent;
    @ViewChild('mediaVueltaTable') mediaVueltaTable: Table;

    PDFPuntaPunta: boolean = false;
    PDFRelevo: boolean = false;
    allowsave: boolean = true;

    @Output() IrDuracion: EventEmitter<HHorariosConfiDto> = new EventEmitter();


    getDescription(item: HFechasConfiDto): string {
        return item.Description;
    }

    AddItemBreadcrumbs(HFechasConfiDto) {

    }

    constructor(
        protected injector: Injector,
        protected service: HFechasConfiService,
        protected _hservice: HServiciosService,
        protected _MediasVueltasservice: MediasVueltasService,
        protected router: Router,
        protected _HHorariosConfiService: HHorariosConfiService,
        protected _HTposHorasService: HTposHorasService,
        protected _banderaservice: BanderaService,
        protected _hchoxserservice: HChoxserService,
        private cfr: ComponentFactoryResolver


    ) {
        super(injector);
        this.Ultimofiltro = new HHorariosConfiFilter()
        this.detail = new HFechasConfiDto();
        //this.icon = "flaticon-clock-2";
        //this.title = "Servicios";
        this.servicios = [];
        this.hHorariosConfiDto = new HHorariosConfiDto();
        //this.filter = new HServiciosFilter();

        this.allowAltaServicio = this.permission.isGranted("Horarios.FechaHorario.AltaServicio");
        this.allowEliminarServicios = this.permission.isGranted("Horarios.FechaHorario.EliminarServicios");
        this.allowEliminarDuracion = this.permission.isGranted("Horarios.FechaHorario.EliminarDuracion");
        this.allowGuardarServicios = this.permission.isGranted("Horarios.FechaHorario.GuardarServicios");
        this.PDFPuntaPunta = this.permission.isGranted('Horarios.FechaHorario.ExportarPuntaPunta');
        this.PDFRelevo = this.permission.isGranted('Horarios.FechaHorario.ExportarRelevo');
        this.allowExportarExcel = this.permission.isGranted('Horarios.FechaHorario.ExportarExcel');

    }


    //selectedItemChangeBandera
    //selectedItemChangeTpoHora($event,row)"

    selectedItemChangeBandera($event: any, row: MediasVueltasDto): void {
        console.log($event);
        var s = this.banderasDto.find(e => e.Id == $event.value);
        if (s) {
            row.DescripcionBandera = s.Description;
        }

        this.MVHasChange(row);
    }

    selectedItemChangeTpoHora($event: any, row: MediasVueltasDto): void {
        console.log($event);
        if (!row) {
            this.filter.CodTdia = $event.value;
        }
        var s = this.htposhoras.find(e => e.Id == $event.value);
        if (s) {
            row.DescripcionTpoHora = s.DscTpoHora;
        }
        this.MVHasChange(row);
    }


    selectedItemChangesubgalpon($event) {

        if (this.Ultimofiltro.CodSubg != this.filter.CodSubg) {

            if (!this.existUnsavedData()) {
                this.BuscarServicios();
            }
            else {
                this.message.confirm(this.confirm, 'Confirmación', (a) => {
                    if (a.value) {
                        this.clearUnsavedData();
                        this.BuscarServicios();
                    }
                    else {
                        this.filter.CodSubg = this.Ultimofiltro.CodSubg;
                    }
                });
            }
        }
    }

    selectedItemChangeCodTdia($event) {

        if (this.Ultimofiltro.CodTdia != this.filter.CodTdia) {

            if (!this.existUnsavedData()) {
                this.BuscarServicios();
            }
            else {
                this.message.confirm(this.confirm, 'Confirmación', (a) => {
                    if (a.value) {
                        this.clearUnsavedData();
                        this.BuscarServicios();
                    }
                    else {
                        this.filter.CodTdia = this.Ultimofiltro.CodTdia;
                    }
                });
            }
        }
    }


    subgalponitemstChange(items: SubGalponDto[]): void {

        if (items && items.length > 0) {
            this.subGalponDto = items;

            this.subGalponDtoSelectItem = this.subGalponDto.map(e => { return { label: e.DesSubg, value: e.Id } })

            this.filter.CodSubg = items[0].Id;
            this.BuscarServicios();
        }
    }
    tipoDiaitemstChange(items: TipoDiaDto[]): void {
        if (items && items.length > 0) {
            this.tipoDiaDto = items;
            this.filter.CodTdia = items[0].Id;
            this.BuscarServicios();
        }
    }


    ngAfterViewInit(): void {
        mApp.initPortlets();
    }


    completedataBeforeShow(item: HFechasConfiDto): any {

        //this.filter = new HServiciosFilter();
        this.filter.CodHfecha = item.Id;

        this.CodSubgCombo.CodHfecha = null;
        this.CodSubgCombo.CodHfecha = this.detail.Id;
        this.CodTdia.CodHfecha = null;
        this.CodTdia.CodHfecha = this.detail.Id;

        var f = new BanderaFilter();

        f.LineaIdRelacionadas = this.detail.CodLinea;
        f.CodHfecha = this.detail.Id;

        this._banderaservice.requestAllByFilter(f).subscribe(result => {
            this.banderasDto = result.DataObject.Items;

            //this.banderasDtoSelectItem = this.banderasDto.map(function (o) {
            //    var rObj = { label: o.Nombre, value: o.Id }; 
            //    return rObj;
            //});


            this.banderasDtoSelectItem = this.banderasDto.map(e => { return { label: e.Nombre, value: e.Id } })

            var bannull = { label: 'Selecione...', value: null };

            this.banderasDtoSelectItem.splice(0, 0, bannull);


            //.map(e => { return new { label: 'Audi', value: 'Audi' } });


        });

        this._HTposHorasService.requestAllByFilter({}).subscribe(result => {

            var ordenado = result.DataObject.Items.sort((a, b) => {
                if (a.Orden < b.Orden) return -1;
                else if (a.Orden > b.Orden) return 1;
                else return 0;
            });
            this.htposhoras = ordenado;
            this.htposhorasSelectItem = this.htposhoras.map(e => { return { label: e.DscTpoHora, value: e.Id } })

            var bannull = { label: 'Selecione...', value: null };
            this.htposhorasSelectItem.splice(0, 0, bannull);
        });

    }

    Ultimofiltro: HHorariosConfiFilter;

    BuscarServicios(idServicioPosicion = null, descripcionServicio: string = null): void {

        if (this.filter.CodTdia && this.filter.CodSubg && this.filter.CodHfecha) {

            //Busco la cabecera para el tipo de dia y sub galpon
            this.Ultimofiltro = new HHorariosConfiFilter();
            this.Ultimofiltro.CodHfecha = this.filter.CodHfecha;
            this.Ultimofiltro.CodSubg = this.filter.CodSubg;
            this.Ultimofiltro.CodTdia = this.filter.CodTdia;

            this._HHorariosConfiService.requestAllByFilter(this.Ultimofiltro)
                .finally(() => { this.isLoading = false })
                .subscribe(e => {

                    if (e.DataObject.Items.length > 0) {
                        this.hHorariosConfiDto = e.DataObject.Items[0];
                        this.viewMode = ViewMode.Modify;
                    }
                    else {
                        this.hHorariosConfiDto = new HHorariosConfiDto();
                        this.hHorariosConfiDto.CodHfecha = this.filter.CodHfecha;
                        this.hHorariosConfiDto.CodSubg = this.filter.CodSubg;
                        this.hHorariosConfiDto.CodTdia = this.filter.CodTdia;
                        this.hHorariosConfiDto.Id = 0;
                        this.viewMode = ViewMode.Add;
                    }
                });

            //Busco los servicios para el tipo de dia y sub galpon
            this.isLoading = true;
            this._hservice.requestAllByFilter(this.filter)
                .finally(() => { this.isLoading = false })
                .subscribe(e => {
                    this.servicios = e.DataObject.Items
                    this.servicios = this.servicios.sort((a, b) => { return (a['Description'] as any) - (b['Description'] as any) })

                    if (this.servicios.length > 0) {
                        let serv = this.servicios[0];
                        if (this.servicios.findIndex(e => e.Id == idServicioPosicion) >= 0) {
                            serv = this.servicios.find(e => e.Id == idServicioPosicion);
                        }
                        else if (this.servicios.findIndex(e => e.NumSer == descripcionServicio) >= 0) {
                            serv = this.servicios.find(e => e.NumSer == descripcionServicio);
                        }

                        if (serv) {
                            this.OnServiceSelectInternal(serv);
                        }

                    }
                    else {
                        this.currentService = null;
                        this.mediaVueltas = [];
                        this.scroll(this.mediaVueltaTable);
                    }
                });
        }
    }

    scroll(table: Table) {
        $(".ui-table-scrollable-body").animate({ scrollTop: 0 }, 500);
    }

    OnServiceSelectInternal(item: HServiciosDto): void {

        this.currentService = item;
        this.servicios.forEach(f => f.IsSelected = false);
        item.IsSelected = true;

        if (item.Id <= 0) {


            this.mediaVueltas = [];

            this.mediaVueltasOriginal = [];

            this.scroll(this.mediaVueltaTable);
        }
        else {
            var filtro = new MediasVueltasFilter();
            filtro.CodServicio = item.Id;
            filtro.Sort = "Orden";
            this.isLoadingMediaVueltas = true;

            this._MediasVueltasservice.requestAllByFilter(filtro)
                .finally(() => { this.isLoadingMediaVueltas = false })
                .subscribe(e => {

                    this.mediaVueltas = e.DataObject.Items.map(i => {
                        var s = new MediasVueltasDto(i);
                        s.Sale = moment(s.Sale).toDate();
                        s.Llega = moment(s.Llega).toDate();

                        s.SaleOriginal = s.Sale;
                        s.LlegaOriginal = s.Llega;
                        s.CodBanOriginal = s.CodBan;
                        s.CodTpoHoraOriginal = s.CodTpoHora;
                        s.HasError = false;
                        s.HasChange = false;
                        return s;
                    });

                    this.scroll(this.mediaVueltaTable);
                    //this.mediaVueltasOriginal = 
                    this.restUnsavedData();


                });
        }
    }

    confirm: string = '¿Está seguro de que desea salir existen datos sin guardar?';
    OnServiceSelect(item: HServiciosDto): void {
        if (this.currentService && this.currentService.NumSer == item.NumSer) {
            return;
        }

        if (!this.existUnsavedData()) {
            this.OnServiceSelectInternal(item);
        }
        else {
            this.message.confirm(this.confirm, 'Confirmación', (a) => {
                //this.isshowalgo = !this.isshowalgo;
                if (a.value) {
                    this.restUnsavedData();
                    this.OnServiceSelectInternal(item);
                }
            });
        }
    }

    cleanData(): void {
        this.servicios = [];
        this.hHorariosConfiDto = new HHorariosConfiDto();
        this.mediaVueltas = [];
    }

    close(): void {
        this.active = false;
        this.viewMode = ViewMode.Undefined;
        this.modalClose.emit(true);

    }


    clearUnsavedData(): void {

        this.mediaVueltas = [];
        this.servicios = [];
        this.currentService = null;
        this.mediaVueltasOriginal = [];

    }

    restUnsavedData(): void {
        this.mediaVueltas.forEach(e => {
            e.HasError = false;
            e.ErrorMessages = [];
            e.HasErrorSale = false;
            e.HasErrorLlega = false;
        });
        this.mediaVueltasOriginal = _.cloneDeep(this.mediaVueltas);
    }

    canDeactivate(): boolean {
        return !this.existUnsavedData();
    }

    existUnsavedData(): boolean {
        var bool = true;
        if (this.mediaVueltasOriginal || (this.mediaVueltas && !this.mediaVueltasOriginal)) {

            bool = _.isEqualWith(this.mediaVueltas, this.mediaVueltasOriginal, this.compareCustom);
        }
        return !bool;
    }

    compareCustom(value1, value2, key) {
        return undefined
    }

    onShowServicio(item: MediasVueltasDto): void {

        //TODO: implementar funcionalidad de mintos por sector
    }

    SaveDetail(): any {

        try {
            $(this.mediaVueltaTable.editingCell).removeClass('ui-editing-cell');
            this.mediaVueltaTable.onEditComplete.emit({ field: null, data: null });
            this.mediaVueltaTable.editingCell = null;
            
        } catch (e) {

        }

        this.closeOnSave = false;
        this.mediaVueltas = this.ReordenarMediasVueltas(this.mediaVueltas);
        if (this.currentService && this.ValidateMediasVueltas(this.mediaVueltas)) {

            let data: HHorariosConfiDto = new HHorariosConfiDto(JSON.parse(JSON.stringify(this.hHorariosConfiDto)));

            var medias = JSON.parse(JSON.stringify(this.mediaVueltas));

            medias.forEach(s => {
                s.Sale = (moment(s.Sale).format("YYYY-MM-DDTHH:mm:ss") as any);
                s.Llega = (moment(s.Llega).format("YYYY-MM-DDTHH:mm:ss") as any);
                s.SaleOriginal = null;
                s.LlegaOriginal = null;
                return s;
            })

            data.CurrentServicio = JSON.parse(JSON.stringify(this.currentService));
            data.CurrentServicio.HMediasVueltas = medias;
            this.buscarDuraciones(data)
                .subscribe(e => {
                    if (e.DataObject.length > 0) {
                        var maxOrden = this.mediaVueltas[this.mediaVueltas.length - 1].Orden;
                        var mediaVueltaswithChanges = this.mediaVueltas.filter(e => e.HasChange && e.Orden != 0 && e.Orden != maxOrden); 
                        if (mediaVueltaswithChanges.length > 0) {
                            var message = "Debe verificar la duración";
                            this.notify.warn(message, "Error");
                            this.IrDuracion.emit(data);
                            this.saving = false;
                            return;
                        } else {
                            this.SaveDetailInternal(data);    
                        }
                    }
                    else {
                        this.SaveDetailInternal(data);
                    }
                });
        }
        else {
            this.saving = false;
        }
    }

    buscarDuraciones(data: HHorariosConfiDto) {
        this.Ultimofiltro = new HHorariosConfiFilter();
        this.Ultimofiltro.CodHfecha = data.CodHfecha;
        this.Ultimofiltro.CodSubg = data.CodSubg;
        this.Ultimofiltro.CodTdia = data.CodTdia;

        return this._hchoxserservice.RecuperarDuraciones(this.Ultimofiltro);
    }

    ReordenarMediasVueltas(mediaVueltas: MediasVueltasDto[]) {
        this.mediaVueltasOrdenadas = mediaVueltas.sort((a: MediasVueltasDto, b: MediasVueltasDto) => {
            if (a.Sale > b.Sale) {
                return 1
            }

            if (a.Sale < b.Sale) {
                return -1
            }

            return 0;
        });

        this.mediaVueltasOrdenadas.sort((a: MediasVueltasDto, b: MediasVueltasDto) => {
            if (a.Dia > b.Dia) {
                return 1;
            }
            if (a.Dia < b.Dia) {
                return -1;
            }
            return 0;
        })
        var i = 1
        this.mediaVueltasOrdenadas.forEach(e => {
            e.Orden = i;
            i += 1;
        })
        return this.mediaVueltasOrdenadas;
    }

    SaveDetailInternal(data: HHorariosConfiDto) {
        this._HHorariosConfiService.createOrUpdate(data, this.viewMode)
            .finally(() => { this.saving = false; })
            .subscribe((t) => {
                this.restUnsavedData();

                if (this.currentService.Id <= 0) {

                    this.currentService.Id = t.DataObject.CurrentServicio.Id;
                }

                if (this.viewMode == ViewMode.Add) {
                    this.BuscarServicios();
                }
                else {
                    this.OnServiceSelectInternal(this.currentService);
                }

                this.notify.info('Guardado exitosamente');

                //if (this.closeOnSave) {
                //    this.close();
                //};
                //this.affterSave(this.detail);
                this.closeOnSave = true;
                this.modalSave.emit(null);
            }, error => {
                if (error.error instanceof ErrorEvent) {
                    // client-side error
                    throw (error);
                } else {
                    // server-side error
                    if (error.error.Messages.find(e => e == "SERVICIO_CON_DURACION")) {
                        var message = "Debe modificar la duración del servicio editado / insertado para poder guardar los datos";
                        this.notify.error(message, "Error");
                        this.IrDuracion.emit(data);
                    }

                }
                console.log(error)
            })
    }

    //Calendar.prototype.formatTime = function (date) {
    //    if (!date) {
    //        return '';
    //    }
    //    var output = '';
    //    var hours = (this.utc) ? date.getUTCHours() : date.getHours();
    //    var minutes = date.getMinutes();
    //    var seconds = date.getSeconds();
    //    if (this.hourFormat == '12' && hours > 11 && hours != 12) {
    //        hours -= 12;
    //    }
    //    output += hours === 0 ? 12 : (hours < 10) ? '0' + hours : hours;
    //    output += ':';
    //    output += (minutes < 10) ? '0' + minutes : minutes;
    //    if (this.showSeconds) {
    //        output += ':';
    //        output += (seconds < 10) ? '0' + seconds : seconds;
    //    }
    //    if (this.hourFormat == '12') {
    //        output += date.getHours() > 11 ? ' PM' : ' AM';
    //    }
    //    return output;
    //};

    CalcularDiaMediaVuelta(mediaVueltas: MediasVueltasDto[]) {

        if (mediaVueltas.length > 1) {

            var sale = mediaVueltas[0].Sale;
            var llega = mediaVueltas[0].Llega;





            mediaVueltas.forEach(e => {
                e.Sale = moment(e.Sale).date(1).toDate();
                e.Llega = moment(e.Llega).date(1).toDate();
                e.Dia = 1;
            });

            if (llega < sale) {
                mediaVueltas[0].Llega = moment(mediaVueltas[0].Llega).add(1, 'days').toDate();
                llega = mediaVueltas[0].Llega;
            }


            mediaVueltas.forEach(mv => {
                if (mv.Sale < sale) {
                    mediaVueltas.filter(e => e.Orden >= mv.Orden).forEach(e => {
                        e.Sale = moment(e.Sale).add(1, 'days').toDate();
                        e.Dia = e.Sale.getDate();
                    }
                    );
                }
                sale = mv.Sale;

                if (mv.Llega < llega) {
                    mediaVueltas.filter(e => e.Orden >= mv.Orden).forEach(e => {
                        e.Llega = moment(e.Llega).add(1, 'days').toDate();
                    }
                    );
                }
                llega = mv.Llega;
            });
        }
        else if (mediaVueltas.length == 1) {
            mediaVueltas.forEach(e => {
                e.Sale = moment(e.Sale).date(1).toDate();
                e.Llega = moment(e.Llega).date(1).toDate();
                e.Dia = 1;
            });

            mediaVueltas.forEach(mv => {
                if (mv.Llega < mv.Sale) {
                    mediaVueltas.filter(e => e.Orden >= mv.Orden).forEach(e => {
                        e.Llega = moment(e.Llega).add(1, 'days').toDate();
                    }
                    );
                }
                llega = mv.Llega;
            });
        }
    }


    ValidateMediasVueltas(mediaVueltas: MediasVueltasDto[]): boolean {
        var isValid = true;
        mediaVueltas.forEach(e => {
            e.HasError = false;
            e.ErrorMessages = [];
            e.HasErrorSale = false;
            e.HasErrorLlega = false;
        });

        //for (var i = 1; i < mediaVueltas.length - 1; i++) {
        //    if (moment(mediaVueltas[i].Sale).diff(moment(mediaVueltas[i - 1].Llega)) <= 0) {
        //        mediaVueltas[i].HasError = true;
        //        mediaVueltas[i - 1].HasError = true;
        //        mediaVueltas[i - 1].ErrorMessages.push("Verifique LLega se solapa con el Sale de la mediavuelta siguiente");
        //        mediaVueltas[i].ErrorMessages.push("Verifique Sale se solapa con el Llega de la mediavuelta anterior");
        //        isValid = false;
        //    }

        //}

        let elementoanterior: MediasVueltasDto = null;

        mediaVueltas.forEach(f => {
            // var FsaleH = f.Sale.getHours();
            // var FsaleM = f.Sale.getMinutes();
            // if (elementoanterior) {
            //     var ElemAntLlegaH = elementoanterior.Llega.getHours();
            //     var ElemAntLlegaM = elementoanterior.Llega.getMinutes();
            // }
            if (elementoanterior) {
                //if (f.Sale.getHours() < elementoanterior.Llega.getHours() || (f.Sale.getHours() == elementoanterior.Llega.getHours() && f.Sale.getMinutes() < elementoanterior.Llega.getMinutes())) 
                if (elementoanterior && f.Sale < elementoanterior.Llega) {

                    elementoanterior.HasError = true;
                    f.HasError = true;
                    elementoanterior.ErrorMessages.push("Verifique LLega se solapa con el Sale de la mediavuelta siguiente");
                    elementoanterior.HasErrorLlega = true;
                    f.ErrorMessages.push("Verifique Sale se solapa con el Llega de la mediavuelta anterior");
                    f.HasErrorSale = true;
                    isValid = false;
                }
            }
            elementoanterior = f;

        });

        mediaVueltas.forEach(f => {
            if (f.Sale > f.Llega) {
                f.HasError = true;
                f.ErrorMessages.push("Verifique Sale mayor que LLega");
                isValid = false;
            }

        });

        this.allowsave = isValid;
        return isValid;
    }

    onRemoveRow(currentDto: MediasVueltasDto) {

        if (this.mediaVueltas.length > 1) {
            var index = this.mediaVueltas.findIndex(e => e == currentDto);

            (this.mediaVueltas as any[]).splice(index, 1);
            this.ReordenarMediasVueltas(this.mediaVueltas);
        }
        else {
            this.notify.warn("No es posible eliminar todas las medias Vueltas", "Tiene que existir al menos una");
        }
    }

    onAddNewRow(currentDto: MediasVueltasDto) {

        var item = new MediasVueltasDto();
        var id = Math.min.apply(Math, this.mediaVueltas.map(function(o) { return o.Id; })) - 1;

        if (id >= 0)
            id = -1;

        item.Id = id;
        item.CodServicio = currentDto.CodServicio;
        //item.CodBan = currentDto.CodBan;
        //item.DescripcionBandera = currentDto.DescripcionBandera;
        //item.CodTpoHora = currentDto.CodTpoHora;
        //item.DescripcionTpoHora = currentDto.DescripcionTpoHora;
        //item.DifMin = 0;
        item.SaleOriginal = currentDto.Llega;
        item.LlegaOriginal = currentDto.Llega;
        item.Dia = currentDto.Llega.getDate();
        item.Orden = currentDto.Orden + 1;
        var index = this.mediaVueltas.findIndex(e => e == currentDto) + 1;

        this.addMediaVuelta(index, item);
    }

    addMediaVuelta(index: number, item: MediasVueltasDto): void {

        (this.mediaVueltas as any[]).splice(index, 0, item);

        for (var i = index + 1; i < this.mediaVueltas.length; i++) {
            this.mediaVueltas[i].Orden += 1;
        }
        console.log(this.mediaVueltas);
    }

    onAddFirstNewRow(): void {

        var mv = new MediasVueltasDto();
        mv.CodServicio = this.currentService.Id;
        mv.Dia = 1;
        mv.HasChange = true;
        mv.Id = -1;
        mv.Orden = 1;

        var first = this.mediaVueltas[0];

        mv.SaleOriginal = first.SaleOriginal;
        mv.LlegaOriginal = first.SaleOriginal;
        mv.Sale = first.SaleOriginal;
        mv.Llega = first.SaleOriginal;

        this.addMediaVuelta(0, mv);
    }

    MVHasChange(currentDto: MediasVueltasDto) {
        currentDto.HasChange = currentDto.Id <= 0
            || currentDto.SaleOriginal.toISOString() != currentDto.Sale.toISOString()
            || currentDto.LlegaOriginal.toISOString() != currentDto.Llega.toISOString()
            || currentDto.CodTpoHoraOriginal != currentDto.CodTpoHora
            || currentDto.CodBanOriginal != currentDto.CodBan;
    }

    $CalcularDiferencia: Subject<MediasVueltasDto> = new Subject<MediasVueltasDto>();

    ngOnInit(): void {
        //super.ngOnInit();

        this.$CalcularDiferencia
            .debounceTime(2500)
            //.distinctUntilChanged()
            // .switchMap(() => { })
            .subscribe(e => {
                this.CalcularDiferenciaCall(e);
            });
    }

    CalcularDiferencia(currentDto: MediasVueltasDto) {
        this.allowsave = false;
        this.$CalcularDiferencia.next(currentDto);
    }

    CalcularDiferenciaCall(currentDto: MediasVueltasDto) {

        var sale = moment(currentDto.Sale);
        var llega = moment(currentDto.Llega);

        if (currentDto.Sale != null) {
            currentDto.Sale = moment(currentDto.SaleOriginal).hours(sale.hours()).minutes(sale.minutes()).toDate();
        }

        if (currentDto.Llega != null) {
            currentDto.Llega = moment(currentDto.LlegaOriginal).hours(llega.hours()).minutes(llega.minutes()).toDate();
        }

        if (currentDto.Sale != null && currentDto.Llega != null) {

            var r = moment.utc(moment(currentDto.Llega).diff(currentDto.Sale));
            currentDto.DifMin = (r.hours() * 60) + r.minutes();

            this.MVHasChange(currentDto);

            this.CalcularDiaMediaVuelta(this.mediaVueltas);

            this.ValidateMediasVueltas(this.mediaVueltas);
        }
    }


    addServicio() {
        if (!this.existUnsavedData()) {
            this.addServicioInterno();
        }
        else {
            this.message.confirm(this.confirm, 'Confirmación', (a) => {
                if (a.value) {
                    this.clearUnsavedData();
                    this.BuscarServicios();
                    this.addServicioInterno();
                }
            });
        }
    }

    RenombrarServicio() {

        var dialog: MatDialog;
        dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;

        var clone = Object.assign({}, this.currentService);
        dialogConfig.data = clone;

        let dialogRef = dialog.open(ServicioNewComponent, dialogConfig);
        dialogRef.componentInstance.title = "Renombrar Servicio";
        dialogRef.afterClosed().subscribe(

            data => {

                if (data) {

                    var servicio = this.servicios.find(s => +s.NumSer == +data.NumSer);

                    if (servicio) {
                        this.message.info('Servicio existente.', "Aviso");
                    }
                    else {
                        this.currentService.NumSer = data.NumSer;
                        this.currentService.Description = data.NumSer;
                        var servicemod = this.servicios.find(e => e.Id == this.currentService.Id);
                        servicemod.NumSer = this.currentService.NumSer;
                        servicemod.Description = this.currentService.NumSer;

                        //if (this.currentService.NroInterno &&  +this.currentService.NroInterno != 0) {
                        //    this.currentService.NroInterno = data.NumSer;
                        //    servicemod.NroInterno = data.NumSer;
                        //}

                        this.servicios = this.servicios.sort((a, b) => { return (a['Description'] as any) - (b['Description'] as any) })
                    }
                }
            }
        );
    }

    private addServicioInterno() {

        var dialog: MatDialog;
        dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.data = new HServiciosDto();

        let dialogRef = dialog.open(ServicioNewComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(

            data => {
                if (data) {

                    var servicio = this.servicios.find(s => +s.NumSer == +data.NumSer);

                    if (servicio) {
                        this.message.info('Servicio existente.', "Aviso");
                    }
                    else {
                        data.Id = Math.min.apply(Math, this.servicios.map(function(o) { return o.Id; })) - 1;

                        if (data.Id > 0)
                            data.Id = -1;

                        var pad = "0000";
                        data.Description = pad.substring(0, pad.length - data.Description.length) + data.Description;
                        data.NumSer = data.Description;
                        data.CodHconfi = this.hHorariosConfiDto.Id;
                        this.servicios.push(data);
                        this.servicios = this.servicios.sort((a, b) => { return (a['Description'] as any) - (b['Description'] as any) });
                        this.OnServiceSelect(data);

                        var mv = new MediasVueltasDto();
                        mv.CodServicio = data.Id;
                        mv.Dia = 1;
                        mv.HasChange = true;
                        mv.Id = -1;
                        mv.Orden = 1;

                        mv.SaleOriginal = moment("2000-01-01").toDate();
                        mv.LlegaOriginal = moment("2000-01-01").toDate();
                        mv.Sale = mv.SaleOriginal;
                        mv.Llega = mv.LlegaOriginal;

                        this.mediaVueltas.push(mv)
                        //this.CalcularDiferencia(mv);
                    }
                }
            }
        );
    }

    onServicioDelete() {
        if (this.currentService) {
            var strindto = this.currentService.NumSer;
            this.message.confirm('¿Está seguro de que desea eliminar el servicio?', strindto || 'Confirmación', (a) => {
                //this.isshowalgo = !this.isshowalgo;
                if (a.value) {
                    mApp.blockPage();
                    this._hservice.delete(this.currentService.Id)
                        .finally(() => { mApp.unblockPage(); })
                        .subscribe(() => {
                            this.BuscarServicios();
                            this.notify.success(this.l('Registro eliminado correctamente'));
                        });
                }
            });
        }
    }

    ngOnDestroy(): void {
        this.modalClose.unsubscribe();
        //super.ngOnDestroy();
    }

    onDeletePlanificacion() {

        if (this.currentService) {
            var strindto = this.currentService.NumSer;

            this.message.confirm('¿Está seguro de que desea eliminar las duraciones asociadas a este servicio? ', strindto || 'Confirmación', (a) => {

                //this.isshowalgo = !this.isshowalgo;
                if (a.value) {
                    mApp.blockPage();
                    this._HHorariosConfiService.deleteDuracionesServicio(this.currentService.Id)
                        .finally(() => { mApp.unblockPage(); })
                        .subscribe(() => {
                            //this.BuscarServicios();
                            this.notify.success(this.l('Duraciones eliminadas correctamente'));
                        });
                }
            });
        }


    }

    GetReportePuntaPunta() {
        var filtermediavuelta = new MediasVueltasFilter();
        filtermediavuelta.CodHfecha = this.Ultimofiltro.CodHfecha;
        filtermediavuelta.CodSubg = this.Ultimofiltro.CodSubg;
        filtermediavuelta.CodLinea = this.detail.CodLinea;
        filtermediavuelta.CodTdia = this.Ultimofiltro.CodTdia;
        filtermediavuelta.Servicios = this.servicios;
        this._MediasVueltasservice.GenerateReportPuntaPunta(filtermediavuelta);
    }


    GetReporteRelevo() {
        var filtro = new ExportarExcelDto();
        filtro.CodHfecha = this.detail.Id;
        filtro.CodTdia = this.filter.CodTdia;
        filtro.CodSubg = this.filter.CodSubg;
        this.service.GenerateReportRelevo(filtro);
    }

    onExportarExcel() {
        var dialog: MatDialog;
        dialog = this.injector.get(MatDialog);

        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.data = new ExportarExcelDto();
        dialogConfig.data.CodHfecha = this.detail.Id;

        let dialogRef = dialog.open(ExportarExcelComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(
            data => {
                if (data) {


                }
            }
        );
    }
    onCellEdit(event) {
        setTimeout(() => {
            var element = "#" + event.field + "_" + event.data.Orden + " input";
            $(element).select();
        }, 20);

    }
    UpDownFocus(row: MediasVueltasDto, $event, Column: string) {
        var x = $event.which || $event.keyCode;
        if (x == 38 || x == 40) {
            $event.preventDefault();
        }
        if (x == 40) {
            var Order = row.Orden;
            if (Order != this.mediaVueltas.length) {
                var newOrder = Order + 1;
                var Id = "#" + Column + "_" + newOrder;

                var input = $($event.target);
                input[0].blur();
                setTimeout(e => {
                    var td = $(Id);
                    var input = td.children();
                    input.click();
                }, 0);
            }
        }

        if (x == 38) {
            var Order = row.Orden;
            var newOrder = Order - 1;
            if (newOrder > 0) {
                var Id = "#" + Column + "_" + newOrder;
                var input = $($event.target);
                input[0].blur();
                setTimeout(e => {
                    var td = $(Id);
                    var input = td.children();
                    input.click();
                }, 0);
            }
        }

    }

    showDto(item: HFechasConfiDto) {
        this.detail = item;

        this.AddItemBreadcrumbs(item);

        this.completedataBeforeShow(item)
        this.active = true;
        //if (this.element && this.element.nativeElement)
        //    this.tabInicializeFirst(this.element.nativeElement.tagName);
        //else
        //    this.tabInicializeFirst();
    }

    save(form: NgForm): void {
        this.saving = true;
        this.SaveDetail();
    }


}
