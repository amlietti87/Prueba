import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Helpers } from '../../../helpers';
import { environment } from '../../../../environments/environment';


@Component({
    selector: "app-footer",
    templateUrl: "./footer.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class FooterComponent implements OnInit {


    version: string;

    constructor() {
        this.version = environment.version;
    }
    ngOnInit() {

    }

}