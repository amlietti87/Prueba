import { Component, ViewChild, Injector, ViewContainerRef, Inject, OnInit, AfterViewInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { NgForm } from '@angular/forms';
import { DetailAgregationComponent } from '../../../../../shared/manager/detail.component';
import { DetalleLesionDto } from '../../model/involucrados.model';
import { InvolucradosService } from '../../involucrados/involucrados.service';




@Component({
    selector: 'detallelesion-detail-modal',
    templateUrl: './detallelesion-detail-modal.component.html',
    styleUrls: ['./detallelesion-detail-modal.component.css']
})
export class DetalleLesionDetailComponent extends DetailAgregationComponent<DetalleLesionDto> implements OnInit, AfterViewInit {

    @ViewChild('detailForm') detailForm: NgForm;




    constructor(
        protected dialogRef: MatDialogRef<DetalleLesionDetailComponent>,
        //protected serviceInvolucrados: InvolucradosService,
        injector: Injector,
        @Inject(MAT_DIALOG_DATA) public data: DetalleLesionDto
    ) {
        super(dialogRef, null, injector, data);

    }

    ngOnInit() {
        super.ngOnInit();

    }



    ngAfterViewInit() {


    }

}
