import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ViewContainerRef, ComponentFactoryResolver, OnInit, Inject } from '@angular/core';
import * as _ from 'lodash';
declare let mApp: any;
import { DetailModalComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { NgForm } from '@angular/forms';
import { ZonasService } from './zonas.service';
import { ZonasDto } from '../model/zonas.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';


@Component({
    selector: 'createOrEditZonasDtoModal',
    templateUrl: './create-or-edit-zonas-modal.component.html',

})
export class CreateOrEditZonasModalComponent extends DetailAgregationComponent<ZonasDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;

    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    allowmodificarzona: boolean = false;
    constructor(protected dialogRef: MatDialogRef<CreateOrEditZonasModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: ZonasDto,
        injector: Injector,
        protected service: ZonasService
    ) {
        super(dialogRef, service, injector, data);
        //this.detail = new ZonasDto();
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;
        this.allowmodificarzona = this.permission.isGranted("Inspectores.Zonas.Modificar");
    }

    save(form: NgForm): void {

        if (this.detailForm && this.detailForm.form.invalid) {
            return;
        }

        this.saving = true;
        this.completedataBeforeSave(this.detail);

        if (!this.validateSave()) {
            this.saving = false;
            return;
        }

        this.SaveDetail();
    }

}



