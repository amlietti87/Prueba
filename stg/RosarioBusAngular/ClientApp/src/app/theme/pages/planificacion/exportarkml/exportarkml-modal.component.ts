import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject, SimpleChange, SimpleChanges } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';


import * as _ from 'lodash';
declare let mApp: any;


import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { PuntoFilter } from '../model/punto.model';



@Component({
    selector: 'ExportarKmlModal',
    templateUrl: './exportarkml-modal.component.html',

})
export class ExportarKmlModalComponent {
    protected cfr: ComponentFactoryResolver;

    filter: PuntoFilter;

    constructor(
        injector: Injector,
        public dialogRef: MatDialogRef<ExportarKmlModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: PuntoFilter
    ) {

        this.cfr = injector.get(ComponentFactoryResolver);
        this.filter = data;
    }




    close(): void {
        this.data = null;
        this.dialogRef.close(false);
    }

    save(): void {
        this.data = this.filter;
        this.dialogRef.close(true);
    }


}

