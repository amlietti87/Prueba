import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, EventEmitter, ChangeDetectorRef, Inject, Input } from '@angular/core';
import { EmpresaDto } from '../model/empresa.model';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';

import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { CreateOrEditCoordenadaModalComponent } from './create-or-edit-coordenadas-modal.component';
import { CoordenadasService } from './coordenadas.service';
import { CoordenadasDto, CoordenadasFilter } from '../model/coordenadas.model';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';
import { MAT_DIALOG_DATA } from '@angular/material';

@Component({

    templateUrl: "./coordenadas.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class CoordenadasComponent extends BaseCrudComponent<CoordenadasDto, CoordenadasFilter> implements AfterViewInit {

    public allowSelect: boolean = false;
    public showAnulado: boolean = false;
    public onCreated: EventEmitter<any>;
    public onSelected: EventEmitter<any>;


    ViewChilds
    @ViewChild('ComboAnulado') ComboAnulado: YesNoAllComboComponent;

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditCoordenadaModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: CoordenadasService,
        protected cdr: ChangeDetectorRef) {
        super(_RolesService, CreateOrEditCoordenadaModalComponent, injector);

        this.loadInMaterialPopup = true;
        this.isFirstTime = true;
        this.title = "Coordenada";
        this.moduleName = "Administración";
        this.icon = "flaticon-settings";
        this.showbreadcum = false;

        this.onCreated = new EventEmitter();
        this.onSelected = new EventEmitter();
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
        this.ComboAnulado.writeValue(this.filter.AnuladoCombo);
        this.ComboAnulado.refresh();
        this.cdr.detectChanges();
    }

    getNewfilter(): CoordenadasFilter {
        var filter = new CoordenadasFilter();
        filter.AnuladoCombo = 2;
        return filter;
    }

    getDescription(item: CoordenadasDto): string {
        return item.Descripcion;
    }
    getNewItem(item: CoordenadasDto): CoordenadasDto {


        var item = new CoordenadasDto(item);
        // item.Activo = true;
        return item;

    }

    onCreate() {
        if (this.allowSelect) {
            this.onCreated.emit();
        } else {
            super.onCreate();
        }
    }

    onSelect(item: CoordenadasDto) {
        this.onSelected.emit(item);
    }



}