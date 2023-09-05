import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ProvinciasService } from './provincias.service';
import { ProvinciasDto } from '../model/localidad.model';

@Component({
    selector: 'createOrEditProvinciasDtoModal',
    templateUrl: './create-or-edit-provincias-modal.component.html',

})
export class CreateOrEditProvinciasModalComponent extends DetailAgregationComponent<ProvinciasDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditProvinciasModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ProvinciasDto,
        injector: Injector,
        protected service: ProvinciasService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;

    }

}

