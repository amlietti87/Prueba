import { Component, Injector, OnInit, Inject } from '@angular/core';
declare let mApp: any;
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { HMediasVueltasImportadaView } from '../../model/mediasvueltas.model';
import { AppComponentBase } from '../../../../../shared/common/app-component-base';
import { PlaDistribucionDeCochesPorTipoDeDiaService } from '../HFechasConfi.service';
import { Router } from '@angular/router';


@Component({
    selector: 'mvueltas-view',
    templateUrl: './mvueltas-view.component.html',

})
export class MvueltasViewComponent extends AppComponentBase implements OnInit {


    items: HMediasVueltasImportadaView[];

    constructor(
        protected dialogRef: MatDialogRef<MvueltasViewComponent>,
        injector: Injector,
        protected _service: PlaDistribucionDeCochesPorTipoDeDiaService,
        @Inject(MAT_DIALOG_DATA) public data: HMediasVueltasImportadaView[],
        protected router: Router,

    ) {
        super(injector);
        this.items = data;
    }

    close() {
        this.dialogRef.close(false);
    }

    ngOnInit(): void {
    }

    save() {
        this.dialogRef.close(true);
    }



    OnMinutosPorSector(row: HMediasVueltasImportadaView) {
        //this.router.navigate(['/planificacion/MinutosPorSector', row]);
    }




}
