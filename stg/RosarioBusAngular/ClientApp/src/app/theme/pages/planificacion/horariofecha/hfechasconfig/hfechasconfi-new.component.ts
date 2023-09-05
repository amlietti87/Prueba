import { Component, ViewChild, Injector, Inject, OnInit, AfterViewInit, Input, EventEmitter, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { NgForm } from '@angular/forms';

import { HFechasConfiDto } from '../../model/HFechasConfi.model';
import { HFechasConfiService } from '../HFechasConfi.service';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { ViewMode } from '../../../../../shared/model/base.model';





@Component({
    selector: 'hfechasconfi-new',
    templateUrl: './hfechasconfi-new.component.html',
    styleUrls: ['./hfechasconfi-new.component.css']
})
export class HfechasConfiNewComponent extends AppComponentBase implements OnInit, AfterViewInit {

    @ViewChild('detailForm') detailForm: NgForm;

    isLoading: boolean;
    cod_hfecha: number;
    fec_desde: Date;
    @Input() LineaId: number;
    IsInCopyMode: boolean;
    CopyConductores: boolean = false;

    constructor(
        public dialogRef: MatDialogRef<HfechasConfiNewComponent>,
        protected hFechasConfiService: HFechasConfiService,
        injector: Injector,
        @Inject(MAT_DIALOG_DATA) public data: HFechasConfiDto
    ) {
        super(injector);

    }

    ngOnInit() {


    }

    ngAfterViewInit() {


    }



    close() {
        this.dialogRef.close(false);
    }

    save() {
        if (this.detailForm.valid) {
            this.isLoading = true;
            if (this.IsInCopyMode) {
                this.hFechasConfiService.CopiarHorario(this.cod_hfecha, this.fec_desde, this.CopyConductores)
                    .finally(() => this.isLoading = false)
                    .subscribe(e => {
                        this.isLoading = false;
                        this.dialogRef.close(e.DataObject);
                    });
            }
            else {

                this.data.CodLinea = this.LineaId;
                this.data.FechaDesde = this.fec_desde;
                this.data.FechaHasta = null;
                this.data.PlaEstadoHorarioFechaId = 1;
                this.hFechasConfiService.createOrUpdate(this.data, ViewMode.Add)
                    .finally(() => this.isLoading = false)
                    .subscribe(e => {
                        this.isLoading = false;
                        this.dialogRef.close(e.DataObject);
                    });


            }

        }

    }



}
