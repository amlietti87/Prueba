import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject, SimpleChange, SimpleChanges } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';


import * as _ from 'lodash';
declare let mApp: any;


import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { DetailEmbeddedComponent } from '../../../../shared/manager/detail.component';
import { ViewMode, ItemDto } from '../../../../shared/model/base.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { FileService } from '../../../../shared/common/file.service';
import { environment } from '../../../../../environments/environment';
import { VisorArchivos } from '../model/documentosprocesados.model';
import { Helpers } from '../../../../helpers';
import { PDFDocumentProxy } from 'ng2-pdf-viewer';
import { DocumentosProcesadosService } from '../services/documentosprocesados.service';

@Component({
    selector: 'SeleccionarCorreoDtoModal',
    templateUrl: './seleccionarcorreo-modal.component.html'

})
export class SeleccionarCorreoModalComponent implements OnInit {

    ngOnInit(): void {

    }
    protected cfr: ComponentFactoryResolver;

    correo: string;

    constructor(
        injector: Injector,
        protected fileService: FileService,
        protected serviceDocumentos: DocumentosProcesadosService,
        public dialogRef: MatDialogRef<SeleccionarCorreoModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: string
    ) {

        this.cfr = injector.get(ComponentFactoryResolver);
        if (data) {
            this.correo = data;
        }
        else {
            this.correo = '';
        }

    }

    EnviarMail(): void {
        this.data = this.correo;
        this.serviceDocumentos.ValidarEmails(this.data).subscribe(e => {
            this.dialogRef.close(this.data);
        })


    }

    close(): void {
        this.data = null;
        this.dialogRef.close(this.data);
    }


}

