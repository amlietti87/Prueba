﻿import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService, CacheCrudService } from '../../../../shared/common/services/crud.service';


import { UserFilter, ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';


import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { SucursalDto } from '../model/sucursal.model';
import { DBLocalStorageService } from '../../../../shared/utils/local-storage.service';
import { SubGalponDto } from '../model/subgalpon.model';

@Injectable()
export class SubGalponService extends CacheCrudService<SubGalponDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService, private _storage: DBLocalStorageService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/SubGalpon';
        this.endpoint = this.identityUrl;
    }


}
@Injectable()
export class SubGalponSinCacheService extends CrudService<SubGalponDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/SubGalpon';
        this.endpoint = this.identityUrl;
    }


}