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
import { LineaDto, LineaFilter } from '../model/linea.model';
import { moment } from 'ngx-bootstrap/chronos/test/chain';
import { SelectItem } from 'primeng/api';
import { LineaAutoCompleteComponent } from '../shared/linea-autocomplete.component';
import { SentidoBanderaComboComponent } from '../shared/sentidoBandera-combo.component';
import { BanderaComboComponent } from '../shared/bandera-combo.component';
import { NgForm } from '@angular/forms';
import { HHorariosConfiDto, HHorariosConfiFilter, DetalleSalidaRelevosFilter, ReportePasajerosFilter } from '../model/hhorariosconfi.model';
import { HHorariosConfiService } from './hhorariosconfi.service';
import { CreateOrEditDetalleSalidasYRelevos } from './create-or-edit-detalleSalidasYRelevos.component';
import { LineaService } from '../linea/linea.service';
import { CreateOrEditLineaModalComponent } from '../linea/create-or-edit-linea-modal.component';


@Component({

    templateUrl: "./reportePasajeros.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class ReportePasajerosComponent extends BaseCrudComponent<HHorariosConfiDto, HHorariosConfiFilter> implements AfterViewInit {
    generando: boolean;

    linea: ItemDto;
    LineaId: number;
    RamalColorId: number;

    BanderasSource: BanderaDto[] = [];
    BanderasTarget: BanderaDto[] = [];


    @ViewChild('FilterForm') filterForm: NgForm;

    constructor(injector: Injector,
        protected _service: HHorariosConfiService,
        protected _banderaService: BanderaService

    ) {

        super(_service, CreateOrEditLineaModalComponent, injector);

        this.isFirstTime = true;
        this.title = "Reporte para Pasajeros";
        this.moduleName = "Administración";
        this.icon = "flaticon-settings";
        this.showbreadcum = false;


    }

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditLineaModalComponent;
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


        var filtro = new ReportePasajerosFilter();

        filtro.LineaId = this.LineaId;

        if (this.BanderasTarget.length == 0) {
            this.notificationService.error("Debe seleccionar al menos una bandera para generar el reporte");
            return;
        }

        this.BanderasTarget.forEach(e => {
            filtro.Banderas.push(e.Id);
        });

        this._service.ReporteParadasPasajeros(filtro);

        this.generando = false;
    }


    OnLineaChange(event: SimpleChanges) {

        if (this.linea) {
            this.LineaId = this.linea.Id;
        }

        this.BanderasSource = [];
        this.BanderasTarget = [];

        var banderafilter = new BanderaFilter();
        banderafilter.LineaId = this.LineaId;

        this._banderaService.requestAllByFilter(banderafilter).subscribe(e => {
            if (e.DataObject) {
                this.BanderasSource = e.DataObject.Items;
            }
        });

    };

    close() {

    }
}
