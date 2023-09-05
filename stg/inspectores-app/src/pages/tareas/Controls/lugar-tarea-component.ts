import { Component, Input, Output, EventEmitter, ViewChild, forwardRef, OnInit, ElementRef, Renderer, ChangeDetectorRef } from '@angular/core';

import { TareasRealizadasBaseComponent } from './tareas-realizadas-base-component';

@Component({
  selector: 'lugar-tarea-component',
  templateUrl: 'lugar-tarea-component.html',
  providers: [{ provide: TareasRealizadasBaseComponent, useExisting: forwardRef(() => LugarTareaComponent) }]
})

export class LugarTareaComponent extends TareasRealizadasBaseComponent implements OnInit{

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
