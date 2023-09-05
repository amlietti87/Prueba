import { Component, OnInit, OnDestroy, AfterViewInit, AfterViewChecked, ElementRef, ViewChild, Injector, Input, Output, EventEmitter, Renderer, forwardRef, SimpleChanges, SimpleChange } from '@angular/core';
import { NgForm, FormGroup, ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CrudService } from '../common/services/crud.service';
import { ADto } from '../model/base.model';
import { MatDialogRef, MatDialogConfig, MatDialog } from '@angular/material';
import { IDetailComponent, DetailComponent } from '../manager/detail.component';
import { ComponentType } from '@angular/cdk/overlay/index';

export abstract class ComboBoxBaseComponent implements OnInit, AfterViewInit, ControlValueAccessor, OnDestroy {

    ngOnDestroy(): void {

        if (!this.disabledSelectPicker) {
            (<any>$(this.comboboxElement.nativeElement)).selectpicker('destroy');
        }
    }

    @Input() disabledSelectPicker: boolean = false;

    @ViewChild('combobox') comboboxElement: ElementRef;


    @Input() showAddButton: boolean = false;
    @Input() selectedItem: string = undefined;
    @Output() selectedItemChange: EventEmitter<string> = new EventEmitter<string>();

    @Input() emptyText = 'Seleccione...';
    @Input() DisplayName = '';
    @Input() allowNullable: boolean = true;

    private _renderer: Renderer

    @Input()
    IsDisabled = false;
    @Input() livesearch = true;
    onChange = (rating: any) => { };
    onTouched = () => {
    };
    isLoading = false;
    private innerValue: any = '';
    constructor(
        protected injector: Injector) {
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
            setTimeout(() => {
                if (!this.disabledSelectPicker) {
                    $(self.comboboxElement.nativeElement).selectpicker('refresh');
                }
            }, 0);
        }
        // this.selectedItem = obj;
        //this.onChange(this.value);
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
        this.onSearch();
    }

    onSearch(): void {

    }
}

export abstract class ComboBoxComponent<T extends ADto> extends ComboBoxBaseComponent implements OnInit, AfterViewInit, ControlValueAccessor {

    @Output() itemstChange: EventEmitter<T[]> = new EventEmitter<T[]>();

    ngOnInit(): void {
        super.ngOnInit();
    }

    protected _items: T[] = [];

    @Input()
    get items(): T[] {
        return this._items;
    }
    set items(val: T[]) {
        this._items = val;
        if (this.itemstChange)
            this.itemstChange.emit(val);
    }

    isLoading = false;

    constructor(protected service: CrudService<T>,
        protected injector: Injector) {
        super(injector);
    }

    onSearch(): void {
        //
        var self = this;
        this.isLoading = true;
        this.service.requestAllByFilter(this.GetFilter()).subscribe(result => {
            this.items = result.DataObject.Items;
            this.onSearchFinish();
            self.isLoading = false;
            setTimeout(() => {
                if (!this.disabledSelectPicker) {
                    $(self.comboboxElement.nativeElement).selectpicker('refresh');
                }
            }, 200);
        });
    }
    onSearchFinish(): void {

    }


    refresh(): void {
        var self = this;
        if (!this.disabledSelectPicker) {
            $(self.comboboxElement.nativeElement).selectpicker('refresh');
        }
    }

    refreshWithTimeout(): void {
        var self = this;
        setTimeout(() => {
            if (!this.disabledSelectPicker) {
                $(self.comboboxElement.nativeElement).selectpicker('refresh');
            }
        }, 200);
    }

    getIDetailComponent(): ComponentType<DetailComponent<T>> {
        return null;
    }

    getNewDto(): T {
        return null;
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
                    this.onSearch();
                    this.selectedItem = data.Id;
                }

            }
        );
    }

    protected GetFilter(): any {
        var f = {};

        return f;
    }


}

export abstract class ComboBoxYesNoAllComponent<T extends ADto> extends ComboBoxBaseComponent implements OnInit, AfterViewInit, ControlValueAccessor {

    @Output() itemstChange: EventEmitter<T[]> = new EventEmitter<T[]>();

    ngOnInit(): void {
        super.ngOnInit();
    }

    protected _items: T[] = [];

    @Input()
    get items(): T[] {
        return this._items;
    }
    set items(val: T[]) {
        this._items = val;
        if (this.itemstChange)
            this.itemstChange.emit(val);
    }

    refresh(): void {
        var self = this;
        if (!this.disabledSelectPicker) {
            $(self.comboboxElement.nativeElement).selectpicker('refresh');
        }
    }


    isLoading = false;

    constructor(
        protected injector: Injector) {
        super(injector);
    }

    onSearch(): void {
        var self = this;
        this.isLoading = true;
    }


    protected GetFilter(): any {
        var f = {};

        return f;
    }


}





