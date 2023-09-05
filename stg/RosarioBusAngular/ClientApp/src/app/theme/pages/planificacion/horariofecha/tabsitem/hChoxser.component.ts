import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, OnInit, Input, ComponentFactoryResolver } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import * as _ from 'lodash';
declare let mApp: any;
import { Paginator } from 'primeng/components/paginator/paginator';
import { RouterModule, Routes, Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { DetailEmbeddedComponent, IDetailComponent } from '../../../../../shared/manager/detail.component';
import { HFechasConfiDto } from '../../model/HFechasConfi.model';
import { HServiciosFilter, HServiciosDto } from '../../model/hServicios.model';
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
import { ImportarDuracionComponent } from './importador/importar-duracion.component';
import { HChoxser, ChofXServImportado, HChoxserExtendedDto, HorarioDuracion } from '../../model/hChoxser.model';
import { HChoxserService } from '../../HChoxser/hChoxser.service';
import { ThrowStmt } from '@angular/compiler';


@Component({
    selector: 'HChoxserView',
    templateUrl: './hChoxser.component.html',
    styleUrls: ['./hChoxser.component.css']
})
export class HChoxserComponent extends DetailEmbeddedComponent<HChoxser> implements IDetailComponent, OnInit {


    EditEnabled: boolean = true;

    isLoading: boolean;
    isPopup: boolean;
    filterValue: HServiciosFilter;
    public dialog: MatDialog;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    @Output() modalClose: EventEmitter<any> = new EventEmitter<any>();



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

    duraciones: HChoxserExtendedDto[];
    duracion: HChoxserExtendedDto;
    disableFilter: boolean = false;
    hHorariosConfiDto: HHorariosConfiDto;
    subGalponDtoSelectItem: SelectItem[];
    subGalponDto: SubGalponDto[];
    tipoDiaDto: TipoDiaDto[];
    htposhoras: HTposHorasDto[];
    htposhorasSelectItem: SelectItem[];
    autoLoad: boolean = false;
    allowImportarDuracion: boolean = false;
    currentServicio: HServiciosDto;
    isValid: boolean = true;
    diaOrig: Date;
    @ViewChild('CodSubg') CodSubgCombo: SubgalponComboComponent;
    @ViewChild('CodTdia') CodTdia: TipoDiaComboComponent;
    @ViewChild('mediaVueltaTable') mediaVueltaTable: Table;

    @Input() parentEntity: HFechasConfiDto;


    getDescription(item: HChoxser): string {
        return item.Description;
    }

    AddItemBreadcrumbs(HFechasConfiDto) {

    }



    constructor(
        protected injector: Injector,
        protected service: HChoxserService,
        protected _hservice: HServiciosService,
        protected router: Router,
        protected _hHorariosConfiService: HHorariosConfiService,
        protected _HTposHorasService: HTposHorasService,
        protected hFechasConfiService: HFechasConfiService,
        private cfr: ComponentFactoryResolver

    ) {
        super(service, injector);
        this.Ultimofiltro = new HHorariosConfiFilter()
        this.detail = new HChoxser();
        this.icon = "flaticon-clock-2";
        this.title = "Duracion";
        this.dialog = injector.get(MatDialog);
        this.hHorariosConfiDto = new HHorariosConfiDto();
        this.allowImportarDuracion = this.permission.isGranted("Horarios.FechaHorario.ImportarDuracion");
        this.diaOrig = moment("2000-01-01").toDate();
        this.filter = new HServiciosFilter();

    }



    selectedItemChangeTpoHora($event: any, row: MediasVueltasDto): void {
        console.log($event);
        var s = this.htposhoras.find(e => e.Id == $event.value);
        if (s) {
            row.DescripcionTpoHora = s.DscTpoHora;
        }
    }



    selectedItemChangesubgalpon($event) {

        if (this.Ultimofiltro.CodSubg != this.filter.CodSubg) {
            this.BuscarDuraciones();
        }
    }

    selectedItemChangeCodTdia($event) {

        if (this.Ultimofiltro.CodTdia != this.filter.CodTdia) {
            this.BuscarDuraciones();
        }
    }





    subgalponitemstChange(items: SubGalponDto[]): void {

        if (items && items.length > 0) {
            this.subGalponDto = items;

            this.subGalponDtoSelectItem = this.subGalponDto.map(e => { return { label: e.DesSubg, value: e.Id } })

            this.filter.CodSubg = items[0].Id;
            this.BuscarDuraciones();
        }
    }
    tipoDiaitemstChange(items: TipoDiaDto[]): void {
        if (items && items.length > 0) {
            this.tipoDiaDto = items;
            this.filter.CodTdia = this.filter.CodTdia;
            this.BuscarDuraciones();
        }
    }


    ngAfterViewInit(): void {
        mApp.initPortlets();



    }

    completedataBeforeShow(item: HChoxser, horario?: HHorariosConfiDto): any {

        this.CodSubgCombo.CodHfecha = null;
        this.CodSubgCombo.CodHfecha = this.filter.CodHfecha;
        this.CodTdia.CodHfecha = null;
        this.CodTdia.CodHfecha = this.filter.CodHfecha;

        if (horario) {
            this.isValid = false;
            this.currentServicio = horario.CurrentServicio;
            this.filter.CodTdia = horario.CodTdia;
            this.filter.CodSubg = horario.CodSubg;
        }


        //f.CodHfecha = this.detail.Id;

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
    BuscarDuraciones(): void {

        if (this.filter.CodTdia && this.filter.CodSubg && this.filter.CodHfecha) {

            //Busco la cabecera para el tipo de dia y sub galpon
            this.Ultimofiltro = new HHorariosConfiFilter();
            this.Ultimofiltro.CodHfecha = this.filter.CodHfecha;
            this.Ultimofiltro.CodSubg = this.filter.CodSubg;
            this.Ultimofiltro.CodTdia = this.filter.CodTdia;
            this.Ultimofiltro.ServicioId = this.filter.ServicioId;

            var selft = this;

            //Busco los servicios para el tipo de dia y sub galpon
            this.isLoading = true;

            this._hHorariosConfiService.requestAllByFilter(this.Ultimofiltro)
                .finally(() => { this.isLoading = false })
                .subscribe(e => {

                    if (e.DataObject.Items.length > 0) {
                        this.hHorariosConfiDto = e.DataObject.Items[0];
                    }
                    else {
                        this.message.error("No se pudo recuperar el registro para persistir la cantidad de coches.")
                    }
                });

            this.service.RecuperarDuraciones(this.Ultimofiltro)
                .finally(() => {

                    this.agregarDuracion();
                    this.isLoading = false
                })
                .subscribe(e => {
                    this.duraciones = e.DataObject.map(e => {
                        var s = new HChoxserExtendedDto(e);

                        s.canEditSale = false;
                        s.canEditSaleR = false;
                        s.canEditSaleA = false;
                        s.canEditLlega = false;
                        s.canEditLlegaR = false;
                        s.canEditLlegaA = false;

                        if (s.Llega != null) {
                            s.Llega = moment(e.Llega).toDate();
                        }

                        if (e.LlegaRelevo != null) {
                            s.LlegaRelevo = moment(e.LlegaRelevo).toDate();
                        }

                        if (e.LlegaAuxiliar != null) {
                            s.LlegaAuxiliar = moment(e.LlegaAuxiliar).toDate();
                        }

                        if (s.Sale != null) {
                            s.Sale = moment(e.Sale).toDate();
                        }

                        if (s.SaleRelevo != null) {
                            s.SaleRelevo = moment(e.SaleRelevo).toDate();
                        }

                        if (s.SaleAuxiliar != null) {
                            s.SaleAuxiliar = moment(e.SaleAuxiliar).toDate();
                        }

                        if (selft.currentServicio && selft.currentServicio.Description == s.DescripcionServicio) {
                            s.HasChange = true;

                            if (s.Llega != null) {
                                s.canEditLlega = true;
                                s.isRequiredLlega = true;

                            } else {
                                s.canEditLlega = false;
                            }

                            if (e.LlegaRelevo != null) {
                                s.canEditLlegaR = true;
                                s.isRequiredLlegaR = true;

                            } else {
                                s.canEditLlegaR = false;
                            }

                            if (e.LlegaAuxiliar != null) {
                                s.canEditLlegaA = true;
                                s.isRequiredLlegaA = true;

                            } else {
                                s.canEditLlegaA = false;
                            }

                            if (s.Sale != null) {
                                s.canEditSale = true;
                                s.isRequiredSale = true;

                            } else {
                                s.canEditSale = false;
                            }

                            if (s.SaleRelevo != null) {
                                s.canEditSaleR = true;
                                s.isRequiredSaleR = true;

                            } else {
                                s.canEditSaleR = false;
                            }

                            if (s.SaleAuxiliar != null) {
                                s.canEditSaleA = true;
                                s.isRequiredSaleA = true;

                            } else {
                                s.canEditSaleA = false;
                            }
                        }
                        return s;
                    });

                    if (selft.isPopup) {
                        this.duraciones.forEach(e => selft.CompararSaleLlega(e));
                    }
                });

        }


    }

    agregarDuracion() {
        console.log(this.duraciones);
        if (this.currentServicio) {
            var newduracion = this.duraciones.find(e => e.DescripcionServicio == this.currentServicio.Description)
            if (!newduracion) {
                var s = new HChoxserExtendedDto();
                s.DescripcionServicio = this.currentServicio.Description;
                //s.Sale = this.diaOrig;
                //s.Llega = this.diaOrig;
                //s.SaleRelevo = this.diaOrig;
                //s.LlegaRelevo = this.diaOrig;
                //s.SaleAuxiliar = this.diaOrig;
                //s.LlegaAuxiliar = this.diaOrig;

                s.Sale = null;
                s.Llega = null;
                s.SaleRelevo = null;
                s.LlegaRelevo = null;
                s.SaleAuxiliar = null;
                s.LlegaAuxiliar = null;

                s.canEditSale = true;
                s.canEditSaleR = true;
                s.canEditSaleA = true;
                s.canEditLlega = true;
                s.canEditLlegaR = true;
                s.canEditLlegaA = true;

                s.HasChange = true;

                this.duraciones.push(s);
            }
        }

    }

    $CalcularDiaSaleLlega: Subject<HChoxserExtendedDto> = new Subject<HChoxserExtendedDto>();

    CalcularDiaSaleLlega(currentDto: HChoxserExtendedDto) {
        this.$CalcularDiaSaleLlega.next(currentDto);
    }

    CalcularDiaSaleLlegaCall(currentDto: HChoxserExtendedDto) {

        if (currentDto.Sale != null) {
            var sale = moment(currentDto.Sale);
            currentDto.Sale = moment(this.diaOrig).hours(sale.hours()).minutes(sale.minutes()).toDate();
        }

        if (currentDto.Llega != null) {
            var mom = moment(currentDto.Llega);
            currentDto.Llega = moment(this.diaOrig).hours(mom.hours()).minutes(mom.minutes()).toDate();
        }

        if (currentDto.SaleRelevo != null) {
            var mom = moment(currentDto.SaleRelevo);
            currentDto.SaleRelevo = moment(this.diaOrig).hours(mom.hours()).minutes(mom.minutes()).toDate();
        }

        if (currentDto.LlegaRelevo != null) {
            var mom = moment(currentDto.LlegaRelevo);
            currentDto.LlegaRelevo = moment(this.diaOrig).hours(mom.hours()).minutes(mom.minutes()).toDate();
        }


        if (currentDto.SaleAuxiliar != null) {
            var mom = moment(currentDto.SaleAuxiliar);
            currentDto.SaleAuxiliar = moment(this.diaOrig).hours(mom.hours()).minutes(mom.minutes()).toDate();
        }

        if (currentDto.LlegaAuxiliar != null) {
            var mom = moment(currentDto.LlegaAuxiliar);
            currentDto.LlegaAuxiliar = moment(this.diaOrig).hours(mom.hours()).minutes(mom.minutes()).toDate();
        }




        var ultimaFecha = currentDto.Sale || this.diaOrig;

        if (currentDto.Llega) {

            var mom = moment(currentDto.Llega);
            if (currentDto.Llega < ultimaFecha) {
                currentDto.Llega = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();

                if (currentDto.SaleRelevo) {
                    var mom = moment(currentDto.SaleRelevo);
                    currentDto.SaleRelevo = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                }
                if (currentDto.LlegaRelevo) {
                    var mom = moment(currentDto.LlegaRelevo);
                    currentDto.LlegaRelevo = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                }
                if (currentDto.SaleAuxiliar) {
                    var mom = moment(currentDto.SaleAuxiliar);
                    currentDto.SaleAuxiliar = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                }
                if (currentDto.LlegaAuxiliar) {
                    var mom = moment(currentDto.LlegaAuxiliar);
                    currentDto.LlegaAuxiliar = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                }
                ultimaFecha = currentDto.Llega;
            }
        }

        ultimaFecha = currentDto.Llega || currentDto.Sale || this.diaOrig;

        if (currentDto.SaleRelevo) {
            var mom = moment(currentDto.SaleRelevo);
            if (currentDto.SaleRelevo < ultimaFecha) {
                currentDto.SaleRelevo = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                if (currentDto.LlegaRelevo) {
                    var mom = moment(currentDto.LlegaRelevo);
                    currentDto.LlegaRelevo = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                }
                if (currentDto.SaleAuxiliar) {
                    var mom = moment(currentDto.SaleAuxiliar);
                    currentDto.SaleAuxiliar = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                }
                if (currentDto.LlegaAuxiliar) {
                    var mom = moment(currentDto.LlegaAuxiliar);
                    currentDto.LlegaAuxiliar = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                }
                ultimaFecha = currentDto.SaleRelevo;
            }
        }

        ultimaFecha = currentDto.SaleRelevo || currentDto.Llega || currentDto.Sale || this.diaOrig;

        if (currentDto.LlegaRelevo) {
            var mom = moment(currentDto.LlegaRelevo);
            if (currentDto.LlegaRelevo < ultimaFecha) {
                currentDto.LlegaRelevo = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                if (currentDto.SaleAuxiliar) {
                    var mom = moment(currentDto.SaleAuxiliar);
                    currentDto.SaleAuxiliar = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                }
                if (currentDto.LlegaAuxiliar) {
                    var mom = moment(currentDto.LlegaAuxiliar);
                    currentDto.LlegaAuxiliar = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                }
                ultimaFecha = currentDto.LlegaRelevo;
            }
        }
        ultimaFecha = currentDto.LlegaRelevo || currentDto.SaleRelevo || currentDto.Llega || currentDto.Sale || this.diaOrig;


        if (currentDto.SaleAuxiliar) {
            var mom = moment(currentDto.SaleAuxiliar);
            if (currentDto.SaleAuxiliar < ultimaFecha) {
                currentDto.SaleAuxiliar = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                if (currentDto.LlegaAuxiliar) {
                    var mom = moment(currentDto.LlegaAuxiliar);
                    currentDto.LlegaAuxiliar = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                }
                ultimaFecha = currentDto.SaleAuxiliar;
            }
        }

        ultimaFecha = currentDto.SaleAuxiliar || currentDto.LlegaRelevo || currentDto.SaleRelevo || currentDto.Llega || currentDto.Sale || this.diaOrig;

        if (currentDto.LlegaAuxiliar) {
            var mom = moment(currentDto.LlegaAuxiliar);
            if (currentDto.LlegaAuxiliar < ultimaFecha) {
                currentDto.LlegaAuxiliar = moment(ultimaFecha).add(1, 'days').hours(mom.hours()).minutes(mom.minutes()).toDate();
                ultimaFecha = currentDto.LlegaAuxiliar;
            }
        }

        this.CompararSaleLlega(currentDto);
        this.ValidarSaleLlegaCoches(currentDto);
        this.ValidarRequired(currentDto);

    }

    CompararSaleLlega(currentDto: HChoxserExtendedDto) {

        this.isValid = true;

        currentDto.HasError = false;
        currentDto.HasErrorLlega = false;
        currentDto.HasErrorSale = false;
        currentDto.HasErrorLlegaR = false;
        currentDto.HasErrorSaleR = false;
        currentDto.HasErrorLlegaA = false;
        currentDto.HasErrorSaleA = false;
        currentDto.ErrorMessages = [];

        var servSale = this.currentServicio.HMediasVueltas.find(e => e.Orden == 1).Sale;
        var servLlega = this.currentServicio.HMediasVueltas.find(e => e.Orden == (this.currentServicio.HMediasVueltas.length)).Llega;
        servSale = moment(servSale).toDate();
        servLlega = moment(servLlega).toDate();

        if (currentDto.Sale != null) {
            var sale = moment(currentDto.Sale);
            if (currentDto.Sale.getTime() != servSale.getTime()) {
                currentDto.HasError = true;
                currentDto.HasErrorSale = true;
                currentDto.ErrorMessages.push("El sale no es igual al sale de la 1ra. MV");
                this.isValid = false;
            }
        }

        else if (currentDto.SaleRelevo != null) {
            var sale = moment(currentDto.SaleRelevo);
            if (currentDto.SaleRelevo.getTime() != servSale.getTime()) {
                currentDto.HasError = true;
                currentDto.HasErrorSaleR = true;
                currentDto.ErrorMessages.push("El sale no es igual al sale de la 1ra. MV");
                this.isValid = false;
            }
        }

        else if (currentDto.SaleAuxiliar) {
            var sale = moment(currentDto.SaleAuxiliar);
            if (currentDto.SaleAuxiliar.getTime() != servSale.getTime()) {
                currentDto.HasError = true;
                currentDto.HasErrorSaleA = true;
                currentDto.ErrorMessages.push("El sale no es igual al sale de la 1ra. MV");
                this.isValid = false;
            }
        }


        if (currentDto.LlegaAuxiliar != null) {
            var llega = moment(currentDto.LlegaAuxiliar);
            if (currentDto.LlegaAuxiliar.getTime() != servLlega.getTime()) {
                currentDto.HasError = true;
                currentDto.HasErrorLlegaA = true;
                currentDto.ErrorMessages.push("El llega no es igual al llega de la última MV");
                this.isValid = false;
            }
        }
        else if (currentDto.LlegaRelevo != null) {
            var llega = moment(currentDto.LlegaRelevo);
            if (currentDto.LlegaRelevo.getTime() != servLlega.getTime()) {
                currentDto.HasError = true;
                currentDto.HasErrorLlegaR = true;
                currentDto.ErrorMessages.push("El llega no es igual al llega de la última MV");
                this.isValid = false;
            }
        }
        else if (currentDto.Llega != null) {
            var llega = moment(currentDto.Llega);
            if (currentDto.Llega.getTime() != servLlega.getTime()) {
                currentDto.HasError = true;
                currentDto.HasErrorLlega = true;
                currentDto.ErrorMessages.push("El llega no es igual al llega de la última MV");
                this.isValid = false;
            }
        }
    }

    ValidarSaleLlegaCoches(currentDto: HChoxserExtendedDto) {

        if (currentDto.Sale && currentDto.SaleRelevo != null && currentDto.SaleRelevo < currentDto.Llega) {
            currentDto.HasError = true;
            currentDto.HasErrorLlega = true;
            currentDto.ErrorMessages.push("Verifique Llega del primer coche se solapa con el Sale del segundo coche");
            this.isValid = false;
        } else if (currentDto.SaleRelevo && currentDto.SaleAuxiliar && currentDto.SaleAuxiliar < currentDto.LlegaRelevo) {
            currentDto.HasError = true;
            currentDto.HasErrorLlegaR = true;
            currentDto.ErrorMessages.push("Verifique Llega del primer coche se solapa con el Sale del segundo coche");
            this.isValid = false;
        }

        if (currentDto.Sale && currentDto.Sale > currentDto.Llega) {
            currentDto.HasError = true;
            currentDto.HasErrorSale = true;
            currentDto.ErrorMessages.push("Verifique Sale mayor que LLega");
            this.isValid = false;

        } else if (currentDto.SaleRelevo && currentDto.SaleRelevo > currentDto.LlegaRelevo) {
            currentDto.HasError = true;
            currentDto.HasErrorSaleR = true;
            currentDto.ErrorMessages.push("Verifique Sale mayor que LLega");
            this.isValid = false;

        } else if (currentDto.SaleAuxiliar && currentDto.SaleAuxiliar > currentDto.LlegaAuxiliar) {
            currentDto.HasError = true;
            currentDto.HasErrorSaleA = true;
            currentDto.ErrorMessages.push("Verifique Sale mayor que LLega");
            this.isValid = false;
        }
    }

    ValidarRequired(currentDto: HChoxserExtendedDto) {
        if (currentDto.Sale != null && currentDto.Llega == null || currentDto.Sale == null && currentDto.Llega != null) {
            currentDto.HasError = true;
            if (currentDto.Llega == null) {
                currentDto.HasErrorLlega = true;
                currentDto.ErrorMessages.push("El Llega de la cantidad 1 es requerido");
                this.isValid = false;
            } else {
                currentDto.HasErrorSale = true;
                currentDto.ErrorMessages.push("El Sale de la cantidad 1 es requerido");
                this.isValid = false;
            }

        }

        if (currentDto.SaleRelevo != null && currentDto.LlegaRelevo == null || currentDto.SaleRelevo == null && currentDto.LlegaRelevo != null) {
            currentDto.HasError = true;
            if (currentDto.LlegaRelevo == null) {
                currentDto.HasErrorLlegaR = true;
                currentDto.ErrorMessages.push("El Llega de la cantidad 2 es requerido");
                this.isValid = false;
            } else {
                currentDto.HasErrorSaleR = true;
                currentDto.ErrorMessages.push("El Sale de la cantidad 2 es requerido");
                this.isValid = false;
            }

        }

        if (currentDto.SaleAuxiliar != null && currentDto.LlegaAuxiliar == null || currentDto.SaleAuxiliar == null && currentDto.LlegaAuxiliar != null) {
            currentDto.HasError = true;
            if (currentDto.LlegaAuxiliar == null) {
                currentDto.HasErrorLlegaA = true;
                currentDto.ErrorMessages.push("El Llega de la cantidad 3 es requerido");
                this.isValid = false;
            } else {
                currentDto.HasErrorSaleA = true;
                currentDto.ErrorMessages.push("El Sale de la cantidad 3 es requerido");
                this.isValid = false;
            }

        }
    }
    SaveDetail(): any {
        this.saving = false;
        this.duracion = this.duraciones.find(e => e.HasChange == true);
        let dur: HChoxserExtendedDto = null;
        if (this.duracion) {

            dur = JSON.parse(JSON.stringify(this.duracion));

            if (dur.Sale != null) {
                dur.Sale = (moment(dur.Sale).format("YYYY-MM-DDTHH:mm:ss") as any);
            }
            if (dur.Llega != null) {
                dur.Llega = (moment(dur.Llega).format("YYYY-MM-DDTHH:mm:ss") as any);
            }
            if (dur.SaleRelevo != null) {
                dur.SaleRelevo = (moment(dur.SaleRelevo).format("YYYY-MM-DDTHH:mm:ss") as any);
            }
            if (dur.LlegaRelevo != null) {
                dur.LlegaRelevo = (moment(dur.LlegaRelevo).format("YYYY-MM-DDTHH:mm:ss") as any);
            }
            if (dur.SaleAuxiliar != null) {
                dur.SaleAuxiliar = (moment(dur.SaleAuxiliar).format("YYYY-MM-DDTHH:mm:ss") as any);
            }
            if (dur.LlegaAuxiliar != null) {
                dur.LlegaAuxiliar = (moment(dur.LlegaAuxiliar).format("YYYY-MM-DDTHH:mm:ss") as any);
            }
        }
        let horarioDuracion: HorarioDuracion = new HorarioDuracion();
        horarioDuracion.Duracion = dur
        horarioDuracion.Horario = JSON.parse(JSON.stringify(this.hHorariosConfiDto));
        if (this.currentServicio) {
            horarioDuracion.Horario.CurrentServicio = JSON.parse(JSON.stringify(this.currentServicio));
        }


        let callbackFunction = function(selft: HChoxserComponent) {
            selft.saving = true;
            if (!selft.currentServicio) {
                selft._hHorariosConfiService.updateCantidadCochesReales(selft.hHorariosConfiDto)
                    .finally(() => { selft.saving = false; })
                    .subscribe((t) => {
                        selft.notify.info('Guardado exitosamente');

                        if (t.Messages && t.Messages.length > 0) {
                            selft.notify.info(t.Messages.join(','));
                        }
                        //selft.modalSave.emit(null);

                    });
            }
            else {
                selft.service.createOrUpdateDurYSer(horarioDuracion)
                    .finally(() => selft.saving = false)
                    .subscribe(e => {
                        selft.notify.info('Guardado exitosamente');

                        if (e.Messages && e.Messages.length > 0) {
                            selft.notify.info(e.Messages.join(','));
                        }

                        if (selft.isPopup) {
                            selft.dialog.closeAll();
                        }
                        selft.modalSave.emit(null);
                    });
            }




        }
        var self = this;

        let idServ;
        if (this.currentServicio) {
            idServ = this.currentServicio.Id;
        }

        //verificamos si esta diagramado 
        this.hFechasConfiService.HorarioDiagramado(this.hHorariosConfiDto.CodHfecha, idServ).subscribe(resp => {
            if (resp.DataObject) {
                this.message.confirm("El servicio esta diagramado. ¿Desea continuar.?", "Diagramacion", (c) => {
                    if (c.value) {
                        callbackFunction(self);
                    }
                });
            }
            else {
                callbackFunction(self);
            }
        })


    }

    ngOnInit(): void {
        super.ngOnInit();

        this.$CalcularDiaSaleLlega
            .debounceTime(2000)
            //.distinctUntilChanged()
            // .switchMap(() => { })
            .subscribe(e => {
                this.CalcularDiaSaleLlegaCall(e);
            });
    }

    showImportarDuracion() {

        if (this.filter.CodTdia) {
            var dialog: MatDialog;
            dialog = this.injector.get(MatDialog);
            const dialogConfig = new MatDialogConfig();
            dialogConfig.disableClose = false;
            dialogConfig.autoFocus = true;

            this.filter.DescTdia = this.CodTdia.items.find(e => e.Id == this.filter.CodTdia).Description;

            dialogConfig.data = this.filter;


            let dialogRef = dialog.open(ImportarDuracionComponent, dialogConfig);

            dialogRef.afterClosed().subscribe(
                data => {
                    if (data) {
                        this.BuscarDuraciones();
                        //this.modalClose.emit(null);
                    }
                }
            );


        }
        else {

            this.notify.error("El tipo de dia es obligatorio", "Error");
        }

    }


    ngOnDestroy(): void {
        this.modalClose.unsubscribe();
        super.ngOnDestroy();
    }

    cleanData() {

        this.filter = new HServiciosFilter();
        this.duraciones = [];
        this.hHorariosConfiDto = new HHorariosConfiDto();
        this.subGalponDtoSelectItem = [];
        this.subGalponDto = [];
        this.tipoDiaDto = [];
        this.currentServicio = null;

    }

    onCellEdit(event) {
        setTimeout(() => {
            var element = "#" + event.field + "_" + event.data.DescripcionServicio + " input";
            $(element).select();
        }, 20);

    }
    confirm: string = '¿Está seguro de que desea cancelar?';
    close(): void {
        this.message.confirm(this.confirm, 'Confirmación', (a) => {
            if (a.value) {
                if (this.isPopup) {
                    this.dialog.closeAll();
                }
                else {
                    super.close();
                }
            }
        });
    }

}
