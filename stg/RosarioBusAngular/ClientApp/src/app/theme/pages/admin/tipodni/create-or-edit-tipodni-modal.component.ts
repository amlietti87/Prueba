import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';

import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { TipoDniDto } from '../../siniestros/model/tipodni.model';
import { TipoDniService } from '../../siniestros/tipodni/tipodni.service';


@Component({
    selector: 'createOrEditTipoDniDtoModal',
    templateUrl: './create-or-edit-tipodni-modal.component.html',

})
export class CreateOrEditTipoDniModalComponent extends DetailAgregationComponent<TipoDniDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditTipoDniModalComponent>,
        injector: Injector,
        protected service: TipoDniService,
        @Inject(MAT_DIALOG_DATA) public data: TipoDniDto) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;
    }

}

