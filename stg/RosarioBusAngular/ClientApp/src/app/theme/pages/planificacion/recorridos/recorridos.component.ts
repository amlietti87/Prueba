import { Component, OnInit, ViewEncapsulation, AfterViewInit, Injector, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AppComponentBase } from "../../../../shared/common/app-component-base";
import { ScriptLoaderService } from "../../../../services/script-loader.service";
import { UserService } from "../../../../auth/services/user.service";

@Component({
    selector: 'app-recorridos',
    templateUrl: "./recorridos.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class RecorridosComponent extends AppComponentBase implements OnInit {

    constructor(injector: Injector,
        private _script: ScriptLoaderService,
        private _activatedRoute: ActivatedRoute,
        private _userService: UserService) {
        super(injector);

    }

    ngOnInit() {

    }

}
