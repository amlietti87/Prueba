import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';

import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { TipoColisionDto } from '../model/tipocolision.model';
import { TipoColisionService } from './tipocolision.service';


@Component({
    selector: 'createOrEditTipoColisionDtoModal',
    templateUrl: './create-or-edit-tipocolision-modal.component.html',

})
export class CreateOrEditTipoColisionModalComponent extends DetailAgregationComponent<TipoColisionDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditTipoColisionModalComponent>,
        injector: Injector,
        protected service: TipoColisionService,
        @Inject(MAT_DIALOG_DATA) public data: TipoColisionDto) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
    }


}

