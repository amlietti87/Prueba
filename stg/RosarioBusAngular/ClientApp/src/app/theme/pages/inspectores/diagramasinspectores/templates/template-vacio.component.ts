// Angular
import { AfterViewInit, Component, ElementRef, Input, OnInit, ViewChild, EventEmitter, Output, OnDestroy, Injector } from '@angular/core';
import { DiasMesDto, InspectorDiaDto, ValidationResult } from '../../model/diagramasinspectores.model';
import { ZonasDto } from '../../model/zonas.model';
import { RangosHorariosDto } from '../../model/rangoshorarios.model';
import { DiagramasInspectoresValidatorService } from '../diagramas-inspectores-validator.service';
import { Notification } from '../../../../../shared/notification/notification.service';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { DiagramasInspectoresService } from '../diagramasinspectores.service';
import * as _ from "lodash"

@Component({
    selector: 'template-vacio',
    templateUrl: './template-vacio.component.html',
    exportAs: 'templateVacio',
    styleUrls: ["./template-vacio.component.css"]
})
export class TemplateVacioComponent extends AppComponentBase implements OnInit, AfterViewInit, OnDestroy {

    @Input() legajo: string;
    @Input() diaMes: DiasMesDto;
    item: InspectorDiaDto;
    itemOriginal: InspectorDiaDto;
    @Input() zonasItems: ZonasDto[];
    @Input() rangoshorariosItems: RangosHorariosDto[] = [];
    @Input() rangohorariosItemsSinFT: RangosHorariosDto[] = [];
    @Input() listModel: DiasMesDto[] = [];
    @Input() diasMesAP: DiasMesDto[] = [];
    @Input() columns: InspectorDiaDto[] = [];
    rango: RangosHorariosDto;
    zona: ZonasDto;
    isDisabledZona: boolean = true;
    injector: Injector;
    allowmodificarDiagramacion: boolean = true;
    // Public properties
    constructor(injector: Injector, private el: ElementRef, private _validator: DiagramasInspectoresValidatorService, private diagramaService: DiagramasInspectoresService) {
        super(injector);
        this.injector = injector;
    }

    ngOnInit() {
        this.item = this.diaMes.Inspectores.find(e => e.Legajo == this.legajo);
        this.itemOriginal = _.cloneDeep(this.item);
        this.allowmodificarDiagramacion = this.permission.isGranted("Inspectores.Diagramacion.Modificar");
    }

    ngAfterViewInit() {

    }

    ngOnDestroy(): void {
        //this.subs.forEach(e => e.unsubscribe());
    }

    OnTurnoComboChanged(newValue, turno) {
        if (turno.RangoHorarioId == null || turno.RangoHorarioId == "null" || turno.RangoHorarioId == 0) {
            this.isDisabledZona = true;
        }
        else {
            this.rango = this.rangohorariosItemsSinFT.find(r => r.Id == newValue);
            this.isDisabledZona = this.rango.EsFranco;

            if (this.rango.EsFranco) {
                this.item.EsFranco = true;
                this.item.RangoHorarioId = this.rango.Id;
                this.item.PasadaSueldos = 'N';
                this.item.NombreRangoHorario = this.rango.Descripcion;
                this.item.Color = this.rango.Color;
                this.item.diaMesFecha = this.diaMes.Fecha;
                this.item.EsModificada = true;
                this.columns.find(e => e.CodEmpleado == this.item.CodEmpleado).CantFrancos++;
                //this.notify.info('Guardado exitosamente');
            }
        }
    }

