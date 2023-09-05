import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, SimpleChange, SimpleChanges } from '@angular/core';


import { TipoDiaDto } from '../model/tipoDia.model';
import { TipoDiaService } from '../tipoDia/tipodia.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';

@Component({
    selector: 'tipoDia-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => TipoDiaComboComponent),
            multi: true
        }
    ]
})
export class TipoDiaComboComponent extends ComboBoxComponent<TipoDiaDto> implements OnInit {


    @Input() ItemsModel: TipoDiaDto[];
    @Input() autoLoad: boolean = false;

    constructor(service: TipoDiaService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        if (this.autoLoad) {
            super.ngOnInit();
        }
    }

    _CodHfecha: number;

    @Input()
    get CodHfecha(): number {

        return this._CodHfecha;
    }

    set CodHfecha(value: number) {
        this._CodHfecha = value;
        if (value) {
            this.onSearch();
        }
    }



    protected GetFilter(): any {

        var f = {
            CodHfecha: this.CodHfecha
        };

        return f;
    }


    onSearch(): void {
        //

        var self = this;
        this.isLoading = true;
        this.service.requestAllByFilter(this.GetFilter()).subscribe(result => {
            this.items = result.DataObject.Items;
            self.isLoading = false;
            setTimeout(() => {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 200);
        });
    }



    //ngOnChanges(changes: SimpleChanges) {

    //    const itemmodel: SimpleChange = changes.ItemsModel;
    //    var self = this;

    //    if (itemmodel && itemmodel.previousValue != itemmodel.currentValue) {

    //        this.items = this.ItemsModel;
    //        setTimeout(() => {
    //            $(self.comboboxElement.nativeElement).selectpicker('refresh');
    //        }, 0);
    //    }
    //}



}
