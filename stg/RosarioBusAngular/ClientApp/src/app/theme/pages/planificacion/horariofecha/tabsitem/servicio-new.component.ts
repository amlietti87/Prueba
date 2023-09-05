import { Component, ViewChild, Injector, Inject, OnInit, AfterViewInit, Input, EventEmitter, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { NgForm } from '@angular/forms';
import { HServiciosDto } from '../../model/hServicios.model';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';





@Component({
    selector: 'servicio-new',
    templateUrl: './servicio-new.component.html',
    //styleUrls: ['./distribuciondecoches-detail.component.css']
})
export class ServicioNewComponent extends AppComponentBase implements OnInit, AfterViewInit {

    @ViewChild('detailForm') detailForm: NgForm;

    isLoading: boolean;
    detail: HServiciosDto;

    @Input() title: string = "Nuevo Servicio";


    constructor(
        public dialogRef: MatDialogRef<ServicioNewComponent>,
        injector: Injector,
        @Inject(MAT_DIALOG_DATA) data: HServiciosDto
    ) {
        super(injector);
        this.detail = data;
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
            this.detail.NumSer = this.detail.Description;

            this.dialogRef.close(this.detail);


        }

    }



}
