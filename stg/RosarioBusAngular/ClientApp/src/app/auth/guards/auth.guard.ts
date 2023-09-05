import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { UserService } from "../services/user.service";
import { Observable, BehaviorSubject, Subscription } from "rxjs/Rx";
import { switchMap, map, skip } from "rxjs/operators";
import { timer } from "rxjs/observable/timer";
import { of } from "rxjs/observable/of";

@Injectable()
export class AuthGuard implements CanActivate {

    public onSessionEnd$: BehaviorSubject<boolean> = new BehaviorSubject(null); // Subscribe to this in order to be notified when sessionTimeout has expired.
    public onSessionTimeoutExtend$: BehaviorSubject<any> = new BehaviorSubject(null); // Subscribe to this in order to get notified when sessionTimeout has been extended.

    constructor(private _router: Router) {
        this.onSessionEnd$
            .subscribe((e) => {
                if (e) {
                    let currentUser = JSON.parse(localStorage.getItem('currentUser'));
                    if (currentUser && currentUser.sessionTimeout != 0) {
                        localStorage.setItem('currentUser', null);
                        this._router.navigate(['/logout']);
                    }
                }
            }
            );


        this.onSessionTimeoutExtend$.pipe(switchMap((e) => timer(e)))
            .subscribe((e) => this.onSessionEnd$.next(true));
    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | boolean {
        let currentUser = JSON.parse(localStorage.getItem('currentUser'));

        if (currentUser != null && currentUser.token != null && new Date(currentUser.expires) > new Date()) {
            if (currentUser.sessionTimeout) {
                this.onSessionTimeoutExtend$.next(currentUser.sessionTimeout); // Como tiene permiso, extiende el sessionTimeout
            }


            return true;
        }

        this.onSessionEnd$.next(false); // Si no tiene permiso para ingresar a alguna ruta, le cierra la sesion
        this._router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }

}