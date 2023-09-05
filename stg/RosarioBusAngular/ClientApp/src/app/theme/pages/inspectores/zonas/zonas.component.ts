import { ViewMode } from './../../../../shared/model/base.model';
import { YesNoAllComboComponent } from './../../../../shared/components/yesnoall-combo.component';
import { ZonasService } from './zonas.service';
import { CreateOrEditZonasModalComponent } from './create-or-edit-zonas-modal.component';
import { IDetailComponent } from './../../../../shared/manager/detail.component';
import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, ChangeDetectorRef } from '@angular/core';
import { BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { ZonasDto, ZonasFilter } from '../model/zonas.model';

@Component({
    templateUrl: "./zonas.component.html",
    encapsulation: ViewEncapsulation.None,
})

export class ZonasComponent extends BaseCrudComponent<ZonasDto, ZonasFilter> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditZonasModalComponent
    }

    @ViewChild('ComboAnulado') ComboAnulado: YesNoAllComboComponent;

    constructor(
        injector: Injector,
        protected _ZonasService: ZonasService,
        protected cdr: ChangeDetectorRef) {
        super(_ZonasService, CreateOrEditZonasModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Zona"
        this.moduleName = "Inspectores";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
        this.advancedFiltersAreShown = true;
    }

    getDescription(item: ZonasDto): string {
        return item.Description
    }

    getNewItem(item: ZonasDto): ZonasDto {
        var item = new ZonasDto(item);
        return item;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
        this.ComboAnulado.writeValue(this.filter.Anulado);
        this.ComboAnulado.refresh();
        this.cdr.detectChanges();
    }

    ngOnInit() {
        super.ngOnInit();
    }

    BorrarFiltros() {
        this.filter = this.getNewfilter();
    }

    getNewfilter(): ZonasFilter {
        var filter = new ZonasFilter();
        filter.Anulado = 2;
        return filter;
    }

    onEdit(row: ZonasDto) {
        this.onEditID(row.Id);
    }

    onEditID(id: any) {

        this.active = false;

        if (this.loadInMaterialPopup) {
            this.service.getById(id).subscribe(e => this.Opendialog(e.DataObject, ViewMode.Modify));
        }
        else {
            this.GetEditComponent().show(id);
        }
    }

    //Opendialog(dto: ZonasDto, viewMode: ViewMode) {
    //    if (!dto) {
    //        dto = new ZonasDto();
    //    }
    //    if (!viewMode) {
    //        viewMode = ViewMode.Add;
    //    }
    //    super.Opendialog(dto, viewMode);
    //}
}