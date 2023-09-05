import { Directive, ElementRef, Input } from '@angular/core';

declare let mApp: any;

@Directive({
    selector: '[busyIf]'
})
export class BusyIfDirective {

    _isBusy: boolean;
    @Input() set busyIf(isBusy: boolean) {
        this._isBusy = isBusy;
        this.refreshState(isBusy);
    }
    _busyText: string;
    @Input('busyText') set busyText(busyText: string) {

        this._busyText = busyText;
        if (this._isBusy) {
            this.refreshState(this._isBusy);
        }

    }

    constructor(private _element: ElementRef) { }

    refreshState(isBusy: boolean): void {
        if (isBusy === undefined) {
            return;
        }


        if (isBusy) {

            mApp.block($(this._element.nativeElement), {
                overlayColor: '#000000',
                opacity: 0.2,
                type: 'loader',
                state: 'primary',
                message: this._busyText || 'Cargando...'
            });

        } else {
            mApp.unblock($(this._element.nativeElement));
        }
    }
}
