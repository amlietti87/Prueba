import { DetailComponent, DetailAgregationComponent } from "../../../../shared/manager/detail.component";
import { Component, Injector, ChangeDetectorRef, Inject } from "@angular/core";
import { TareaDto } from "../model/tarea.model";
import { TareaService } from "./tarea.service";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material";
import { TareaCampoDto } from "../model/tarea-campo.model";
import { TareaCampoConfigService } from "./tarea-campo-config.service";

@Component({
    selector: 'createOrEditTareaModalComponent',
    templateUrl: './create-or-edit-tarea-modal.component.html',
    styleUrls: ['./create-or-edit-tarea-modal.component.css']

})
export class CreateOrEditTareaModalComponent extends DetailAgregationComponent<TareaDto> {

    allowModificar: boolean = false;


    constructor(protected dialogRef: MatDialogRef<CreateOrEditTareaModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: TareaDto,
        injector: Injector, 
        protected service: TareaService,
        private tareasCampoConfigService : TareaCampoConfigService,
        private cdr: ChangeDetectorRef) {
        super(dialogRef, service, injector, data);
        this.allowModificar = this.permission.isGranted('Inspectores.Tareas.Modificar');
    }

    onCampoAdded() : void {
        if (!this.detail.TareasCampos) {
            this.detail.TareasCampos = [];
        }
        this.detail.TareasCampos = [...this.detail.TareasCampos, this.createNewCampo()];
    }

    onComboChanged(id, row): void {
        if(isNaN(id)){
            row.Etiqueta = "";
            row.NombreTareaCampo = "";
            return;
        }
        this.tareasCampoConfigService.getById(id).subscribe( e=> {
            if (e.Status == "Ok" && e.DataObject) { 
                row.Description = e.DataObject.Descripcion;
                row.TareaCampoConfigId = id;
                row.Etiqueta = e.DataObject.Description;
                row.NombreTareaCampo = e.DataObject.Description;
                this.detail.TareasCampos = [...this.detail.TareasCampos];
            }
        })
    }
    createNewCampo(): TareaCampoDto {
        return new TareaCampoDto();
    }

    SaveDetail() {
        if(this.validateBeforeSave()){
            super.SaveDetail();
        }
        return;
    }

    validateBeforeSave() : boolean {
        let validaitonsPassed : boolean = true;
        this.saving = true;

        if(!this.detail.TareasCampos || this.detail.TareasCampos.length == 0){
            this.notify.warn("Debe agregar al menos un campo");
            this.saving = false;
            validaitonsPassed = false;
        }    

        if(this.detail.TareasCampos && this.detail.TareasCampos.length > 0){
            this.detail.TareasCampos.forEach( item => {
                if(!item.Etiqueta){
                    this.notify.warn("Debe completar el campo Etiqueta.");
                    this.saving = false;
                    validaitonsPassed = false;
                }

                if(!item.TareaCampoConfigId || isNaN(item.TareaCampoConfigId)){
                    this.notify.warn("Debe seleccionar una Descripción.");
                    this.saving = false;
                    validaitonsPassed = false;
                }
            }) 
        }       
        return validaitonsPassed;        
    }

    completedataBeforeShow(item: TareaDto): any {
        if (this.detail.Id == null) {                        
            this.cdr.detectChanges();
        }
    }

    onRequeridoChanges(row : TareaCampoDto) {
        console.log('Before' , this.detail.TareasCampos);
        row.Requerido = !row.Requerido;
        console.log('After' , this.detail.TareasCampos);
    }
    
    OnItemRemoved(rowIndex : number){
        let lista = [...this.detail.TareasCampos];
        lista.splice(rowIndex, 1);
        this.detail.TareasCampos = [...lista];
    }
    
}