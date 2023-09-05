import { DiagramasInspectoresService } from './../diagramasinspectores.service';
// Angular
import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild, EventEmitter, Output, OnDestroy, Injector } from '@angular/core';
import { DiasMesDto, InspectorDiaDto, ValidationResult } from '../../model/diagramasinspectores.model';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { EditJornadaTrabajadaComponent } from './edit-jornada-trabajada.component';
import { NotificationService } from '../../../../../shared/notification/notification.service';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { ZonasDto } from '../../model/zonas.model';
import * as _ from "lodash"
import * as moment from 'moment';
import { DiagramasInspectoresValidatorService } from '../diagramas-inspectores-validator.service';

@Component({
    selector: 'template-jornada-trabajada',
    templateUrl: './template-jornada-trabajada.component.html',
    exportAs: 'templateJornadaTrabajada',
    styleUrls: ["./template-jornada-trabajada.component.css"]
})
export class TemplateJornadaTrabajadaComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {

    diagramacionBusyText: string;
    @Input() BlockDate: Date;
    loading: boolean = true;
    @Input() legajo: string;
    @Input() diaMes: DiasMesDto;
    @Input() zonasItems: ZonasDto[];
    @Input() listModel: DiasMesDto[];
    @Input() diasMesAP: DiasMesDto[];
    @Input() columns: InspectorDiaDto[] = [];
    @Output()
    setloading = new EventEmitter<boolean>();

    item: InspectorDiaDto;
    injector: Injector;
    public dialog: MatDialog;
    allowmodificarDiagramacion: boolean = false;
    pagacambia: boolean = false;

    constructor(injector: Injector, private diagramaService: DiagramasInspectoresService, private _validator: DiagramasInspectoresValidatorService) {
        super(injector);
        this.dialog = injector.get(MatDialog);
        this.injector = injector;
    }

