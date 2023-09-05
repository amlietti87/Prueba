import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { EmpresaDto } from '../model/empresa.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditEmpresaModalComponent } from './create-or-edit-empresa-modal.component';
import { EmpresaService } from './empresa.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';

@Component({

    templateUrl: "./empresa.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class EmpresaComponent extends CrudComponent<EmpresaDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditEmpresaModalComponent
    }

    constructor(injector: Injector, protected _RolesService: EmpresaService) {
        super(_RolesService, CreateOrEditEmpresaModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Empresas"
        this.moduleName = "Administración";
        this.icon = "flaticon-settings";
        this.showbreadcum = false;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
    }






    getDescription(item: EmpresaDto): string {
        return item.Nombre;
    }
    getNewItem(item: EmpresaDto): EmpresaDto {


        var item = new EmpresaDto(item)
        // item.Activo = true;
        return new EmpresaDto(item);

    }






}