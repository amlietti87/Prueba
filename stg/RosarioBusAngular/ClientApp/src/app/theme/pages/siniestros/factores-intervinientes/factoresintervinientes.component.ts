import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { FactoresIntervinientesDto } from '../model/factoresintervinientes.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditFactoresIntervinientesModalComponent } from './create-or-edit-factoresintervinientes-modal.component';
import { FactoresIntervinientesService } from './factoresintervinientes.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./factoresintervinientes.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class FactoresIntervinientesComponent extends CrudComponent<FactoresIntervinientesDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditFactoresIntervinientesModalComponent
    }

    constructor(injector: Injector, protected _RolesService: FactoresIntervinientesService) {
        super(_RolesService, CreateOrEditFactoresIntervinientesModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Factores intervinientes"
        this.moduleName = "Siniestros";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: FactoresIntervinientesDto): string {
        return item.Descripcion;
    }
    getNewItem(item: FactoresIntervinientesDto): FactoresIntervinientesDto {

        var item = new FactoresIntervinientesDto(item);
        item.Anulado = false;
        return item;
    }






}