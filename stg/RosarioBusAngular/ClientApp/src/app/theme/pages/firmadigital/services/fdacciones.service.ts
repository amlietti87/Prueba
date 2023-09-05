import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService } from '../../../../shared/common/services/crud.service';


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
import { FDAccionesPermitidasDto } from '../model/fdaccionespermitidas.model';
import { FDAccionesDto } from '../model/fdacciones.model';
import { AplicarAccionDto } from '../model/aplicaraccion.model';

@Injectable()
export class FdAccionesService extends CrudService<FDAccionesDto> {

    private firmaDigitalUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.firmaDigitalUrl = environment.firmaDigitalUrl + '/FdAcciones';
        this.endpoint = this.firmaDigitalUrl;
    }

    AplicarAccion(dto: AplicarAccionDto): Observable<ResponseModel<any>> {

        let url = this.endpoint + '/AplicarAccion';
        return this.http.post<ResponseModel<any>>(url, dto);
    }

}

