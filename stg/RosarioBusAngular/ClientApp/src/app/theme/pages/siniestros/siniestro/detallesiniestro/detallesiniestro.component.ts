import { Component, Input, Type, ViewChild, ChangeDetectionStrategy, ChangeDetectorRef, DebugElement, Injector, OnInit, AfterViewInit, EventEmitter, Output } from '@angular/core';
import { SiniestrosDto, SiniestrosConsecuenciasDto } from '../../model/siniestro.model';
import { CochesDto } from '../../model/coche.model';
import { ItemDto, ItemDtoStr } from '../../../../../shared/model/base.model';
import { LineaDto } from '../../../planificacion/model/linea.model';
import { CausasService } from '../../causas/causas.service';
import { ActivatedRoute } from '@angular/router';
import { PermissionCheckerService } from '../../../../../shared/common/permission-checker.service';
import { IDetailComponent } from '../../../../../shared/manager/detail.component';
import { CreateOrEditCausasModalComponent } from '../../causas/create-or-edit-causas-modal.component';
import { CausasDto, SubCausasDto } from '../../model/causas.model';
import { ConsecuenciasService } from '../../consecuencias/consecuencias.service';
import { CreateOrEditConsecuenciasModalComponent } from '../../consecuencias/create-or-edit-consecuencias-modal.component';
import { ConsecuenciasDto, CategoriasDto, ConsecuenciasFilter } from '../../model/consecuencias.model';
import { Subscription } from 'rxjs';
import { forEach } from '@angular/router/src/utils/collection';
import { NgModel, Validators, FormBuilder } from '@angular/forms';
import { CiaSegurosService } from '../../seguros/seguros.service';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { CausaComboComponent } from '../../shared/causa-combo.component';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { CreateCausaDialogComponent } from '../causas/createcausadialog.component';
import { CategoriasService } from '../../consecuencias/categorias.service';
import { AddConsecuenciaModalComponent } from './add-consecuencia-modal.component';
import { InformeComboComponent } from '../../shared/informe-combo.component';
import { SancionComboComponent } from '../../shared/sancion-combo.component';
import { SubCausaComboComponent } from '../../shared/subcausa-combo.component';

@Component({
    selector: 'detalle-siniestro',
    templateUrl: './detallesiniestro.component.html',
})
export class DetalleSiniestroComponent extends AppComponentBase {
    disableSancionSugerida: boolean = false;
    disableInforme: boolean = false;
    detalleSiniestroForm1: any;
    detalleSiniestroForm2: any;
    _SiniestroConsecuencias: SiniestrosConsecuenciasDto[];

    @Input()
    get SiniestroConsecuencias(): SiniestrosConsecuenciasDto[] {
        return this._SiniestroConsecuencias;
    }
    set SiniestroConsecuencias(value: SiniestrosConsecuenciasDto[]) {
        this._SiniestroConsecuencias = value;
    }

    ngOnInit() {

    }

    @ViewChild('AgregarConsecuencias') AgregarConsecuencias: CreateOrEditConsecuenciasModalComponent;
    @ViewChild('AgregarCausas') AgregarCausas: CreateOrEditCausasModalComponent;
    @ViewChild('causascombo') causascombo: CausaComboComponent;
    @ViewChild('subcausacombo') subcausacombo: SubCausaComboComponent;
    @ViewChild('InformeConId') InformeConId: InformeComboComponent;
    @ViewChild('SancionSugeridaId') SancionSugeridaId: SancionComboComponent;
    self: DetalleSiniestroComponent;

    allowAddCausa: boolean = false;
    allowAddFactor: boolean = false;
    allowAddNorma: boolean = false;
    allowAddConsecuencia: boolean = false;
    allowAddSancionSugerida: boolean = false;
    allowAddSiniestroConsecuencia: boolean = false;
    allowAddTipoColision: boolean = false;

