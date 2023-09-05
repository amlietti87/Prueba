import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { SucursalDto } from '../model/sucursal.model';
import { BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditBanderaModalComponent } from './create-or-edit-bandera-modal.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { BanderaDto, BanderaFilter } from '../model/bandera.model';
import { BanderaService } from './bandera.service';
import { SucursalService } from '../sucursal/sucursal.service';
import { Subscription } from 'rxjs';
import { RbMapServices } from '../../../../components/rbmaps/RbMapServices';
import { RamalColorService } from '../ramalcolor/ramalcolor.service';
import { ItemDto, GroupItemDto } from '../../../../shared/model/base.model';
import { RamalColorFilter } from '../model/ramalcolor.model';

@Component({

    templateUrl: "./bandera.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class BanderaComponent extends BaseCrudComponent<BanderaDto, BanderaFilter> implements OnInit, AfterViewInit {
    sub: Subscription;
    subQ: Subscription;
    Sucursal: string;
    Sucursalid: number;
    customdetail: CreateOrEditBanderaModalComponent;
    Sucursales: SucursalDto[] = [];
    showRamalFilterAutocomplete: boolean;

    constructor(injector: Injector,
        protected _SucursalService: SucursalService,
        protected _Service: BanderaService,
        protected _RcService: RamalColorService,
        private _activatedRoute: ActivatedRoute) {
        super(_Service, CreateOrEditBanderaModalComponent, injector);
        this.icon = "fa fa-flag-o";
        this.title = "Banderas";
    }

    ngAfterViewInit() {
        this.GetEditComponent();
    }

    getNewfilter(): BanderaFilter {
        var f = new BanderaFilter();
        f.Ramales = [];
        return f;
    }

    OnRamalChange(item: ItemDto) {
        if (item !== null) {
            this.filter.RamaColorId = item.Id;
        }
        else {
            this.filter.RamaColorId = null;
        }
    }

    OnLineaChange(item: ItemDto) {
        if (item !== null) {
            this.findAndAddRamalColor(item);
        }
        else {
            this.filter.Ramales = [];
            this.filter.RamaColorId = null;
            this.showRamalFilterAutocomplete = false;
        }
    }

    private findAndAddRamalColor(linea: ItemDto) {
        var filterRamal = new RamalColorFilter();
        filterRamal.LineaId = linea.Id;
        this._RcService.GetItemsAsync(filterRamal)
            .finally(() => {
            })
            .subscribe((result) => {
                var g = new GroupItemDto();
                g.Id = linea.Id;
                g.Description = linea.Description;
                g.Items = result.DataObject;
                this.filter.Ramales = [];
                let lista = [...this.filter.Ramales];
                lista.push(g);
                this.filter.Ramales = lista;
                this.showRamalFilterAutocomplete = true;
            });
    }


    GetEditComponent(): IDetailComponent {
        var e = super.GetEditComponent();
        (e as CreateOrEditBanderaModalComponent).setSucursal(this.Sucursalid, this.Sucursal);
        return e;
    }

    ngOnInit() {
        super.ngOnInit();
        this._SucursalService.requestAllByFilterCached()
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
            this.Sucursalid = +params['sucursalid'];
            this.filter.SucursalId = this.Sucursalid;
            if (this.Sucursales.length > 0) {
                var e = this.Sucursales.find(e => e.Id == this.Sucursalid);
                this.SetSucursal(e, params);
            }
            else {
                this._SucursalService.getByIdCached(this.Sucursalid)
                    .then(e => {
                        this.SetSucursal(e, params)
                    });
            }
        }
    }

    private SetSucursal(e: SucursalDto, params: any) {
        this.Sucursal = e.Description
        this.GetEditComponent().active = false;
        var selft = this;
        var close = function() {
            selft.CloseChild()
        }
        this.breadcrumbsService.AddItem(this.title + ' ' + this.Sucursal, this.icon, '', null, close);
        if (params.id) {
            if (params.RutaId) {
                this.service.getById(params.id).subscribe(function(result) {
                    selft.active = false;
                    (selft.GetEditComponent() as CreateOrEditBanderaModalComponent).createOrEditRutaModalComponent.active = false;
                    (selft.GetEditComponent() as CreateOrEditBanderaModalComponent).showInRutaMode(result.DataObject);
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
        else {
            this.onSearch();
        }
    }

    ngOnDestroy() {
        super.ngOnDestroy();
        if (this.sub) {
            this.sub.unsubscribe();
            this.subQ.unsubscribe();
        }
    }

    getDescription(item: BanderaDto): string {
        return item.Nombre;
    }
    getNewItem(item: BanderaDto): BanderaDto {
        var itemconunidad = new BanderaDto(item);
        itemconunidad.SucursalId = this.Sucursalid;

        itemconunidad.TipoBanderaId = 2;
        return itemconunidad;
    }

    onEditRutasEnBandera(item: BanderaDto) {
        if (!this.allowModify) {
            return;
        }
        this.active = false;
        (this.GetEditComponent() as CreateOrEditBanderaModalComponent).showInRutaMode(item);
    }

    getReporteCambiosPorSector(item: BanderaDto) {
        this._Service.GetReporteCambiosDeSector(item.Id);
    }
}