import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { SubGalponDto, ConfiguDto } from '../model/subgalpon.model';
import { SubGalponSinCacheService } from './subgalpon.service';
import { CreateOrEditSubGalponModalComponent } from './create-or-edit-subgalpon-modal.component';

@Component({

    templateUrl: "./subgalpon.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class SubGalponComponent extends CrudComponent<SubGalponDto> implements OnInit, AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditSubGalponModalComponent;
    }

    constructor(injector: Injector, protected _RolesService: SubGalponSinCacheService) {
        super(_RolesService, CreateOrEditSubGalponModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Sub-Galpon"
        this.moduleName = "Administración";
        this.icon = "flaticon-settings";
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





    getDescription(item: SubGalponDto): string {
        return item.DesSubg;
    }
    getNewItem(item: SubGalponDto): SubGalponDto {


        var item = new SubGalponDto(item)
        // item.Activo = true;
        let list: Array<ConfiguDto> = []
        item.BalanceoCheck = false;
        item.ComodinesCheck = false;
        item.Configu = list;
        return item;

    }






}