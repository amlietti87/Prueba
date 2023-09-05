import { Injectable } from '@angular/core';
import { Response, ResponseContentType } from '@angular/http';
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


import { SiniestrosDto, SiniestroHistorialEmpleado, SiniestrosFilter } from '../model/siniestro.model';
import { LineaDto } from '../../planificacion/model/linea.model';

@Injectable()
export class SiniestroService extends CrudService<SiniestrosDto> {

    private siniestrosUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.siniestrosUrl = environment.siniestrosUrl + '/Siniestros';
        this.endpoint = this.siniestrosUrl;
    }

    GetHistorialEmpPract(empleado: boolean, id: number): Observable<ResponseModel<SiniestroHistorialEmpleado>> {

        let url = this.endpoint + '/GetHistorialEmpPract?empleado=' + empleado + '&id=' + id;
        return this.http.get<ResponseModel<SiniestroHistorialEmpleado>>(url);
    }

    GetNroSiniestroMax(): Observable<ResponseModel<number>> {

        let url = this.endpoint + '/GetNroSiniestroMax';
        return this.http.get<ResponseModel<number>>(url);
    }

    GenerateReport(data: SiniestrosDto): any {

        let url = this.endpoint + '/GenerateReport';
        return this.http.post(url, data, { responseType: 'blob' });

    }


    GenerateReportById(data: number): any {

        let url = this.endpoint + '/GenerateReportById?Id=' + data;
        return this.http.post(url, data, { responseType: 'blob' });

    }

    search(filter: SiniestrosFilter): Observable<ResponseModel<PaginListResultDto<SiniestrosDto>>> {
        let url = this.endpoint + '/GetPagedList';
        let data = filter;

        return this.http.post<ResponseModel<PaginListResultDto<SiniestrosDto>>>(url, data);
    }

}

