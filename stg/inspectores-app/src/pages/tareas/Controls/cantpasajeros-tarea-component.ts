import { Component, Input, Output, EventEmitter, ViewChild, forwardRef, OnInit, ElementRef, Renderer, ChangeDetectorRef } from '@angular/core';

import { TareasRealizadasBaseComponent } from './tareas-realizadas-base-component';

@Component({
  selector: 'cantpasajeros-tarea-component',
  templateUrl: 'cantpasajeros-tarea-component.html',
  providers: [{ provide: TareasRealizadasBaseComponent, useExisting: forwardRef(() => CantPasajerosTareaComponent) }]
})

export class CantPasajerosTareaComponent extends TareasRealizadasBaseComponent implements OnInit{


  isNegative: boolean = false;
  label: string;

  constructor(private ngEl: ElementRef, private renderer: Renderer, private changeDetectorRef: ChangeDetectorRef) {
    super(ngEl, renderer);

  }

  ngOnInit() {
    
  }

  setLabel(Etiqueta: string) {
    this.label = Etiqueta;
    this.changeDetectorRef.detectChanges();
  }

  onCantPasajerosChange(event) {
    let valor = event.value;
    if(valor.indexOf("-") === 0) {
      this.isNegative = true;
    }
  }
}
