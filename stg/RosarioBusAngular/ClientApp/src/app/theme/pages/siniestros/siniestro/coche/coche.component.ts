import { Component, Input, Injector, ViewChild, ChangeDetectorRef, AfterContentChecked } from '@angular/core';
import { SiniestrosDto } from '../../model/siniestro.model';
import { CochesDto } from '../../model/coche.model';
import { ItemDto, ItemDtoStr, ViewMode } from '../../../../../shared/model/base.model';
import { CochesService } from '../../coches/coches.service';
import { LineaDto } from '../../../planificacion/model/linea.model';
import { timeParse, easeBack } from 'd3';
import { FormBuilder, Validators } from '@angular/forms';
import { CochesAutoCompleteComponent } from '../../shared/coche-autocomplete.component';
import { PermissionCheckerService } from '../../../../../shared/common/permission-checker.service';


@Component({
    selector: 'coche',
    templateUrl: './coche.component.html',
})
export class CocheComponent implements AfterContentChecked {
    @ViewChild('autocompletecoche') autocompletecoche: CochesAutoCompleteComponent;
    cocheForm: any;

    permission: PermissionCheckerService;

    constructor(
        protected serviceCoche: CochesService,
        injector: Injector,
        private cocheFB: FormBuilder,
        private cdref: ChangeDetectorRef
    ) {
        this.cocheFB = injector.get(FormBuilder);
        this.createForm();
        this.permission = injector.get(PermissionCheckerService);
        this.SetAllowPermission();
    }


    allowAddTipoDanio: boolean = false;
    allowAddSeguro: boolean = false;

    SetAllowPermission() {
        this.allowAddTipoDanio = this.permission.isGranted('Siniestro.TipoDanio.Agregar');
        this.allowAddSeguro = this.permission.isGranted('Siniestro.Seguro.Agregar');
    }

    ngAfterContentChecked() {

        this.cdref.detectChanges();

    }

    _detail: SiniestrosDto;
    _viewMode: ViewMode;
    @Input() CurrentCoche: CochesDto;
    @Input() CurrentLinea: LineaDto;


    @Input()
    get detail(): SiniestrosDto {

        return this._detail;
    }

    set detail(value: SiniestrosDto) {

        this._detail = value;
    }

    @Input()
    get ViewMode(): ViewMode {

        return this._viewMode;
    }

    set ViewMode(value: ViewMode) {

        this._viewMode = value;
    }

    @Input()
    get selectCoches(): ItemDtoStr {
        return this.detail.selectCoches;
    }

    set selectCoches(value: ItemDtoStr) {
        if (value) {
            this.detail.selectCoches = value;
            if (this.detail.selectCoches && this.detail.selectCoches.Id != this.detail.CocheId) {
                this.detail.CocheId = value.Id;
                this.GetCurrentCoche(this.detail);
                this.GetLineaPorDefecto();
            }
            else {
                var coche = new CochesDto();
                coche.Id = this.detail.CocheId;
                coche.Dominio = this.detail.CocheDominio;
                coche.Ficha = this.detail.CocheFicha;
                coche.Interno = this.detail.CocheInterno;
                this.CurrentCoche = coche;
            }
        }
        else {
            this.CurrentCoche = null;
        }
    }

    @Input()
    get selectLineas(): ItemDto {

        return this.detail.selectLineas;
    }

    set selectLineas(value: ItemDto) {
        if (value) {
            this.detail.selectLineas = value;
        }
        else {
            this.CurrentLinea = null;
        }
    }



    private GetLineaPorDefecto(): void {

        if (this.detail.EmpPract == 'E' && this.detail.selectEmpleados && this.detail.ConductorId && this.detail.Fecha && this.detail.Hora) {
            this.serviceCoche.GetLineaPorDefecto(this.detail.ConductorId, this.detail.Fecha, this.detail.Hora)
                .subscribe((t) => {
                    if (t.DataObject != 0) {

                        this.detail.selectLineas.Id = t.DataObject;
                    }
                    else {
                        this.detail.selectLineas = null;
                    }
                })

        }

    }


    createForm() {
        if (this.detail == null) {
            this.detail = new SiniestrosDto();
            this.CurrentCoche = null;
            this.CurrentLinea = null;
        }
        this.cocheForm =
            this.cocheFB.group({
                CocheId: [this.detail.CocheId, Validators.required],
                CocheLineaId: [this.detail.CocheLineaId, Validators.required],
                CostoReparacion: [this.detail.CostoReparacion, null],
                TipoDanioId: [this.detail.TipoDanioId, null],
                InformaTaller: [this.detail.InformaTaller, Validators.required],
                SeguroId: [this.detail.SeguroId, null],
            });

        this.formControlValueChanged();

    }

    formControlValueChanged() {

    }


    GetCurrentCoche(item: SiniestrosDto) {
        if (item.CocheId) {
            this.serviceCoche.getById(item.CocheId)
                .subscribe((t) => {
                    if (t.DataObject) {
                        item.CocheFicha = t.DataObject.Ficha;
                        item.CocheDominio = t.DataObject.Dominio;
                        item.CocheInterno = t.DataObject.Interno;
                        this.CurrentCoche = t.DataObject;
                    }
                })
        }
    }

}