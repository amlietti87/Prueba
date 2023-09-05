// Angular
import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild, EventEmitter, Output, OnDestroy, Inject, Injector } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { NgForm } from '@angular/forms';
import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { InspectorDiaDto, DiagramaMesAnioDto, DiagramasInspectoresDto } from '../model/diagramasinspectores.model';
import { PersTurnosDto } from '../model/persturnos.model';
import { Observable } from "rxjs/Observable";
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

declare let mApp: any;

@Component({
    selector: 'exportarminutosporsector',
    templateUrl: './edit-diagramacion.component.html',
    styleUrls: ['./diagramacion.component.css']
})

export class EditDiagramacionComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {

    //@Input() TurnosItems: TurnosDto[];
    selectedTurnos: Array<PersTurnosDto> = [];
    itemsTurnos: PersTurnosDto[] = [];
    filter: DiagramasInspectoresDto;
    filterargs = { title: 'hello' };

    @ViewChild('editDiagramacionForm') editDiagramacionForm: NgForm;
    // Public properties
    constructor(
        protected dialogRef: MatDialogRef<EditDiagramacionComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DiagramasInspectoresDto,
        injector: Injector) {

        super(injector);

        this.filter = data;


    }

    ngOnInit() {
        this.filter.InspDiagramaInspectoresTurnos.forEach(insp => {
            this.itemsTurnos.push(insp.Turno);
        });
    }

    ngAfterViewInit() {
        mApp.init();
    }

    ngOnDestroy(): void {
        //this.subs.forEach(e => e.unsubscribe());

    }

    close(): void {
        this.dialogRef.close(false);
    }

    OnAceptar() {
        let itemsTurnosSelected = this.selectedTurnos;
        if (itemsTurnosSelected.length > 0) {

            this.dialogRef.close(this.selectedTurnos);
        }
    }







}
