import { Component, ViewChild, Injector, Inject, OnInit, AfterViewInit, Input, EventEmitter, Output } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { NgForm, FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { UserDto } from '../../admin/model/user.model';
import { FdCertificadosService } from '../services/fdcertificados.service';
import { FDCertificadosFilter } from '../model/fdcertificados.model';



@Component({
    selector: 'certificadoemailcomponent',
    templateUrl: "./certificado-email.component.html",
})
export class CertificadoEmailComponent extends AppComponentBase implements OnInit, AfterViewInit {


    @ViewChild('exporExcelForm') exporExcelForm: NgForm;

    isExporting: boolean = false;
    userFilter: FDCertificadosFilter;
    sendEmailForm: FormGroup;
    sending: boolean = false;

    constructor(
        protected dialogRef: MatDialogRef<CertificadoEmailComponent>,
        @Inject(MAT_DIALOG_DATA) public data: FDCertificadosFilter,
        protected service: FdCertificadosService,
        private emailForm: FormBuilder,
        injector: Injector) {

        super(injector);
        if (data) {
            this.userFilter = data;
        }
        else {
            this.userFilter.UserEmail = '';
        }
        this.createForm();
    }

    ngOnInit() {

    }
    close(result?: any): void {
        this.dialogRef.close(result);
    }

    ngAfterViewInit() {

    }

    createForm() {
        this.sendEmailForm =
            this.emailForm.group({
                Email: [this.userFilter.UserEmail, [Validators.required, Validators.email]],
            });
    }

    onSubmit() {
        console.log(this.sendEmailForm);
        this.userFilter.UserEmail = this.sendEmailForm.value.Email;
        this.sending = true;
        this.service.sendCertificateByEmail(this.userFilter)
            .subscribe(e => {
                if (e.DataObject == "OK") {
                    this.sending = false;
                    this.close("OK");
                }
            })
    }


}