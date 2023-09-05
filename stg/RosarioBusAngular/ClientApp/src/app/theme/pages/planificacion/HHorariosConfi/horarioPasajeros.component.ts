import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, Input, SimpleChanges } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Helpers } from '../../../../helpers';
import { ScriptLoaderService } from '../../../../services/script-loader.service';
import { AppComponentBase } from '../../../../shared/common/app-component-base';

import { LazyLoadEvent } from 'primeng/components/common/lazyloadevent';

import { DataTable } from 'primeng/components/datatable/datatable';
import { Paginator } from 'primeng/components/paginator/paginator';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';

import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { SentidoBanderaDto } from '../model/sentidoBandera.model';

import { HorariosPorSectorDto } from '../model/horariosPorSector.model';
import { BanderaService } from '../bandera/bandera.service';
import { BanderaDto, BanderaFilter } from '../model/bandera.model';
import { ItemDto, ResponseModel, SabanaViewMode } from '../../../../shared/model/base.model';
import { SentidoBanderaService } from '../sentidoBandera/sentidoBandera.service';
import { retry } from 'rxjs/operators';
import { LineaDto } from '../model/linea.model';
import { moment } from 'ngx-bootstrap/chronos/test/chain';
import { SelectItem } from 'primeng/api';
import { LineaAutoCompleteComponent } from '../shared/linea-autocomplete.component';
import { SentidoBanderaComboComponent } from '../shared/sentidoBandera-combo.component';
import { BanderaComboComponent } from '../shared/bandera-combo.component';
import { NgForm } from '@angular/forms';
import { HHorariosConfiDto, HHorariosConfiFilter, DetalleSalidaRelevosFilter, ReporteHorarioPasajerosFilter } from '../model/hhorariosconfi.model';
import { HHorariosConfiService } from './hhorariosconfi.service';
import { CreateOrEditDetalleSalidasYRelevos } from './create-or-edit-detalleSalidasYRelevos.component';


@Component({

    templateUrl: "./horarioPasajeros.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class HorarioPasajerosComponent extends BaseCrudComponent<HHorariosConfiDto, HHorariosConfiFilter> implements AfterViewInit {
    generando: boolean;
    linea: ItemDto;
    LineaId: number;
    CodHfecha: number;
    codTdia: number;

    sentidoOrigenId: number;
    sentidoDestinoId: number;
    BanderasOrigenSource: BanderaDto[];
    BanderasDestinoSource: BanderaDto[];
    BanderasOrigentarget: BanderaDto[] = [];
    BanderasDestinotarget: BanderaDto[] = [];



    @ViewChild('SabanaFilterForm') filterForm: NgForm;
    @ViewChild('SentidoOrigenCombo') SentidoOrigen: SentidoBanderaComboComponent
    @ViewChild('SentidoDestinoCombo') SentidoDestino: SentidoBanderaComboComponent

    constructor(injector: Injector,
        protected _service: HHorariosConfiService,
        protected _banderaService: BanderaService

    ) {

        super(_service, CreateOrEditDetalleSalidasYRelevos, injector);

        this.isFirstTime = true;
        this.title = " Horarios para Pasajeros";
        this.moduleName = "Administración";
        this.icon = "flaticon-settings";
        this.showbreadcum = false;


    }

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditDetalleSalidasYRelevos;
    };











    getNewfilter(): HHorariosConfiFilter {
        var f = new HHorariosConfiFilter();
        return f;
    }

    Search(event?: LazyLoadEventData) {


    }

    GetReporte() {


        this.filterForm.onSubmit(null);

        if (!this.filterForm.valid) {
            return;
        }

        if (this.BanderasOrigentarget.length <= 0 || this.BanderasDestinotarget.length <= 0) {
            this.message.error("Complete las banderas de Origen y de Destino", "Error");
            return;
        }


        this.generando = true;


        var filtro = new ReporteHorarioPasajerosFilter();

        filtro.codTdia = this.codTdia;
        filtro.codHfecha = this.CodHfecha;
        filtro.cod_lin = this.LineaId;
        var sentidoOrig = this.SentidoOrigen.items.find(e => e.Id == this.sentidoOrigenId)
        filtro.SentidoOrigen = sentidoOrig.Description;
        var sentidoDest = this.SentidoDestino.items.find(e => e.Id == this.sentidoDestinoId)
        filtro.SentidoDestino = sentidoDest.Description;
        this.BanderasOrigentarget.forEach(b => filtro.BanderasIda.push(b.Id));

        this.BanderasDestinotarget.forEach(b => filtro.BanderasVueltas.push(b.Id));


        this._service.reporteHorarioPasajeros(filtro);

        this.generando = false;
    }


    OnLineaChange(event: SimpleChanges) {
        this.filterForm.controls['cod_hfecha'].reset();
        this.filterForm.controls['FilterCodTdiaDetino'].reset();
        this.filterForm.controls['sentidoOrigenId'].reset();
        this.BanderasOrigenSource = [];
        this.BanderasOrigentarget = [];
        this.filterForm.controls['sentidoDestinoId'].reset();
        this.BanderasDestinoSource = [];
        this.BanderasDestinotarget = [];


        if (this.linea) {
            this.LineaId = this.linea.Id;
        }



    };

    OnfechasconfiChange(event: SimpleChanges) {




        if (this.linea) {
            this.LineaId = this.linea.Id;
        }

        this.BanderasOrigenSource = [];
        this.BanderasDestinoSource = [];

        this.BanderasOrigentarget = [];
        this.BanderasDestinotarget = [];


    };


    OnSentidoDestinoChange(event: SimpleChanges) {
        this.BanderasDestinoSource = [];
        this.BanderasDestinotarget = [];

        if (this.LineaId && this.sentidoDestinoId && this.CodHfecha) {

            this._banderaService.GetItemsAsync({ LineaId: this.LineaId, CodHfecha: this.CodHfecha, SentidoBanderaId: this.sentidoDestinoId })
                .subscribe(e => {
                    e.DataObject.forEach(b => this.BanderasDestinoSource.push(new BanderaDto(b)));
                });
        }
    };

    OnSentidoOrigenChange(event: SimpleChanges) {
        this.BanderasOrigenSource = [];
        this.BanderasOrigentarget = [];

        if (this.LineaId && this.sentidoOrigenId && this.CodHfecha) {

            this._banderaService.GetItemsAsync({ LineaId: this.LineaId, CodHfecha: this.CodHfecha, SentidoBanderaId: this.sentidoOrigenId })
                .subscribe(e => {
                    e.DataObject.forEach(b => this.BanderasOrigenSource.push(new BanderaDto(b)));
                });
        }

    };
    close() {

    }
}
