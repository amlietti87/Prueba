import { TareasDbService } from './../db/tareasDb.service';
import { TareasRealizadasService } from './../tareas/tareas.service';
import { TranslateService } from '@ngx-translate/core';
import { ViewMode } from '../../models/Base/base.model';
import { ToolsProvider } from '../../shared/page/tools';
import { Injectable } from '@angular/core';
import { AlertController } from 'ionic-angular';
import { TaskBaseService } from '../service/taskBase.service';

@Injectable()
export class TareasTask extends TaskBaseService {

  isRunning: boolean = false;
  permissionErrorAuthString: string;

  constructor(
    private tareaRealizadaService: TareasRealizadasService,
    public tareasDb: TareasDbService,
    public alertCtrl: AlertController,
    public tools: ToolsProvider,
    public translateService: TranslateService) {
    super();
  }

  Run():void {

    if(!this.isRunning) {
      this.tareasDb.getTareas().then((info) => {
        console.log("TAREAS DB:", info);        
       
        if(info!=null){
          var data=info;
          let currentId = data.id;
          data.id = 0;

          //data.TareasRealizadasDetalle = JSON.parse(info.TareasRealizadasDetalle);
          this.isRunning=true;
          // var  inf : InspPlanillaIncognitosDto = new InspPlanillaIncognitosDto(data);

          this.tareaRealizadaService.createOrUpdate(data, ViewMode.Add)
          
          .subscribe( resp => {
            if(resp.Status == 'Ok') {
              this.tareasDb.deleteTareas(currentId);
              var msj = "Tarea guardada con exito!";
              if(resp.Messages.length>0) {
                msj = "<div>"+ msj + "</div>" + "<br/><span style='font-style: italic;'>" + resp.Messages[0] + "</span>";
              }  
              let alert = this.alertCtrl.create({
                title: "Guardado!",
                enableBackdropDismiss:false,
                message: msj,
                buttons: [{text: 'Aceptar' }] 
            });  
                
            alert.present();
            this.isRunning=false;
            this.ContinuarProximoInforme();
            }
          },(e)=> {
            console.log(e);
            this.translateService.get('PERMISSION_ERROR_AUTHORIZATION')
            .subscribe((value) => {
              this.permissionErrorAuthString = value;
            });  
            if(e.status == 401 || e.status == 403) {
              this.tools.toastPosition(this.permissionErrorAuthString ,"top")
            } 
            this.isRunning = false; 
          });
        }
      });
    }
  }

  ContinuarProximoInforme() {
    this.tareasDb.getTareas().then((info) => {
      if(info!=null){
        this.Run();
      }
    });
  }
}