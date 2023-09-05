import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { TipoParadaDto, TiempoEsperadoDeCargaDto } from '../model/tipoParada.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditTipoParadaModalComponent } from './create-or-edit-tipoparada-modal.component';
import { TipoParadaService } from './tipoparada.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { timeout } from 'rxjs/operator/timeout';

@Component({

    templateUrl: "./tipoparada.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class TipoParadaComponent extends CrudComponent<TipoParadaDto> implements OnInit, AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditTipoParadaModalComponent
    }

    constructor(injector: Injector, protected _RolesService: TipoParadaService) {
        super(_RolesService, CreateOrEditTipoParadaModalComponent, injector);

        this.icon = "fa fa-map-signs";
        this.title = " Tipo de Parada";
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



    getDescription(item: TipoParadaDto): string {
        return item.Nombre;
    }
    getNewItem(item: TipoParadaDto): TipoParadaDto {


        var item = new TipoParadaDto(item)

        let list: Array<TiempoEsperadoDeCargaDto> = []

        item.TiempoEsperadoDeCarga = list;
        // item.Activo = true;
        return new TipoParadaDto(item);

    }






}