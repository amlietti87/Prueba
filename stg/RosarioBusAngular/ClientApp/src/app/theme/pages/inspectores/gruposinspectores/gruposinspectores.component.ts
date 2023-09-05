import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, ChangeDetectorRef } from '@angular/core';

import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';

import { IDetailComponent } from '../../../../shared/manager/detail.component';

import { CreateOrEditGruposInspectoresModalComponent } from './create-or-edit-gruposinspectores-modal.component';
import { GruposInspectoresService } from './gruposinspectores.service';
import { GruposInspectoresDto, GruposInspectoresFilter } from '../model/gruposinspectores.model';
import { ViewMode } from '../../../../shared/model/base.model';
import { HfechasConfiNewComponent } from '../../planificacion/horariofecha/hfechasconfig/hfechasconfi-new.component';
import { GrupoLineasFilter } from '../../planificacion/model/grupolineas.model';
import { FormGroup, FormControl } from '@angular/forms';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';

@Component({

    templateUrl: "./gruposinspectores.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class GruposInspectoresComponent extends BaseCrudComponent<GruposInspectoresDto, GruposInspectoresFilter> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditGruposInspectoresModalComponent
    }
    @ViewChild('ComboAnulado') ComboAnulado: YesNoAllComboComponent;



    constructor(injector: Injector, protected _GInspectoresService: GruposInspectoresService, protected cdr: ChangeDetectorRef) {
        super(_GInspectoresService, CreateOrEditGruposInspectoresModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Grupos de Inspectores"
        this.moduleName = "Inspectores";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
        this.advancedFiltersAreShown = true;

    }

    getDescription(item: GruposInspectoresDto): string {
        return item.Description
    }
    getNewItem(item: GruposInspectoresDto): GruposInspectoresDto {

        var item = new GruposInspectoresDto(item);
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

    getNewfilter(): GruposInspectoresFilter {
        var filter = new GruposInspectoresFilter();
        filter.Anulado = 2;
        return filter;
    }

    onEdit(row: GruposInspectoresDto) {
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