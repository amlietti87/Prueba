import { RangosHorariosService } from './../../rangoshorarios/rangoshorarios.service';
// Angular
import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild, EventEmitter, Output, OnDestroy, Injector } from '@angular/core';
import { DiasMesDto, InspectorDiaDto, ValidationResult } from '../../model/diagramasinspectores.model';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { EditFrancoComponent } from './edit-franco.component';
import { ZonasDto } from '../../model/zonas.model';
import { RangosHorariosDto } from '../../model/rangoshorarios.model';
import * as _ from "lodash"
import { DiagramasInspectoresService } from '../diagramasinspectores.service';
import { DiagramasInspectoresValidatorService } from '../diagramas-inspectores-validator.service';

@Component({
    selector: 'template-franco',
    templateUrl: './template-franco.component.html',
    exportAs: 'templateFranco',
    styleUrls: ["./template-franco.component.css"]
})
export class TemplateFrancoComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {

    @Input() legajo: string;
    @Input() diaMes: DiasMesDto;
    @Input() zonasItems: ZonasDto[];
    @Input() rangoshorariosItems: RangosHorariosDto[];
    @Input() listModel: DiasMesDto[];

    @Input() BlockDate: Date;
    @Input() diasMesAP: DiasMesDto[];
    @Input() columns: InspectorDiaDto[] = [];
    @Output()
    setloading = new EventEmitter<boolean>();

    item: InspectorDiaDto;
    allowmodificarDiagramacion: boolean = false
    pagacambia: boolean = false;
    rangos: RangosHorariosDto[] = [];
    active: boolean = false;
    injector: Injector;
    public dialog: MatDialog;

    constructor(injector: Injector, private rangosHorarios: RangosHorariosService, private diagramaService: DiagramasInspectoresService, private _validator: DiagramasInspectoresValidatorService, ) {
        super(injector);
        this.dialog = injector.get(MatDialog);
        this.injector = injector;
    }

    ngOnInit() {
        this.item = this.diaMes.Inspectores.find(e => e.Legajo == this.legajo);
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

    completedataBeforeShow(item: InspectorDiaDto): any {
    }

    editarFranco() {
        setTimeout(() => {
            let fpft: ValidationResult;
            fpft = this._validator.ValidateFeriadoPermiteFrancoTrabajadoCelda(this.item, this.diaMes);
            if (!fpft.isValid) {
                this.item.Pago = 0;
                this.pagacambia = true;
            }
            else {
                this.item.Pago =null;
            }
            let rangosConFrancotrabajado = this.rangoshorariosItems.find(r => r.EsFrancoTrabajado == true);
            if (rangosConFrancotrabajado == null || rangosConFrancotrabajado == undefined) {
                this.message.warn('Debe crear el Rango Horario para el franco trabajado.', '');
                return;
            }
            this.message.confirm('¿Cambiar a Franco Trabajado?', '', (a) => {
                if (a.value) {
                    this.editar();
                }
                else {
                    this.item.Pago = 0;
                }
            });
        }, 200);
    }

    editar() {
        if (this.item.PasadaSueldos != "S") {
            var dialog: MatDialog;
            dialog = this.injector.get(MatDialog);
            const dialogConfig = new MatDialogConfig();
            dialogConfig.disableClose = false;
            dialogConfig.autoFocus = true;
            var cloneData = _.cloneDeep(this.item);
            dialogConfig.data = cloneData;
            dialogConfig.data.zonasItems = this.zonasItems;
            dialogConfig.data.rangosItems = this.rangoshorariosItems;
            dialogConfig.data.diaMes = this.diaMes;
            dialogConfig.data.listModel = this.listModel;
            dialogConfig.data.diasMesAP = this.diasMesAP;
            dialogConfig.data.allowPagaCambia = this.pagacambia;

            let dialogRef = this.dialog.open(EditFrancoComponent, dialogConfig);

            dialogRef.afterClosed().subscribe(
                data => {
                    if (data) {
                        this.rangosHorarios.getById(data.RangoHorarioId).subscribe(e => {
                            this.item.Color = e.DataObject.Color;
                            this.item.NombreRangoHorario = e.DataObject.Description;
                            this.item.RangoHorarioId = e.DataObject.Id;
                            this.item.EsModificada = true;
                            this.item.EsFrancoTrabajado = true;
                            this.active = true;
                            this.item.HoraDesde = data.HoraDesdeModificada;
                            this.item.HoraHasta = data.HoraHastaModificada;
                            this.item.HoraDesdeModificada = data.HoraDesdeModificada;
                            this.item.HoraHastaModificada = data.HoraHastaModificada;
                            var newZona = this.zonasItems.find(z => z.Id == data.ZonaId);
                            this.item.ZonaId = data.ZonaId;
                            this.item.NombreZona = newZona.Description;
                            this.item.diaMesFecha = data.diaMesFecha;
                            this.item.Pago = data.Pago;                            
                            this.columns.find(e => e.CodEmpleado == this.item.CodEmpleado).CantFrancos--;
                        });
                    }
                    else {
                        this.item.Pago = 0;
                    }
                });
        } else {
            this.message.error('Este Franco ya fue pasado a sueldo, no es posible editarlo', 'Franco en sueldo')
        }
    }


    eliminarFranco(item: InspectorDiaDto) {

        let diaMes = this.diagramaService.getRowFromDiagramaOriginal(this.diaMes);
        let itemOriginal = diaMes.Inspectores.find(x => x.Legajo == this.legajo);

        this.message.confirm('¿Está seguro que quiere eliminar el registro?', '', (a) => {
            if (a.value) {
                if (item.EsModificada) {
                    if (itemOriginal) {
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
                        this.item.Pago = itemOriginal.Pago;
                        // this.item.zonasItems = itemOriginal.zonasItems;
                        // this.item.rangosItems = itemOriginal.rangosItems;
                        //this.item.diaMesFecha = itemOriginal.diaMesFecha;
                        this.item.GrupoInspectoresId = itemOriginal.GrupoInspectoresId;
                        this.item.validations = [];
                        this.item.EsModificada = false;
                        this.columns.find(e => e.CodEmpleado == this.item.CodEmpleado).CantFrancos--;
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
        cloneData.Inspectores = [item];

        cloneData.BlockDate = this.BlockDate;
        this.setloading.emit(true);
        this.diagramaService.eliminarCelda(cloneData)
            .finally(() => {
                this.setloading.emit(false);
                
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
            this.columns.find(e => e.CodEmpleado == this.item.CodEmpleado).CantFrancos--;
            this.notify.info('Guardado exitosamente');
            }, error => {
                this.handleErros(error)
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
