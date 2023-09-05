import { Component, ElementRef, Renderer, OnInit, Output, EventEmitter } from '@angular/core';
import { TareasCamposDto } from '../../../models/tareas.model';

@Component({
  selector: 'tareas-realizadas-base',
  template: '<ng-content></ng-content>'
})
export class TareasRealizadasBaseComponent {

  constructor(protected _ngEl: ElementRef, protected _renderer: Renderer) {
  }

  param: TareasParametersModel;
  @Output() valueChange: EventEmitter<TareasValueChage> = new EventEmitter<TareasValueChage>();
  value: any;
  isValidControl: boolean;
  showRequired: boolean = false;

  public get element(): any {
    return this._ngEl.nativeElement;
  }

  removeFromParent() {
    this._ngEl.nativeElement.remove();
  }
  ngOnInit(): void {
    this._renderer.setElementClass(this._ngEl.nativeElement, 'widget', true);
  }


  setLabel(Etiqueta: string) {

  }

  setParameters(_param: TareasParametersModel) {
    this.param = _param;

  }
  onOtherComponentChanged(e: TareasValueChage): void {
    
  }


  Validate(): Boolean {
    this.isValidControl=true;
    this.showRequired = false;

    if (this.param.tareaCampoDto.Requerido == true && !this.value ) {

      this.isValidControl =  false;
      this.showRequired = true;
    }
    else{
      this.isValidControl =  true;
      
    }
    if (this.param.tareaCampoDto.Campo == "cantPasajeros" && this.value) {
      if(this.value.indexOf("-") == 0) {
        this.isValidControl =  false;
      }
    }

    return this.isValidControl;
  }

}

export class TareasParametersModel {
  tareaCampoDto: TareasCamposDto;
    FechaString: string;

}

export class TareasValueChage {

  key: string;
  value: any;

}

