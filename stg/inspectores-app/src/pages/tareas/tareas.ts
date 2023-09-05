import { TareasDbService } from './../../providers/db/tareasDb.service';
import { CocheTareaComponent } from './Controls/coche-tarea-component';
import { NetworkProvider } from './../../providers/network/network';
import { ParadaTareaComponent } from './Controls/parada-tarea-component';
import { CantPasajerosTareaComponent } from './Controls/cantpasajeros-tarea-component';
import { InformeTareaComponent } from './Controls/informe-tarea-component';
import { LugarTareaComponent } from './Controls/lugar-tarea-component';
import { SentidoTareaComponent } from './Controls/sentido-tarea-component';
import { HoraDescensoTareaComponent } from './Controls/horadescenso-tarea-component';
import { HoraAscensoTareaComponent } from './Controls/horaascenso-tarea-component';
import { LineaService } from './../../providers/linea/linea.service';
import { ViewMode } from './../../models/Base/base.model';
import { TareasService, TareasRealizadasService } from './../../providers/tareas/tareas.service';
import { ToolsProvider } from './../../shared/page/tools';
import { TareasDto, TareasFilter, TareasRealizadasDto } from './../../models/tareas.model';
import { Component, ViewRef, ViewChild } from '@angular/core';
import { IonicPage, NavController, NavParams, ModalController, ViewController, AlertController } from 'ionic-angular';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import moment from 'moment';
import { CalendarComponentOptions } from 'ion2-calendar';
import _ from 'lodash';
import { ConductorTareaComponent } from './Controls/conductor-tarea-component';
import { TareasRealizadasBaseComponent, TareasValueChage } from './Controls/tareas-realizadas-base-component';
import { DynamicComponent } from './Controls/dynamic-component';
import { TextoLibreTareaComponent } from './Controls/textolibre-tarea-component';
import { LineaTareaComponent } from './Controls/linea-tarea-component';
import { ServicioTareaComponent } from './Controls/servicio-tarea-component';

@IonicPage()
@Component({
  selector: 'page-tareas',
  templateUrl: 'tareas.html',
    entryComponents: [ConductorTareaComponent,
                     TextoLibreTareaComponent, 
                     LineaTareaComponent, 
                     HoraAscensoTareaComponent, 
                     HoraDescensoTareaComponent, 
                     ServicioTareaComponent,
                     SentidoTareaComponent,
                     LugarTareaComponent,
                     InformeTareaComponent,
                     CantPasajerosTareaComponent,
                     ParadaTareaComponent,
                     CocheTareaComponent]
})
export class TareasPage {

  public elements: Array<{ key: string, view: ViewRef, component: TareasRealizadasBaseComponent }> = [];
  private allComponents = {
    'ConductorId': ConductorTareaComponent,
    'observaciones': TextoLibreTareaComponent,
    'cod_lin': LineaTareaComponent,
    'horaDesde': HoraAscensoTareaComponent,
    'horaHasta': HoraDescensoTareaComponent,
    'num_ser': ServicioTareaComponent,
    'sentido': SentidoTareaComponent,
    'lugar': LugarTareaComponent,
    'generaInforme': InformeTareaComponent,
    'cantPasajeros': CantPasajerosTareaComponent,
    'lugarParadas': ParadaTareaComponent,
    'nro_interno': CocheTareaComponent
  }

  @ViewChild(DynamicComponent) dynamicComponent: DynamicComponent;

  public form: FormGroup;
  public entity: TareasRealizadasDto;
  toDay = new Date();
  tareas: TareasDto[];
  cu: any;
  tareaId: number = null;
  horaHasta: string; 
  horaDesde: string; 
  horaHastaValidar: string
  horaDesdeValidar: string
  fechaModificada: Date;


  constructor(
    public navCtrl: NavController,
    public navParams: NavParams,
    private formBuilder: FormBuilder,
    public modalCtrl: ModalController,
    public tools: ToolsProvider,
    private tareasService: TareasService,
    private tareaRealizadaService: TareasRealizadasService,
    public lineaService: LineaService,
    public viewCtrl: ViewController, 
    public alertCtrl: AlertController,
    public networkProvider: NetworkProvider,
    public tareasDb: TareasDbService
    ) {
      this.cu =  JSON.parse(localStorage.getItem('currentUser'));  
      this.validarInfoUsrLog();     
    }

