
import { Component, ViewChild, Injector, ComponentFactoryResolver, ViewContainerRef, Inject } from '@angular/core';
import * as _ from 'lodash';
declare let mApp: any;
import { DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { GruposInspectoresDto, InspGrupoInspectoresZonaDto, InspGrupoInspectoresTareasDto, InspGrupoInspectoresRangosHorariosDto, InspGrupoInspectoresTurnoDto } from '../model/gruposinspectores.model';
import { GruposInspectoresService } from './gruposinspectores.service';
import { NgForm } from '@angular/forms';
import { ZonasService } from '../zonas/zonas.service';
import { RangosHorariosService } from '../rangoshorarios/rangoshorarios.service';
import { PersTurnosService } from '../turnos/persturnos.service';
import { ItemDto } from '../../../../shared/model/base.model';
import { TareaService } from '../tareas/tarea.service';


@Component({
    selector: 'createOrEditGruposInspectoresDtoModal',
    templateUrl: './create-or-edit-gruposinspectores-modal.component.html',

})
export class CreateOrEditGruposInspectoresModalComponent extends DetailAgregationComponent<GruposInspectoresDto> {

    protected cfr: ComponentFactoryResolver;

    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;
    self: CreateOrEditGruposInspectoresModalComponent;

    allowmodificargropoInsp: boolean = false;
    allowAddZona: boolean = false;
    constructor(dialogRef: MatDialogRef<CreateOrEditGruposInspectoresModalComponent>,
        @Inject(MAT_DIALOG_DATA) public data: GruposInspectoresDto,
        injector: Injector,
        protected service: GruposInspectoresService,
        protected serviceZonas: ZonasService,
        protected rangosHorarios: RangosHorariosService,
        protected serviceTarea: TareaService,
        protected serviceTurnos: PersTurnosService,
    ) {
        super(dialogRef, service, injector, data);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.detail = new GruposInspectoresDto();
        this.IsInMaterialPopupMode = true;
        this.allowmodificargropoInsp = this.permission.isGranted("Inspectores.GruposInspectores.Modificar");
        this.allowAddZona = this.permission.isGranted("Inspectores.Zonas.Agregar")
    }



    save(form: NgForm): void {
        if (!this.detail.NotificacionId) {
            this.detail.NotificacionId = undefined;
        }

        if (this.detailForm && this.detailForm.form.invalid) {
            return;
        }

        if (this.detail.InspGrupoInspectoresZona) {
            this.detail.InspGrupoInspectoresZona = this.detail.InspGrupoInspectoresZona.filter(e=> e.ZonaId != null);
        }

        if (this.detail.InspGrupoInspectoresRangosHorarios) {
            this.detail.InspGrupoInspectoresRangosHorarios = this.detail.InspGrupoInspectoresRangosHorarios.filter(e=> e.RangoHorarioId != null);
        }

        if (this.detail.InspGrupoInspectoresTareas) {
            this.detail.InspGrupoInspectoresTareas = this.detail.InspGrupoInspectoresTareas.filter(e=> e.TareaId != null);
        }

        if (this.detail.InspGrupoInspectoresTurnos) {
            this.detail.InspGrupoInspectoresTurnos = this.detail.InspGrupoInspectoresTurnos.filter(e=> e.TurnoId != null);
        }

        this.saving = true;
        this.completedataBeforeSave(this.detail);

        if (!this.validateSave()) {
            this.saving = false;
            return;
        }
        this.SaveDetail();
    }

    CreateNewGruposInspectoresZona(): InspGrupoInspectoresZonaDto {
        var item = new InspGrupoInspectoresZonaDto();
        return item;
    }

    OnGruposInspectoresZonaRowAdded(): void {

        if (!this.detail.InspGrupoInspectoresZona) {
            this.detail.InspGrupoInspectoresZona = [];
        }
        this.detail.InspGrupoInspectoresZona = [...this.detail.InspGrupoInspectoresZona, this.CreateNewGruposInspectoresZona()];
    }

    OnGruposInspectoresZonaRowRemoved(row: InspGrupoInspectoresZonaDto): void {
        var index = this.detail.InspGrupoInspectoresZona.findIndex(x => x.ZonaId == row.ZonaId);

        if (index >= 0) {
            let lista = [...this.detail.InspGrupoInspectoresZona];
            lista.splice(index, 1);
            this.detail.InspGrupoInspectoresZona = [...lista];
        }
    }

    OnZonaComboChanged(newValue, oldValue): void {        
        if(isNaN(newValue)){
            oldValue.ZonaNombre = "";
            oldValue.ZonaId = null;
            return;
        }
        var zonasFiltered = this.detail.InspGrupoInspectoresZona.filter(e => e.ZonaId == newValue);
        if (zonasFiltered && zonasFiltered.length > 1) {
            this.notify.warn("La zona seleccionada ya fue agregada");
            oldValue.GrupoInspectoresId = null;
            oldValue.ZonaId = null;
            oldValue.Zona = null;
            oldValue.ZonaNombre = null;
            return;
        } else {
            this.serviceZonas.getById(newValue).subscribe(e => {
                if (e.Status == "Ok" && e.DataObject) {
                    oldValue.Zona = e.DataObject;
                    oldValue.ZonaNombre = e.DataObject.Descripcion;
                    this.detail.InspGrupoInspectoresZona = [...this.detail.InspGrupoInspectoresZona];
                }
            });
        }
    }

    CreateNewGruposInspectoresRangosHorarios(): InspGrupoInspectoresRangosHorariosDto {
        var item = new InspGrupoInspectoresRangosHorariosDto();
        return item;
    }

    OnGruposInspectoresRangosHorariosRowAdded(): void {
        if (!this.detail.InspGrupoInspectoresRangosHorarios) {
            this.detail.InspGrupoInspectoresRangosHorarios = [];
        }
        this.detail.InspGrupoInspectoresRangosHorarios = [...this.detail.InspGrupoInspectoresRangosHorarios, this.CreateNewGruposInspectoresRangosHorarios()];
    }

    OnGruposInspectoresRangosHorariosRowRemoved(row: InspGrupoInspectoresRangosHorariosDto): void {
        var index = this.detail.InspGrupoInspectoresRangosHorarios.findIndex(x => x.Id == row.Id);

        if (index >= 0) {
            let lista = [...this.detail.InspGrupoInspectoresRangosHorarios];
            lista.splice(index, 1);
            this.detail.InspGrupoInspectoresRangosHorarios = [...lista];
        }
    }

    OnRangosHorariosComboChanged(newValue, oldValue): void {
        if(isNaN(newValue)){
            oldValue.NombreRangoHorario = null;
            oldValue.RangoHorarioId = null;
            oldValue.RangoHorario = null;
            return;
        }

        var rangosHorariosFiltered = this.detail.InspGrupoInspectoresRangosHorarios.filter(e => e.RangoHorarioId == newValue);
        if (rangosHorariosFiltered && rangosHorariosFiltered.length > 1) {
            this.notify.warn("El rango horario seleccionado ya fue agregado");
            oldValue.GrupoInspectoresId = null;
            oldValue.RangoHorarioId = null;
            oldValue.RangoHorario = null;
            oldValue.NombreRangoHorario = null;
            return;
        } else {
            this.rangosHorarios.getById(newValue).subscribe(e => {
                if (e.Status == "Ok" && e.DataObject) {
                    oldValue.RangoHorario = e.DataObject;
                    oldValue.NombreRangoHorario = e.DataObject.Descripcion;
                    this.detail.InspGrupoInspectoresRangosHorarios = [...this.detail.InspGrupoInspectoresRangosHorarios];
                }
            });
        }
    }

    CreateNewGruposInspectoresTarea(): InspGrupoInspectoresTareasDto {
        var item = new InspGrupoInspectoresTareasDto();        
        return item;
    }

    OnGruposInspectoresTareaRowAdded(): void {
        if (!this.detail.InspGrupoInspectoresTareas) {
            this.detail.InspGrupoInspectoresTareas = [];
        }
        this.detail.InspGrupoInspectoresTareas = [...this.detail.InspGrupoInspectoresTareas, this.CreateNewGruposInspectoresTarea()];
    }

    OnGruposInspectoresTareaRowRemoved(row: InspGrupoInspectoresTareasDto): void {
        var index = this.detail.InspGrupoInspectoresTareas.findIndex(x => x.Id == row.Id);

        if (index >= 0) {
            let lista = [...this.detail.InspGrupoInspectoresTareas];
            lista.splice(index, 1);
            this.detail.InspGrupoInspectoresTareas = [...lista];
        }
    }

    OnTareaComboChanged(newValue, oldValue): void {
        if(isNaN(newValue)){
            oldValue.TareaId = null;
            oldValue.Tarea = null;
            oldValue.TareaNombre = null;
            return;
        }
        var TareaFiltered = this.detail.InspGrupoInspectoresTareas.filter(e => e.TareaId == newValue);
        if (TareaFiltered && TareaFiltered.length > 1) {
            this.notify.warn("La tarea seleccionada ya fue agregada");
            oldValue.GrupoInspectoresId = null;
            oldValue.TareaId = null;
            oldValue.Tarea = null;
            oldValue.TareaNombre = null;
            return;
        } else {
            this.serviceTarea.getById(newValue).subscribe(e => {
                if (e.Status == "Ok" && e.DataObject) {
                    oldValue.Tarea = e.DataObject;
                    oldValue.TareaNombre = e.DataObject.Descripcion;
                    this.detail.InspGrupoInspectoresTareas = [...this.detail.InspGrupoInspectoresTareas];
                }
            });
        }
    }

    //Turnos
    CreateNewGruposInspectoresTurnos(): InspGrupoInspectoresTurnoDto {
        var item = new InspGrupoInspectoresTurnoDto();
        return item;
    }

    OnGruposInspectoresTurnosRowAdded(): void {
        if (!this.detail.InspGrupoInspectoresTurnos) {
            this.detail.InspGrupoInspectoresTurnos = [];
        }
        this.detail.InspGrupoInspectoresTurnos = [...this.detail.InspGrupoInspectoresTurnos, this.CreateNewGruposInspectoresTurnos()];
    }

    OnGruposInspectoresTurnosRowRemoved(row: InspGrupoInspectoresTurnoDto): void {
        var index = this.detail.InspGrupoInspectoresTurnos.findIndex(x => x.Id == row.Id);

        if (index >= 0) {
            let lista = [...this.detail.InspGrupoInspectoresTurnos];
            lista.splice(index, 1);
            this.detail.InspGrupoInspectoresTurnos = [...lista];
        }
    }

    OnTurnosComboChanged(newValue, oldValue): void {        
        if(!newValue){
            oldValue.TurnoId = null;
            oldValue.Turno = null;
            oldValue.TurnoNombre = null;
            return;
        }

        var turnoFiltered = this.detail.InspGrupoInspectoresTurnos.filter(e => e.TurnoId == newValue);
        if (turnoFiltered && turnoFiltered.length > 1) {
            this.notify.warn("El turno seleccionado ya fue agregado");
            oldValue.GrupoInspectoresId = null;
            oldValue.TurnoId = null;
            oldValue.Turno = null;
            oldValue.TurnoNombre = null;
            return;
        } else {
            this.serviceTurnos.getById(newValue).subscribe(e => {
                if (e.Status == "Ok" && e.DataObject) {
                    oldValue.Turno = e.DataObject;
                    oldValue.TurnoNombre = e.DataObject.DscTurno;
                    this.detail.InspGrupoInspectoresTurnos = [...this.detail.InspGrupoInspectoresTurnos];
                }
            });
        }
    }


    OnLineaChange(item: ItemDto) {
        if (item !== null) {
            this.detail.LineaId = item.Id;
        }
        else {
            this.detail.LineaId = null;
        }
    }

}