    OnZonaComboChanged(newValue, item) {
        if (item.ZonaId == null || item.ZonaId == "null" || item.ZonaId == 0) {

        }
        else {
            this.isDisabledZona = false;

            this.zona = this.zonasItems.find(r => r.Id == newValue);
            this.item.EsJornada = true;
            this.item.PasadaSueldos = 'N';
            var dia = new Date(this.diaMes.Fecha.toString());
            this.item.HoraDesde = new Date(dia.getFullYear(), dia.getMonth(), dia.getDate(), this.rango.HoraDesde.hour, this.rango.HoraDesde.minute);//this.rango.HoraDesde.fecha;  
            this.item.HoraHasta = new Date(dia.getFullYear(), dia.getMonth(), dia.getDate(), this.rango.HoraHasta.hour, this.rango.HoraHasta.minute); //this.rango.HoraHasta.fecha;
            this.item.HoraDesdeModificada = new Date(dia.getFullYear(), dia.getMonth(), dia.getDate(), this.rango.HoraDesde.hour, this.rango.HoraDesde.minute);
            this.item.HoraHastaModificada = new Date(dia.getFullYear(), dia.getMonth(), dia.getDate(), this.rango.HoraHasta.hour, this.rango.HoraHasta.minute);

            var otroDia = new Date(this.rango.HoraHasta.fecha.toString());
            if (otroDia.getDate() == 2) {
                this.item.HoraHasta = new Date(dia.getFullYear(), dia.getMonth(), dia.getDate() + 1, this.rango.HoraHasta.hour, this.rango.HoraHasta.minute);
                this.item.HoraHastaModificada = new Date(dia.getFullYear(), dia.getMonth(), dia.getDate() + 1, this.rango.HoraHasta.hour, this.rango.HoraHasta.minute);
            }

            this.item.RangoHorarioId = this.rango.Id;
            this.item.Color = this.rango.Color;
            this.item.ZonaId = this.zona.Id;
            this.item.NombreZona = this.zona.Description;
            this.item.diaMes = _.cloneDeep(this.diaMes);
            this.item.diaMes.Inspectores = null;
            this.item.diaMesFecha = this.diaMes.Fecha;
            this.item.EsModificada = true;

            if (this.item.EsJornada) {
                var fpe = new ValidationResult();
                fpe.isValid = true;
                var hfi = new ValidationResult();
                hfi.isValid = true;
                var hfg = new ValidationResult();
                hfg.isValid = true;

                fpe = this._validator.ValidateFeriadoPermiteHsExtras(this.item, this.diaMes, this.listModel, this.diasMesAP);
                hfi = this._validator.ValidateHorasFeriadoParaInspector(this.item, this.listModel, this.diasMesAP);
                hfg = this._validator.ValidateHorasFeriadoPorGrupo(this.item, this.listModel, this.diasMesAP);

                let hei = this._validator.ValidateHorasExtrasParaInspector(this.item, this.listModel, this.diasMesAP);
                let heg = this._validator.ValidateHorasExtrasPorGrupo(this.item, this.listModel, this.diasMesAP);

                if (!fpe.isValid || !hfi.isValid || !hfg.isValid || !hei.isValid || !heg.isValid) {
                    //let diasMes = this.diagramaService.getRowFromDiagramaOriginal(this.diaMes);
                    //let itemOriginal = diasMes.Inspectores.find(x => x.Legajo == this.legajo);
                    //item modificado = item original
                    this.item.Nombre = this.itemOriginal.Nombre;
                    this.item.Apellido = this.itemOriginal.Apellido;
                    this.item.CodEmpleado = this.itemOriginal.CodEmpleado;
                    this.item.DescripcionInspector = this.itemOriginal.DescripcionInspector;
                    this.item.InspColor = this.itemOriginal.InspColor;
                    this.item.InspTurno = this.itemOriginal.InspTurno;
                    this.item.Legajo = this.itemOriginal.Legajo;
                    this.item.Id = this.itemOriginal.Id;
                    this.item.EsJornada = false;
                    this.item.CodJornada = this.itemOriginal.CodJornada;
                    this.item.EsFrancoTrabajado = false;
                    this.item.EsFranco = false;
                    this.item.EsNovedad = false;
                    this.item.FrancoNovedad = false;
                    this.item.RangoHorarioId = null;
                    console.log(this.item.ZonaId, item.ZonaId);

                    setTimeout(() => {
                        this.item.ZonaId = null;
                        item.ZonaId = "null"; 
                    },10)
                                   

                    this.item.HoraHastaModificada = this.itemOriginal.HoraHastaModificada;
                    this.item.HoraDesdeModificada = this.itemOriginal.HoraDesdeModificada;
                    this.item.HoraDesde = this.itemOriginal.HoraDesde;
                    this.item.HoraHasta = this.itemOriginal.HoraHasta;
                    this.item.Color = null;
                    this.item.NombreZona = null;
                    this.item.NombreRangoHorario = null;
                    this.item.DescNovedad = null;
                    this.item.PasadaSueldos = null;
                    //this.item.zonasItems = itemOriginal.zonasItems;
                    //this.item.rangosItems = itemOriginal.rangosItems;
                    this.item.diaMes = null;
                    this.item.GrupoInspectoresId = this.itemOriginal.GrupoInspectoresId;
                    this.item.validations = [];
                    this.item.EsModificada = false;
                    this.isDisabledZona = true;

                    if (!fpe.isValid)
                        this.notify.warn(fpe.Messages[0]);
                    if (!hfi.isValid)
                        this.notify.warn(hfi.Messages[0]);
                    if (!hfg.isValid)
                        this.notify.warn(hfg.Messages[0]);
                    if (!hei.isValid)
                        this.notify.warn(hei.Messages[0]);
                    if (!heg.isValid)
                        this.notify.warn(heg.Messages[0]);
                    
                    return;
                }
            }
        }

    }

}

