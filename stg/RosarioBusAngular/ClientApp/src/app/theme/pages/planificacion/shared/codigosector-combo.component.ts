import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, SimpleChanges, SimpleChange, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent, ComboBoxBaseComponent } from '../../../../shared/components/comboBase.component';
import { GrupoLineasDto } from '../model/grupolineas.model';
import { GrupoLineasService } from '../grupolineas/grupolineas.service';
import { RutaDto } from '../model/ruta.model';
import { RutaService } from '../ruta/ruta.service';
import { ItemDto } from '../../../../shared/model/base.model';


@Component({
    selector: 'codigosector-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => CodigoSectorComboComponent),
            multi: true
        }
    ]
})
export class CodigoSectorComboComponent extends ComboBoxBaseComponent implements OnChanges, OnInit {


    items: ItemDto[] = [];
    isLoading = false;
    @Input() name: string;
    private _name: string;

    constructor(protected rutaService: RutaService, injector: Injector) {
        super(injector);
    }


    private _CodLin: number;
    get CodLin(): number {
        return this._CodLin;
    }

    @Input()
    set CodLin(CodLin: number) {
        this._CodLin = CodLin;
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    ngOnChanges(changes: SimpleChanges) {

        const CodLin: SimpleChange = changes.CodLin;

        if (CodLin && CodLin.previousValue != CodLin.currentValue) {
            this.onSearch();
        }
    }


    onSearch(): void {
        var self = this;
        this.isLoading = true;
        if (this.CodLin && this.CodLin != 0) {
            this.rutaService.RecuperarHbasecPorLinea(this.CodLin).subscribe(result => {
                this.items = result.DataObject;
                self.isLoading = false;
                setTimeout(() => {
                    $(self.comboboxElement.nativeElement).selectpicker('refresh');
                }, 0);
            });
        }
        else {
            this.items = [];
            self.isLoading = false;
            setTimeout(() => {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 0);
        }

    }

}
