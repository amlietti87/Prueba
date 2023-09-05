import { CocheTareaComponent } from './Controls/coche-tarea-component';
import { ParadaTareaComponent } from './Controls/parada-tarea-component';
import { CantPasajerosTareaComponent } from './Controls/cantpasajeros-tarea-component';
import { InformeTareaComponent } from './Controls/informe-tarea-component';
import { LugarTareaComponent } from './Controls/lugar-tarea-component';
import { SentidoTareaComponent } from './Controls/sentido-tarea-component';
import { HoraDescensoTareaComponent } from './Controls/horadescenso-tarea-component';
import { HoraAscensoTareaComponent } from './Controls/horaascenso-tarea-component';
import { ComponentsModule } from './../../components/components.module';
import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { TareasPage } from './tareas';
import { TareasRealizadasBaseComponent } from './Controls/tareas-realizadas-base-component';
import { ConductorTareaComponent } from './Controls/conductor-tarea-component';
import { DynamicComponent } from './Controls/dynamic-component';
import { ViewDirective } from './Controls/view.directive';
import { TextoLibreTareaComponent } from './Controls/textolibre-tarea-component';
import { LineaTareaComponent } from './Controls/linea-tarea-component';
import { ServicioTareaComponent } from './Controls/servicio-tarea-component';

@NgModule({
  declarations: [
    TareasPage,
    TareasRealizadasBaseComponent,
    TextoLibreTareaComponent,
    ConductorTareaComponent,
    LineaTareaComponent,
    ServicioTareaComponent,
    HoraAscensoTareaComponent,
    HoraDescensoTareaComponent,
    DynamicComponent,
    ViewDirective,
    SentidoTareaComponent,
    LugarTareaComponent,
    InformeTareaComponent,
    CantPasajerosTareaComponent,
    ParadaTareaComponent,
    CocheTareaComponent

  ],
  imports: [
    IonicPageModule.forChild(TareasPage),
    ComponentsModule
  ],
})
export class TareasPageModule {}
