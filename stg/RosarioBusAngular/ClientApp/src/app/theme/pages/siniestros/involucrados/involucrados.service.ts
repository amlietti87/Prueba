import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService } from '../../../../shared/common/services/crud.service';


import { UserFilter, ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';


import 'rxjs';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { } from '../model/user.model';


import { InvolucradosDto, HistorialInvolucrados } from '../model/involucrados.model';

@Injectable()
export class InvolucradosService extends CrudService<InvolucradosDto> {

    private siniestrosUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.siniestrosUrl = environment.siniestrosUrl + '/Involucrados';
        this.endpoint = this.siniestrosUrl;
    }

    HistorialSiniestros(TipoDocId: number, NroDoc: string): Observable<ResponseModel<HistorialInvolucrados>> {

        let url = this.endpoint + '/HistorialSiniestros?TipoDocId=' + TipoDocId + '&NroDoc=' + NroDoc;
        return this.http.get<ResponseModel<HistorialInvolucrados>>(url);
    }


}

