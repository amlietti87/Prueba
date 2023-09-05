import { Component, Input, Type, ViewChild, ChangeDetectionStrategy, ChangeDetectorRef, DebugElement, Injector, Output, EventEmitter, ViewContainerRef, ComponentFactoryResolver } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forEach } from '@angular/router/src/utils/collection';
import { NgModel } from '@angular/forms';
import { DataTable } from 'primeng/primeng';
import { PlaDistribucionDeCochesPorTipoDeDiaDto, HFechasConfiDto } from '../../model/HFechasConfi.model';
import { AgregationListComponent } from '../../../../../shared/manager/agregationlist.component';

import { MediasVueltasService } from '../../mediasvueltas/mediasvueltas.service';
import { MediasVueltasFilter, HMediasVueltasImportadaView } from '../../model/mediasvueltas.model';
import { MatDialogConfig, MatDialog } from '@angular/material';
import { MvueltasViewComponent } from '../mvueltas/mvueltas-view.component';
import { Observable } from 'rxjs';
import { DistribucionDeCochesDetailComponent } from './distribuciondecoches-detail.component';
import { PlaDistribucionDeCochesPorTipoDeDiaService } from '../HFechasConfi.service';
declare let mApp: any;



@Component({
    selector: 'distribuciondecoches-agregation',
    templateUrl: './distribuciondecoches-agregation.html'
})
export class DistribucionDeCochesAgregation extends AgregationListComponent<PlaDistribucionDeCochesPorTipoDeDiaDto> {


    @Output() ImportarServicioEvent: EventEmitter<PlaDistribucionDeCochesPorTipoDeDiaDto> = new EventEmitter<PlaDistribucionDeCochesPorTipoDeDiaDto>();

    @ViewChild('dt') dt: DataTable;

    @ViewChild('importarhorariofecha', { read: ViewContainerRef }) importarhorariofecha: ViewContainerRef;
    allowImportarHorario: boolean = false;
    @Input() parentEntity: HFechasConfiDto;
    constructor(
        protected _service: PlaDistribucionDeCochesPorTipoDeDiaService,
        protected _mvueltasService: MediasVueltasService,
        protected injector: Injector,
        protected cfr: ComponentFactoryResolver

    ) {
        super(DistribucionDeCochesDetailComponent, injector);

        //this.popupWidth = "800px";
        //this.popupHeight = "";

    }


    ngOnInit() {
        super.ngOnInit();

        this.listChange.subscribe(s => {
            this.LeerDistribucionesIncompletas();
        });


    }


    SetAllowPermission() {
        //this.allowAdd = this.permission.isGranted(key + '.Agregar');
        //this.allowDelete = this.permission.isGranted(key + '.Eliminar');
        //this.allowModify = this.permission.isGranted(key + '.Modificar');
        this.allowAdd = true;
        this.allowDelete = true;
        this.allowModify = true;
        this.allowImportarHorario = this.permission.isGranted('Horarios.FechaHorario.ImportarHorario');
    }

    LeerDistribucionesIncompletas(): void {

        this.list.forEach(f => {
            this.ExistenMediasVueltasIncompletas(f);
        });

    }


    ExistenMediasVueltasIncompletas(row: PlaDistribucionDeCochesPorTipoDeDiaDto) {
        this._service.ExistenMediasVueltasIncompletas(row).subscribe(r => {
            row.Estado = r.DataObject;
        });
    }

    LeerMediasVueltasIncompletas(row: PlaDistribucionDeCochesPorTipoDeDiaDto) {

        var f = new MediasVueltasFilter();
        f.CodHfecha = row.CodHfecha;
        f.CodTdia = row.CodTdia;

        mApp.blockPage();
        this._mvueltasService.LeerMediasVueltasIncompletas(f).subscribe(r => {
            mApp.unblockPage();
            this.openLineaDialog(r.DataObject, row);
        });

    }



    openLineaDialog(view: HMediasVueltasImportadaView[], row: PlaDistribucionDeCochesPorTipoDeDiaDto) {
        var dialog: MatDialog;
        dialog = this.injector.get(MatDialog);

        const dialogConfig = new MatDialogConfig();
        dialogConfig.disableClose = false;
        dialogConfig.autoFocus = true;
        dialogConfig.data = view;

        let dialogRef = dialog.open(MvueltasViewComponent, dialogConfig);

        dialogRef.afterClosed().subscribe(
            data => {
                if (data) {

                    mApp.blockPage();
                    this._service.RecrearSabanaSector(row).subscribe(r => {
                        mApp.unblockPage();
                        this.ExistenMediasVueltasIncompletas(row);
                    });
                }
            }
        );
    }



    //GetEditComponentType(): Type<IDetailComponent> {
    //    return InvolucradoDetailComponent
    //}


    onImportarServicio(row: PlaDistribucionDeCochesPorTipoDeDiaDto) {

        this.ImportarServicioEvent.emit(row);


    }




    getNewItem(item: any): PlaDistribucionDeCochesPorTipoDeDiaDto {
        item = new PlaDistribucionDeCochesPorTipoDeDiaDto(item);
        item.IsNew = true;
        return item;
    }


    ValidateDelete(item: PlaDistribucionDeCochesPorTipoDeDiaDto): Observable<boolean> {
        var selft = this;

        return Observable.create(observer => {

            this._service.TieneMinutosAsignados(item.Id).subscribe(tieneMinutos => {
                if (tieneMinutos.DataObject) {
                    selft.message.error("No se puede eliminar ya que tiene minutos asignados.", "Error");
                    observer.next(false);
                    observer.complete();
                }
                else {
                    observer.next(true);
                    observer.complete();

                }

            })

        });




    }

    Opendialog(_detail) {
        super.Opendialog(_detail);
        var detailC = this.detailElement as DistribucionDeCochesDetailComponent;

        detailC.parentEntity = this.parentEntity;
    }

}