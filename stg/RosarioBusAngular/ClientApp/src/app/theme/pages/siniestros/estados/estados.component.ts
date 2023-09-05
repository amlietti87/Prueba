import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { EstadosDto, SubEstadosDto } from '../model/estados.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditEstadosModalComponent } from './create-or-edit-estados-modal.component';
import { EstadosService } from './estados.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./estados.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class EstadosComponent extends CrudComponent<EstadosDto> implements AfterViewInit {



    constructor(injector: Injector, protected _RolesService: EstadosService) {
        super(_RolesService, CreateOrEditEstadosModalComponent, injector);
        this.isFirstTime = true;
        this.loadInMaterialPopup = true;
        this.title = "Estados"
        this.moduleName = "Siniestros";
        this.icon = "flaticon-settings";
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
    }


    ngOnInit() {
        super.ngOnInit();
    }


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditEstadosModalComponent
    }

    getDescription(item: EstadosDto): string {
        return item.Descripcion;
    }
    getNewItem(item: EstadosDto): EstadosDto {

        var item = new EstadosDto(item);
        item.Judicial = false;
        item.Anulado = false;
        let list: Array<SubEstadosDto> = []
        item.SubEstados = list;
        return item;
    }






}