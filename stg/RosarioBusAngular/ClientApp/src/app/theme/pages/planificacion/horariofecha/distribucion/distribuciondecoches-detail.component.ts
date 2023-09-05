import { Component, ViewChild, Injector, ViewContainerRef, Inject, OnInit, AfterViewInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogConfig, MatDialog } from '@angular/material';
import { NgForm } from '@angular/forms';
import { DetailAgregationComponent } from '../../../../../shared/manager/detail.component';
import { PlaDistribucionDeCochesPorTipoDeDiaDto, HFechasConfiDto } from '../../model/HFechasConfi.model';
import { HFechasConfiService, PlaDistribucionDeCochesPorTipoDeDiaService } from '../HFechasConfi.service';
import { TipoDiaComboComponent } from '../../shared/tipoDia-combo.component';
import { DescripcionTipoDiaPredictivoComponent } from '../../shared/descripcionTipoDia-predictivo.component';




@Component({
    selector: 'distribuciondecoches-detail',
    templateUrl: './distribuciondecoches-detail.component.html',
    //styleUrls: ['./distribuciondecoches-detail.component.css']
})
export class DistribucionDeCochesDetailComponent extends DetailAgregationComponent<PlaDistribucionDeCochesPorTipoDeDiaDto> implements OnInit, AfterViewInit {

    @ViewChild('detailForm') detailForm: NgForm;
    @ViewChild('tipoDiaCombo') tipoDiaCombo: TipoDiaComboComponent;
    @ViewChild('PredictivoDesTDia') PredictivoDesTDia: DescripcionTipoDiaPredictivoComponent;
    parentEntity: HFechasConfiDto;

    public dialog: MatDialog
    constructor(
        protected dialogRef: MatDialogRef<DistribucionDeCochesDetailComponent>,
        protected service: PlaDistribucionDeCochesPorTipoDeDiaService,
        injector: Injector,
        @Inject(MAT_DIALOG_DATA) public data: PlaDistribucionDeCochesPorTipoDeDiaDto
    ) {
        super(dialogRef, null, injector, data);

        this.dialog = injector.get(MatDialog);

    }

    ngOnInit() {
        super.ngOnInit();

    }

    ngAfterViewInit() {

    }

    validateSave(): boolean {
        if (this.parentEntity && this.parentEntity.DistribucionDeCochesPorTipoDeDia) {
            var x = this.parentEntity.DistribucionDeCochesPorTipoDeDia.findIndex(e =>
                (e.Id != this.detail.Id) &&
                e.CodTdia == this.detail.CodTdia
            );

            if (x != -1) {
                this.message.error("ya existe una planificacion para este tipo de dia", "Error");
                return false;
            }
        }

        return true;
    }

    onChangeTipoDedia(event) {
        var x = this.tipoDiaCombo.items.find(e => e.Id == this.detail.CodTdia);
        if (x) {
            this.detail.TipoDediaDescripcion = x.DesTdia;
        }

        this.PredictivoDesTDia.Refrescar();

    }



}
