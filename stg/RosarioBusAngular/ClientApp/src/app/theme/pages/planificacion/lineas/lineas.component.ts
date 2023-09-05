import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Route } from '@angular/router';

import { SucursalDto } from '../model/sucursal.model';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { LineaDto, LineaFilter, LineasFilter, BanderaItemLongDto, RutasViewFilter } from '../model/linea.model';
import { SucursalService } from '../sucursal/sucursal.service';
import { Subscription } from 'rxjs';
import { CreateOrEditLineaModalComponent } from '../linea/create-or-edit-linea-modal.component';
import { LineaService } from '../linea/linea.service';
import { ItemDto, GroupItemDto } from '../../../../shared/model/base.model';
import { SelectItemGroup } from 'primeng/primeng';
import { RamalColorService } from '../ramalcolor/ramalcolor.service';
import { RamalColorFilter } from '../model/ramalcolor.model';
import { BanderaService } from '../bandera/bandera.service';
import { RamalColorComboComponent } from '../shared/ramalColor-combo.component';
import { BanderaFilter } from '../model/bandera.model';
import { ViewRutasComponent } from './view-rutas.component';
import { PlaLineaService } from '../linea/pla-linea.service';

declare let mPortlet: any;

@Component({

    templateUrl: "./lineas.component.html",
    encapsulation: ViewEncapsulation.None,
    styleUrls: ['./lineas.component.css']
})
export class LineasComponent extends BaseCrudComponent<LineaDto, LineasFilter> implements OnInit, AfterViewInit {
    sub: Subscription;
    subQ: Subscription;
    Sucursal: string;
    Sucursalid: number;

    Sucursales: SucursalDto[] = [];
    RamalesID: number[];
    Original: boolean = true;
    Vigente: boolean = false;
    RamalesEsRequerido: boolean = false;




    @ViewChild('ramalcolorfilter') ramalcolorfilter: RamalColorComboComponent;

    constructor(injector: Injector,
        protected _sucursalService: SucursalService,
        protected _Service: PlaLineaService,
        protected _RcService: RamalColorService,
        protected _BanderaService: BanderaService,
        private _activatedRoute: ActivatedRoute,
        protected cdRef: ChangeDetectorRef
    ) {
        super(_Service, ViewRutasComponent, injector);

        this.icon = "fa fa-bus"
        this.title = "Lineas";
    }



    getNewfilter(): LineasFilter {
        var f = new LineasFilter();
        f.Lineas = [];
        f.Ramales = [];
        f.RamalesSelect = [];
        f.BanderasComerciales = [];
        f.BanderasPosicionamiento = [];

        return f;
    }

    portlet1: any;
    portlet2: any;

    ngAfterViewInit() {
        this.GetEditComponent();

        this.portlet1 = new mPortlet('m_portlet_tools_1');
        this.portlet2 = new mPortlet('m_portlet_tools_2');
    }


