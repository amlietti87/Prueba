import { LineaService } from './../../../providers/linea/linea.service';
import { ToolsProvider } from './../../../shared/page/tools';
import { Component, Input, forwardRef, OnInit, ElementRef, Renderer, ChangeDetectorRef } from '@angular/core';
import { TareasCamposDto } from '../../../models/tareas.model';
import { TareasRealizadasBaseComponent, TareasValueChage } from './tareas-realizadas-base-component';
import { HServiciosFilter } from '../../../models/hServicios.model';
import { HServiciosService } from '../../../providers/servicio-conductor/servicio.service';
import { LineaDto } from '../../../models/linea.model';
import moment from 'moment';

@Component({
  selector: 'linea-tarea-component',
  templateUrl: 'linea-tarea-component.html',
  providers: [{ provide: TareasRealizadasBaseComponent, useExisting: forwardRef(() => LineaTareaComponent) }]
})



export class LineaTareaComponent extends TareasRealizadasBaseComponent implements OnInit{

  @Input() entity: TareasCamposDto = new TareasCamposDto();
  label: string;

  ConductorId: string;
  lineas: LineaDto[];
  ccoche: boolean;


  constructor(
    private ngEl: ElementRef,
    private renderer: Renderer, 
    private changeDetectorRef: ChangeDetectorRef, 
    private hserviciosService: HServiciosService,
    public tools: ToolsProvider,
    public lineaService: LineaService) {
    super(ngEl, renderer);

  }

  ngOnInit() {
    
  }

  setLabel(Etiqueta: string) {
    this.label = Etiqueta;
    this.changeDetectorRef.detectChanges();
  }

  onOtherComponentChanged(e: TareasValueChage): void {
    if (e.key == "ConductorId") {
      if (this.ConductorId != e.value) {
        this.ConductorId = e.value.conductor.Id;
        this.completarLinea();
      }
    }

    if (e.key == 'fechaTarea') {
      if (this.param.FechaString  != e.value) {
        this.param.FechaString = e.value;
        this.completarLinea();
      }

    }      
  }

  public completarLinea() {

    //limpiar
    this.value = null;

    var filtro = new HServiciosFilter();
    filtro.Fecha = moment(this.param.FechaString, "DD/MM/YYYY").toDate().toISOString();
    filtro.ConductorId = this.ConductorId;

    this.hserviciosService.RecuperarLineasPorConductor(filtro).subscribe(lineasPorCond => {

      this.lineas = lineasPorCond.DataObject;
      if(this.lineas.length > 0) {
        this.value = lineasPorCond.DataObject[0].Id;
        this.valueChange.emit({ key: "cod_lin", value: this.value });
      }
    });   
  }

  public verOtrasLineas() {
    this.tools.ShowWait(this.lineaService.GetLineas(), e => {
      this.lineas = e.DataObject;
      this.value = null;
      this.valueChange.emit({ key: "cod_lin", value: this.value });
    });      
  }

  onLineachanged() {
      this.valueChange.emit({ key: "cod_lin", value: this.value });     
  }
}
