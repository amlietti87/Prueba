import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { PlaTalleresIvuDto } from '../model/talleresivu.model';
import { PlaTalleresIvuService } from './talleresivu.service';


@Component({
    selector: 'createOrEditTalleresIvuDtoModal',
    templateUrl: './create-or-edit-talleresivu-modal.component.html',

})
export class CreateOrEditTalleresIvuModalComponent extends DetailAgregationComponent<PlaTalleresIvuDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditTalleresIvuModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: PlaTalleresIvuDto,
        injector: Injector,
        protected service: PlaTalleresIvuService

    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;

    }

    completeDataBeforeSave(item: any) {

    }
}

