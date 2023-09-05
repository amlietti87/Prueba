import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ComboBoxComponent } from '../../../../shared/components/comboBase.component';
import { TipoDanioService } from '../tipodedanio/tipodanio.service';
import { TipoDanioDto } from '../model/tipodanio.model';
import { TipoInvolucradoDto } from '../model/tipoinvolucrado.model';
import { TipoInvolucradoService } from '../tipoinvolucrado/tipoinvolucrado.service';
import { EstadosDto } from '../model/estados.model';
import { EstadosService } from '../estados/estados.service';
import { InvolucradosDto } from '../model/involucrados.model';
import { InvolucradosService } from '../involucrados/involucrados.service';

@Component({
    selector: 'involucrado-combo',
    templateUrl: '../../../../shared/components/comboBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => InvolucradosComboComponent),
            multi: true
        }
    ]
})
export class InvolucradosComboComponent extends ComboBoxComponent<InvolucradosDto> implements OnInit {


    constructor(service: InvolucradosService, injector: Injector) {
        super(service, injector);
    }

    ngOnInit(): void {
        super.ngOnInit();
    }

    _SiniestroId: number;
    _InvolucradoId: number;

    @Input()
    get SiniestroId(): number {

        return this._SiniestroId;
    }

    set SiniestroId(value: number) {

        this._SiniestroId = value;
        if (value) {
            if (this.InvolucradoId && this.InvolucradoId != null) {
                var find = this.items.find(e => e.Id == this.InvolucradoId);

                if (!(find && find != null)) {
                    this.onSearch();
                }
            }
            else {
                this.onSearch();
            }
        }
    }

    @Input()
    get InvolucradoId(): number {

        return this._InvolucradoId;
    }

    set InvolucradoId(value: number) {

        this._InvolucradoId = value;

        if (value) {
            var find = this.items.find(e => e.Id == value);

            if (!(find && find != null)) {
                this.onSearch();
            }
        }
    }

    protected GetFilter(): any {

        var f = {
            SiniestroID: this.SiniestroId,
            InvolucradoId: this.InvolucradoId,
            Reclamo: true
        };
        return f;
    }

    onSearch(): void {

        var filter = this.GetFilter();
        if ((filter.SiniestroID && filter.SiniestroID != null)) {
            var self = this;
            this.isLoading = true;
            this.service.requestAllByFilter(filter).subscribe(result => {

                if (self.InvolucradoId && self.InvolucradoId != null) {
                    var find = result.DataObject.Items.find(e => e.Id == self.InvolucradoId);

                    if (find && find != null) {
                        self.items = result.DataObject.Items;
                        self.isLoading = false;
                        setTimeout(() => {
                            $(self.comboboxElement.nativeElement).selectpicker('refresh');
                        }, 200);
                    }
                    else {
                        self.isLoading = false;
                        setTimeout(() => {
                            $(self.comboboxElement.nativeElement).selectpicker('refresh');
                        }, 200);
                    }

                }
                else {
                    self.items = result.DataObject.Items;
                    self.isLoading = false;
                    setTimeout(() => {
                        $(self.comboboxElement.nativeElement).selectpicker('refresh');
                    }, 200);
                }

            });
        }
    }
}
