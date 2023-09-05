import { CocheService } from './../../../providers/coche/coche.service';
import { ToolsProvider } from './../../../shared/page/tools';
import { ItemDto } from './../../../models/Base/base.model';
import { CocheFilter, CocheDto } from './../../../models/coche.model';
import { CochesSearcheablesComponent } from './../../../components/coches-searcheables/coches-searcheables';
import { Component, Input, ViewChild, forwardRef, OnInit, ElementRef, Renderer, ChangeDetectorRef } from '@angular/core';
import { TareasCamposDto } from '../../../models/tareas.model';
import { TareasRealizadasBaseComponent, TareasValueChage } from './tareas-realizadas-base-component';
import moment from 'moment';

@Component({
  selector: 'coche-tarea-component',
  templateUrl: 'coche-tarea-component.html',
  providers: [{ provide: TareasRealizadasBaseComponent, useExisting: forwardRef(() => CocheTareaComponent) }]
})

export class CocheTareaComponent extends TareasRealizadasBaseComponent implements OnInit{

  label: string;
  LineaId: number;
  ServicoId: number;
  coches: CocheDto[];
  currentCoche: CocheDto;
  public ccoche: boolean = true;

  @Input() entity: TareasCamposDto = new TareasCamposDto();
  value: any;

  @ViewChild('cocheSearchables') cocheSearchables: CochesSearcheablesComponent;

  constructor(
    private ngEl: ElementRef, 
    private renderer: Renderer, 
    private changeDetectorRef: ChangeDetectorRef,
    public tools: ToolsProvider,
    private cocheService: CocheService,) {
    super(ngEl, renderer);

  }

  ngOnInit() {
    
  }

  setLabel(Etiqueta: string) {
    this.label = Etiqueta;
    this.changeDetectorRef.detectChanges();
  }

  onOtherComponentChanged(e: TareasValueChage): void {
    if (e.key == "num_ser") {
      if (this.ServicoId != e.value) {
        this.ServicoId = e.value;
        this.recuperarCoches();
      }
    }

    if (e.key == 'cod_lin') {
      if (this.LineaId != e.value) {
        this.LineaId = e.value;
        this.recuperarCoches();
      }
    }

    if (e.key == 'fechaTarea') {
      if (this.param.FechaString != e.value) {
        this.param.FechaString = e.value;
        this.recuperarCoches();
      }
    }
  }

  public recuperarCoches() {
    this.ccoche = true;
    this.value = null;
    if (this.param && this.param.FechaString && this.ServicoId && this.LineaId) {
      var filtro = new CocheFilter();
      filtro.Fecha = moment(this.param.FechaString, "DD/MM/YYYY").toDate().toISOString();
      filtro.Cod_Linea = this.LineaId;
      filtro.cod_servicio = this.ServicoId;
      this.tools.ShowWait(this.cocheService.RecuperarCCochesPorFechaServicioLinea(filtro), (e) => {
        this.coches = e.DataObject;
        if (this.coches) {
          if(this.coches.length > 0) {
            this.value = e.DataObject[0].Id;
            // this.currentCoche.Ficha = this.value
          }
        }
      });
    }
  }

  verOtrosCoches() {
    this.ccoche = false;
  }

  onCocheChange(event) {
    if(event.coche) {
      this.value = event.coche.Id;
    }
  }
}
