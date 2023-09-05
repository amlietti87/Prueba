import { ItemDto } from './../../../models/Base/base.model';
import { SentidoBanderaService } from './../../../providers/bandera/sentidoBandera.service';
import { Component, forwardRef, OnInit, ElementRef, Renderer, ChangeDetectorRef } from '@angular/core';
import { TareasRealizadasBaseComponent, TareasValueChage } from './tareas-realizadas-base-component';
import { ToolsProvider } from '../../../shared/page/tools';

@Component({
  selector: 'sentido-tarea-component',
  templateUrl: 'sentido-tarea-component.html',
  providers: [{ provide: TareasRealizadasBaseComponent, useExisting: forwardRef(() => SentidoTareaComponent) }]
})

export class SentidoTareaComponent extends TareasRealizadasBaseComponent implements OnInit{

  label: string;
  LineaId: number;
  sentidos: ItemDto[];

  constructor(private ngEl: ElementRef, private renderer: Renderer,
    private changeDetectorRef: ChangeDetectorRef,
    public tools: ToolsProvider,
    private sentidoBanderaService: SentidoBanderaService
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
        this.buscarSentiodos();
      }
    }
  }

  buscarSentiodos() {
    if (this.LineaId) {
      this.tools.ShowWait(this.sentidoBanderaService.GetItemsAsync({LineaId: this.LineaId }), e =>  {      
        this.sentidos = e.DataObject;
      }, 
      {
        spinner: 'crescent',
        content: 'Buscando sentidos',
      }); 
      
    }

  }


  // onServicioChange(event) {
  //   this.valueChange.emit({ key: "num_ser", value: event });
  // }

}
