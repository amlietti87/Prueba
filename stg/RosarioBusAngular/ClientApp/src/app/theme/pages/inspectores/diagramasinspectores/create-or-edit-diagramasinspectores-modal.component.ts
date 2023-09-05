import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit, Inject, Input, ChangeDetectorRef } from '@angular/core';
import * as _ from 'lodash';
declare let mApp: any;
import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { DiagramasInspectoresDto, EstadosDiagrama } from '../model/diagramasinspectores.model';
import { DiagramasInspectoresService } from './diagramasinspectores.service';
import { NgForm } from '@angular/forms';
import { EstadosDiagramaInspectoresService } from '../estadosdiagramainspectores/estadosdiagramainspectores.service';
import { EstadosDiagramaInspectoresDto } from '../model/estadosdiagramainspectores.model';


@Component({
    selector: 'createOrEditDiagramasInspectoresDtoModal',
    templateUrl: './create-or-edit-diagramasinspectores-modal.component.html',

})
export class CreateOrEditDiagramasInspectoresModalComponent extends DetailAgregationComponent<DiagramasInspectoresDto> {

    loading: boolean;
    diagramacionBusyText: string;
    protected cfr: ComponentFactoryResolver;

    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    self: CreateOrEditDiagramasInspectoresModalComponent;


    allowmodificargropoInsp: boolean = false;
    allowAddZona: boolean = false;
    grupoInspRequerido: boolean = false;
    mesRequerido: boolean = false;
    anioRequerido: boolean = false;
    anioIncorrecto: boolean = false;
    estadoRequerido: boolean = true;
    constructor(dialogRef: MatDialogRef<CreateOrEditDiagramasInspectoresModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DiagramasInspectoresDto,
        injector: Injector,
        service: DiagramasInspectoresService,
        private estadosDiagrama: EstadosDiagramaInspectoresService,
        private cdr: ChangeDetectorRef
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);

        this.IsInMaterialPopupMode = true;
    }

    completedataBeforeShow(item: DiagramasInspectoresDto): any {
        if (this.detail.Id == null) {
            var today = new Date();
            this.detail.Anio = today.getFullYear();
            this.detail.Mes = today.getMonth() + 1;
            this.detail.EstadoDiagramaId = EstadosDiagrama.Borrador;
            if (this.detail.EstadoDiagramaId == null || this.detail.EstadoDiagramaId.toString() == '' || this.detail.EstadoDiagramaId == 0) {
                this.estadoRequerido = true;
            }
            else {
                this.estadoRequerido = false;
            }
            this.cdr.detectChanges();

        }
    }

    save(form: NgForm): void {

        this.grupoInspRequerido = (this.detail.GrupoInspectoresId == null || this.detail.GrupoInspectoresId.toString() == 'null' || this.detail.GrupoInspectoresId.toString() == '');
        this.mesRequerido = (this.detail.Mes == null || this.detail.Mes.toString() == 'null' || this.detail.Mes.toString() == '');
        this.anioRequerido = (this.detail.Anio == null || this.detail.Anio.toString() == 'null' || this.detail.Anio.toString() == '');
        this.anioIncorrecto = (this.anioRequerido || this.detail.Anio.toString().length != 4)
        if (this.grupoInspRequerido || this.mesRequerido || this.anioRequerido || this.anioIncorrecto)
            return;

        this.saving = true;
        this.completedataBeforeSave(this.detail);

        if (!this.validateSave()) {
            this.saving = false;
            return;
        }
        this.SaveDetail();
    }


    OnMesInspectoresComboChanged($$event) {
        if (this.detail.Mes == null || this.detail.Mes.toString() == 'null' || this.detail.Mes.toString() == '') {
            this.mesRequerido = true;
        }
        else {
            this.mesRequerido = false;
        }
    }


    OnAnioInspectoresComboChanged($event) {
        if ($event == null || $event == 'null' || $event == '') {
            this.anioRequerido = true;
            this.anioIncorrecto = false;
        }
        else {
            this.anioRequerido = false;
            if ($event.toString().length == 4) {
                this.anioIncorrecto = false;
            }
            else {
                this.anioIncorrecto = true;
            }
        }
    }

    OnGrupoInspectoresComboChanged($event) {
        if (this.detail.GrupoInspectoresId == null || this.detail.GrupoInspectoresId.toString() == '') {
            this.grupoInspRequerido = true;
        }
        else {
            this.grupoInspRequerido = false;
        }
    }


}



