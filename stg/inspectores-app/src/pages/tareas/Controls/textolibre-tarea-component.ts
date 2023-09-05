import { Component, Input, Output, EventEmitter, ViewChild, forwardRef, OnInit, ElementRef, Renderer, ChangeDetectorRef } from '@angular/core';

import { TareasRealizadasBaseComponent } from './tareas-realizadas-base-component';

@Component({
  selector: 'textolibre-tarea-component',
  templateUrl: 'textolibre-tarea-component.html',
  providers: [{ provide: TareasRealizadasBaseComponent, useExisting: forwardRef(() => TextoLibreTareaComponent) }]
})

export class TextoLibreTareaComponent extends TareasRealizadasBaseComponent implements OnInit{



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

  //onConductorChange(event) {
  //  this.valueChange.emit({ key: "observaciones", value: event });
  //}
}
