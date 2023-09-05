import { Component, ViewChild, Injector, ViewContainerRef, Inject, OnInit } from '@angular/core';

import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { NgForm } from '@angular/forms';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';



@Component({
    selector: 'add-consecuencia-modal',
    templateUrl: './add-consecuencia-modal.component.html'
})
export class AddConsecuenciaModalComponent extends AppComponentBase implements OnInit {

    @ViewChild('detailForm') detailForm: NgForm;

    consecuenciaId: any;


    constructor(
        injector: Injector,
        public dialogRef: MatDialogRef<AddConsecuenciaModalComponent>
    ) {
        super(injector);
    }

    ngOnInit() {
        this.dialogRef.updateSize('50%');
    }


    save(): void {

        if (this.detailForm && this.detailForm.form.invalid) {
            return;
        }
        this.dialogRef.close(this.consecuenciaId);
    }



    close(): void {
        this.dialogRef.close(false);
    }
}
