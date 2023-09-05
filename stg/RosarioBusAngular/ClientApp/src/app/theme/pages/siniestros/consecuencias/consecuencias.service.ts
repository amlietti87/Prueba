import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService } from '../../../../shared/common/services/crud.service';


import { UserFilter, ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';


import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { } from '../model/user.model';

import { ConsecuenciasDto } from '../model/consecuencias.model';

@Injectable()
export class ConsecuenciasService extends CrudService<ConsecuenciasDto> {

    private siniestrosUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.siniestrosUrl = environment.siniestrosUrl + '/Consecuencias';
        this.endpoint = this.siniestrosUrl;
    }

    GetConsecuenciasSinAnular(): Observable<ResponseModel<ConsecuenciasDto[]>> {
        let url = this.endpoint + '/GetConsecuenciasSinAnular';
        return this.http.get<ResponseModel<ConsecuenciasDto[]>>(url);
    }


}

