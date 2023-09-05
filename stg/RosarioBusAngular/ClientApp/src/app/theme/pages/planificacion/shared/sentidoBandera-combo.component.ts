import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef, SimpleChange, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { OnChanges } from '@angular/core/src/metadata/lifecycle_hooks';

import { SentidoBanderaDto } from '../model/sentidoBandera.model';
import { SentidoBanderaService } from '../sentidoBandera/sentidoBandera.service';
import { ItemDto } from '../../../../shared/model/base.model';

@Component({
    selector: 'sentidoBandera-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => SentidoBanderaComboComponent),
            multi: true
        }
    ]
})

export class SentidoBanderaComboComponent extends ComboBoxComponent<SentidoBanderaDto> implements OnInit {



    constructor(service: SentidoBanderaService, injector: Injector) {
        super(service, injector);
    }

    @Input() IsSabanaMode: boolean = false;


    _linea: ItemDto;
    get linea(): ItemDto {
        return this._linea;
    }

    @Input()
    set linea(linea: ItemDto) {
        this._linea = linea;
        this.buscarSentidosLineaBanderas();
    }


    ngOnInit(): void {
        if (this.IsSabanaMode == false) {
            super.ngOnInit();
        }
    }


    buscarSentidosLineaBanderas() {
        var self = this;

        if (this.linea) {
            this.service.GetItemsAsync({ LineaId: this.linea.Id }).subscribe(e => {

                var _items = [];
                //var itemdto = new ItemDto();
                //itemdto.Id = 0;
                //itemdto.Description = "Seleccione";
                //_items.push(itemdto);
                e.DataObject.forEach(d => {
                    _items.push(d);
                });

                this.items = _items;

                self.isLoading = false;
                setTimeout(() => {
                    $(self.comboboxElement.nativeElement).selectpicker('refresh');
                }, 200);
            });
        }
        else {
            this.items = [];
            self.isLoading = false;
            setTimeout(() => {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }, 200);
        }


    }


}
