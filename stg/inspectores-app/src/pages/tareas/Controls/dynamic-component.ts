import {
  Component, ComponentFactoryResolver, ViewChild,
  ViewContainerRef, ComponentFactory, Type
} from '@angular/core';
import { TareasRealizadasBaseComponent } from './tareas-realizadas-base-component';


@Component({

  selector: 'dynamic-component',
  template: `<div #container><ng-content></ng-content></div>`

})
export class DynamicComponent {
  private _elements: TareasRealizadasBaseComponent[] = [];
  @ViewChild('container', { read: ViewContainerRef }) container: ViewContainerRef;

  constructor(private compFactoryResolver: ComponentFactoryResolver) {

  }

  public addComponent(ngItem: Type<TareasRealizadasBaseComponent>): TareasRealizadasBaseComponent {
    let factory = this.compFactoryResolver.resolveComponentFactory(ngItem);
    const ref = this.container.createComponent(factory);
    const newItem: TareasRealizadasBaseComponent = ref.instance;
    this._elements.push(newItem);
    return newItem;
  }


  resetContainer() {
    this._elements = [];
  }
}
