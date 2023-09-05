import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
    selector: ".m-wrapper",
    templateUrl: "./not-found.component.html",
    encapsulation: ViewEncapsulation.None,
})
export class NotFoundComponent implements OnInit {

    constructor() {
    }

    ngOnInit() {
    }
}


@Component({
    selector: ".m-wrapper",
    template: `<span class="m-error_title">
        <h1>
				Oops!
    </h1>
    </span>`,
    encapsulation: ViewEncapsulation.None,
})
export class NotFoundComponentChild implements OnInit {

    constructor() {

    }

    ngOnInit() {
    }
}