import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ActivatedRoute, Route } from '@angular/router';

import { SucursalDto } from '../model/sucursal.model';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditLineaModalComponent } from './create-or-edit-linea-modal.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { LineaDto, LineaFilter } from '../model/linea.model';
import { LineaService } from './linea.service';
import { SucursalService } from '../sucursal/sucursal.service';
import { Subscription } from 'rxjs';
import { PlaLineaService } from './pla-linea.service';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';

@Component({

    templateUrl: "./linea.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class LineaComponent extends BaseCrudComponent<LineaDto, LineaFilter> implements OnInit, AfterViewInit {
    sub: Subscription;
    subQ: Subscription;
    sucursal: string;
    sucursalid: number;
    customdetail: CreateOrEditLineaModalComponent;
    Sucursales: SucursalDto[] = [];


    constructor(injector: Injector,
        protected _sucursalService: SucursalService,
        protected _Service: PlaLineaService,
        private _activatedRoute: ActivatedRoute) {
        super(_Service, CreateOrEditLineaModalComponent, injector);
        this.advancedFiltersAreShown = true;
        this.icon = "fa fa-bus"
        this.title = "Lineas";
    }



    getNewfilter(): LineaFilter {
        var f = new LineaFilter();
        //f.Activo = true;
        f.Sort = "";
        return f;
    }

    ngAfterViewInit() {
        this.GetEditComponent();

    }


    GetEditComponent(): IDetailComponent {

        var e = super.GetEditComponent();
        (e as CreateOrEditLineaModalComponent).setSucursal(this.sucursalid, this.sucursal);
        return e;
    }

    onDelete(item: LineaDto) {

        if (!this.allowDelete) {
            return;
        }


        var strindto = this.getDescription(item);
        //var aa = this.getNewItem(item);
        //var stringentity = aa.getDescription();
        var mje = '¿Está seguro de que desea eliminar el registro?';

        this._Service.TieneMapasEnBorrador(item.Id).subscribe(e => {
            if (e.DataObject) {
                mje = 'Existen mapas en Borrador ¿Está seguro de que desea eliminar el registro ? ';
            }


            this.message.confirm(mje, strindto || 'Confirmación', (a) => {

                //this.isshowalgo = !this.isshowalgo;
                if (a.value) {
                    this.service.delete(item.Id)
                        .subscribe(() => {
                            this.Search();
                            this.notify.success(this.l('Registro eliminado correctamente'));
                        });
                }

            });
        });







    }


    ngOnInit() {

        super.ngOnInit();
        this._sucursalService.requestAllByFilterCached()
            .then(e => {
                this.Sucursales = e;
            });

        this.sub = this._activatedRoute.params.subscribe(params => {
            this.paramsSubscribe(params);
        }
        );

        this.subQ = this._activatedRoute.queryParams.subscribe(params => {
            this.paramsSubscribe(params);
        }
        );
    }

    paramsSubscribe(params: any) {
        this.breadcrumbsService.defaultBreadcrumbs(this.title);

        if (params.sucursalid) {
            this.active = true;

            this.sucursalid = +params['sucursalid'];

            this.filter.SucursalId = this.sucursalid;


            if (this.Sucursales.length > 0) {
                var e = this.Sucursales.find(e => e.Id == this.sucursalid);
                this.SetSucursal(e, params);

            }
            else {
                this._sucursalService.getByIdCached(this.sucursalid)
                    .then(e => {
                        this.SetSucursal(e, params);
                    });
            }

        }
    }


    private SetSucursal(e: SucursalDto, params) {
        this.sucursal = e.DscSucursal;
        this.GetEditComponent().active = false;


        var selft = this;
        var close = function() {
            selft.CloseChild()
        }

        this.breadcrumbsService.AddItem(this.title + ' ' + this.sucursal, this.icon, '', null, close);

        if (params.id) {
            if (this.allowModify) {

                if (params.RamalId) {

                    this.service.getById(params.id).subscribe(function(result) {
                        (selft.GetEditComponent() as CreateOrEditLineaModalComponent).showInRamalMode(result.DataObject);
                    });


                    if (!this.allowModify) {
                        return;
                    }
                    this.active = false;

                }
                else {
                    this.onEditID(params.id);
                }

            }
        }
        else {
            this.onSearch();
        }
    }

    CloseChild(): void {
        var e = super.GetEditComponent();
        (e as CreateOrEditLineaModalComponent).CloseChild();
        (e as CreateOrEditLineaModalComponent).close();
    }


    ngOnDestroy() {
        super.ngOnDestroy();
        if (this.sub) {
            this.sub.unsubscribe();
            this.subQ.unsubscribe();
        }
    }

    getDescription(item: LineaDto): string {
        return item.DesLin;
    }
    getNewItem(item: LineaDto): LineaDto {

        var itemconunidad = new LineaDto(item)
        itemconunidad.SucursalId = this.sucursalid;
        return itemconunidad;
    }


    onEditRamalesEnLinea(item: LineaDto) {
        if (!this.allowModify) {
            return;
        }
        this.active = false;
        (this.GetEditComponent() as CreateOrEditLineaModalComponent).showInRamalMode(item);
    }

}