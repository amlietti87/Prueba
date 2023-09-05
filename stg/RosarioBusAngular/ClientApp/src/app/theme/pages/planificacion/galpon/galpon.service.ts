import { Injectable } from '@angular/core';
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
import { DBLocalStorageService } from '../../../../shared/utils/local-storage.service';
import { GalponDto } from '../model/subgalpon.model';


@Injectable()
export class GalponService extends CrudService<GalponDto> {

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.identityUrl = environment.identityUrl + '/Galpon';
        this.endpoint = this.identityUrl;
    }


}