import { Component, OnInit, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, Renderer, forwardRef, SimpleChanges, SimpleChange } from '@angular/core';
import { NgForm, FormGroup, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CrudService } from '../common/services/crud.service';
import { ADto, ItemDto } from '../model/base.model';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { DetailComponent } from '../manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';

//@Component({

//    template: ComboBoxComponentTemplete._TEMPLATE,
//    providers: [
//        {
//            provide: NG_VALUE_ACCESSOR,
//            useExisting: forwardRef(() => ComboBoxComponent),
//            multi: true
//        }
//    ]
//})
export abstract class AutoCompleteComponent<T extends ADto> implements OnInit, AfterViewInit, ControlValueAccessor {

    @ViewChild('autocomplete') autocompleteElement: ElementRef;

    selectItem: any;

    items: any[] = [];


    @Input() placeHolder = '';
    @Input() field = 'Description';
    @Input() minLength = 1;
    @Input() showAddButton: boolean = false;


    @Output() selectedItemChange: EventEmitter<string> = new EventEmitter<string>();

    private _renderer: Renderer

    IsDisabled = false;
    @Input() livesearch = true;
    onChange = (rating: any) => { };
    onTouched = () => {
    };
    isLoading = false;
    private innerValue: any = '';
    constructor(
        protected service: CrudService<T>,
        private injector: Injector) {
        this._renderer = injector.get(Renderer)
    }

    ngAfterViewInit(): void {

    }

    //get accessor
    get value(): any {
        return this.innerValue;
    };

    //set accessor including call the onchange callback
    set value(v: any) {
        if (v !== this.innerValue) {
            this.innerValue = v;
            this.onChange(v);
        }
    }

    writeValue(value: any): void {

        var self = this;
        if (value != this.innerValue) {
            this.innerValue = value;
            this.onChange(this.value);
        }
    }
    registerOnChange(fn: any): void {
        // throw new Error("Method not implemented.");
        this.onChange = fn;
    }
    registerOnTouched(fn: any): void {
        //throw new Error("Method not implemented.");
        this.onTouched = fn;

    }
    setDisabledState?(isDisabled: boolean): void {

        //throw new Error("Method not implemented.");
        this.IsDisabled = isDisabled;
    }

    ngOnInit(): void {

    }

    //sobrescrivir para filtro custom
    protected GetFilter(query: any): any {
        var f = {
            FilterText: query
        };

        return f;
    }

    filterItems(event) {
        let query = null;
        if (event != null) { query = event.query; }
        this.service.GetItemsAsync(this.GetFilter(query)).subscribe(x => {
            this.items = [];
            for (var i in x.DataObject) {
                var item = x.DataObject[i];
                this.items.push(item);
            }

        });
    }

    Unselect(event) {

    }


    Clear(event) {
        //Hack. limpiar valor cuando se deseleciona el autocomplete
        this.value = null;
    }

    onAddButtonClick() {
        var dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;

        let dialogRef = dialog.open(this.getIDetailComponent(), dialogConfig);
        dialogRef.componentInstance.showNew(this.getNewDto());
        dialogRef.afterClosed().subscribe(
            data => {
                if (data) {
                    this.filterItems(null);
                }

            }
        );
    }

    getIDetailComponent(): ComponentType<DetailComponent<T>> {
        return null;
    }

    getNewDto(): T {
        return null;
    }
}


