import { Component, ViewChild, Injector, Inject, OnInit, AfterViewInit, Input, EventEmitter, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { NgForm } from '@angular/forms';
import { HServiciosDto, ExportarExcelDto } from '../../model/hServicios.model';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { SubgalponComboComponent } from '../../shared/subgalpon-combo.component';
import { TipoDiaComboComponent } from '../../shared/tipoDia-combo.component';
import { HFechasConfiService } from '../HFechasConfi.service';





@Component({
    selector: 'servicio-new',
    templateUrl: './exportar-excel.component.html',

})
export class ExportarExcelComponent extends AppComponentBase implements OnInit, AfterViewInit {

    @ViewChild('detailForm') detailForm: NgForm;

    isLoading: boolean;
    detail: ExportarExcelDto;
    @ViewChild('CodSubg') CodSubgCombo: SubgalponComboComponent;
    @ViewChild('CodTdia') CodTdia: TipoDiaComboComponent;



    constructor(
        public dialogRef: MatDialogRef<ExportarExcelComponent>,
        injector: Injector,
        protected service: HFechasConfiService,
        @Inject(MAT_DIALOG_DATA) data: ExportarExcelDto
    ) {
        super(injector);
        this.detail = data;

    }

    ngOnInit() {


    }

    ngAfterViewInit() {

        setTimeout(() => {
            this.CodSubgCombo.CodHfecha = this.detail.CodHfecha;
            this.CodTdia.CodHfecha = this.detail.CodHfecha;
        });


    }



    close() {
        this.dialogRef.close(false);
    }

    save() {
        if (this.detailForm.valid) {

            this.service.GetReporteExcel(this.detail);
            this.dialogRef.close(this.detail);


        }

    }



}