  validarInfoUsrLog() {

    if(this.cu.empleadoId == null) {
      let alert = this.alertCtrl.create({
        title: "¡Atención!",
        enableBackdropDismiss:false,
        message: "Notifique a Sistemas que no existe el empleado asociado a su usuario.",
        buttons: [{text: 'Aceptar',  handler: () => this.volver()}] 
      });       
      alert.present();
    }  

    if(this.cu.sucursalId == null) {
      this.tools.alert("¡Atención!", "Notifique a Sistemas que no existe unidad de negocio asociada a su usuario.")
    }  

  }

  ngOnInit() {
    if(!this.networkProvider.checkNetwork()) this.tools.toast("No hay conexión a Internet");
    this.recuperarTareas();
    this.entity = new TareasRealizadasDto();
    this.entity.FechaHora = new Date();
    this.entity.FechaString = moment(this.entity.FechaHora).format('DD/MM/YYYY');

    this.form = this.formBuilder.group({
      FechaString: [this.entity.FechaString, Validators.required],
      TareaId: [this.entity.TareaId, Validators.required],
    });

    this.form.valueChanges.subscribe(values => {
        const form = values as TareasDto;
        form.isValid = this.form.valid;
        console.log(form);    
    })
  }

  //Recupera Tareas
  recuperarTareas() {
    var filtro = new TareasFilter();
    filtro.Anulado = false;
    this.tools.ShowWait(this.tareasService.GetPagedList(filtro),(e) => {
      this.tareas = e.DataObject.Items  
    });
  }

  onTareachanged() {
    this.buildControls();
  }

  buildControls() {

    this.tareaId = this.form.get('TareaId').value;
    let tarea = this.tareas.find(e => e.Id == this.tareaId);
    let _allComponents = this.allComponents;

    if (tarea) {
      this.elements = [];

      this.dynamicComponent.resetContainer();

      tarea.TareasCampos.sort((a, b) => { return a.Orden - b.Orden }).forEach(t => {
        
        let type = _allComponents[t.Campo.trim()];

        if (type) {
          let component = this.dynamicComponent.addComponent(type);

          component.setLabel(t.Etiqueta);
          component.setParameters({ tareaCampoDto: t, FechaString: this.entity.FechaString})
          component.valueChange.subscribe(e => this.onComponentChanged(e))
          let key: string = t.Campo.trim();

          let view: ViewRef = this.dynamicComponent.container.detach(0);
          this.elements.push({ key, view, component });
        }

        this.dynamicComponent.resetContainer();
      });
    }
    
  }

  onComponentChanged(e: TareasValueChage) {

    this.elements.filter(t => t.key != e.key).forEach(t => t.component.onOtherComponentChanged(e));
  }

  //Selecciona Fecha 
  public seleccionarFecha() {
    const differentOptions: CalendarComponentOptions = {
      from: new Date(2000, 0, 1),
      to:  this.toDay,
      weekdays: ['D', 'L', 'M', 'Mi', 'J', 'V', 'S'],
      monthPickerFormat: ['ENE', 'FEB', 'MAR', 'ABR', 'MAY', 'JUN', 'JUL', 'AGO', 'SEP', 'OCT', 'NOV', 'DIC'],  
    };
    let modal = this.modalCtrl.create('DatePickerPage', { differentOptions: differentOptions});
    modal.onDidDismiss(fechaSeleccionada => {
      if(fechaSeleccionada) {
        const fechaFormat = moment(fechaSeleccionada).format('DD/MM/YYYY');
        this.entity.FechaString = fechaFormat;
        this.onComponentChanged({ key: "fechaTarea", value: fechaFormat });

      }
    });
    modal.present();
  }

  cancel() {
    this.navCtrl.push("HomePage");
  }

