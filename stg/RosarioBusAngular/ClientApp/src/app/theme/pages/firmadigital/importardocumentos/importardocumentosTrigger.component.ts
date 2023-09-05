import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject, SimpleChange, SimpleChanges, ChangeDetectorRef, AfterViewInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';


import * as _ from 'lodash';
declare let mApp: any;


import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { DetailEmbeddedComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { ViewMode, ItemDto } from '../../../../shared/model/base.model';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog, MatDialogConfig } from '@angular/material';
import { FileService } from '../../../../shared/common/file.service';
import { environment } from '../../../../../environments/environment';
import { ImportarDocumentosModalComponent } from './importardocumentos-modal.component';
import { DocumentosProcesadosService } from '../services/documentosprocesados.service';
import { NotificationService } from '../../../../shared/notification/notification.service';
import { DocumentosProcesadosFilter } from '../model/documentosprocesados.model';


@Component({
    selector: 'ImportarDocumentosTriggerDtoModal',
    templateUrl: './importardocumentosTrigger.component.html'
})
export class ImportarDocumentosTriggerComponent implements AfterViewInit {

    //material dialog
    protected popupHeight: string = '';
    protected popupWidth: string = '40%';
    protected dialog: MatDialog;
    protected detailElement: IDetailComponent;


    ngAfterViewInit(): void {
        setTimeout((e) => {
            this.AbrirMensajeConfirmacion();
        }, 100)

    }

    protected cfr: ComponentFactoryResolver;



    constructor(
        injector: Injector,
        protected cdr: ChangeDetectorRef,
        private service: DocumentosProcesadosService,
        protected notificationService: NotificationService
    ) {

        this.dialog = injector.get(MatDialog);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.cdr.detectChanges();
    }



    AbrirMensajeConfirmacion(): void {
        const dialogConfig = new MatDialogConfig<boolean>();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.hasBackdrop = false;
        dialogConfig.width = this.popupWidth;
        dialogConfig.height = this.popupHeight;

        dialogConfig.data = false;
        let dialogRef = this.dialog.open(ImportarDocumentosModalComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(

            data => {
                if (data == true) {


                    this.service.ImportarDocumentos()
                        .subscribe((t) => {

                        })
                    this.notificationService.info("La importación de documentos comenzó a realizarse");
                }
            }
        );
    }

}

