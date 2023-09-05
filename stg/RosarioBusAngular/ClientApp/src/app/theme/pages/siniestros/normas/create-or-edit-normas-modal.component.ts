import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';

import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { ConductasNormasService } from './normas.service';
import { ConductasNormasDto } from '../model/normas.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
    selector: 'createOrEditConductasNormasDtoModal',
    templateUrl: './create-or-edit-normas-modal.component.html',

})
export class CreateOrEditConductasNormasModalComponent extends DetailAgregationComponent<ConductasNormasDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditConductasNormasModalComponent>,
        injector: Injector,
        protected service: ConductasNormasService,
        @Inject(MAT_DIALOG_DATA) public data: ConductasNormasDto) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
    }


}

