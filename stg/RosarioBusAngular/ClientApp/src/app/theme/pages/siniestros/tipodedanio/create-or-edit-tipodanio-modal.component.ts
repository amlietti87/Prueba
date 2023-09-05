import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';

import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { TipoDanioDto } from '../model/tipodanio.model';
import { TipoDanioService } from './tipodanio.service';

@Component({
    selector: 'createOrEditTipoDanioDtoModal',
    templateUrl: './create-or-edit-tipodanio-modal.component.html',

})
export class CreateOrEditTipoDanioModalComponent extends DetailAgregationComponent<TipoDanioDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditTipoDanioModalComponent>,
        injector: Injector,
        protected service: TipoDanioService,
        @Inject(MAT_DIALOG_DATA) public data: TipoDanioDto) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
    }


}

