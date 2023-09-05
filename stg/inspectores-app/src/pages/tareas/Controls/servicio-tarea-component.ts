import { Component, forwardRef, OnInit, ElementRef, Renderer, ChangeDetectorRef } from '@angular/core';

import { TareasRealizadasBaseComponent, TareasValueChage } from './tareas-realizadas-base-component';
import { HServiciosFilter } from '../../../models/hServicios.model';
import moment from 'moment';
import { ToolsProvider } from '../../../shared/page/tools';
import { HServiciosService } from '../../../providers/servicio-conductor/servicio.service';
import { ItemDto } from '../../../models/Base/base.model';

@Component({
  selector: 'servicio-tarea-component',
  templateUrl: 'servicio-tarea-component.html',
  providers: [{ provide: TareasRealizadasBaseComponent, useExisting: forwardRef(() => ServicioTareaComponent) }]
})

export class ServicioTareaComponent extends TareasRealizadasBaseComponent implements OnInit{

  label: string;
  ConductorId: string;
  servicios: ItemDto[];
  LineaId: number;

  constructor(private ngEl: ElementRef, private renderer: Renderer,
    private changeDetectorRef: ChangeDetectorRef,
    public tools: ToolsProvider,
    private hserviciosService: HServiciosService
  ) {
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
      if (this.ConductorId != e.value.conductor.Id) {
        this.ConductorId = e.value.conductor.Id;
        this.buscarServicios();
      }
    }
    
    if (e.key == 'cod_lin') {
      if (this.LineaId != e.value) {
        this.LineaId = e.value;
        this.buscarServicios();
      }
    }

    if (e.key == 'fechaTarea') {
      if (this.param.FechaString != e.value) {
        this.param.FechaString = e.value;
        this.buscarServicios();
      }
    }
  }

  private buscarServicios() {
    //Limpiar
    this.value = null;

    if (this.param && this.param.FechaString && this.ConductorId && this.LineaId) {
      var filtro = new HServiciosFilter();
      filtro.Fecha = moment(this.param.FechaString, "DD/MM/YYYY").toDate().toISOString();
      filtro.ConductorId = this.ConductorId;
      filtro.LineaId = this.LineaId;
      this.tools.ShowWait(this.hserviciosService.RecuperarServiciosPorLinea(filtro), (e) => {
        this.servicios = e.DataObject;
        if (this.servicios) {
          if (this.servicios.length > 0) {
            this.value = e.DataObject[0].Id;
            this.valueChange.emit({ key: 'num_ser', value: this.value }); 
          }
        }
      },
      {
        spinner: 'crescent',
        content: 'Cargando servicios...',
      }, e => 
      { 
        this.tools.toastErrorsHttp(e);
      });
    }
  }

  public verOtrosServicios() {
    var filtro = new HServiciosFilter();
    filtro.Fecha = moment(this.param.FechaString, "DD/MM/YYYY").toDate().toISOString();
    filtro.LineaId = this.LineaId;
    this.tools.ShowWait(this.hserviciosService.RecuperarServiciosPorLinea(filtro),(e) => {
      this.servicios = e.DataObject; 
      if(this.servicios.length > 0) {
        // this.value = e.DataObject[0].Id;
        this.valueChange.emit({ key: "num_ser", value: this.value });
      }     
    },
    {
      spinner: 'crescent',
      content: 'Buscando Servicios...',
    }, e => 
    { 
      this.tools.toastErrorsHttp(e);
    });
  }
}
