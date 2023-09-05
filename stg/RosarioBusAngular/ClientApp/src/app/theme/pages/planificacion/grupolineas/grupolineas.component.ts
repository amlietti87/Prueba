import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ActivatedRoute, Route } from '@angular/router';

import { SucursalDto } from '../model/sucursal.model';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditGrupoLineasModalComponent } from './create-or-edit-grupolineas-modal.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { GrupoLineasDto, GrupoLineasFilter } from '../model/grupolineas.model';
import { GrupoLineasService } from './grupolineas.service';
import { SucursalService } from '../sucursal/sucursal.service';
import { Subscription } from 'rxjs';

@Component({

    templateUrl: "./grupolineas.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class GrupoLineasComponent extends BaseCrudComponent<GrupoLineasDto, GrupoLineasFilter> implements OnInit, AfterViewInit {
    sub: Subscription;
    Sucursal: string;
    Sucursalid: number;
    customdetail: CreateOrEditGrupoLineasModalComponent;
    Sucursales: SucursalDto[] = [];

    constructor(injector: Injector,
        protected _SucursalService: SucursalService,
        protected _Service: GrupoLineasService,
        private _activatedRoute: ActivatedRoute) {
        super(_Service, CreateOrEditGrupoLineasModalComponent, injector);
        this.icon = "flaticon-layers"
        this.title = "Grupo de Líneas";

    }


    getNewfilter(): GrupoLineasFilter {
        return new GrupoLineasFilter();
    }


    ngAfterViewInit() {
        this.GetEditComponent();
    }


    GetEditComponent(): IDetailComponent {

        var e = super.GetEditComponent();
        (e as CreateOrEditGrupoLineasModalComponent).setSucursal(this.Sucursalid, this.Sucursal);
        return e;
    }

    ngOnInit() {
        super.ngOnInit();

        this._SucursalService.requestAllByFilterCached()
            .then(e => {
                //this.Sucursal = e;
            });

        this.sub = this._activatedRoute.params.subscribe(params => {
            this.paramsSubscribe(params);
        }
        );
    }

    paramsSubscribe(params: any) {

        this.breadcrumbsService.defaultBreadcrumbs(this.title);

        if (params.sucursalid) {
            this.active = true;

            this.Sucursalid = +params['sucursalid'];

            this.filter.SucursalId = this.Sucursalid;


            if (this.Sucursales.length > 0) {
                var e = this.Sucursales.find(e => e.Id == this.Sucursalid);
                this.SetSucursal(e, params.id);
            }
            else {
                this._SucursalService.getByIdCached(this.Sucursalid)
                    .then(e => {
                        this.SetSucursal(e, params.id);
                    });
            }

        }
    }

    private SetSucursal(e: SucursalDto, id: any) {
        this.Sucursal = e.Description
        this.GetEditComponent().active = false;


        var selft = this;
        var close = function() {
            selft.CloseChild()
        }
        this.breadcrumbsService.AddItem(this.title + ' ' + this.Sucursal, this.icon, '', null, close);
        if (id) {
            this.onEditID(id);
        }
        else {
            this.list = [];
            this.primengDatatableHelper.records = [];
        }
    }



    ngOnDestroy() {
        super.ngOnDestroy();
        if (this.sub) {
            this.sub.unsubscribe();

        }
    }

    getDescription(item: GrupoLineasDto): string {
        return item.Nombre;
    }
    getNewItem(item: GrupoLineasDto): GrupoLineasDto {
        var itemconunidad = new GrupoLineasDto(item)

        itemconunidad.SucursalId = this.Sucursalid;
        return itemconunidad;
    }






}