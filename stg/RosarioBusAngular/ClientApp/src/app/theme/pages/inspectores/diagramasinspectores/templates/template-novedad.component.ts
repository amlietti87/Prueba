// Angular
import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild, EventEmitter, Output, OnDestroy, Injector } from '@angular/core';
import { DiasMesDto, InspectorDiaDto } from '../../model/diagramasinspectores.model';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { ZonasDto } from '../../model/zonas.model';
import { RangosHorariosDto } from '../../model/rangoshorarios.model';
import * as _ from "lodash"
import { AgregarFrancoComponent } from './agregar-franco.component';
import { RangosHorariosService } from '../../rangoshorarios/rangoshorarios.service';

@Component({
    selector: 'template-novedad',
    templateUrl: './template-novedad.component.html',
    exportAs: 'templateNovedad',
    styleUrls: ["./template-novedad.component.css"]
})
export class TemplateNovedadComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {

    @Input() legajo: string;

    @Input() diaMes: DiasMesDto;
    item: InspectorDiaDto;
    @Input() zonasItems: ZonasDto[];
    @Input() rangoshorariosItems: RangosHorariosDto[];
    @Input() listModel: DiasMesDto[];
    @Input() columns: InspectorDiaDto[] = [];

    active: boolean;

    injector: Injector;
    public dialog: MatDialog;
    allowmodificarDiagramacion: boolean = false;
    // Public properties
    constructor(injector: Injector, private rangosHorarios: RangosHorariosService) {
        super(injector);
        this.dialog = injector.get(MatDialog);
        this.injector = injector;
    }

    ngOnInit() {
        this.active = false;
        this.item = this.diaMes.Inspectores.find(e => e.Legajo == this.legajo);
        this.item.EsModificada = false;
        this.allowmodificarDiagramacion = this.permission.isGranted("Inspectores.Diagramacion.Modificar");
    }

    ngAfterViewInit() {

    }

    ngOnDestroy(): void {
        //this.subs.forEach(e => e.unsubscribe());
    }

    getColor() {
        return this.item.Color;
    }

    agregarFranco(row) {

        this.message.confirm('¿Insertar Franco?', '', (a) => {
            //this.isshowalgo = !this.isshowalgo;
            if (a.value) {
                if (this.item.PasadaSueldos != "S") {
                    var rangoFranco = this.rangoshorariosItems.filter(r => r.EsFranco == true && r.EsFrancoTrabajado == false);


                    if (rangoFranco.length == 1) {
                        this.item.EsModificada = true;
                        this.item.EsFranco = true;
                        this.item.RangoHorarioId = rangoFranco[0].Id;
                        this.item.NombreRangoHorario = rangoFranco[0].Description;
                        this.item.Color = rangoFranco[0].Color;
                        this.item.diaMesFecha = this.diaMes.Fecha;
                        this.columns.find(e => e.CodEmpleado == this.item.CodEmpleado).CantFrancos++;
                        this.active = true;
                    }
                    else if (rangoFranco.length > 1) {
                        var dialog: MatDialog;
                        dialog = this.injector.get(MatDialog);
                        const dialogConfig = new MatDialogConfig();
                        dialogConfig.disableClose = false;
                        dialogConfig.autoFocus = true;
                        var cloneData = _.cloneDeep(this.item);
                        dialogConfig.data = cloneData;
                        dialogConfig.data.rangoshorariosItems = this.rangoshorariosItems;
                        dialogConfig.data.diaMes = this.diaMes;
                        dialogConfig.data.listModel = this.listModel;
                        //dialogConfig.width = '60%';
                        let dialogRef = this.dialog.open(AgregarFrancoComponent, dialogConfig);

                        dialogRef.afterClosed().subscribe(
                            data => {
                                if (data) {
                                    this.item.RangoHorarioId = data.RangoHorarioId;
                                    this.rangosHorarios.getById(data.RangoHorarioId)
                                        .subscribe(e => {
                                            this.item.EsModificada = true;
                                            this.item.EsFranco = true;
                                            this.item.NombreRangoHorario = e.DataObject.Description;
                                            this.item.RangoHorarioId = e.DataObject.Id;
                                            this.item.Color = e.DataObject.Color;
                                            this.item.diaMesFecha = this.diaMes.Fecha;
                                            this.columns.find(e => e.CodEmpleado == this.item.CodEmpleado).CantFrancos++;
                                            this.active = true;
                                        });
                                }
                            }
                        );
                    }
                    else if (rangoFranco.length == 0) {
                        this.message.error('No existe rango horario para el franco', 'Franco Novedad')
                    }




                } else {
                    this.message.error('Esta Jornada ya fue pasada a sueldo, no es posible editarla', 'Jornada Trabajada en sueldo')
                }
            }

        });
    }

}