import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ConsecuenciasDto, CategoriasDto } from '../model/consecuencias.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditConsecuenciasModalComponent } from './create-or-edit-consecuencias-modal.component';
import { ConsecuenciasService } from './consecuencias.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { ViewMode } from '../../../../shared/model/base.model';

@Component({

    templateUrl: "./consecuencias.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class ConsecuenciasComponent extends CrudComponent<ConsecuenciasDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditConsecuenciasModalComponent
    }

    constructor(injector: Injector, protected _RolesService: ConsecuenciasService) {
        super(_RolesService, CreateOrEditConsecuenciasModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Consecuencias"
        this.moduleName = "Siniestros";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
    }


    ngOnInit() {
        super.ngOnInit();
    }

    getDescription(item: ConsecuenciasDto): string {
        return item.Descripcion;
    }
    getNewItem(item: ConsecuenciasDto): ConsecuenciasDto {

        var item = new ConsecuenciasDto(item);
        item.Anulado = false;
        let list: Array<CategoriasDto> = []

        item.Categorias = list;
        return item;
    }

    onEdit(row: ConsecuenciasDto) {
        this.onEditID(row.Id);
    }

    onEditID(id: any) {
        if (!this.allowModify) {
            return;
        }
        this.active = false;

        if (this.loadInMaterialPopup) {
            this.service.getById(id).subscribe(e => this.Opendialog(e.DataObject, ViewMode.Modify));

        }
        else {
            this.GetEditComponent().show(id);
        }

    }

    Opendialog(dto: ConsecuenciasDto, viewMode: ViewMode) {
        if (!dto) {
            dto = new ConsecuenciasDto();
        }
        if (!viewMode) {
            viewMode = ViewMode.Add;
        }
        super.Opendialog(dto, viewMode);
    }

}