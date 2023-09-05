import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { SancionSugeridaDto } from '../model/sancionsugerida.model';
import { SancionSugeridaService } from './sancion.service';
import { CreateOrEditSancionModalComponent } from './create-or-edit-sancion-modal.component';


@Component({

    templateUrl: "./sancion.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class SancionSugeridaComponent extends CrudComponent<SancionSugeridaDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditSancionModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: SancionSugeridaService) {
        super(_RolesService, CreateOrEditSancionModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Sanción Sugerida"
        this.moduleName = "Siniestros";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }


    getDescription(item: SancionSugeridaDto): string {
        return item.Descripcion;
    }
    getNewItem(item: SancionSugeridaDto): SancionSugeridaDto {
        var item = new SancionSugeridaDto(item);
        item.Anulado = false;
        return item;
    }






}