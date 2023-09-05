import { RangosHorariosService } from './rangoshorarios.service';
import { RangosHorariosFilter, RangosHorariosDto } from '../model/rangoshorarios.model';
import { ViewMode } from '../../../../shared/model/base.model';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';
import { CreateOrEditRangosHorariosModalComponent } from './create-or-edit-rangoshorarios-modal.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, ChangeDetectorRef } from '@angular/core';
import { BaseCrudComponent } from '../../../../shared/manager/crud.component';

@Component({
    templateUrl: "./rangoshorarios.component.html",
    encapsulation: ViewEncapsulation.None,
})

export class RangosHorariosComponent extends BaseCrudComponent<RangosHorariosDto, RangosHorariosFilter> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditRangosHorariosModalComponent
    }

    @ViewChild('ComboAnulado') ComboAnulado: YesNoAllComboComponent;

    constructor(
        injector: Injector,
        protected _RangosHorariosService: RangosHorariosService,
        protected cdr: ChangeDetectorRef) {
        super(_RangosHorariosService, CreateOrEditRangosHorariosModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Rangos Horarios"
        this.moduleName = "Inspectores";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
        this.advancedFiltersAreShown = true;
    }

    getDescription(item: RangosHorariosDto): string {
        return item.Description
    }

    getNewItem(item: RangosHorariosDto): RangosHorariosDto {
        var item = new RangosHorariosDto(item);
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

    getNewfilter(): RangosHorariosFilter {
        var filter = new RangosHorariosFilter();
        filter.Anulado = 2;
        return filter;
    }

    onEdit(row: RangosHorariosDto) {
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
}