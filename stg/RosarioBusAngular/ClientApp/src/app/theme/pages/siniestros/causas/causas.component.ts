import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CausasDto, SubCausasDto } from '../model/causas.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditCausasModalComponent } from './create-or-edit-causas-modal.component';
import { CausasService } from './causas.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./causas.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class CausasComponent extends CrudComponent<CausasDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditCausasModalComponent
    }

    constructor(injector: Injector, protected _RolesService: CausasService) {
        super(_RolesService, CreateOrEditCausasModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Causas"
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



    getDescription(item: CausasDto): string {
        return item.Descripcion;
    }
    getNewItem(item: CausasDto): CausasDto {

        var item = new CausasDto(item);
        item.Anulado = false;
        let list: Array<SubCausasDto> = []
        item.SubCausas = list;
        return item;
    }






}