import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject, SimpleChange, SimpleChanges, ChangeDetectorRef, AfterViewInit } from '@angular/core';
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


@Component({
    selector: 'ImportarDocumentosDtoModal',
    templateUrl: './importardocumentos-modal.component.html'

})
export class ImportarDocumentosModalComponent {


    protected cfr: ComponentFactoryResolver;


    constructor(
        injector: Injector,
        public dialogRef: MatDialogRef<ImportarDocumentosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: boolean,
        protected cdr: ChangeDetectorRef
    ) {
        this.cfr = injector.get(ComponentFactoryResolver);

    }

    close(resp: boolean): void {

        this.dialogRef.close(resp);
    }



}

