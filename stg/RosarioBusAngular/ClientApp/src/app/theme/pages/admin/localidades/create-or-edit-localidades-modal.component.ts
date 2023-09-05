import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { LocalidadesDto } from '../../siniestros/model/localidad.model';
import { LocalidadesService } from '../../siniestros/localidades/localidad.service';


@Component({
    selector: 'createOrEditLocalidadesDtoModal',
    templateUrl: './create-or-edit-localidades-modal.component.html',

})
export class CreateOrEditLocalidadesModalComponent extends DetailAgregationComponent<LocalidadesDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    constructor(protected dialogRef: MatDialogRef<CreateOrEditLocalidadesModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: LocalidadesDto,
        injector: Injector,
        protected service: LocalidadesService

    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;

    }

    completeDataBeforeSave(item: any) {
        console.log(item);

    }
}

