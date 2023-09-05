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

import { EmpleadosDto } from '../model/empleado.model';

@Injectable()
export class EmpleadosService extends CrudService<EmpleadosDto> {

    private siniestrosUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.siniestrosUrl = environment.siniestrosUrl + '/Empleados';
        this.endpoint = this.siniestrosUrl;
    }


    ExisteEmpleado(Id: number): Observable<ResponseModel<boolean>> {

        let url = this.endpoint + '/ExisteEmpleado?id=' + Id;
        return this.http.get<ResponseModel<boolean>>(url);
    }
    ExisteLegajoEmpleado(Id: number): Observable<ResponseModel<boolean>> {

        let url = this.endpoint + '/ExisteLegajoEmpleado?id=' + Id;
        return this.http.get<ResponseModel<boolean>>(url);
    }
}

