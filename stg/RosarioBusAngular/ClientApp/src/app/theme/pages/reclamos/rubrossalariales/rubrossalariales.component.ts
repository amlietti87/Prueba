import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { RubrosSalarialesDto } from '../model/rubrossalariales.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditRubrosSalarialesModalComponent } from './create-or-edit-rubrossalariales-modal.component';
import { RubrosSalarialesService } from './rubrossalariales.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./rubrossalariales.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class RubrosSalarialesComponent extends CrudComponent<RubrosSalarialesDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditRubrosSalarialesModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: RubrosSalarialesService) {
        super(_RolesService, CreateOrEditRubrosSalarialesModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Rubros Salariales"
        this.moduleName = "Reclamos";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: RubrosSalarialesDto): string {
        return item.Descripcion;
    }
    getNewItem(item: RubrosSalarialesDto): RubrosSalarialesDto {

        var item = new RubrosSalarialesDto();
        item.Anulado = false;
        return item;
    }






}