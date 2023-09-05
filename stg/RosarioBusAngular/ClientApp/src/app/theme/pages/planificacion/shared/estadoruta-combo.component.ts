import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, ViewEncapsulation } from '@angular/core';

import { TipoLineaService } from '../tipoLinea/tipoLinea.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';

import { EstadoRutaDto } from '../model/estadoruta.model';
import { EstadoRutaService } from '../estadoruta/estadoruta.service';
import { ComboBoxBaseAsyncComponent, ComboBoxAsync } from '../../../../shared/components/comboBaseAsync.component';

@Component({
    selector: 'estadoruta-combo',
    templateUrl: '../../../../shared/components/comboBaseAsync.component.html',
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['../../../../shared/components/comboBaseAsync.component.css'],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => EstadoRutaComboComponent),
            multi: true
        }
    ]
})
export class EstadoRutaComboComponent extends ComboBoxAsync<EstadoRutaDto> implements OnInit {


    constructor(service: EstadoRutaService, injector: Injector) {
        super(service, injector);
    }


}
