import { RespuestasIncognitosDto } from '../model/respuestasIncognitos.model';
import { RespuestasIncognitosService } from './respuestasIncognitos.service';
import { Component, ViewChild, Injector, ViewContainerRef, ComponentFactoryResolver, OnInit, Inject } from '@angular/core';
import * as _ from 'lodash';
declare let mApp: any;
import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { NgForm } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';


@Component({
    selector: 'createOrEditRespuestasDtoModal',
    templateUrl: './create-or-edit-respuestas-modal.component.html',

})
export class CreateOrEditRespuestasModalComponent extends DetailAgregationComponent<RespuestasIncognitosDto> implements OnInit {

    protected cfr: ComponentFactoryResolver;

    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    allowmodificarrespuestas: boolean = false;
    constructor(protected dialogRef: MatDialogRef<CreateOrEditRespuestasModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: RespuestasIncognitosDto,
        injector: Injector,
        protected service: RespuestasIncognitosService
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.saveLocal = false;
        this.IsInMaterialPopupMode = true;
        this.allowmodificarrespuestas = this.permission.isGranted("Inspectores.RespuestasIncognitos.Modificar");
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



