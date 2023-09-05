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
import { VisorArchivos, AplicarAccioneResponseDto } from '../model/documentosprocesados.model';
import { Helpers } from '../../../../helpers';
import { PDFDocumentProxy } from 'ng2-pdf-viewer';

@Component({
    selector: 'errorResponseModal',
    templateUrl: './errorResponse.component.html',
    styleUrls: ['./errorResponse.component.css']

})
export class ErrorResponseAplicarAccionModalComponent implements OnInit {

    ngOnInit(): void {

    }
    protected cfr: ComponentFactoryResolver;
    datosGrilla: any[] = [];

    constructor(
        injector: Injector,

        public dialogRef: MatDialogRef<ErrorResponseAplicarAccionModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: AplicarAccioneResponseDto
    ) {

        this.cfr = injector.get(ComponentFactoryResolver);
        this.datosGrilla = [];
        if (data) {
            data.Detalles.forEach(e => {
                if (!e.IsValid) {
                    this.datosGrilla.push(e);
                }
            });
        }

    }



    close(): void {

        this.dialogRef.close(false);
    }


}