  //Boton Guardar Tarea
  GuardarTareas() {
    this.entity.SucursalId = this.cu.sucursalId;
    this.entity.EmpleadoId = this.cu.empleadoId;
    this.entity.FechaHora = moment(this.entity.FechaString, "DD/MM/YYYY").toDate();
    this.entity.Fecha = this.entity.FechaHora;

    let saveData = _.cloneDeep(this.entity);

    _.assign(saveData, this.form.value);

    saveData.TareasRealizadasDetalle=[];

    //Validar Horas
    if(this.validateEntity(saveData)) {
      this.elements.forEach(e => {

        if (e.key == "horaHasta" && e.component.value) {        
          this.horaHasta = moment(this.entity.FechaHora).format('YYYY-MM-DD') + ' ' + e.component.value;          
        }

        if(e.key == 'horaDesde' && e.component.value) {

        //HoraDesde
        let armandoHoraDesde: string = "0001-01-01" + ' ' + e.component.value;
        let toDateHoraDesde :Date = moment(armandoHoraDesde).toDate();
        let horaDesdeValidar: string = toDateHoraDesde.getHours().toString();
        let minutosDesde: string =  toDateHoraDesde.getMinutes().toString();

        if (toDateHoraDesde.getHours().toString().length < 2) this.horaHastaValidar = '0' + this.horaHastaValidar;
        if (toDateHoraDesde.getMinutes().toString().length < 2) minutosDesde = '0' + minutosDesde;

        this.horaDesde =  moment(this.entity.FechaHora).format('YYYY-MM-DD') + ' ' + horaDesdeValidar + ':' + minutosDesde;
        }
      
      });

      if (this.horaDesde) {

        //HoraHasta
        let armandoHoraHasta: string = this.horaHasta;
        let toDateHoraHasta :Date = moment(armandoHoraHasta).toDate();
        this.horaHastaValidar = toDateHoraHasta.getHours().toString();
        let minutosHasta: string =  toDateHoraHasta.getMinutes().toString();

        if (toDateHoraHasta.getHours().toString().length < 2) this.horaHastaValidar = '0' + this.horaHastaValidar;
        if (toDateHoraHasta.getMinutes().toString().length < 2) minutosHasta = '0' + minutosHasta;

        this.horaHasta =  moment(this.entity.FechaHora).format('YYYY-MM-DD') + ' ' + this.horaHastaValidar + ':' + minutosHasta;

        if(this.horaHasta) {
          let hd: Date = moment(this.horaDesde).toDate()
          if(this.horaHastaValidar < hd.getHours().toString()) {
            this.fechaModificada = new Date(this.horaHasta);
            this.fechaModificada.setDate(this.fechaModificada.getDate() + 1);
            this.horaHasta= moment(this.fechaModificada).format('YYYY-MM-DD') + ' ' + this.horaHastaValidar + ':' + minutosHasta;
          }
        }

      } 
    }   

    if (this.validateEntity(saveData)) {
      this.elements.forEach(e => {

        let item = { TareaRealizadaId:0, TareaCampoId: e.component.param.tareaCampoDto.Id, Valor: e.component.value, PosiblesCampos: null  };

        if (e.key == "horaHasta" && e.component.value) {
          item.Valor = this.horaHasta;
        }
        if (e.key == "horaDesde" && e.component.value) {
          item.Valor = this.horaDesde;
        }

        saveData.TareasRealizadasDetalle.push(item);
      });

      console.log("ENTITY:::::",saveData);
      if(this.networkProvider.checkNetwork()) {

        this.tools.ShowWait(this.tareaRealizadaService.createOrUpdate(saveData, ViewMode.Add), resp => {

          if(resp.Status == 'Ok') {
            let alert = this.alertCtrl.create({
              title: "Guardada!",
              enableBackdropDismiss:false,
              message: "La tarea fue registrada correctamente.",
              buttons: [{text: 'Aceptar',  handler: () => this.volver()}] 
          });       
          alert.present();
    
          } else {
            this.tools.alert("Cuidado!","No se guardo correctamente");
          } 
  
        },
        {
          spinner: 'crescent',
          content: 'Guardando...',
        }, e => 
        { 
          this.tools.toastErrorsHttp(e);
        });

      } else {
        var tarea = saveData;
        this.tareasDb.addTareas(tarea); 
        this.tareasDb.getTareas().then((info) => {
          console.log("TAREA DB:", info);
        });
        
        let alert = this.alertCtrl.create({
          title: "Sin conexión!",
          enableBackdropDismiss:false,
          message: "No te preocupes. La tarea se guardará cuando se recupere la conexión.",
          buttons: [{text: 'Aceptar',  handler: () => this.volver()}] 
        });       
        alert.present();
      }

    }
    else{
      this.tools.toast("Verifique informacion");

    }

  }

  validateEntity(saveData: TareasRealizadasDto):Boolean {
    
    let isValid = true;

    this.elements.forEach(e=> { e.component.Validate(); });

    isValid = this.form.valid && this.elements.filter(e=> !e.component.isValidControl).length == 0;

    return isValid;
  }

  volver() {
    this.navCtrl.push("HomePage");
  }

}
