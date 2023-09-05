import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { NuevoDesvioPage } from './nuevo-desvio';

@NgModule({
  declarations: [
    NuevoDesvioPage,
  ],
  imports: [
    IonicPageModule.forChild(NuevoDesvioPage),
  ],
})
export class NuevoDesvioPageModule {}
