// Angular
import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild, EventEmitter, Output, OnDestroy, Inject, Injector } from '@angular/core';
import { InspectorDiaDto } from '../../model/diagramasinspectores.model';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { NgForm } from '@angular/forms';
import { ZonasDto } from '../../model/zonas.model';
import { DiagramasInspectoresValidatorService } from '../diagramas-inspectores-validator.service';
import { DiagramasInspectoresService } from '../diagramasinspectores.service';

@Component({
    selector: 'exportarminutosporsector',
    templateUrl: './agregar-franco.component.html',
})
export class AgregarFrancoComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {

    filter: InspectorDiaDto;
    @ViewChild('agregarFrancoForm') agregarFrancoForm: NgForm;
    // Public properties
    constructor(
        protected dialogRef: MatDialogRef<AgregarFrancoComponent>, private _validator: DiagramasInspectoresValidatorService, private _diagramacion: DiagramasInspectoresService,
        @Inject(MAT_DIALOG_DATA) public data: InspectorDiaDto,
        injector: Injector) {
        super(injector);

        this.filter = data;
    }

    ngOnInit() {
        console.log("filter franco", this.filter);
    }

    ngAfterViewInit() {

    }

    ngOnDestroy(): void {
        //this.subs.forEach(e => e.unsubscribe());
    }

    close(): void {
        this.dialogRef.close(false);
    }

    agregarFranco(RangoHorarioId) {
        if (RangoHorarioId != null && RangoHorarioId != 'null') {
            this.filter.NombreRangoHorario = this.agregarFrancoForm.controls['ComboRangoHorario'].value;
            this.filter.diaMesFecha = this.filter.diaMes.Fecha;
            this.dialogRef.close(this.filter);

            console.log("agregar-franco::::", this.filter);
        }
    }

}
