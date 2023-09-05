import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { RamalColorDto } from '../model/ramalcolor.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';

import { RamalColorService } from './ramalcolor.service';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { CreateOrEditRamaColorModalComponent } from './create-or-edit-ramalcolor-modal.component';
import { Subscription } from 'rxjs';

@Component({

    templateUrl: "./ramalcolor.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class RamalColorComponent extends CrudComponent<RamalColorDto> implements OnInit, AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditRamaColorModalComponent
    }

    constructor(injector: Injector,
        protected _RamalColorService: RamalColorService,
        private _activatedRoute: ActivatedRoute
    ) {
        super(_RamalColorService, CreateOrEditRamaColorModalComponent, injector);
        this.isFirstTime = true;


    }
    sub: Subscription;

    ngAfterViewInit() {
    }






    getDescription(item: RamalColorDto): string {
        return item.Nombre;
    }
    getNewItem(item: RamalColorDto): RamalColorDto {


        var item = new RamalColorDto(item)
        // item.Activo = true;
        return new RamalColorDto(item);

    }


    ngOnInit() {
        super.ngOnInit();
        this.sub = this._activatedRoute.params.subscribe(params => {
            if (params.id) {
                var id = +params['id'];
                this.active = false;
                this.GetEditComponent().show(id);
            }
        }
        );
    }
    ngOnDestroy() {
        super.ngOnDestroy();
        if (this.sub) {
            this.sub.unsubscribe();
        }
    }



}