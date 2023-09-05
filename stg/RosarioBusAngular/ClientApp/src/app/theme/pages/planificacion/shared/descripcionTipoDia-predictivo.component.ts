import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { TipoDiaDto } from '../model/tipoDia.model';
import { TipoDiaService } from '../tipoDia/tipodia.service';
import { PlaDistribucionDeCochesPorTipoDeDiaDto } from '../model/HFechasConfi.model';

@Component({
    selector: 'destdia-predictivo',
    templateUrl: '../../../../shared/components/predictiveBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => DescripcionTipoDiaPredictivoComponent),
            multi: true
        }
    ]
})
export class DescripcionTipoDiaPredictivoComponent extends AutoCompleteComponent<TipoDiaDto> implements OnInit {


    items: string[] = [];

    primeravez: boolean = true;

    @Input() detail: PlaDistribucionDeCochesPorTipoDeDiaDto;
    @Input() detailTipoDeDia: TipoDiaDto;


    tipodiaant: number;

    constructor(protected serviceBase: TipoDiaService, injector: Injector) {
        super(serviceBase, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }


    protected GetFilter(query: any): any {
        var f = {
            FilterText: query
        };

        return f;
    }

    Unselect(event) {
    }

    Refrescar() {
        this.filterItems(event);
    }

    Clear(event) {
    }

    filterItems(event): void {

        var query = null;

        if (event && event.query) {
            query = event.query;
        }
        if (query == null) {
            query = '';
        }
        var filter;

        if (this.detail) {
            filter = {
                FilterText: query,
                TipoDiaId: this.detail.CodTdia
            };
        }
        else if (this.detailTipoDeDia) {
            filter = {
                FilterText: query,
                TipoDiaId: this.detailTipoDeDia.Id
            };
        }
        else {
            filter = this.GetFilter(query);
        }

        this.serviceBase.DescripcionPredictivo(filter).subscribe(x => {

            var itemsClone = [];
            if (x.DataObject && x.DataObject.length > 0) {
                if (this.primeravez) {

                    if (this.detail && this.detail.IsNew) {
                        var find = x.DataObject.find(e => e.Key == true);
                        if (find != null) {
                            this.writeValue(find.Value);
                        }
                    }
                    else if (this.detailTipoDeDia && !(this.detailTipoDeDia.Id && this.detailTipoDeDia.Id != 0)) {
                        var find = x.DataObject.find(e => e.Key == true);
                        if (find != null) {
                            this.writeValue(find.Value);
                        }
                    }
                }
                x.DataObject.forEach(e => itemsClone.push(e.Value));
            }
            this.items = itemsClone;
            this.primeravez = false;
        });
    }


}
