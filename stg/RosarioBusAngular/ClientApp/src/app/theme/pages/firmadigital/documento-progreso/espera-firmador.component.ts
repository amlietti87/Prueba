declare let mApp: any;
import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, Type, ChangeDetectorRef, Inject } from '@angular/core';
import { environment } from '../../../../../environments/environment';
import { AplicarAccionDto, RechazarDto } from '../model/aplicaraccion.model';
import { MatDialogConfig, MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { AppComponentBase } from '../../../../shared/common/app-component-base';
import { SignalRService } from '../../../../services/signalr.service';
import { Subscriber, Subscription } from 'rxjs';
import { AplicarAccioneResponseDto } from '../model/documentosprocesados.model';

@Component({

    templateUrl: "./espera-firmador.component.html",
    encapsulation: ViewEncapsulation.None,

})



export class EsperaFirmadorComponent extends AppComponentBase implements AfterViewInit, OnInit {

    progressValue: number = 0;
    progressCount: number = 0;
    progressTotal: number = 0;
    header: string;
    Subs: Subscription[] = [];

    response: AplicarAccioneResponseDto;

    constructor(injector: Injector,
        public dialogRef: MatDialogRef<EsperaFirmadorComponent>,
        @Inject(MAT_DIALOG_DATA) public data: AplicarAccionDto,
        private _SignalRService: SignalRService
    ) {
        super(injector);
        this.header = data.DescripcionAccion;
        this.progressTotal = data.Documentos.length;
    }


    ngAfterViewInit() {

    }

    ngOnInit() {
        this.InitializeSignalR();
    }

    InitializeSignalR() {
        this._SignalRService.joinGroup("Firmador_JNLPProgress");

        let s = this._SignalRService.OnMessageRecibed.subscribe(e => {
            if (e) {

                if (e.groupname == "Firmador_JNLPProgress" && e.message == this.response.FirmadorId) {
                    this.progressCount = this.progressCount + 1;
                    this.progressValue = (this.progressCount / this.progressTotal) * 100;
                    if (this.progressTotal <= this.progressCount) {
                        this.Subs.forEach(s => s.unsubscribe());
                        this.dialogRef.close(true);
                    }
                }
            }



        });
        this.Subs.push(s);
    }

    close(): void {
        this.Subs.forEach(s => s.unsubscribe());
        this.dialogRef.close(true);
    }

    Next() {
        this.progressCount = this.progressCount + 1;
        this.progressValue = (this.progressCount / this.progressTotal) * 100;
    }

}