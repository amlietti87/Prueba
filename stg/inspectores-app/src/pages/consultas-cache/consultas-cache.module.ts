import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ConsultasCachePage } from './consultas-cache';

@NgModule({
  declarations: [
    ConsultasCachePage,
  ],
  imports: [
    IonicPageModule.forChild(ConsultasCachePage),
  ],
})
export class ConsultasCachePageModule {}
