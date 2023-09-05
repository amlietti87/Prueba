import { Component, forwardRef, OnInit, ElementRef, Renderer, ChangeDetectorRef } from '@angular/core';
import { TareasRealizadasBaseComponent } from './tareas-realizadas-base-component';

@Component({
  selector: 'horaascenso-tarea-component',
  templateUrl: 'horaascenso-tarea-component.html',
  providers: [{ provide: TareasRealizadasBaseComponent, useExisting: forwardRef(() => HoraAscensoTareaComponent) }]
})

export class HoraAscensoTareaComponent extends TareasRealizadasBaseComponent implements OnInit{

  label: string;
  horaHasta: string;

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
