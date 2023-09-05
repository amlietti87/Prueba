import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, ChangeDetectorRef } from '@angular/core';

import { CrudComponent, BaseCrudComponent } from '../../../../shared/manager/crud.component';

import { IDetailComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';

import { ViewMode } from '../../../../shared/model/base.model';
import { FormGroup, FormControl } from '@angular/forms';
import { YesNoAllComboComponent } from '../../../../shared/components/yesnoall-combo.component';
import { DiagramasInspectoresDto, DiagramasInspectoresFilter } from '../model/diagramasinspectores.model';
import { DiagramasInspectoresService } from './diagramasinspectores.service';
import { CreateOrEditDiagramasInspectoresModalComponent } from './create-or-edit-diagramasinspectores-modal.component';
import { MonthComboComponent } from '../../../../shared/components/month-combo.component';
import { DATE } from 'ngx-bootstrap/chronos/units/constants';
import { Router, NavigationStart } from '@angular/router';
import { Subscription } from 'rxjs';
import { MatDialogRef, MatDialog, MatDialogConfig } from '@angular/material';
import { EditDiagramacionComponent } from './edit-diagramacion.component';
import * as _ from "lodash"
import { isFakeMousedownFromScreenReader } from '@angular/cdk/a11y';
import { takeUntil } from 'rxjs/operator/takeUntil';


@Component({

    templateUrl: "./diagramasinspectores.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class DiagramasInspectoresComponent extends BaseCrudComponent<DiagramasInspectoresDto, DiagramasInspectoresFilter> implements AfterViewInit {

    TurnosId: number[] = [];

    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditDiagramasInspectoresModalComponent
    }

    @ViewChild('ComboMes') ComboMes: MonthComboComponent;
    public dialog: MatDialog;
    allowImprimir: boolean = false;
    allowPublicar: boolean = false;
    allowDesbloquear: boolean = false;

    constructor(injector: Injector, protected _DInspectoresService: DiagramasInspectoresService, protected cdr: ChangeDetectorRef) {
        super(_DInspectoresService, CreateOrEditDiagramasInspectoresModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Diagramación"
        this.moduleName = "Inspectores";
        this.icon = "flaticon-map";
        this.loadInMaterialPopup = true;
        this.advancedFiltersAreShown = true;
        this.dialog = injector.get(MatDialog);
        this.injector = injector;
        this.allowPublicar = this.permission.isGranted("Inspectores.Diagramacion.Publicar");
        this.allowImprimir = this.permission.isGranted("Inspectores.Diagramacion.Imprimir");
        this.allowDesbloquear = this.permission.isGranted("Inspectores.Diagramacion.Desbloquear");
        

    }

    getDescription(item: DiagramasInspectoresDto): string {
        return item.Description
    }
    getNewItem(item: DiagramasInspectoresDto): DiagramasInspectoresDto {

        var item = new DiagramasInspectoresDto(item);
        return item;
    }

    ngAfterViewInit() {
        //debugger;
        super.ngAfterViewInit();
        this.ComboMes.writeValue(this.filter.Mes);
        this.ComboMes.refresh();

        this.cdr.detectChanges();

    }


    Opendialog(_detail: DiagramasInspectoresDto, viewMode: ViewMode) {

        var popupHeight: string = '';
        var popupWidth: string = '80%';

        var dialog = this.injector.get(MatDialog);

        const dialogConfig = new MatDialogConfig<DiagramasInspectoresDto>();

        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.width = popupWidth;
        dialogConfig.height = popupHeight;
        let selft = this;



        dialogConfig.data = _detail;

        let dialogRef = dialog.open(this.GetEditComponentType(), dialogConfig);

        this.detailElement = dialogRef.componentInstance;

        dialogRef.componentInstance.viewMode = viewMode;

        (dialogRef.componentInstance as DetailAgregationComponent<DiagramasInspectoresDto>).saveLocal = false;
        dialogRef.afterOpen()
            .pipe((e) => e.takeUntil(this.unsubscriber))  
            .subscribe(
            data => { this.afterOpenDialog(data) }
        );
        dialogRef.afterClosed()
            .pipe((e) => e.takeUntil(this.unsubscriber))  
            .subscribe(


            data => {
                //console.log("Dialog output:", data);
                this.active = true;

                if (data) {
                    if (viewMode == ViewMode.Add) {
                        let id = data.Id;
                        selft.onEditDiagramacion(new DiagramasInspectoresDto({ Id: id }));
                    }
                    else {
                        this.onSearch();

                    }
                }

            }
        );

        //dialogRef.updateSize(this.popupWidth, this.popupHeight);


    }


    ngOnInit() {

        super.ngOnInit();


    }

    BorrarFiltros() {
        var newFilter = new DiagramasInspectoresFilter();
        var today = new Date();
        newFilter.Anio = today.getFullYear();
        newFilter.Mes = null;
        newFilter.EstadoDiagrama = null;
        newFilter.GruposInspectores = null;
        this.filter = newFilter;
        localStorage.setItem('filter', JSON.stringify(this.filter));


    }

    getNewfilter(): DiagramasInspectoresFilter {
        var filter = new DiagramasInspectoresFilter();
        var diagramas = localStorage.getItem('filter');
        var oldFilter = JSON.parse(diagramas);
        if (oldFilter) {
            filter.Mes = oldFilter.Mes;
            filter.Anio = oldFilter.Anio;
            if (oldFilter.GruposInspectores) {
                filter.GruposInspectores = oldFilter.GruposInspectores;
            }
            if (oldFilter.EstadoDiagrama) {
                filter.EstadoDiagrama = oldFilter.EstadoDiagrama;
            }
        }
        else {
            var today = new Date();
            filter.Mes = today.getMonth() + 1;
            filter.Anio = today.getFullYear();
        }

        return filter;

    }

    onEdit(row: DiagramasInspectoresDto) {
        this.onEditID(row.Id);
    }

    onEditID(id: any) {
        this.active = false;

        if (this.loadInMaterialPopup) {
            this.service.getById(id, false)
                .pipe((e) => e.takeUntil(this.unsubscriber))  
                .subscribe(e => {
                this.Opendialog(e.DataObject, ViewMode.Modify);
            });
        }
        else {
            this.GetEditComponent().show(id);
        }
    }

    onPublicar(row: DiagramasInspectoresDto) {
        this.active = false;
        this._DInspectoresService.publicarDiagramacion(row)
            .finally(() => { this.active = true; })
            .pipe((e) => e.takeUntil(this.unsubscriber))  
            .subscribe(e => {
                if (e.Status == 'Ok') {
                    this.notify.info('Diagramación Publicada');
                    this.onSearch();
                }
                console.log("publicar", e.Status);
            }, error => {
                    this.handleErros(error, row.Id);
                });
    }

    onDesbloquear(row: DiagramasInspectoresDto): void {
        let solft = this.unsubscriber;
        this.service.unBlockEntity(row.Id)
            .pipe((e)=>e.takeUntil(this.unsubscriber))  
            .subscribe(e => {
                this.notify.success("Desbloquedo correctamente.")
        });

    }

    GetReporteExcel(row: DiagramasInspectoresDto): void {
        var dialog: MatDialog;
        dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;

        this.service.getById(row.Id, false)
            .pipe((e) => e.takeUntil(this.unsubscriber))  
            .subscribe(e => {
            dialogConfig.data = e.DataObject
            let dialogRef = this.dialog.open(EditDiagramacionComponent, dialogConfig);



                dialogRef.afterClosed()
                    .pipe((e) => e.takeUntil(this.unsubscriber))  
                    .subscribe(
                data => {
                    if (data) {

                        data.forEach(tur => {
                            this.TurnosId.push(tur.Id)
                        });

                        this._DInspectoresService.getImprimirDiagrama(row.Id, this.TurnosId);
                        this.TurnosId = [];
                    }

                });
        });

    }

    onEditDiagramacion(row: DiagramasInspectoresDto) {

        var dialog: MatDialog;
        dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.width = '50%';
        dialogConfig.height = 'auto';
        localStorage.setItem('filter', JSON.stringify(this.filter));
        this.service.getById(row.Id, false)
            .pipe((e) => e.takeUntil(this.unsubscriber))  
            .subscribe(e => {
            dialogConfig.data = e.DataObject
            let dialogRef = this.dialog.open(EditDiagramacionComponent, dialogConfig);

            

                dialogRef.afterClosed()
                    .pipe((e) => e.takeUntil(this.unsubscriber))  
                    .subscribe(
                data => {
                    if (data) {
                        this.active = false
                        data.forEach(tur => {
                            this.TurnosId.push(tur.Id)
                        });
                        const queryParams = { TurnoId: this.TurnosId, DiagramaInspectoresId: row.Id };
                        localStorage.setItem('showDiagramacion', 'true');
                        this.router.navigate(['/inspectores/diagramacionEdit/' + row.Id], { queryParams: queryParams });
                    }
                }
            );
        },
            err => this.handleErros(err, row.Id));

    }

    onClearFilters() : void {
        this.BorrarFiltros();
    }


}