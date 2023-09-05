import { ParadaService } from './../../providers/parada/parada.service';
import { ParadaDto, PlaParadaFilter } from './../../models/parada.model';
import { ToolsProvider } from '../../shared/page/tools';
import { NetworkProvider } from '../../providers/network/network';
import { ItemGenericDto } from '../../models/Base/base.model';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, FormBuilder } from '@angular/forms';
import { IonicSelectableComponent } from 'ionic-selectable';
import { Subscription } from 'rxjs';
import { Events } from 'ionic-angular';

@Component({
  selector: 'paradas-searcheables',
  templateUrl: 'paradas-searcheables.html'
})
export class ParadasSearcheablesComponent {

  //paradas
  _parada_searcheable: ItemGenericDto<number>;
  form: FormGroup;
  paradaControl: FormControl;
  paradas_searcheables: ParadaDto[];
  paradas_searcheablesSubscription: Subscription;

  @Input() currentItem:ParadaDto;
  @Input() fecha: string;
  @Output() paradaSeleccionada = new EventEmitter;

  @Input() label: string = "Parada";
  constructor(   
    public paradaService: ParadaService,
    private formBuilder: FormBuilder,
    public networkProvider: NetworkProvider,
    private events: Events,
    public tools: ToolsProvider) { 
      // this.suscripcion_fecha_modificada();
      // this.suscripcion_clean_conductor();
    }

  ngOnInit(){
    this.paradaControl = this.formBuilder.control(this.paradas_searcheables);
    this.form = this.formBuilder.group({
      parada_searcheable: this.paradaControl
    });

    this.form.get('parada_searcheable').valueChanges.subscribe((v) => {
      this._parada_searcheable = v as ItemGenericDto<number>;
      if(this._parada_searcheable != null){
        console.log("_parada_searcheable", this._parada_searcheable);       
        this.paradaSeleccionada.emit({Codigo: this._parada_searcheable});       
      }
    })
  }

  busquedaPorParadas(event: {
    component: IonicSelectableComponent,
    text: string,
    value: string,
    }) {

    let text = event.text.trim().toLowerCase();
    event.component.startSearch();
    
    // Close any running subscription.
    if (this.paradas_searcheablesSubscription) {
      this.paradas_searcheablesSubscription.unsubscribe();
    }

    if (!text) {
      // Close any running subscription.
      if (this.paradas_searcheablesSubscription) {
        this.paradas_searcheablesSubscription.unsubscribe();
      }

      event.component.items = [];
      event.component.endSearch();
      return;
    }

    if (!text) {
      event.component.items = [];
      event.component.endSearch();
      return;}
    // } else if (event.text.length < 3) {
    //   return;
    // }

    var filtro = new PlaParadaFilter();
    filtro.FilterText = text;

    if(this.networkProvider.checkNetwork()) {
      this.paradaService.GetPagedList(filtro).subscribe(paradas_searcheables => {        
      event.component.items = paradas_searcheables.DataObject.Items;
      event.component.endSearch();
    })
    } else {
      this.tools.toast("No hay conexión a Internet");
    }
  }

}
