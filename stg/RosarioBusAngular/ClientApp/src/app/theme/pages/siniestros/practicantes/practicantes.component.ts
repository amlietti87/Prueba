import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type } from '@angular/core';
import { PracticantesDto } from '../model/practicantes.model';
import { CrudComponent } from '../../../../shared/manager/crud.component';
import { CreateOrEditPracticantesModalComponent } from './create-or-edit-practicantes-modal.component';
import { PracticantesService } from './practicantes.service';
import { IDetailComponent, DetailComponent, DetailAgregationComponent } from '../../../../shared/manager/detail.component';
import { MatDialog, MatDialogConfig } from '@angular/material';
import { ComponentType } from '@angular/cdk/overlay/index';
import { ViewMode } from '../../../../shared/model/base.model';

@Component({

    templateUrl: "./practicantes.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class PracticantesComponent extends CrudComponent<PracticantesDto> implements AfterViewInit {


    GetEditComponentType(): Type<IDetailComponent> {
        return CreateOrEditPracticantesModalComponent
    }

    constructor(injector: Injector, protected _RolesService: PracticantesService) {
        super(_RolesService, CreateOrEditPracticantesModalComponent, injector);
        this.isFirstTime = true;
        this.title = "Practicantes"
        this.moduleName = "Siniestros";
        this.icon = "flaticon-settings";
        this.loadInMaterialPopup = true;
    }

    ngAfterViewInit() {
        super.ngAfterViewInit();
    }


    ngOnInit() {
        super.ngOnInit();


    }



    getDescription(item: PracticantesDto): string {
        return item.ApellidoNombre;
    }
    getNewItem(item: PracticantesDto): PracticantesDto {

        var item = new PracticantesDto(item);
        item.Anulado = false;
        return item;
    }

    onCreate() {

        //super.onCreate();
        var dialog = this.injector.get(MatDialog);
        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;

        let dialogRef = dialog.open(this.getIDetailComponent(), dialogConfig);
        dialogRef.componentInstance.showNew(this.getNewDto());
        dialogRef.afterClosed().subscribe(
            data => {
                if (data) {
                    this.onSearch();
                }

            }
        );

    }
    getIDetailComponent(): ComponentType<DetailComponent<PracticantesDto>> {
        return CreateOrEditPracticantesModalComponent;
    }

    getNewDto(): PracticantesDto {
        return new PracticantesDto();
    }





}