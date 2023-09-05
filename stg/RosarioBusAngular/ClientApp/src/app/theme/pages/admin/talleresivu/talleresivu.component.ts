import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';

import { CrudComponent } from '../../../../shared/manager/crud.component';

import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { PlaTalleresIvuDto } from '../model/talleresivu.model';
import { PlaTalleresIvuService } from './talleresivu.service';
import { CreateOrEditTalleresIvuModalComponent } from './create-or-edit-talleresivu-modal.component';

@Component({

    templateUrl: "./talleresivu.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TalleresIvuComponent extends CrudComponent<PlaTalleresIvuDto> implements AfterViewInit {

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTalleresIvuModalComponent
    }

    constructor(injector: Injector, protected _RolesService: PlaTalleresIvuService) {
        super(_RolesService, CreateOrEditTalleresIvuModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Talleres IVU"
        this.moduleName = "Admin";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    getDescription(item: PlaTalleresIvuDto): string {
        return item.getDescription();
    }
    getNewItem(item: PlaTalleresIvuDto): PlaTalleresIvuDto {

        var item = new PlaTalleresIvuDto(item);
        return item;
    }






}