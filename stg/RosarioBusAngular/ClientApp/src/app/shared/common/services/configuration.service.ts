import { Injectable } from '@angular/core';
import { Response, RequestOptionsArgs, RequestMethod, Headers } from '@angular/http';
import { IConfiguration } from '../models/configuration.model';
import { StorageService, LocalStorageService } from './storage.service';

import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import { Subject } from 'rxjs/Subject';
import { CacheCrudService } from './crud.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../environments/environment';
import { DBLocalStorageService } from '../../utils/local-storage.service';
import { LocatorService } from './locator.service';



@Injectable()
export class ConfigurationService {

    private identityUrl: string = '';
    constructor(
        private http: HttpClient,
        private _localStorageService: LocalStorageService
    ) {

        this.identityUrl = environment.identityUrl + '/api/Configurations';
    }

    requestAllByFilter(reqParams?: any): Observable<any[]> {

        return this.http.get<any[]>(this.identityUrl, );
    }



    loadConfigurations() {
        var list = this._localStorageService.retrieve('Configurations');

        if (list) {
            return list;
        }
        else {
            this.requestAllByFilter().toPromise().then(r => {
                this.SetCache(r);
                return r;
            });
        }


    }

    getConfiguration(): any {
        return this._localStorageService.retrieve('Configurations');
    }



    SetCache(res: any[]): void {
        return this._localStorageService.store('Configurations', res);
    }

}
