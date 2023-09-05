import { Component, forwardRef, OnInit, ElementRef, Renderer, ChangeDetectorRef } from '@angular/core';
import { TareasRealizadasBaseComponent, TareasValueChage } from './tareas-realizadas-base-component';
import moment from 'moment';

@Component({
  selector: 'horadescenso-tarea-component',
  templateUrl: 'horadescenso-tarea-component.html',
  providers: [{ provide: TareasRealizadasBaseComponent, useExisting: forwardRef(() => HoraDescensoTareaComponent) }]
})

export class HoraDescensoTareaComponent extends TareasRealizadasBaseComponent implements OnInit{

  label: string;

  constructor(private ngEl: ElementRef, private renderer: Renderer, private changeDetectorRef: ChangeDetectorRef) {
    super(ngEl, renderer);

  }

  ngOnInit() {
    
  }

  
  onOtherComponentChanged(e: TareasValueChage): void {

    if (e.key == 'fechaTarea') {
      if (this.param.FechaString  != e.value) {
        this.param.FechaString = e.value;
        this.completarHoraDescenso();
      }
    }    
  }

  completarHoraDescenso() {
    if (this.value) {
      let fecha = moment(this.param.FechaString, "DD/MM/YYYY").toDate();
      this.value = moment(fecha).format('YYYY-MM-DD') + ' ' + this.value;
    }
  }

  setLabel(Etiqueta: string) {
    this.label = Etiqueta;
    this.changeDetectorRef.detectChanges();
  }
}
