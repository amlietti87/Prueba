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

import {
    SafeResourceUrl,
    DomSanitizer
} from '@angular/platform-browser';

@Component({
    selector: 'AbrirArchivosDtoModal',
    templateUrl: './abrirarchivo-modal.component.html',
    styleUrls: ['./abrirarchivo-modal.component.css']

})
export class AbrirArchivosModalComponent implements OnInit {

    ngOnInit(): void {

    }
    protected cfr: ComponentFactoryResolver;

    archivosUrl: VisorArchivos[];
    time: string;
    appDownloadUrl: string;
    currentIndex: number = 0;
    showNext: boolean = false;
    showPrevious: boolean = false;
    currentSrc: SafeResourceUrl;
    isLoading: boolean = true;

    constructor(
        injector: Injector,
        private sanitizer: DomSanitizer,
        protected fileService: FileService,
        public dialogRef: MatDialogRef<AbrirArchivosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: VisorArchivos[]
    ) {
        this.cfr = injector.get(ComponentFactoryResolver);
        if (data) {
            this.archivosUrl = data;
        }
        else {
            this.archivosUrl = [];
        }
        //let env = "https://api-rosariobus.net.tecso.coop/ROSBUS.WebService.Siniestros"; 
        let env = environment.siniestrosUrl; 

        this.appDownloadUrl = 'https://docs.google.com/gview?url=' + env + '/Adjuntos/DownloadFiles';
        this.CheckShows();
        this.currentSrc = sanitizer.bypassSecurityTrustResourceUrl(this.appDownloadUrl + '\?id=' + this.archivosUrl[this.currentIndex].Archivo + '&c=' + Date.now() + '&embedded=true');
    }

    PreviousFile(): void {
        this.currentIndex = this.currentIndex - 1;
        this.currentSrc = this.sanitizer.bypassSecurityTrustResourceUrl(this.appDownloadUrl + '\?id=' + this.archivosUrl[this.currentIndex].Archivo + '&c=' + Date.now() + '&embedded=true');
        this.CheckShows();
    }

    NextFile(): void {
        this.currentIndex = this.currentIndex + 1;
        this.currentSrc = this.sanitizer.bypassSecurityTrustResourceUrl(this.appDownloadUrl + '\?id=' + this.archivosUrl[this.currentIndex].Archivo + '&c=' + Date.now() + '&embedded=true');
        this.CheckShows();
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

