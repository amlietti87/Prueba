import { ConductoresLegajoDto } from './../../../models/hServicios.model';
import { Component, Input, Output, EventEmitter, ViewChild, forwardRef, OnInit, ElementRef, Renderer, ChangeDetectorRef } from '@angular/core';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';

import { IonicSelectableComponent } from 'ionic-selectable';
import { Subscription } from 'rxjs';
import { Events, PopoverController } from 'ionic-angular';
import { TareasCamposDto } from '../../../models/tareas.model';
import { ConductoresSearcheablesComponent } from '../../../components/conductores-searcheables/conductores-searcheables';
import { TareasRealizadasBaseComponent } from './tareas-realizadas-base-component';

@Component({
  selector: 'conductor-tarea-component',
  templateUrl: 'conductor-tarea-component.html',
  providers: [{ provide: TareasRealizadasBaseComponent, useExisting: forwardRef(() => ConductorTareaComponent) }]
})

export class ConductorTareaComponent extends TareasRealizadasBaseComponent implements OnInit{

  @Input() entity: TareasCamposDto = new TareasCamposDto();
  value: any;
  conductor : ConductoresLegajoDto;

  @ViewChild('conductorSearchables') conductorSearchables: ConductoresSearcheablesComponent

  constructor(
    private ngEl: ElementRef, 
    private renderer: Renderer, 
    private changeDetectorRef: ChangeDetectorRef,
    private popoverCtrl: PopoverController) {
    super(ngEl, renderer);

  }

  ngOnInit() {
    
  }

  setLabel(Etiqueta: string) {
    this.conductorSearchables.label = Etiqueta;
    this.changeDetectorRef.detectChanges();
  }

  onConductorChange(event) {
    this.valueChange.emit({ key: "ConductorId", value: event });
    
    this.value = event.conductor.Id;
    this.conductor = event.conductor
  }

  public verConductor(): void {
    let popover = this.popoverCtrl.create('DatosConductorPage', { infoConductor: this.conductor}, {cssClass: 'popoverOpacity'});
    popover.onDidDismiss(resp => {
    });
    popover.present();
  }

  
}