    constructor(injector: Injector,
        protected serviceConsecuencias: ConsecuenciasService,
        protected serviceCausas: CausasService,
        protected serviceCategorias: CategoriasService,
        //private consecuenciaFB: FormBuilder,
        private cdr: ChangeDetectorRef,
        private causaFB: FormBuilder,
        public dialog: MatDialog) {

        super(injector);
        //this.consecuenciaFB = injector.get(FormBuilder);
        //this.createForm();
        this.SetAllowPermission();
        this.causaFB = injector.get(FormBuilder);
        this.createForm();

        this.SetAllowPermission();

    }

    SetAllowPermission() {
        this.allowAddCausa = this.permission.isGranted('Siniestro.Causa.Agregar');
        this.allowAddConsecuencia = this.permission.isGranted('Siniestro.Consecuencia.Agregar');
        this.allowAddFactor = this.permission.isGranted('Siniestro.FactoresIntervinientes.Agregar');
        this.allowAddNorma = this.permission.isGranted('Siniestro.Norma.Agregar');;
        this.allowAddSancionSugerida = this.permission.isGranted('Siniestro.SancionSugerida.Agregar');
        this.allowAddSiniestroConsecuencia = this.permission.isGranted('Siniestro.Siniestro.Modificar');
        this.allowAddTipoColision = this.permission.isGranted('Siniestro.TipoColision.Agregar');
    }

    _detail: SiniestrosDto;
    @Input()
    get detail(): SiniestrosDto {
        return this._detail;
    }
    set detail(value: SiniestrosDto) {
        this._detail = value;
        if (!this._SiniestroConsecuencias || this._SiniestroConsecuencias.length < 1) {
            this.OnConsecuenciasRowAdded();
        }
    }



    crearCausa() {
        this.AgregarCausas.showNew(this.getNewItemCausas());
    }

    getNewItemCausas(): CausasDto {

        var item = new CausasDto(item);
        item.Anulado = false;
        item.Responsable = false;
        let list: Array<SubCausasDto> = []
        item.SubCausas = list;
        return item;
    }

