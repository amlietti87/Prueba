import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { ConfigurationService } from '../../../../shared/common/services/configuration.service';

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
import { } from '../model/user.model';

import { SubCausasDto } from '../model/causas.model';

@Injectable()
export class SubCausasService extends CrudService<SubCausasDto> {

    private siniestrosUrl: string = '';
    constructor(

        protected http: HttpClient,
        private authService: AuthService,
        private configurationService: ConfigurationService) {
        super(http);
        this.siniestrosUrl = environment.siniestrosUrl + '/SubCausas';
        this.endpoint = this.siniestrosUrl;
    }


}

