import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';

import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { SancionSugeridaDto } from '../model/sancionsugerida.model';
import { SancionSugeridaService } from './sancion.service';


@Component({
    selector: 'createOrEditSancionDtoModal',
    templateUrl: './create-or-edit-sancion-modal.component.html',

})
export class CreateOrEditSancionModalComponent extends DetailAgregationComponent<SancionSugeridaDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditSancionModalComponent>,
        injector: Injector,
        protected service: SancionSugeridaService,
        @Inject(MAT_DIALOG_DATA) public data: SancionSugeridaDto) {

        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;
    }







}

