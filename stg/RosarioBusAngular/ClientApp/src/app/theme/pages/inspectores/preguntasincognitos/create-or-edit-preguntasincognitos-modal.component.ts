import { RespuestasIncognitosService } from './../respuestasincognitos/respuestasIncognitos.service';
import { InspPreguntasIncognitosRespuestasDto } from './../model/preguntasincognitos.model';
import { PreguntasIncognitosService } from './preguntasincognitos.service';
import { Component, ViewChild, Injector, ComponentFactoryResolver, ViewContainerRef, Inject, ChangeDetectorRef } from '@angular/core';
import * as _ from 'lodash';
declare let mApp: any;
import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { NgForm } from '@angular/forms';
import { PreguntasIncognitosDto } from '../model/preguntasincognitos.model';

@Component({
    selector: 'createOrEditPreguntasIncognitosDtoModal',
    templateUrl: './create-or-edit-preguntasincognitos-modal.component.html',

})
export class CreateOrEditPreguntasIncognitosModalComponent extends DetailAgregationComponent<PreguntasIncognitosDto> {

    protected cfr: ComponentFactoryResolver;
    allowRespuestas: boolean = false;   

    cdr : ChangeDetectorRef
    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    self: CreateOrEditPreguntasIncognitosModalComponent;
    allowmodificarpreguntas: boolean = false;
    allowAddRespuestas: boolean = false;
    constructor(dialogRef: MatDialogRef<CreateOrEditPreguntasIncognitosModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: PreguntasIncognitosDto,
        injector: Injector,
        protected service: PreguntasIncognitosService,
        protected serviceRespuestas: RespuestasIncognitosService,
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.cdr = injector.get(ChangeDetectorRef);

        this.IsInMaterialPopupMode = true;
        this.allowmodificarpreguntas = this.permission.isGranted("Inspectores.PreguntasIncognitos.Modificar");
        this.allowAddRespuestas = this.permission.isGranted("Inspectores.PreguntasIncognitos.Agregar");
    }

    save(form: NgForm): void {

        if (this.detail.InspPreguntasIncognitosRespuestas) {
            this.detail.InspPreguntasIncognitosRespuestas = this.detail.InspPreguntasIncognitosRespuestas.filter(e=> e.RespuestaId != null);
        }


        
        if(!this.detail.InspPreguntasIncognitosRespuestas || this.detail.InspPreguntasIncognitosRespuestas.length == 0){
            this.notify.warn("Debe ingresar al menos una respuesta");
            return;
        }

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

    CreateNewPreguntasIncognitosRespuestas(): InspPreguntasIncognitosRespuestasDto {
        var item = new InspPreguntasIncognitosRespuestasDto();
        return item;
    }

    OnPreguntasIncognitosRespuestasRowAdded(): void {
        if (!this.detail.InspPreguntasIncognitosRespuestas) {
            this.detail.InspPreguntasIncognitosRespuestas = [];
        }
        this.detail.InspPreguntasIncognitosRespuestas = [...this.detail.InspPreguntasIncognitosRespuestas, this.CreateNewPreguntasIncognitosRespuestas()];
    }

    OnPreguntasIncognitosRespuestasRowRemoved(row: InspPreguntasIncognitosRespuestasDto): void {
        var index = this.detail.InspPreguntasIncognitosRespuestas.findIndex(x => x.RespuestaId == row.RespuestaId);

        if (index >= 0) {
            let lista = [...this.detail.InspPreguntasIncognitosRespuestas];
            lista.splice(index, 1);
            this.detail.InspPreguntasIncognitosRespuestas = [...lista];
        }
    }

    OnRespuestasComboChanged(newValue, oldValue): void {
        if(isNaN(newValue)){
            oldValue.RespuestaId = null;
            oldValue.Respuesta = null;
            oldValue.RespuestaNombre = null;
            oldValue.PreguntaIncognitoId = null;
            return;
        }

        var respuestaFiltered = this.detail.InspPreguntasIncognitosRespuestas.filter(e => e.RespuestaId == newValue);
        if (respuestaFiltered && respuestaFiltered.length > 1) {
            this.notify.warn("La respuesta seleccionada ya fue agregada");
            oldValue.PreguntaIncognitoId = null;
            oldValue.RespuestaId = null;
            oldValue.Respuesta = null;
            oldValue.RespuestaNombre = null;
            return;
        } else {
            this.allowRespuestas = false;
            this.serviceRespuestas.getById(newValue).subscribe(e => {
                if (e.Status == "Ok" && e.DataObject) {
                    oldValue.Respuesta = e.DataObject;
                    oldValue.RespuestaNombre = e.DataObject.Descripcion;
                    oldValue.RespuestaId = e.DataObject.Id;
                    oldValue.PreguntaIncognitoId = this.detail.Id;
                    this.detail.InspPreguntasIncognitosRespuestas = [...this.detail.InspPreguntasIncognitosRespuestas];
                }
            });
        }
    }

}



