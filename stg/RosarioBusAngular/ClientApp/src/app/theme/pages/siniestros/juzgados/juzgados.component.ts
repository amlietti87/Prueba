import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { JuzgadosDto } from '../model/juzgados.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditJuzgadosModalComponent } from './create-or-edit-juzgados-modal.component';
import { JuzgadosService } from './juzgados.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./juzgados.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class JuzgadosComponent extends CrudComponent<JuzgadosDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditJuzgadosModalComponent
    }

    constructor(injector: Injector, protected _RolesService: JuzgadosService) {
        super(_RolesService, CreateOrEditJuzgadosModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Juzgados"
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



    getDescription(item: JuzgadosDto): string {
        return item.Descripcion;
    }
    getNewItem(item: JuzgadosDto): JuzgadosDto {

        var item = new JuzgadosDto(item);
        item.Anulado = false;
        return item;
    }






}