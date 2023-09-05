import { Component, Input, Output, EventEmitter, ViewChild, forwardRef, OnInit, ElementRef, Renderer, ChangeDetectorRef } from '@angular/core';

import { TareasRealizadasBaseComponent } from './tareas-realizadas-base-component';

@Component({
  selector: 'informe-tarea-component',
  templateUrl: 'informe-tarea-component.html',
  providers: [{ provide: TareasRealizadasBaseComponent, useExisting: forwardRef(() => InformeTareaComponent) }]
})

export class InformeTareaComponent extends TareasRealizadasBaseComponent implements OnInit{

  label: string;

  informeOpciones: any[] = [
    {
      value: true,
      label: 'Si',
    }, {
      value: false,
      label: 'No',
    }
  ]

  constructor(private ngEl: ElementRef, private renderer: Renderer, private changeDetectorRef: ChangeDetectorRef) {
    super(ngEl, renderer);

  }

  ngOnInit() {

  }

  setLabel(Etiqueta: string) {
    this.label = Etiqueta;
    this.changeDetectorRef.detectChanges();
  }


}
