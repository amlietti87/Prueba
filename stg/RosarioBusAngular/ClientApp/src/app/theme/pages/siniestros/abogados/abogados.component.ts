import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { AbogadosDto } from '../model/abogados.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditAbogadosModalComponent } from './create-or-edit-abogados-modal.component';
import { AbogadosService } from './abogados.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./abogados.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class AbogadosComponent extends CrudComponent<AbogadosDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditAbogadosModalComponent
    }

    constructor(injector: Injector, protected _RolesService: AbogadosService) {
        super(_RolesService, CreateOrEditAbogadosModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Abogados"
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



    getDescription(item: AbogadosDto): string {
        return item.ApellidoNombre;
    }
    getNewItem(item: AbogadosDto): AbogadosDto {

        var item = new AbogadosDto(item)
        item.Anulado = false;
        return item;
    }






}