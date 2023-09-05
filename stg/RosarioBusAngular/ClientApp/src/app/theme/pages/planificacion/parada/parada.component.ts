import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { EmpresaDto } from '../model/empresa.model';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';

import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { ParadaService } from './parada.service';
import { ParadaDto, ParadaFilter } from '../model/parada.model';
import { CreateOrEditParadaModalComponent } from './create-or-edit-parada-modal.component';
import { ItemDto } from '../../../../shared/model/base.model';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';

@Component({

    templateUrl: "./parada.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class ParadaComponent extends BaseCrudComponent<ParadaDto, ParadaFilter> implements AfterViewInit {

    public allowSelect: boolean = false;
    public inMapa: boolean = false;
    public onCreated: EventEmitter<any>;
    public onSelected: EventEmitter<any>;
    filterselectLocalidades: ItemDto;

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditParadaModalComponent;
    }

    //ViewChilds
    @ViewChild('ComboAnulado') ComboAnulado: YesNoAllComboComponent;

    constructor(injector: Injector, protected _RolesService: ParadaService, protected cdr: ChangeDetectorRef) {
        super(_RolesService, CreateOrEditParadaModalComponent, injector);

        this.loadInMaterialPopup = true;
        this.isFirstTime = true;
        this.title = "Parada";
        this.moduleName = "Administración";
        this.icon = "flaticon-settings";
        this.showbreadcum = false;

        this.onCreated = new EventEmitter();
        this.onSelected = new EventEmitter();
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
        this.ComboAnulado.writeValue(this.filter.Anulada);
        this.ComboAnulado.refresh();
        this.cdr.detectChanges();
    }

    getNewfilter(): ParadaFilter {
        var filter = new ParadaFilter();
        filter.AnuladoCombo = 2;
        return filter;
    }

    getDescription(item: ParadaDto): string {
        return item.Codigo;
    }
    getNewItem(item: ParadaDto): ParadaDto {


        var item = new ParadaDto(item);
        // item.Activo = true;
        return item;

    }
    Search(event?: LazyLoadEventData) {
        if (this.filter && this.filterselectLocalidades) {
            this.filter.LocalidadId = this.filterselectLocalidades.Id;
        }
        super.Search(event);
    }


    onCreate() {
        if (this.allowSelect) {
            this.onCreated.emit();
        } else {
            super.onCreate();
        }
    }

    onSelect(item: ParadaDto) {
        this.onSelected.emit(item);
    }

    //public afterOpenDialog(data: any) {
    //    (this.GetEditComponent() as CreateOrEditParadaModalComponent).inicializarMapa();
    //}



}