    ngOnInit() {

        this.item = this.diaMes.Inspectores.find(e => e.Legajo == this.legajo);
        //this.item.HoraDesde = new Date(this.item.HoraDesde.toString());
        //this.item.HoraHasta = new Date(this.item.HoraHasta.toString());
        this.item.HoraDesdeModificada = new Date(this.item.HoraDesdeModificada.toString());
        this.item.HoraHastaModificada = new Date(this.item.HoraHastaModificada.toString());
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

    editarJorTrabajo() {
        if (this.item.PasadaSueldos != "S") {

            let fpft: ValidationResult;
            fpft = this._validator.ValidateFeriadoPermiteFrancoTrabajadoCelda(this.item, this.diaMes);
            if (!fpft.isValid && this.item.Pago == 0) {
                this.pagacambia = true;
            }

            var dialog: MatDialog;
            dialog = this.injector.get(MatDialog);
            const dialogConfig = new MatDialogConfig();
            dialogConfig.disableClose = false;
            dialogConfig.autoFocus = true;
            var cloneData = _.cloneDeep(this.item);
            dialogConfig.data = cloneData;
            dialogConfig.data.HoraDesdeModificada = this.item.HoraDesdeModificada;
            dialogConfig.data.HoraHastaModificada = this.item.HoraHastaModificada;
            dialogConfig.data.diaMes = this.diaMes;
            dialogConfig.data.zonasItems = this.zonasItems;
            dialogConfig.data.listModel = this.listModel;
            dialogConfig.data.diasMesAP = this.diasMesAP;
            dialogConfig.data.allowPagaCambia = this.pagacambia;
           

            let dialogRef = this.dialog.open(EditJornadaTrabajadaComponent, dialogConfig);

            dialogRef.afterClosed().subscribe(
                data => {

                    if (data) {
                        this.item.EsModificada = true;
                        this.item.HoraDesdeModificada = data.HoraDesdeModificada;
                        this.item.HoraHastaModificada = data.HoraHastaModificada;
                        var newZona = this.zonasItems.find(z => z.Id == data.ZonaId);
                        this.item.ZonaId = data.ZonaId;
                        this.item.NombreZona = newZona.Description;
                        this.item.diaMesFecha = data.diaMes.Fecha;
                        this.item.validations = [];
                        this.item.Pago = data.Pago;
                        this.item.Pago = data.Pago;

                        //this.notify.info('Guardado exitosamente');
                    }
                }
            );
        } else {
            this.message.error('Esta Jornada ya fue pasada a sueldo, no es posible editarla', 'Jornada Trabajada en sueldo')
        }
    }

    getErrorMessage(item: InspectorDiaDto): string {
        return item.validations.map(e => e.Messages).join("-");
    }

    eliminarJornada(item: InspectorDiaDto) {

        let diaMes = this.diagramaService.getRowFromDiagramaOriginal(this.diaMes);
        let itemOriginal = diaMes.Inspectores.find(x => x.Legajo == this.legajo);

        this.message.confirm('¿Está seguro que quiere eliminar el registro?', '', (a) => {

            if (a.value) {
                if (item.EsModificada) {
                    if (itemOriginal) {
                        if (itemOriginal.EsFranco && !itemOriginal.EsFrancoTrabajado) {
                            //item modificado = item original
                            this.item.Nombre = itemOriginal.Nombre;
                            this.item.Apellido = itemOriginal.Apellido;
                            this.item.CodEmpleado = itemOriginal.CodEmpleado;
                            this.item.DescripcionInspector = itemOriginal.DescripcionInspector;
                            this.item.InspColor = itemOriginal.InspColor;
                            this.item.InspTurno = itemOriginal.InspTurno;
                            this.item.Legajo = itemOriginal.Legajo;
                            this.item.Id = itemOriginal.Id;
                            this.item.EsJornada = itemOriginal.EsJornada;
                            this.item.CodJornada = itemOriginal.CodJornada;
                            this.item.EsFrancoTrabajado = itemOriginal.EsFrancoTrabajado;
                            this.item.EsFranco = itemOriginal.EsFranco;
                            this.item.EsNovedad = itemOriginal.EsNovedad;
                            this.item.FrancoNovedad = itemOriginal.FrancoNovedad;
                            this.item.RangoHorarioId = itemOriginal.RangoHorarioId;
                            this.item.ZonaId = itemOriginal.ZonaId;
                            this.item.HoraDesde = itemOriginal.HoraDesde;
                            this.item.HoraHasta = itemOriginal.HoraHasta;
                            this.item.HoraDesdeModificada = itemOriginal.HoraDesdeModificada;
                            this.item.HoraHastaModificada = itemOriginal.HoraHastaModificada;
                            this.item.Color = itemOriginal.Color;
                            this.item.NombreZona = itemOriginal.NombreZona;
                            this.item.NombreRangoHorario = itemOriginal.NombreRangoHorario;
                            this.item.DescNovedad = itemOriginal.DescNovedad;
                            this.item.PasadaSueldos = itemOriginal.PasadaSueldos;
                            // this.item.zonasItems = itemOriginal.zonasItems;
                            // this.item.rangosItems = itemOriginal.rangosItems;
                            // this.item.diaMes = itemOriginal.diaMes;
                            //this.item.diaMesFecha = itemOriginal.diaMesFecha;
                            this.columns.find(e => e.CodEmpleado == this.item.CodEmpleado).CantFrancos++;
                            this.item.GrupoInspectoresId = itemOriginal.GrupoInspectoresId;
                            // this.item.validations = itemOriginal.validations;
                            this.item.validations = [];
                            this.item.EsModificada = false;
                        }
                        else {
                            if (itemOriginal.CodJornada != 0) {
                                this.eliminar(item, this.diaMes);
                            }
                            else {
                                this.item.EsFranco = false;
                                this.item.EsFrancoTrabajado = false;
                                this.item.EsJornada = false;
                                this.item.EsModificada = false;
                                this.item.EsNovedad = false;
                                this.item.FrancoNovedad = false;
                                this.item.ZonaId = null;
                                this.item.RangoHorarioId = null;
                                this.item.EsModificada = false;
                            }
                        }
                    }
                    else {
                        this.item.EsFranco = false;
                        this.item.EsFrancoTrabajado = false;
                        this.item.EsJornada = false;
                        this.item.EsModificada = false;
                        this.item.EsNovedad = false;
                        this.item.FrancoNovedad = false;
                        this.item.ZonaId = null;
                        this.item.RangoHorarioId = null;
                    }
                } else {
                    this.eliminar(item, this.diaMes);
                }
            }
        });
    }

    eliminar(item: InspectorDiaDto, row: DiasMesDto) {
        let cloneData: DiasMesDto = _.cloneDeep(row);
        cloneData.BlockDate = this.BlockDate;
        cloneData.Inspectores = [item];
        console.log("Eliminar Jornada Trabajada:::::", cloneData);
        this.setloading.emit(true);
        this.diagramaService.eliminarCelda(cloneData)
            .finally(() => {
                this.loading = false;
                this.setloading.emit(false);
                this.diagramacionBusyText = "Cargando...";
            })
            .subscribe(e => {
            this.item.Nombre = e.DataObject.Nombre;
            this.item.Apellido = e.DataObject.Apellido;
            this.item.CodEmpleado = e.DataObject.CodEmpleado;
            this.item.DescripcionInspector = e.DataObject.DescripcionInspector;
            this.item.InspColor = e.DataObject.InspColor;
            this.item.InspTurno = e.DataObject.InspTurno;
            this.item.Legajo = e.DataObject.Legajo;
            this.item.Id = e.DataObject.Id;
            this.item.Nombre = e.DataObject.Nombre;
            this.item.EsJornada = e.DataObject.EsJornada;
            this.item.CodJornada = e.DataObject.CodJornada;
            this.item.EsFrancoTrabajado = e.DataObject.EsFrancoTrabajado;
            this.item.EsFranco = e.DataObject.EsFranco;
            this.item.EsNovedad = e.DataObject.EsNovedad;
            this.item.FrancoNovedad = e.DataObject.FrancoNovedad;
            this.item.RangoHorarioId = e.DataObject.RangoHorarioId;
            this.item.ZonaId = e.DataObject.ZonaId;
            this.item.HoraDesde = e.DataObject.HoraDesde;
            this.item.HoraHasta = e.DataObject.HoraHasta;
            this.item.HoraDesdeModificada = e.DataObject.HoraDesdeModificada;
            this.item.HoraHastaModificada = e.DataObject.HoraHastaModificada;
            this.item.Color = e.DataObject.Color;
            this.item.NombreZona = e.DataObject.NombreZona;
            this.item.NombreRangoHorario = e.DataObject.NombreRangoHorario;
            this.item.DescNovedad = e.DataObject.DescNovedad;
            this.item.PasadaSueldos = e.DataObject.PasadaSueldos;
            // this.item.zonasItems = e.DataObject.zonasItems;
            // this.item.rangosItems = e.DataObject.rangosItems;
            // this.item.diaMes = e.DataObject.diaMes;
            this.item.GrupoInspectoresId = e.DataObject.GrupoInspectoresId;
            this.item.validations = [];
            this.item.EsModificada = false;
            this.notify.info('Guardado exitosamente');
            }, error => {
                    this.handleErros(error);
                });
    }

    protected handleErros(err: any) {
        console.log('sever error:', err);  // debug

        if (err.error.Status == "ConcurrencyValidator") {
            this.notify.error("No es posible realizar los cambios ya que la entidad fue alterada por otro usuario.");

        }
        else if (err.error && err.error.Status == "ValidationError") {
            this.notify.error(err.error.Messages.toString())
        }
        else {
            this.notify.error("A ocurrido un erro por favor contactese con el administrador")
        }
    }

}
