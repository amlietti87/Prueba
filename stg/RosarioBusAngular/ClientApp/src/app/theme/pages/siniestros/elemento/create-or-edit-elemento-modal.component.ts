import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Input, ChangeDetectorRef, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailEmbeddedComponent, DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { DetailComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { ViewMode, ResponseModel } from '../../../../shared/model/base.model';
import { TipoElementoDto } from '../model/tipoelemento.model';
import { ElementosDto } from '../model/elemento.model';
import { ElementosService } from './elemento.service';
import { AdjuntosDto } from '../model/adjuntos.model';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditElementosDtoModal',
    templateUrl: './create-or-edit-elemento-modal.component.html',

})
export class CreateOrEditElementosModalComponent extends DetailAgregationComponent<ElementosDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    constructor(
        injector: Injector,
        protected service: ElementosService,
        protected http: HttpClient,
        protected auth: AuthService,
        private cd: ChangeDetectorRef,
        protected dialogRef: MatDialogRef<CreateOrEditElementosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ElementosDto,
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.appUploadUrl = environment.siniestrosUrl + '/Adjuntos/UploadOrUpdateFile';
        this.appDownloadUrl = environment.siniestrosUrl + '/Adjuntos/DownloadFiles';
        this.appRemoveFileUrl = environment.siniestrosUrl + '/Adjuntos/DeleteById';


        this.appGetAllFileByParent = "GetAdjunto";

    }

    completedataBeforeShow(item: ElementosDto): any {
        this.imagencambiada = false;
    }

    imagencambiada: boolean = false;

    @Output() onAdjuntosUpload: EventEmitter<AdjuntosDto> = new EventEmitter<AdjuntosDto>();
    @Output() onAdjuntoRemove: EventEmitter<AdjuntosDto> = new EventEmitter<AdjuntosDto>();


    isLoading: boolean;
    uploadedFiles: any[] = [];
    adjuntoDto: AdjuntosDto = null;

    @Input()
    allowClose: boolean = false;

    @Input()
    maxFileSize: number = null;


    @Input()
    accept: string = null; //example "image/*";



    @Input()
    appRemoveFileUrl: string;

    @Input()
    appUploadUrl: string;

    @Input()
    appDownloadUrl: string;
    @Input()
    appGetAllFileByParent: string;

    _Parent: any;

    @Input()
    get Parent(): any {

        return this._Parent;
    }

    set Parent(value: any) {

        this._Parent = value;
    }



    time: string;

    onUpload(event) {

        console.log(event);

        var adj = JSON.parse(event.xhr.response);

        this.adjuntoDto = adj;

        for (let file of event.files) {
            this.uploadedFiles.push(file);
        }

        if (this.onAdjuntosUpload) {
            this.onAdjuntosUpload.emit(adj);
        }

        if (this.viewMode == ViewMode.Add) {
            this.detail.Id = this.adjuntoDto.Id;

            //var img = document.getElementById("inputimagenAdd") as HTMLImageElement;
            //img.src = this.appDownloadUrl + '\?id=' + this.detail.Id + "#" + new Date().getTime();
            this.time = new Date().getTime().toString();
            this.cd.markForCheck();
        }
        else {

            //var img = document.getElementById("inputimagenEdit") as HTMLImageElement;
            //img.src = this.appDownloadUrl + '\?id=' + this.detail.Id + "#" + new Date().getTime();
            this.time = new Date().getTime().toString();
            this.cd.markForCheck();

        }

        this.imagencambiada = true;




        this.notificationService.info("File Uploaded!", "");
    }

    OnRemoveFile(item: AdjuntosDto) {
        let url = this.appRemoveFileUrl + '?Id=' + item.Id;
        this.http.post(url, null).subscribe(s => {
            if (this.onAdjuntoRemove) {
                this.onAdjuntoRemove.emit(item);
            }
            this.adjuntoDto = null;;
        });
    }

    SaveDetail(): any {

        if (this.detail.Id && this.detail.Id != null) {
            super.SaveDetail();
        }
        else {
            this.saving = false;
            this.notificationService.warn("Debe subir la imagen antes de guardar el elemento");
        }


    }

    onBeforeSend(event): void {
        event.xhr.setRequestHeader("Authorization", 'Bearer ' + this.auth.getToken());
    }

    onError(event): void {
        this.notificationService.error("Ha ocurrido un error al internar subir los archivos");
    }


}