    openDialog() {

        const dialogConfig = new MatDialogConfig();

        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;

        dialogConfig.data = {
            id: 1,
            title: 'Crear causa',
            height: '400px',
            width: '600px',
            hasBackdrop: false
        };

        let dialogRef = this.dialog.open(CreateCausaDialogComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(
            data => console.log("Dialog output:", data)
        );
    }

    createForm() {
        if (this.detail == null) {
            this.detail = new SiniestrosDto();
        }
        this.detalleSiniestroForm1 =
            this.causaFB.group({
                Comentario: [this.detail.Comentario, Validators.required],
                ObsDanios: [this.detail.ObsDanios, null],
                ObsInterna: [this.detail.ObsInterna, null],
                InformeConId: [this.detail.InformeConId, Validators.required],
                CodInforme: [this.detail.CodInforme, null],
                NroSiniestroSeguro: [this.detail.NroSiniestroSeguro, null],
                DescargoConId: [this.detail.DescargoConId, Validators.required],
                SancionSugeridaId: [this.detail.SancionSugeridaId, null],
                ResponsableConId: [this.detail.ResponsableConId, Validators.required],
                TipoColisionId: [this.detail.TipoColisionId, Validators.required]
            });

        this.detalleSiniestroForm2 =
            this.causaFB.group({
                CausaId: [this.detail.CausaId, null],
                SubCausaId: [this.detail.SubCausaId, null],
                FactoresId: [this.detail.FactoresId, null],
                ConductaId: [this.detail.ConductaId, null],
            });

        this.formControlValueChanged();
    }

    formControlValueChanged() {

        this.detalleSiniestroForm2.get('CausaId').valueChanges.subscribe(
            (CausaId: number) => {

                this.CheckResponsable(CausaId);

            });
    }

    CheckCausaRequired() {
        if (this.subcausacombo && this.subcausacombo != null) {
            this.detalleSiniestroForm2.get('SubCausaId').clearValidators();
            if (this.subcausacombo.items.length >= 1) {
                this.detalleSiniestroForm2.get('SubCausaId').setValidators([Validators.required]);
            }
            else {
                this.detalleSiniestroForm2.get('SubCausaId').setValidators(null);
            }
            this.detalleSiniestroForm2.get('SubCausaId').updateValueAndValidity();
        }
    }

    CheckResponsable(CausaId: number) {

        if (this.causascombo && this.causascombo.items.length > 0 && !(this.detail.Id && this.detail.Id != 0)) {

            var responsableCausa = this.causascombo.items.find(e => e.Id == CausaId);

            var responsableConsecuencias = false;

            if (this.SiniestroConsecuencias && this.SiniestroConsecuencias != null && this.SiniestroConsecuencias.length >= 1) {
                var find = this.SiniestroConsecuencias.find(e => e.Consecuencia && e.Consecuencia.Responsable == true);
                if (find != null) {
                    responsableConsecuencias = true;
                }
            }

            if (responsableCausa && responsableCausa != null && (responsableCausa.Responsable == true || responsableConsecuencias == true)) {
                this.detail.Responsable = true;
                this.detail.ResponsableConId = 1;
            }
            else {
                this.detail.Responsable = false;
                this.detail.ResponsableConId = 2;

            }
            this.cdr.detectChanges();
        }
    }

    //Consecuencias Stuff ↓

    CreateNewSiniestroConsecuencia(Id: number): SiniestrosConsecuenciasDto {
        var item = new SiniestrosConsecuenciasDto();
        item.Id = Id;
        return item;
    }

    OnConsecuenciasRowAdded(): void {
        if (!this._SiniestroConsecuencias) {
            this._SiniestroConsecuencias = new Array<SiniestrosConsecuenciasDto>();
        }
        this._SiniestroConsecuencias = [...this._SiniestroConsecuencias, this.CreateNewSiniestroConsecuencia(this._SiniestroConsecuencias.length * -1)];

        this.CheckResponsable(this.detail.CausaId);
    }

    OnConsecuenciasRowRemoved(row: SiniestrosConsecuenciasDto): void {
        var index = this._SiniestroConsecuencias.findIndex(x => x.Id == row.Id);

        if (index >= 0) {
            let lista = [...this._SiniestroConsecuencias];
            lista.splice(index, 1);
            this._SiniestroConsecuencias = [...lista];
        }

        this.CheckResponsable(this.detail.CausaId);
    }

    OnConsecuenciasComboChanged(newValue, oldValue): void {
        var consecuenciasFiltered = this._SiniestroConsecuencias.filter(e => e.ConsecuenciaId == newValue);
        if (consecuenciasFiltered && consecuenciasFiltered.length > 1) {
            this.notificationService.warn("La consecuencia seleccionada ya fue agregada");
            oldValue.Consecuencia = null;
            oldValue.ConsecuenciaId = null;
            oldValue.ConsecuenciaNombre = null;
            oldValue.Categoria = null;
            oldValue.CategoriaNombre = null;
            oldValue.CategoriaId = null;
            oldValue.Categorias = null;
            return;
        } else {
            this.serviceConsecuencias.getById(newValue).subscribe(e => {
                if (e.Status == "Ok" && e.DataObject) {
                    oldValue.Consecuencia = e.DataObject;
                    oldValue.ConsecuenciaNombre = e.DataObject.Descripcion;
                    oldValue.Categoria = null;
                    oldValue.CategoriaNombre = null;
                    oldValue.CategoriaId = null;
                    oldValue.Categorias = e.DataObject.Categorias;
                    this._SiniestroConsecuencias = [...this._SiniestroConsecuencias];
                }
            });
        }
    }

    //End Consecuencias Stuff ↑

    OnCategoriasComboChanged(newValue, oldValue): void {
        if (newValue != "") {
            this.serviceCategorias.getById(newValue).subscribe(e => {
                if (e.Status == "Ok" && e.DataObject) {
                    oldValue.Categoria = e.DataObject;
                    oldValue.CategoriaNombre = e.DataObject.Descripcion;
                    this._SiniestroConsecuencias = [...this._SiniestroConsecuencias];
                }
            });
        }
        else {
            oldValue.Categoria = '';
            oldValue.CategoriaNombre = '';
            this._SiniestroConsecuencias = [...this._SiniestroConsecuencias];
        }
    }
}