import { ParadaService } from './../../../providers/parada/parada.service';
import { ToolsProvider } from './../../../shared/page/tools';
import { PlaParadaFilter, ParadaDto } from './../../../models/parada.model';
import { ParadasSearcheablesComponent } from './../../../components/paradas-searcheables/paradas-searcheables';
import { Component, Input, Output, EventEmitter, ViewChild, forwardRef, OnInit, ElementRef, Renderer, ChangeDetectorRef } from '@angular/core';
import { TareasCamposDto } from '../../../models/tareas.model';
import { TareasRealizadasBaseComponent, TareasValueChage } from './tareas-realizadas-base-component';
import moment from 'moment';

@Component({
  selector: 'parada-tarea-component',
  templateUrl: 'parada-tarea-component.html',
  providers: [{ provide: TareasRealizadasBaseComponent, useExisting: forwardRef(() => ParadaTareaComponent) }]
})

export class ParadaTareaComponent extends TareasRealizadasBaseComponent implements OnInit{

  @Input() entity: TareasCamposDto = new TareasCamposDto();
  value: any;
  label: string;
  LineaId: number;
  paradas: ParadaDto[];
  public parada: boolean = true;

  @ViewChild('paradaSearchables') paradaSearchables: ParadasSearcheablesComponent

  constructor(
    private ngEl: ElementRef, 
    private renderer: Renderer, 
    private changeDetectorRef: ChangeDetectorRef,
    public tools: ToolsProvider,
    public paradaService: ParadaService
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

    if (e.key == 'cod_lin') {
      if (this.LineaId != e.value) {
        this.LineaId = e.value;
        this.recuperarParadas();
      }
    }

    if (e.key == 'fechaTarea') {
      if (this.param.FechaString != e.value) {
        this.param.FechaString = e.value;
        this.recuperarParadas();
      }
    }
  }

  recuperarParadas() {

    this.parada = true;
    if (this.param && this.param.FechaString  && this.LineaId) {
      var filtro = new PlaParadaFilter();
      filtro.Fecha = moment(this.param.FechaString, "DD/MM/YYYY").toDate().toISOString();
      filtro.LineaId = this.LineaId;
      filtro.SoloParadasAsociadasALineas = true;
      this.tools.ShowWait(this.paradaService.GetPagedList(filtro), (e) => {
        this.paradas = e.DataObject.Items;
        if (this.paradas) {
          if(this.paradas.length > 0) { 
            this.value = e.DataObject.Items[0].Id;
          }
        }
      });
    }
  }

  verOtrasParadas() {
    this.parada = false;
  }

  onParadaChange(event) {
    this.value = event.Codigo.Id;
  }
}
