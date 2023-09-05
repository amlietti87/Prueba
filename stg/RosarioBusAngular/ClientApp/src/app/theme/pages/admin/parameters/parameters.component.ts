import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CrudComponent } from "../../../../shared/manager/crud.component";
import { ParametersDto } from "../model/parameters.model";
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { ParametersService } from './parameters.service';
import { CreateOrEditParametersModalComponent } from './create-or-edit-parameters-modal.component';
@Component({

    templateUrl: "./parameters.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class ParametersComponent extends CrudComponent<ParametersDto> implements OnInit, AfterViewInit {
    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditParametersModalComponent
    }
    constructor(injector: Injector, protected _RolesService: ParametersService) {
        super(_RolesService, CreateOrEditParametersModalComponent, injector);

        this.icon = "fa fa-map-signs";
        this.title = "Parámetros";
        this.moduleName = "Administración";
        this.showbreadcum = false;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
        var selft = this;
        var close = function() {
            selft.CloseChild()
        }
    }

    ngOnInit() {
        super.ngOnInit();
    }

    getDescription(item: ParametersDto): string {
        return item.Description;
    }
    getNewItem(item: ParametersDto): ParametersDto {
        var item = new ParametersDto(item)
        return new ParametersDto(item);
    }
}