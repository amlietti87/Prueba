import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { FactoresIntervinientesDto } from '../model/factoresintervinientes.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { TipoElementoDto } from '../model/tipoelemento.model';
import { ElementosDto } from '../model/elemento.model';
import { ElementosService } from './elemento.service';
import { CreateOrEditElementosModalComponent } from './create-or-edit-elemento-modal.component';

@Component({

    templateUrl: "./elemento.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class ElementosComponent extends CrudComponent<ElementosDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditElementosModalComponent;
    }

    constructor(injector: Injector, protected _service: ElementosService) {
        super(_service, CreateOrEditElementosModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Elementos"
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



    getDescription(item: ElementosDto): string {
        return item.Nombre;
    }
    getNewItem(item: ElementosDto): ElementosDto {

        var item = new ElementosDto(item);
        item.TipoId = 1;
        return item;
    }






}