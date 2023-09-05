import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { CrudService } from '../../../../shared/common/services/crud.service';


import { UserFilter, ResponseModel, PaginListResultDto } from '../../../../shared/model/base.model';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';


import 'rxjs';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { } from '../model/user.model';
import { ReclamosDto, ReclamosHistoricosDto, ReclamosFilter } from '../model/reclamos.model';
import { FileService } from '../../../../shared/common/file.service';
import { ReclamoImportadorFileFilter, ReclamoImportadorDTO } from '../../reclamos/reclamos/reclamos-importador/reclamos-importador.component';

@Injectable()
export class ReclamosService extends CrudService<ReclamosDto> {

    private siniestrosUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService,
        private fileService: FileService) {
        super(http);
        this.siniestrosUrl = environment.siniestrosUrl + '/Reclamos';
        this.endpoint = this.siniestrosUrl;
    }


    CambioEstado(reclamo: ReclamosDto, historico: ReclamosHistoricosDto): Observable<ReclamosDto> {
        var data = new PostReclamo;
        data.Reclamo = reclamo;
        data.Historico = historico;
        let url = this.endpoint + '/CambioEstado';
        return this.http.post<ReclamosDto>(url, data);
    }

    Anular(data: ReclamosDto): Observable<ResponseModel<ReclamosDto>> {

        let url = this.endpoint + '/Anular';
        return this.http.post<ResponseModel<ReclamosDto>>(url, data);
    }

    CheckNuevoReclamoNoNecesario(data: any): Observable<boolean> {

        let url = this.endpoint + '/CheckNuevoReclamoNoNecesario';
        return this.http.post<boolean>(url, data);
    }

    GetReporteExcel(filter: ReclamosFilter): void {
        let url = this.endpoint + '/GetReporteExcel';
        this.fileService.dowloadAuthenticatedByPost(url, filter);
    }

    UploadExcel(data: any): Observable<ResponseModel<string>> {

        let formData: FormData = new FormData();
        formData.append('file', data, data.name);
        let url = this.endpoint + '/UploadExcel';
        return this.http.post<ResponseModel<string>>(url, formData);
    }

    RecuperarPlanilla(filter: ReclamoImportadorFileFilter): Observable<ResponseModel<ReclamoImportadorDTO[]>> {

        var params = new HttpParams();
        if (filter) {
            Object.keys(filter).forEach(function(item) {
                params = params.set(item, filter[item]);
            });
        }
        let url = this.endpoint + '/RecuperarPlanilla';
        return this.http.get<ResponseModel<ReclamoImportadorDTO[]>>(url, { params: params });
    }

    ImportarReclamos(input: ReclamoImportadorFileFilter): Observable<ResponseModel<string>> {

        let url = this.endpoint + '/ImportarReclamos';
        return this.http.post<ResponseModel<string>>(url, input);
    }


}

export class PostReclamo {

    Reclamo: ReclamosDto;
    Historico: ReclamosDto;
}