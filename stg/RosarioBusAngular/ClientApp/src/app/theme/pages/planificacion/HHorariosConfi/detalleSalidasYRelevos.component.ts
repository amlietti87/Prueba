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
import { HHorariosConfiDto, HHorariosConfiFilter, DetalleSalidaRelevosFilter } from '../model/hhorariosconfi.model';
import { HHorariosConfiService } from './hhorariosconfi.service';
import { CreateOrEditDetalleSalidasYRelevos } from './create-or-edit-detalleSalidasYRelevos.component';


@Component({

    templateUrl: "./detalleSalidasYRelevos.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class DetalleSalidasYRelevosComponent extends BaseCrudComponent<HHorariosConfiDto, HHorariosConfiFilter> implements AfterViewInit {
    generando: boolean;
    linea: ItemDto;
    LineaId: number;
    CodHfecha: number;
    codTdia: number;


    @ViewChild('SabanaFilterForm') filterForm: NgForm;

    constructor(injector: Injector,
        protected _service: HHorariosConfiService

    ) {

        super(_service, CreateOrEditDetalleSalidasYRelevos, injector);

        this.isFirstTime = true;
        this.title = " Detalle Salidas y Relevos";
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


        this.generando = true;


        var filtro = new DetalleSalidaRelevosFilter();

        filtro.codTdia = this.codTdia;
        filtro.cod_hfecha = this.CodHfecha;
        filtro.cod_lin = this.LineaId;

        this._service.reporteDetalleSalidasYRelevos(filtro);

        this.generando = false;
    }


    OnLineaChange(event: SimpleChanges) {




        if (this.linea) {
            this.LineaId = this.linea.Id;
        }



    };

    close() {

    }
}
