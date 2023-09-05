import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject, SimpleChange, SimpleChanges } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';


import * as _ from 'lodash';
declare let mApp: any;


import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { DetailEmbeddedComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { ViewMode, ItemDto } from '../../../../shared/model/base.model';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogConfig, MatDialog } from '@angular/material';
import { FileService } from '../../../../shared/common/file.service';
import { environment } from '../../../../../environments/environment';
import { VisorArchivos, DocumentosProcesadosDto, VerDetalle, HistoricosDocumentos } from '../model/documentosprocesados.model';
import { Helpers } from '../../../../helpers';
import { PDFDocumentProxy } from 'ng2-pdf-viewer';
import { DocumentosProcesadosService } from '../services/documentosprocesados.service';
import { AbrirArchivosModalComponent } from '../abrirarchivo-modal/abrirarchivo-modal.component';
import { BandejaEmpleadoComponent } from '../bdempleado/bandejaempleado.component';
import { forEach } from '@angular/router/src/utils/collection';
import { PermissionCheckerService } from '../../../../shared/common/permission-checker.service';

@Component({
    selector: 'VerArchivosDtoModal',
    templateUrl: './verarchivo-modal.component.html',
    styleUrls: ['./verarchivo-modal.component.css']

})
export class VerArchivosModalComponent implements OnInit {

    protected popupHeight: string = '';
    protected popupWidth: string = '80%';
    protected dialog: MatDialog;
    protected detailElement: IDetailComponent;
    ngOnInit(): void {

    }
    protected cfr: ComponentFactoryResolver;

    //permission: PermissionCheckerService
    archivosUrl: VerDetalle[];
    time: string;
    appDownloadUrl: string;
    currentIndex: number = 0;
    showNext: boolean = false;
    showPrevious: boolean = false;
    currentSrc: string;
    isLoading: boolean = true;
    historial: HistoricosDocumentos[] = [];
    allowAbrirArchivo: boolean = false;
    allowDescargarArchivo: boolean = false;

    constructor(
        injector: Injector,
        protected fileService: DocumentosProcesadosService,
        protected permission: PermissionCheckerService,
        public dialogRef: MatDialogRef<VerArchivosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: VerDetalle[]

    ) {
        this.allowAbrirArchivo = this.permission.isGranted("FirmaDigital.BD-Empleador.Abrir");
        this.allowDescargarArchivo = this.permission.isGranted("FirmaDigital.BD-Empleador.Descargar");
        this.dialog = injector.get(MatDialog);
        this.cfr = injector.get(ComponentFactoryResolver);
        if (data) {
            this.archivosUrl = data;
        }
        else {
            this.archivosUrl = [];
        }
        this.appDownloadUrl = environment.firmaDigitalUrl + '/FdAcciones/DownloadFiles';
        this.CheckShows();
        this.historial = this.archivosUrl[this.currentIndex].Historicos;
        this.currentSrc = this.appDownloadUrl + '\?id=' + this.archivosUrl[this.currentIndex].Archivo;

    }

    PreviousFile(): void {
        this.currentIndex = this.currentIndex - 1;
        this.currentSrc = this.appDownloadUrl + '\?id=' + this.archivosUrl[this.currentIndex].Archivo;
        this.historial = this.archivosUrl[this.currentIndex].Historicos;
        this.CheckShows();
    }

    NextFile(): void {
        this.currentIndex = this.currentIndex + 1;
        this.currentSrc = this.appDownloadUrl + '\?id=' + this.archivosUrl[this.currentIndex].Archivo;
        this.historial = this.archivosUrl[this.currentIndex].Historicos;
        this.CheckShows();
    }

    onDescargar() {
        window.open(this.currentSrc);
    }

    open() {
    }

    onAbrir() {
        var archivo: VerDetalle[] = [];
        archivo.push(this.archivosUrl.find(a => a.Archivo == this.archivosUrl[this.currentIndex].Archivo))

        this.AbrirPopupVisor(archivo as VisorArchivos[]);
    }

    AbrirPopupVisor(lista: VisorArchivos[]): void {
        const dialogConfig = new MatDialogConfig<VisorArchivos[]>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        dialogConfig.data = lista;
        let dialogRefe = this.dialog.open(AbrirArchivosModalComponent, dialogConfig);

        dialogRefe.afterClosed().subscribe(

            data => {

            }
        );
    }


    onProgress(event: any) {
        Helpers.setLoading(true);
        this.isLoading = true;
    }

    callBackFn(pdf: PDFDocumentProxy) {
        Helpers.setLoading(false);
        this.isLoading = false;
    }


    CheckShows() {
        if (this.archivosUrl.length - 1 == this.currentIndex) {
            this.showNext = false;
        }
        else if (this.archivosUrl.length - 1 > this.currentIndex) {
            this.showNext = true;
        }

        if (this.currentIndex > 0) {
            this.showPrevious = true;
        }
        else {
            this.showPrevious = false;
        }
    }

    close(): void {

        this.dialogRef.close(false);
    }


}

