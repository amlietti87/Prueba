import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, forwardRef } from '@angular/core';


import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { AutoCompleteComponent } from '../../../../shared/components/autocompleteBase.component';
import { BanderaDto } from '../model/bandera.model';
import { RamalColorService } from '../ramalcolor/ramalcolor.service';
import { RamalColorDto } from '../model/ramalcolor.model';
import { GroupItemDto } from '../../../../shared/model/base.model';

@Component({
    selector: 'ramalcolor-autocomplete',
    templateUrl: '../../../../shared/components/autocompleteBase.component.html',
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => RamalColorAutoCompleteComponent),
            multi: true
        }
    ]
})
export class RamalColorAutoCompleteComponent extends AutoCompleteComponent<RamalColorDto> implements OnInit {




    constructor(service: RamalColorService, injector: Injector) {
        super(service, injector);
        this.Items = [];
    }

    ngOnInit(): void {
        super.ngOnInit();
    }



    private _ItemsModel: GroupItemDto[];
    get ItemsModel(): GroupItemDto[] {
        return this._ItemsModel;
    }

    @Input()
    set ItemsModel(itemsModel: GroupItemDto[]) {
        this._ItemsModel = itemsModel;
        this.Items = [];
        this._ItemsModel.forEach(f => {
            f.Items.forEach(i => {
                this.Items.push(i);
            });
        });
    }

    private Items: any[];




    filterItems(event): void {



        let query = event.query;


        this.items = this.Items.filter(i =>
            i.Description.toLowerCase().includes(event.query.toLowerCase()));

    }


}
