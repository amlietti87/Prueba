import { Injectable } from '@angular/core';
import { NgxPermissionsService } from 'ngx-permissions';
import { Observer } from 'rxjs/Rx';

import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';
import { LocalStorageService } from './services/storage.service';



@Injectable()
export class PermissionCheckerService {

    endpoint: string;
    private _PermissionList: string[];

    constructor(private permissionsService: NgxPermissionsService,
        private _localStorageService: LocalStorageService,
        protected http: HttpClient) {
        this._PermissionList = [];

        this.endpoint = environment.identityUrl;

    }

    isGranted(permissionName: string): boolean {

        //var p = this.permissionsService.getPermission(permissionName);

        //var result = this.permissionsService.hasPermission(permissionName).then(e => {

        //    console.log(e);
        //});

        //var r = p != null;
        return this._PermissionList.indexOf(permissionName) != -1;
    }

    loadPermissions(): void {

        var list = this._localStorageService.retrieve('permisos_usuario') as string[];
        if (list) {
            this.permissionsService.loadPermissions(list);
            this._PermissionList = [];
            for (var i in list) {
                this._PermissionList.push(list[i]);
            }
        }

    }




    setPermissions(per): void {
        //buscar en servicio         
        return this._localStorageService.store('permisos_usuario', per);
    }



    GetPermissions(): Observable<any> {
        //buscar en servicio 
        return this.http.get<any>(this.endpoint + '/api/Permission');
    }


    clearPermissions(): void {
        this._localStorageService.removeItem('permisos_usuario');
        this.permissionsService.flushPermissions();
    }
}


