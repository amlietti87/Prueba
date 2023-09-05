import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CiaSegurosDto } from '../model/seguros.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditSegurosModalComponent } from './create-or-edit-seguros-modal.component';
import { CiaSegurosService } from './seguros.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./seguros.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class CiaSegurosComponent extends CrudComponent<CiaSegurosDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditSegurosModalComponent
    }

    constructor(injector: Injector, protected _RolesService: CiaSegurosService) {
        super(_RolesService, CreateOrEditSegurosModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Cía de seguro"
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



    getDescription(item: CiaSegurosDto): string {
        return item.Descripcion;
    }
    getNewItem(item: CiaSegurosDto): CiaSegurosDto {

        var item = new CiaSegurosDto(item)
        item.Anulado = false;
        return new CiaSegurosDto(item);
    }


}