    GetEditComponent(): IDetailComponent {

        var e = super.GetEditComponent();
        return e;
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

            this.Sucursalid = +params['sucursalid'];

            this.filter.SucursalId = this.Sucursalid;


            if (this.Sucursales.length > 0) {
                var e = this.Sucursales.find(e => e.Id == this.Sucursalid);
                this.SetUnidadNegocio(e, params.id);

            }
            else {
                this._sucursalService.getByIdCached(this.Sucursalid)
                    .then(e => {
                        this.SetUnidadNegocio(e, params.id);
                    });
            }

        }
    }


    private SetUnidadNegocio(e: SucursalDto, id: any) {

        this.Sucursal = e.Description
        this.GetEditComponent().active = false;

        this.filter.Lineas = [];
        this.filter.Ramales = [];
        this.filter.RamalesSelect = [];
        this.filter.BanderasComerciales = [];
        this.filter.BanderasPosicionamiento = [];
        this.filter.selectBanderaComercial = null;
        this.filter.selectBanderaPosicionamiento = null;
        this.filter.selectLinea = null;
        this.filter.selectRamal = null;

        var selft = this;
        var close = function() {
            selft.CloseChild()
        }


        this.breadcrumbsService.AddItem(this.title + ' ' + this.Sucursal, this.icon, '', null, close);

        if (id) {
            if (this.allowModify) {
                this.onEditID(id);
            }
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
            this.subQ.unsubscribe();
        }
    }

    getDescription(item: LineaDto): string {
        return item.DesLin;
    }
    getNewItem(item: LineaDto): LineaDto {

        var itemconunidad = new LineaDto(item)
        itemconunidad.SucursalId = this.Sucursalid;
        return itemconunidad;
    }



    addLinea(item: ItemDto, AddDesdeBandera: boolean = false) {
        if (item.Id) {
            if (!this.filter.Lineas.some(x => x.Id == item.Id)) {

                item.IsSelected = true;
                this.filter.Lineas.push(item);

                this.findAndAddRamalColor(item, AddDesdeBandera);
            }
            else {
                var c = this.filter.Lineas.find(x => x.Id == item.Id);
                this.notificationService.warn("La linea ya fue agregada", c.Description);
                c.animate = true;

                setTimeout(() => {
                    c.animate = false;
                }, 1000);
            }

        }

    }



    private findAndAddRamalColor(linea: ItemDto, AddDesdeBandera: boolean = false) {


        var filterRamal = new RamalColorFilter();


        filterRamal.LineaId = linea.Id;

        this._RcService.GetAllAsyncSinSentidos(filterRamal)
            .finally(() => {
            })
            .subscribe((result) => {

                var g = new GroupItemDto();
                g.Id = linea.Id;
                g.Description = linea.Description;
                g.Items = result.DataObject;

                let lista = [...this.filter.Ramales];

                lista.push(g);
                this.filter.Ramales = null;
                this.filter.Ramales = lista;

                if (AddDesdeBandera) {
                    this.AddSelectedLinea(linea);
                }
            });
    }


    addRamal(item: ItemDto) {

        if (item) {

            var ramalId = item.Id;

            var gruporamal = this.filter.Ramales.find(x => x.Items.some(e => e.Id == ramalId));


            if (gruporamal) {

                var existegrupo = this.filter.RamalesSelect.find(x => x.Items.some(e => e.Id == ramalId));
                if (existegrupo) {

                    var existeramal = existegrupo.Items.find(e => e.Id == ramalId);
                    if (!existeramal) {

                        var ramalparaagregar = gruporamal.Items.find(e => e.Id == ramalId);
                        existegrupo.Items.push(Object.assign({}, ramalparaagregar));
                    }
                    else {
                        this.notificationService.warn("El rammal ya fue agregado", existeramal.Description);
                        existeramal.animate = true;

                        setTimeout(() => {
                            existeramal.animate = false;
                        }, 1000);
                    }
                }
                else {
                    var ramalparaagregar = gruporamal.Items.find(e => e.Id == ramalId);

                    var g = new GroupItemDto();
                    g.Id = gruporamal.Id;
                    g.Description = gruporamal.Description;
                    g.Items = [];
                    g.Items.push(Object.assign({}, ramalparaagregar));

                    this.filter.RamalesSelect.push(g)

                }

                this.SetRamalesFilter();
            }

        }

    }


    addBanderaComercial(item: BanderaItemLongDto) {

        if (item.Id) {
            if (!this.filter.BanderasComerciales.some(x => x.Id == item.Id)) {

                item.IsSelected = true;
                let lista = [...this.filter.BanderasComerciales];

                if (!item.RamaColor_Nombre) {

                    //busco la descripcion del ramal;
                    var lg = this.filter.Ramales.find(e => e.Items.some(s => s.Id == item.RamaColorId));
                    if (lg) {
                        var ramal = lg.Items.find(x => x.Id == item.RamaColorId);
                        if (ramal) {
                            item.RamaColor_Nombre = ramal.Description;
                        }
                    }
                    else {
                        this._RcService.getById(item.RamaColorId).subscribe(e => {
                            if (e.DataObject) {
                                var itemdto = new ItemDto();
                                itemdto.Description = e.DataObject.NombreLinea;
                                itemdto.Id = e.DataObject.LineaId;
                                this.addLinea(itemdto, true);
                            }
                        });
                    }

                }


                lista.push(item);
                this.filter.BanderasComerciales = lista;

            }
            else {
                var c = this.filter.BanderasComerciales.find(x => x.Id == item.Id);
                this.notificationService.warn("La bandera ya fue agregada", c.Description);
                c.animate = true;

                setTimeout(() => {
                    c.animate = false;
                }, 1000);
            }

        }

    }

    addBanderaPosicionamiento(item: BanderaItemLongDto) {
        if (item.Id) {
            if (!this.filter.BanderasPosicionamiento.some(x => x.Id == item.Id)) {

                item.IsSelected = true;
                let lista = [...this.filter.BanderasPosicionamiento];

                if (!item.RamaColor_Nombre) {

                    //busco la descripcion del ramal;
                    var lg = this.filter.Ramales.find(e => e.Items.some(s => s.Id == item.RamaColorId));
                    if (lg) {
                        var ramal = lg.Items.find(x => x.Id == item.RamaColorId);
                        if (ramal) {
                            item.RamaColor_Nombre = ramal.Description;
                        }
                    }

                }


                lista.push(item);
                this.filter.BanderasPosicionamiento = lista;

            }
            else {
                var c = this.filter.BanderasPosicionamiento.find(x => x.Id == item.Id);
                this.notificationService.warn("La bandera ya fue agregada", c.Description);
                c.animate = true;

                setTimeout(() => {
                    c.animate = false;
                }, 1000);
            }

        }

    }




    AddSelectedLinea(linea: ItemDto): void {

        var f = this.filter.Ramales.find(e => e.Id == linea.Id);

        if (f != null) {
            var existegrupo = this.filter.RamalesSelect.find(e => e.Id == linea.Id);
            if (existegrupo) {
                existegrupo.Items = [...f.Items];
            }
            else {

                var g = new GroupItemDto();
                g.Id = f.Id;
                g.Description = f.Description;
                g.Items = [...f.Items];
                this.filter.RamalesSelect.push(g);
            }

            this.SetRamalesFilter();

        }


    }

    AddSelectedRamal(ramal: ItemDto): void {

        if (ramal) {
            var filterRamal = new BanderaFilter();


            filterRamal.RamaColorId = ramal.Id;



            this._BanderaService.FindItemsAsync(filterRamal)
                .finally(() => {
                })
                .subscribe((result) => {



                    let lista = [...this.filter.BanderasComerciales];

                    result.DataObject.forEach(r => {

                        var l = lista.find(e => e.Id == r.Id);
                        if (!l) {
                            var item = r as BanderaItemLongDto;
                            item.RamaColor_Nombre = ramal.Description;
                            lista.push(item);
                        }

                    });


                    this.filter.BanderasComerciales = lista;
                });
        }





    }

    removeLinea(item: ItemDto) {

        var index = this.filter.Lineas.findIndex(x => x.Id == item.Id);

        if (index >= 0) {
            this.filter.Lineas.splice(index, 1);

            var indexg = this.filter.RamalesSelect.findIndex(x => x.Id == item.Id);
            if (indexg >= 0) {
                this.filter.RamalesSelect.splice(index, 1);
            }

            var indexg = this.filter.Ramales.findIndex(x => x.Id == item.Id);
            if (indexg >= 0) {

                let lista = [...this.filter.Ramales];
                lista.splice(index, 1);
                this.filter.Ramales = lista;



            }


            this.SetRamalesFilter();
        }

    }

    removeRamal(ramal: ItemDto) {

        var gruporamal = this.filter.RamalesSelect.find(x => x.Items.some(e => e.Id == ramal.Id));

        var index = gruporamal.Items.findIndex(x => x.Id == ramal.Id);
        if (index >= 0) {
            gruporamal.Items.splice(index, 1);
            if (gruporamal.Items.length == 0) {
                var indexg = this.filter.RamalesSelect.findIndex(x => x.Id == gruporamal.Id);

                this.filter.RamalesSelect.splice(index, 1);

            }
            this.removeBanderaPorRamal(ramal);
            this.SetRamalesFilter();
        }

    }

    removeBanderaPorRamal(ramal: ItemDto) {

        //borrar las banderas de un ramal;
        var banderasaborrar = this.filter.BanderasComerciales.filter(x => x.RamaColorId == ramal.Id);
        banderasaborrar.forEach(b => {
            this.removeBanderaComercial(b);
        });
    }



    removeBanderaPosicionamiento(bandera: ItemDto) {
        var index = this.filter.BanderasPosicionamiento.findIndex(x => x.Id == bandera.Id);
        if (index >= 0) {
            this.filter.BanderasPosicionamiento.splice(index, 1);
            let lista = [...this.filter.BanderasPosicionamiento];
            this.filter.BanderasPosicionamiento = lista;
        }
    }

    removeBanderaComercial(bandera: ItemDto) {
        var index = this.filter.BanderasComerciales.findIndex(x => x.Id == bandera.Id);
        if (index >= 0) {
            this.filter.BanderasComerciales.splice(index, 1);
            let lista = [...this.filter.BanderasComerciales];
            this.filter.BanderasComerciales = lista;
        }
    }


    SetRamalesFilter() {
        var _ramalesID = [];
        this.filter.RamalesSelect.forEach(g => {

            g.Items.forEach(r => {
                _ramalesID.push(r.Id);
            });
        })

        this.RamalesID = _ramalesID;
    }


    porletclick(id: String): void {
        // this.filter.Banderas = [];
        //this.filter.selectBandera = null;
        //if (id == 'm_portlet_tools_1') {
        //    this.portlet2.toggle();
        //}
        //else {
        //    this.portlet1.toggle();
        //}
    }


    VerMapaComercial(): void {


        var rvf = new RutasViewFilter();
        rvf.Original = this.Original;
        rvf.Vigente = this.Vigente;
        rvf.BanderasId = [];
        rvf.Sucursalid = this.Sucursalid;
        this.filter.BanderasComerciales.forEach(f => {
            rvf.BanderasId.push(f.Id);
        }
        );

        this.active = false;



        (this.GetEditComponent() as ViewRutasComponent).fillMapa(rvf);
    }

    VerMapaPosicionamiento(): void {


        var rvf = new RutasViewFilter();
        rvf.Original = this.Original;
        rvf.Vigente = this.Vigente;
        rvf.BanderasId = [];
        rvf.Sucursalid = this.Sucursalid;
        this.filter.BanderasPosicionamiento.forEach(f => {
            rvf.BanderasId.push(f.Id);
        }
        );

        this.active = false;
        (this.GetEditComponent() as ViewRutasComponent).fillMapa(rvf);
    }

}