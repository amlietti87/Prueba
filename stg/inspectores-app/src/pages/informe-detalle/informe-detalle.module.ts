import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { InformeDetallePage } from './informe-detalle';

@NgModule({
  declarations: [
    InformeDetallePage,
  ],
  imports: [
    IonicPageModule.forChild(InformeDetallePage),
  ],
})
export class InformeDetallePageModule {}
