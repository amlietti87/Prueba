import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject, Input } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';


import * as _ from 'lodash';
import { environment } from '../../../../../../environments/environment';
import { AdjuntosDto } from '../../model/adjuntos.model';
import { AuthService } from '../../../../../auth/auth.service';
import { HttpClient } from '@angular/common/http';
import { ResponseModel } from '../../../../../shared/model/base.model';
import { MatDialogRef } from '@angular/material';
import { debounce } from 'rxjs/operators';
declare let mApp: any;
@Component({
    selector: 'adjuntos',
    templateUrl: './adjunto.component.html',
    styleUrls: ['./adjunto.component.css']
})
export class AdjuntoComponent extends AppComponentBase implements OnInit {

    @Input() onlyDownload: boolean = false;

    ngOnInit(): void {

    }

    @Input()
    AllowUploadFiles: boolean;

    constructor(injector: Injector, protected auth: AuthService, protected http: HttpClient,
        protected dialogRef: MatDialogRef<AdjuntoComponent>) {
        super(injector)
        this.appUploadUrl = environment.siniestrosUrl + '/Adjuntos/UploadFiles';
        this.appDownloadUrl = environment.siniestrosUrl + '/Adjuntos/DownloadFiles';
        this.appRemoveFileUrl = environment.siniestrosUrl + '/Adjuntos/DeleteById';
    }


    @Output() onAdjuntosUpload: EventEmitter<AdjuntosDto[]> = new EventEmitter<AdjuntosDto[]>();
    @Output() onAdjuntoRemove: EventEmitter<AdjuntosDto> = new EventEmitter<AdjuntosDto>();

    isLoading: boolean;
    uploadedFiles: any[] = [];
    adjuntosDto: AdjuntosDto[] = [];

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
        this.onSearch();
    }

    onSearch() {
        this.isLoading = true;
        this.adjuntosDto = [];
        if (this.appGetAllFileByParent && this.Parent) {
            let url = this.appGetAllFileByParent + "/" + this.Parent;
            this.http.get<ResponseModel<AdjuntosDto[]>>(url)
                .finally(() => {
                    this.isLoading = false;
                })
                .subscribe(r => {
                    this.adjuntosDto = r.DataObject;
                });
        }


    }

    OnClose() {
        this.dialogRef.close(false);
    }

    onUpload(event) {
        if (this.onlyDownload) {
            return;
        }

        if (!this.AllowUploadFiles) {
            return;
        }
        console.log(event);
        var adj = JSON.parse(event.xhr.response);

        for (let file of adj) {
            this.adjuntosDto.push(file);
        }

        for (let file of event.files) {
            this.uploadedFiles.push(file);
        }

        if (this.onAdjuntosUpload) {
            this.onAdjuntosUpload.emit(adj);
        }

        this.notificationService.info("Files Uploaded!", "");
    }

    OnRemoveFile(item: AdjuntosDto) {

        if (this.onlyDownload) {
            this.notificationService.warn("Solamente permitida la descarga de archivos");
            return;
        }

        if (!this.AllowUploadFiles) {
            this.notificationService.warn("No posee permisos para quitar Adjuntos");
            return;
        }
        let url = this.appRemoveFileUrl + '?Id=' + item.Id;
        this.http.post(url, null).subscribe(s => {
            if (this.onAdjuntoRemove) {
                this.onAdjuntoRemove.emit(item);
            }

            var index = this.adjuntosDto.findIndex(i => i.Id == item.Id);
            this.adjuntosDto.splice(index, 1);


        });
    }


    onBeforeSend(event): void {
        if (this.onlyDownload) {
            this.notificationService.warn("Solamente permitida la descarga de archivos");
            return;
        }


        if (!this.AllowUploadFiles) {
            this.notificationService.warn("No posee permisos para agregar Adjuntos");
            return;
        }
        event.xhr.setRequestHeader("Authorization", 'Bearer ' + this.auth.getToken());
    }

    onError(event): void {

        if (this.AllowUploadFiles && !this.onlyDownload) {
            this.notificationService.error("Ha ocurrido un error al intentar subir los archivos");
        }
    }

}



export enum LlamadorAdjuntos {
    Siniestro, Reclamo, Involucrado, Denuncia
}
