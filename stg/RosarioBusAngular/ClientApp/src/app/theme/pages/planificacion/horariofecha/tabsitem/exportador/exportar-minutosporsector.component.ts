import { Component, ViewChild, Injector, Inject, OnInit, AfterViewInit, Input, EventEmitter, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { NgForm } from '@angular/forms';
import { AppComponentBase } from '../../../../../../shared/common/app-component-base';
import { BanderaComboComponent } from '../../../shared/bandera-combo.component';
import { ExportarExcelDto } from '../../../model/hServicios.model';
import { TipoDiaComboComponent } from '../../../shared/tipoDia-combo.component';
import { HMinxtipoService } from '../../../hminxtipo/hminxtipo.service';
import { HTposHorasService } from '../../../htposhoras/htposhoras.service';
import { HTposHorasDto } from '../../../model/htposhoras.model';
import { SelectItem } from 'primeng/primeng';
import { HMinxtipoFilter } from '../../../model/hminxtipo.model';


@Component({
    selector: 'exportarminutosporsector',
    templateUrl: "./exportar-minutosporsector.component.html",
})
export class ExportarMinutosPorSectorComponent extends AppComponentBase implements OnInit, AfterViewInit {

    @ViewChild('banderaComboMinutos') banderaCombo: BanderaComboComponent;
    @ViewChild('codTipoDiaCombo') codTipoDiaCombo: TipoDiaComboComponent;
    @ViewChild('exporExcelForm') exporExcelForm: NgForm;
    htposhoras: HTposHorasDto[];
    htposhorasSelectItem: SelectItem[];
    filter: HMinxtipoFilter;
    isExporting: boolean = false;

    constructor(
        protected dialogRef: MatDialogRef<ExportarMinutosPorSectorComponent>,
        @Inject(MAT_DIALOG_DATA) public data: HMinxtipoFilter,
        protected service: HMinxtipoService,
        protected _HTposHorasService: HTposHorasService,
        injector: Injector) {

        super(injector);

        this.filter = data;
    }

    ngOnInit() {

        this._HTposHorasService.requestAllByFilter({})
            .finally(() => {
                //this.FiltroTipoHoraCombo.isLoading = false;
            })
            .subscribe(result => {

                var ordenado = result.DataObject.Items.sort((a, b) => {
                    if (a.Orden < b.Orden) return -1;
                    else if (a.Orden > b.Orden) return 1;
                    else return 0;
                });
                // ordenado.unshift( {CodTpoHorabsas: null, Description: "Todos", DscTpoHora: "Todos", Id: null, Orden: null } );
                this.htposhoras = ordenado;
                this.htposhorasSelectItem = this.htposhoras.map(e => { return { label: e.DscTpoHora, value: e.Id } })
                this.htposhorasSelectItem.unshift({ label: "Todos", value: "" })
                if (this.htposhoras && this.htposhoras.length > 0) {
                    this.filter.TipoHora = "";
                }


            });
    }
    close(): void {
        this.dialogRef.close(true);
    }

    ngAfterViewInit() {

        setTimeout(() => {
            this.banderaCombo.CodHfecha = this.filter.CodHfecha;
            this.codTipoDiaCombo.CodHfecha = this.filter.CodHfecha;
        });

    }

    export() {
        if (this.exporExcelForm.valid) {
            this.filter.BanderasId = [];
            if (this.filter.CodBan == null) {
                this.banderaCombo.items.map(e => {
                    this.filter.BanderasId.push(e.Id)
                }
                );
            } else {
                this.filter.BanderasId.push(this.filter.CodBan)
                this.filter.CodBan = null;
            }
            this.isExporting = true;
            var selft = this;
            this.service.GetReporteExcelMinxSec(this.filter, function() { selft.dialogRef.close(true); selft.isExporting = false });


        }
    }


}