import { Component, ViewChild, Injector, Output, EventEmitter, ElementRef, Type, ComponentFactoryResolver, ViewContainerRef, OnInit } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
//import { UserServiceProxy, ProfileServiceProxy, UserEditDto, CreateOrUpdateUserInput, OrganizationUnitDto, UserRoleDto, PasswordComplexitySetting } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '../../../../shared/common/app-component-base';


import * as _ from 'lodash';
declare let mApp: any;

import { DetailEmbeddedComponent, DetailModalComponent } from '../../../../shared/manager/detail.component';
import { DetailComponent, IDetailComponent } from '../../../../shared/manager/detail.component';
import { TipoParadaService } from './tipoparada.service';
import { TipoParadaDto } from '../model/tipoParada.model';
import { TiempoEsperadoDeCargaDto } from '../model/tipoParada.model';
import { TipoDiaDto } from '../model/tipoDia.model';
import { TipoDiaService } from '../tipoDia/tipodia.service';
import { retry } from 'rxjs/operator/retry';
import { NgForm } from '@angular/forms';
import { SelectItem } from 'primeng/api';

@Component({
    selector: 'createOrEditTipoParadaDtoModal',
    templateUrl: './create-or-edit-tipoparada-modal.component.html',

})
export class CreateOrEditTipoParadaModalComponent extends DetailEmbeddedComponent<TipoParadaDto> implements OnInit {
    protected cfr: ComponentFactoryResolver;

    @ViewChild('createOrEditChild', { read: ViewContainerRef }) createOrEditChild: ViewContainerRef;

    TiposDias: TipoDiaDto[];
    TiposDiasSelectItem: SelectItem[];

    constructor(
        injector: Injector,
        protected service: TipoParadaService,
        protected _servicetp: TipoDiaService,
    ) {
        super(service, injector);
        this.cfr = injector.get(ComponentFactoryResolver);
        this.detail = new TipoParadaDto();

        this.icon = "fa fa-map-signs";
        this.title = "Tipo de Parada";
    }

    addNewTiempoEsperadoDeCarga() {
        let lista = [...this.detail.TiempoEsperadoDeCarga];

        lista.push(this.getNewItem(null, lista.length * -1));

        this.detail.TiempoEsperadoDeCarga = lista;
    }

    getDescription(item: TipoParadaDto): string {
        return item.Nombre;
    }

    getNewItem(item: TiempoEsperadoDeCargaDto, id: number): TiempoEsperadoDeCargaDto {
        var item = new TiempoEsperadoDeCargaDto(item)
        item.TipoParadaId = this.detail.Id;
        item.Id = id;
        item.Nombre = null;
        item.TipodeDiaId = null;
        item.HoraDesde = null;
        item.HoraHasta = null;
        item.TiempoDeCarga = null;
        item.TipoDiaNombre = null;
        return item;

    }

    ngOnInit(): void {
        super.ngOnInit();
        this._servicetp.requestAllByFilter().subscribe(result => {

            this.TiposDias = result.DataObject.Items;
            this.TiposDiasSelectItem = this.TiposDias.map(e => { return { label: e.DesTdia, value: e.Id } })
        });
        //this.detailForm.form.addControl()
        // this.ReadDto(this.id);
    }

    onTipoDiaChange(event, row): void {
        if (event && event.value) {
            row.TipoDiaNombre = this.TiposDias.find(x => x.Id == event.value).DesTdia;
        }

        else {
            row.TipoDiaNombre = "";
        }


    }

    onCargaDelete(row: TiempoEsperadoDeCargaDto): void {
        var index = this.detail.TiempoEsperadoDeCarga.findIndex(x => x.Id == row.Id);

        if (index >= 0) {
            let lista = [...this.detail.TiempoEsperadoDeCarga];
            lista.splice(index, 1);
            this.detail.TiempoEsperadoDeCarga = lista;
        }
    }


    save(form: NgForm): void {

        var validateAll = true;
        this.TiposDias.forEach((td, i) => {
            if (this.detail.TiempoEsperadoDeCarga.filter(e => e.TipodeDiaId == td.Id).length == 0) {
                validateAll = false;
                return;
            }
        });

        if (!validateAll) {
            this.message.confirm("Faltan cargar tipos de días. ¿Desea agregarlos?", "Tipo de parada", (result) => {

                if (result.value) {
                    this.TiposDias.forEach((td, i) => {
                        if (this.detail.TiempoEsperadoDeCarga.filter(e => e.TipodeDiaId == td.Id).length == 0) {
                            let lista = [...this.detail.TiempoEsperadoDeCarga];

                            var newItem = this.getNewItem(null, lista.length * -1);
                            newItem.TipodeDiaId = td.Id;
                            newItem.HoraDesde = '00:00';
                            newItem.HoraHasta = '23:59';
                            newItem.TipoDiaNombre = td.DesTdia;
                            lista.push(newItem);

                            this.detail.TiempoEsperadoDeCarga = lista;
                        }
                    });
                }
                else {
                    super.save(form);
                }
            });
        }
        else {
            super.save(form);
        }

    }

}
