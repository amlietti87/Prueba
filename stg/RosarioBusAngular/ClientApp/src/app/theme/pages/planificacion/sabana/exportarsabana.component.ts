import { Component, OnInit, AfterViewInit, Injector, Inject, ViewEncapsulation, ViewChild } from '@angular/core';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { HorariosPorSectorDto } from '../model/horariosPorSector.model';
import { BanderaService } from '../bandera/bandera.service';
import { NgForm } from '@angular/forms';

@Component({
    selector: 'exportarsabana',
    templateUrl: "./exportarsabana.component.html",
    styleUrls: ["./exportarsabana.component.css"],
    encapsulation: ViewEncapsulation.None,

})
export class ExportarsabanaComponent extends AppComponentBase implements OnInit, AfterViewInit {

    horarios: HorariosPorSectorDto;
    isExporting: boolean = false;
    @ViewChild('exporExcelForm') exporExcelForm: NgForm;

    ngAfterViewInit(): void {

    }

    constructor(
        protected dialogRef: MatDialogRef<ExportarsabanaComponent>,
        @Inject(MAT_DIALOG_DATA) public data: HorariosPorSectorDto,
        protected banderaService: BanderaService,
        injector: Injector) {

        super(injector)
        this.horarios = data;

    }

    ngOnInit() {
    }

    close(): void {
        this.dialogRef.close(true);
    }

    export() {
        if (this.exporExcelForm.valid) {
            this.isExporting = true;
            var selft = this;
            this.banderaService.GetReporteSabana(this.horarios, function() { selft.dialogRef.close(true); selft.isExporting = false });
        }
    }

}
