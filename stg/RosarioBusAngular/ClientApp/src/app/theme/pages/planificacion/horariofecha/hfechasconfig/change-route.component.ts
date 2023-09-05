import { Component, OnInit, Inject, Injector } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { filter } from 'rxjs/operator/filter';
import { RutaService } from '../../ruta/ruta.service';
import { RutasFilteredFilter } from '../../model/linea.model';
import { RutaDto } from '../../model/ruta.model';
import { HFechasConfiService } from '../HFechasConfi.service';
import { HBasecDto } from '../../model/HFechasConfi.model';


@Component({
    selector: 'app-change-route',
    templateUrl: './change-route.component.html',
})
export class ChangeRouteComponent extends AppComponentBase implements OnInit {

    filter: RutasFilteredFilter;
    routes: RutaDto[]
    selectedroute: RutaDto;
    HbasecNew: HBasecDto;
    loadingroutes: boolean = true;
    saving: boolean = false;
    constructor(protected dialogRef: MatDialogRef<ChangeRouteComponent>,
        @Inject(MAT_DIALOG_DATA) public data: RutasFilteredFilter,
        protected _RutaService: RutaService,
        protected _HFechasConfiService: HFechasConfiService,
        injector: Injector) {
        super(injector)
        this.filter = data
    }

    ngOnInit() {
        this._RutaService.GetRutasFiltradas(this.filter).subscribe(data => {
            this.routes = data.DataObject;
            this.loadingroutes = false;
        });
    }

    close(): void {
        this.dialogRef.close(true);
    }

    save(): void {
        this.saving = true;
        this.HbasecNew = new HBasecDto();
        this.HbasecNew.CodBan = this.selectedroute.BanderaId;
        this.HbasecNew.CodRec = this.selectedroute.Id;
        this.HbasecNew.CodHfecha = this.filter.CodHFecha;
        this._HFechasConfiService.UpdateHBasec(this.HbasecNew).subscribe(data => {
            if (data.DataObject != null) {
                this.message.info("La ruta para la bandera seleccionada fue correctamente actualizada", 'Ruta de Bandera Actualizada');
                this.close();
            }
            else {
                this.message.error("La ruta para la bandera seleccionada no pudo ser actulizada", 'Cambio de ruta');
            }
        })
    }

    RutaSeleccionada(RowData: RutaDto): void {
        this.selectedroute = RowData;
        console.log(this.selectedroute)

    }

}
