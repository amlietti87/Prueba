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
//import { } from '../model/user.model';
import { DocumentosProcesadosDto, ArchivosTotalesPorEstado, DocumentosProcesadosFilter } from '../model/documentosprocesados.model';

@Injectable()
export class DocumentosProcesadosService extends CrudService<DocumentosProcesadosDto> {

    private firmaDigitalUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.firmaDigitalUrl = environment.firmaDigitalUrl + '/FdDocumentosProcesados';
        this.endpoint = this.firmaDigitalUrl;
    }

    HistorialArchivosPorEstado(reqParams?: any): Observable<ResponseModel<ArchivosTotalesPorEstado[]>> {
        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {
                params = params.set(item, reqParams[item]);
            });
        }
        let url = this.endpoint + '/HistorialArchivosPorEstado';
        return this.http.get<ResponseModel<ArchivosTotalesPorEstado[]>>(url, { params: params });
    }

    GetEmailDefault(): Observable<ResponseModel<string>> {

        let url = this.endpoint + '/GetEmailDefault';
        return this.http.get<ResponseModel<string>>(url);
    }

    ImportarDocumentos(): any {
        let url = this.endpoint + '/ImportarDocumentos';
        return this.http.post<any>(url, null);
    }

    ValidarEmails(emails: string): Observable<ResponseModel<string>> {

        let url = this.endpoint + '/ValidarEmails?emails=' + emails;
        return this.http.get<ResponseModel<string>>(url);
    }
}

