import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild, ElementRef, Injectable } from '@angular/core';
import { Helpers } from '../../../helpers';
import { ScriptLoaderService } from '../../../services/script-loader.service';
import { AppComponentBase } from '../../../shared/common/app-component-base';
import { Observable } from 'rxjs/Rx';

import * as moment from 'moment';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

import { NgbDatepickerI18n } from '@ng-bootstrap/ng-bootstrap';
import { CustomDatepickerI18n } from '../../../shared/helpers/CustomDatepickerI18n';

import { ActivatedRoute, Router } from '@angular/router';


@Component({
    selector: ".m-grid__item.m-grid__item--fluid.m-wrapper",
    templateUrl: "./admin-default.component.html",
    encapsulation: ViewEncapsulation.None
})
export class AdminDefaultComponent extends AppComponentBase implements OnInit, AfterViewInit {

    @ViewChild('SampleDatePicker') sampleDatePicker: ElementRef;
    @ViewChild('SampleDateTimePicker') sampleDateTimePicker: ElementRef;
    public datepickerModel: NgbDateStruct;

    dateRangePickerStartDate: moment.Moment;
    dateRangePickerEndDate: moment.Moment;
    bsValue = new Date();
    date2: Date;
    date3: Date;
    constructor(injector: Injector, private _script: ScriptLoaderService, private _router: Router) {
        super(injector);
    }



    ngOnInit() {
        this.dateRangePickerStartDate = moment().add(-7, 'days').endOf('day');
        this.dateRangePickerEndDate = moment().startOf('day');
        this.date2 = new Date();
        this.date3 = new Date();



    }
    ngAfterViewInit() {
        // this._script.load('.m-grid__item.m-grid__item--fluid.m-wrapper',
        //     'assets/app/js/dashboard.js');



        // default date picker
        $(this.sampleDatePicker.nativeElement).datetimepicker({
            minView: 2,
            startView: 2,
            todayHighlight: true,
            autoclose: true,
            pickerPosition: 'bottom-left',
            todayBtn: true,
            format: 'dd/mm/yyyy',
            locale: 'es'
        });

        // default date time picker
        $(this.sampleDateTimePicker.nativeElement).datepicker({
            locale: 'es',
            //format: 'L LT'
        });

    }
    isshowalgo = false;

    testn(tipo: number): void {
        if (tipo == 1) {
            this.notificationService.warn("pepe", "grilllo", true);
        }
        if (tipo == 2) {
            this.notificationService.success("pepe", "grilllo", true);
        }
        if (tipo == 3) {
            this.notificationService.error("pepe", "grilllo", true);
        }
        if (tipo == 4) {
            this.notificationService.info("pepe");
        }
        if (tipo == 5) {
            this.notificationService.brand("pepe", "grilllo", true);
        }
        if (tipo == 6) {
            this.notificationService.primary("pepe", "grilllo", true);
        }
    }

    testtoast(tipo: number): void {
        if (tipo == 1) {
            this.notify.warn("pepe", "grillo");
        }
        if (tipo == 2) {
            this.notify.success("pepe", "grillo");
        }
        if (tipo == 3) {
            this.notify.error("pepe", "grillo");
        }
        if (tipo == 4) {
            this.notify.info("pepe", "grillo");
        }
    }


    testalert(tipo: number): void {
        if (tipo == 1) {
            this.message.warn("pepe", "grillo");
        }
        if (tipo == 2) {
            this.message.success("pepe", "grillo");
        }
        if (tipo == 3) {
            this.message.error("pepe", "grillo");
        }
        if (tipo == 4) {
            this.message.info("pepe", "grillo");
        }
    }
    messageService(tipo: number): void {
        if (tipo == 1) {
            this.message.warn("pepe", "grillo");
        }
        if (tipo == 2) {
            this.message.success("pepe", "grillo");
        }
        if (tipo == 3) {
            this.message.error("pepe", "grillo");
        }
        if (tipo == 4) {
            this.message.info("pepe", "grillo");
        }
    }


    showalgo(): void {


        //this._router.navigate(['admin/linea', { id: 4 }]);
        // this._router.navigate(['admin/linea', { unidaddenegocioid: 4 }]);

        this._router.navigate(['admin/linea', { id: 0, empresaid: 4 }]);

        //this.message.warn("pepe", "grillo");

        //this.isshowalgo = !this.isshowalgo;
        //this.message.confirm("Eliminar datos", "borar usuario", (a) => {

        //    //this.isshowalgo = !this.isshowalgo;
        //    if (a.value) {
        //        this.notify.info("true");
        //    }
        //    else {
        //        this.notify.success("false");
        //    }
        //});



        ////Observable.interval(10000)
        ////    .takeWhile(() => !this.isshowalgo)
        ////    .subscribe(i => {
        ////        // This will be called every 10 seconds until `stopCondition` flag is set to true
        ////    })

        //let fakeResponse = [1, 2, 3];
        //let delayedObservable = Observable.of(fakeResponse).delay(1000);
        //delayedObservable.subscribe(data => {
        //    this.notify.success("un", "error");
        //    this.isshowalgo = !this.isshowalgo;
        //});
    }

    isshog = false;
    showg(): void {

        this.isshog = !this.isshog;
        let fakeResponse = [1, 2, 3];
        let delayedObservable = Observable.of(fakeResponse).delay(1000);
        delayedObservable.subscribe(data => {
            this.notify.error("un", "error");
            this.isshog = !this.isshog;
        });
    }

}