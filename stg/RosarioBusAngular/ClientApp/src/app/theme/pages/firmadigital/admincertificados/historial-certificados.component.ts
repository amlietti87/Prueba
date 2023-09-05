import { Component, ViewChild, Injector, Inject, OnInit, AfterViewInit, Input, EventEmitter, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { NgForm } from '@angular/forms';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { FDCertificadosDto } from '../model/fdcertificados.model';
import { FdCertificadosService } from '../services/fdcertificados.service';

@Component({
    selector: 'exportarminutosporsector',
    templateUrl: "./historial-certificados.component.html",
})
export class HistorialCertificadosComponent extends AppComponentBase implements OnInit, AfterViewInit {

    filter: FDCertificadosDto[];
    isExporting: boolean = false;

    constructor(
        protected dialogRef: MatDialogRef<HistorialCertificadosComponent>,
        @Inject(MAT_DIALOG_DATA) public data: FDCertificadosDto[],
        protected service: FdCertificadosService,
        injector: Injector) {

        super(injector);
        this.filter = data;
        this.primengDatatableHelper.records = this.filter;
    }

    ngOnInit() {

    }
    close(): void {
        this.dialogRef.close(true);
    }

    ngAfterViewInit() {

    }

}