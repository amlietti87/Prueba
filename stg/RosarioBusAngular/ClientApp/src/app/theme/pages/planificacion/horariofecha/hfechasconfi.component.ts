import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { ActivatedRoute, Route } from '@angular/router';
import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';
import { IDetailComponent } from '../../../../shared/manager/detail.component';
import { HFechasConfiService } from './HFechasConfi.service';
import { HFechasConfiDto, HorarioFechaFilter, PlaHorarioFechaLineaListView } from '../model/HFechasConfi.model';
import { LazyLoadEventData } from '../../../../shared/helpers/PrimengDatatableHelper';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { AsignacionLineaLineaHoraria } from './asignacion-linea-lineahoraria.component';
import { LineaService } from '../linea/linea.service';
import { LineaDto } from '../model/linea.model';
import { ViewMode } from '../../../../shared/model/base.model';
import { CreateOrEditHFechasConfiModalComponent } from './hfechasconfig/create-or-edit-hfechasconfi-modal.component';


@Component({

    templateUrl: "./hfechasconfi.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class HFechasConfiComponent extends BaseCrudComponent<HFechasConfiDto, HorarioFechaFilter> implements OnInit, AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditHFechasConfiModalComponent;

    }





    constructor(
        protected injector: Injector,
        protected _Service: HFechasConfiService,
        protected lineaservice: LineaService
    ) {
        super(_Service, CreateOrEditHFechasConfiModalComponent, injector);

        this.icon = "la la-clock-o"
        this.title = "Horario";

        this.SetAllowPermission

    }

    SetAllowPermission() {
        this.allowAdd = this.permission.isGranted('Horarios.FechaHorario.CrearLineasAsociadas');
        this.allowModify = this.permission.isGranted('Horarios.FechaHorario.ModificarLineasAsociadas');

        //this.allowModify = this.permission.isGranted('Siniestro.Involucrado.Modificar');
        //this.allowAddTipoInvolucrado = this.permission.isGranted('Siniestro.TipoInvolucrado.Agregar');
        //this.allowAddTipoMueInmueble = this.permission.isGranted('Siniestro.TipoMuebleInmueble.Agregar');
        //this.allowAddTipoLesionado = this.permission.isGranted('Siniestro.TipoLesionado.Agregar');
        //this.allowAddCiaSeguro = this.permission.isGranted('Siniestro.Seguro.Agregar');
        //this.allowAddLocalidades = this.permission.isGranted('Admin.Localidad.Agregar');
        //this.allowAddTipoDni = this.permission.isGranted('Admin.TipoDni.Agregar');
    }


    ngAfterViewInit() {
        super.ngAfterViewInit();
        this.GetEditComponent();
    }


    getNewfilter(): HorarioFechaFilter {
        var f = new HorarioFechaFilter();

        return f;
    }


    ngOnInit() {

        super.ngOnInit();

        this.onSearch();


    }

    Search(event?: LazyLoadEventData) {

    }


    onEditLineaHorario(row: PlaHorarioFechaLineaListView) {
        this.active = false
        //this.hfechasconfilinea.onSearchLineas(row);
        this.router.navigate(['/planificacion/horariolinea/' + row.CodLinea]);

    }

    onEditLineaAsociadas(row: PlaHorarioFechaLineaListView) {

        this.lineaservice.RecuperarLineasConLineasAsociadas(row.CodLinea).subscribe(e => this.openLineaDialog(e.DataObject, ViewMode.Modify))
    }




    onSearch(event?: LazyLoadEventData) {
        this.isTableLoading = true;
        this.primengDatatableHelper.showLoadingIndicator();

        this._Service.GetLineasHorarias()
            .finally(() => {
                this.isTableLoading = false;
                this.primengDatatableHelper.hideLoadingIndicator()
            }).subscribe(r => {
                this.primengDatatableHelper.records = r.DataObject
            });
    }


    getDescription(item: HFechasConfiDto): string {
        return item.CodLinea + " " + item.FechaDesde;
    }


    getNewItem(item: HFechasConfiDto): HFechasConfiDto {
        var cloneItem = new HFechasConfiDto(item);

        return cloneItem;
    }

    onCreate() {
        var newItem = new LineaDto();
        this.openLineaDialog(newItem, ViewMode.Add);
    }

    openLineaDialog(linea: LineaDto, viewMode: ViewMode) {

        var dialog: MatDialog;
        dialog = this.injector.get(MatDialog);

        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.width = '80%';
        dialogConfig.height = '';
        dialogConfig.data = linea;

        let dialogRef = dialog.open(AsignacionLineaLineaHoraria, dialogConfig);
        dialogRef.componentInstance.IsInMaterialPopupMode = true;
        dialogRef.componentInstance.saveLocal = false;
        dialogRef.componentInstance.viewMode = viewMode;
        dialogRef.componentInstance.showDto(linea);


        dialogRef.afterClosed().subscribe(
            data => {
                if (data) {
                }

            }
        );
    }

}