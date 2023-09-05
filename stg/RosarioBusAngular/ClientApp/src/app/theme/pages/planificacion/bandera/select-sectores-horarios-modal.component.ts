import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '../../../../shared/common/app-component-base';

import * as _ from 'lodash';
import { DetailEmbeddedComponent, DetailModalComponent } from '../../../../shared/manager/detail.component';
import { BanderaDto } from '../model/bandera.model';
import { BanderaService } from './bandera.service';
import { RutaService } from '../ruta/ruta.service';
import { RutaDto, RutaFilter, MinutosPorSectorFilter } from '../model/ruta.model';
import { TipoParadaService } from '../tipoParada/tipoparada.service';
import { TipoDiaService } from '../tipoDia/tipodia.service';
import { TipoDiaDto } from '../model/tipoDia.model';
import { SectorService } from '../sector/sector.service';
import { SectorFilter } from '../model/sector.model';
import { ItemDto } from '../../../../shared/model/base.model';

declare let mApp: any;


@Component({
    selector: 'selectSectoresHorarios',
    templateUrl: './select-sectores-horarios-modal.component.html',

})
export class SelectSectoresHorariosModalComponent extends AppComponentBase {

    @ViewChild('selectSectoresHorarios') modal: ModalDirective;

    _tipoParadaService: TipoDiaService;
    _sectorService: SectorService;
    _serviceRuta: RutaService;

    TiposDias: TipoDiaDto[];
    Sectores: ItemDto[];
    SectoresSelected: ItemDto[];
    row: RutaDto;

    saving = false;
    resettingPermissions = false;

    userId: number;
    userName: string;
    TipodeDiaId: number;

    constructor(
        injector: Injector,
        banderaService: BanderaService,
        tipoParadaService: TipoDiaService,
        sectorService: SectorService,
        serviceRuta: RutaService
    ) {
        super(injector);
        this._tipoParadaService = tipoParadaService;
        this._sectorService = sectorService;
        this._serviceRuta = serviceRuta;
    }

    ngOnInit(): void {
        this._tipoParadaService.requestAllByFilter().subscribe(result => {
            this.TiposDias = result.DataObject.Items;
            if (this.TiposDias.length > 0)
                this.TipodeDiaId = this.TiposDias[0].Id;
        });
    }

    show(_row: RutaDto): void {
        this.row = _row;
        var sectorfilter = new SectorFilter();
        sectorfilter.RutaId = this.row.Id;
        this.SectoresSelected = [];
        this._sectorService.FindItemsAsync(sectorfilter).subscribe(result => {
            this.Sectores = result.DataObject;
        });

        this.modal.show();
    }

    save(): void {
        this.ExportarDemoras();
    }

    close(): void {
        this.modal.hide();
    }


    ExportarDemoras() {

        var filter = new MinutosPorSectorFilter();
        filter.Id = this.row.Id;
        filter.SectoresIds = this.SectoresSelected.map(function(x) {
            return x.Id;
        });
        filter.TipoDiaId = this.TipodeDiaId;
        this._serviceRuta.MinutosPorSectorReporte(filter);
    }
}
