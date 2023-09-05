import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';

import { TipoLineaService } from '../tipoLinea/tipoLinea.service';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { HFechasConfiDto } from '../model/HFechasConfi.model';
import { HFechasConfiService } from '../horariofecha/HFechasConfi.service';
import { moment } from 'ngx-bootstrap/chronos/test/chain';


@Component({
    selector: 'hfechasconfi-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => HFechasConfiComboComponent),
            multi: true
        }
    ]
})
export class HFechasConfiComboComponent extends ComboBoxComponent<HFechasConfiDto> implements OnInit {
    _LineaId: number;

    @Input() autoLoad: boolean = true;
    @Input() autoSelecionaVigente: boolean = false;


    @Input()
    get LineaId(): number {
        return this._LineaId;
    }
    set LineaId(val: number) {
        this._LineaId = val;
        if (val) {
            this.onSearch()
        }
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
                if (self.autoSelecionaVigente) {
                    self.writeValue(null);
                    if (self.items.length > 0) {

                        var hoy = new Date();
                        var vigente = self.items.filter(e => { return self.BuscarVigente(e); });

                        if (vigente && vigente.length > 0) {
                            self.writeValue(vigente[vigente.length - 1].Id);
                            self.onChange(vigente[vigente.length - 1].Id);
                        }
                        else {
                            self.writeValue(this.items[0].Id);
                            self.onChange(this.items[0].Id);
                        }

                    }

                }


            }, 200);
        });
    }


    private BuscarVigente(e: HFechasConfiDto): boolean {



        var m = moment().utcOffset(0);
        m.set({ hour: 0, minute: 0, second: 0, millisecond: 0 })
        var hoy = m.toDate();


        var desde = moment(e.FechaDesde).toDate();
        var hasta = moment(e.FechaHasta).toDate();
        var result = desde <= hoy && (!e.FechaHasta || moment(hasta).toDate() >= hoy);
        return result;
    }



    constructor(service: HFechasConfiService, injector: Injector) {
        super(service, injector);

        //this.autoLoad = false;
    }

    ngOnInit(): void {
        if (this.autoLoad) {

            super.ngOnInit();
        }

    }

    protected GetFilter(): any {
        var f =
            {
                LineaId: this.LineaId
            };

        return f;
    }

}
