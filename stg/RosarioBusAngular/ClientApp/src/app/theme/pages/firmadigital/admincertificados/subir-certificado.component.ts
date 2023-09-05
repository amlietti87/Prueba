import { Component, OnInit, Input, Injector, Output, EventEmitter, Inject } from "@angular/core";
import { AppComponentBase } from "../../../../shared/common/app-component-base";
import { AuthService } from "../../../../auth/auth.service";
import { HttpClient } from "@angular/common/http";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { environment } from "../../../../../environments/environment";
import { AdjuntosDto } from "../../siniestros/model/adjuntos.model";
import { ResponseModel } from "../../../../shared/model/base.model";
import { FDCertificadosFilter } from '../model/fdcertificados.model';

declare let mApp: any;
@Component({
    selector: 'adjuntos',
    templateUrl: './subir-certificado.component.html',
    styleUrls: ['./subir-certificado.component.css']
})
export class SubirCertificadoComponent extends AppComponentBase implements OnInit {

    @Input() onlyDownload: boolean = false;
    filter: FDCertificadosFilter;

    ngOnInit(): void {

    }

    @Input()
    AllowUploadFiles: boolean;

    constructor(injector: Injector,
        protected auth: AuthService,
        protected http: HttpClient,
        protected dialogRef: MatDialogRef<SubirCertificadoComponent>,
        @Inject(MAT_DIALOG_DATA) public data: FDCertificadosFilter, ) {
        super(injector)
        this.appUploadUrl = environment.firmaDigitalUrl + '/FDCertificados/SubirCertificado';
        this.filter = data;
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
    accept: string = ".p12"; //example "image/*";

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

    UploadCertificate(event) {
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
        this.isLoading = false;
        this.notificationService.info("Certificado agregado exitosamente!", "");
        this.OnClose();
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
        this.isLoading = true;
        event.xhr.setRequestHeader("Authorization", 'Bearer ' + this.auth.getToken());
        event.formData.append('UsuarioId', this.filter);
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
