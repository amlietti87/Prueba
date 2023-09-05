import { Injectable } from '@angular/core';
import { Response, ResponseContentType } from '@angular/http';
import { CrudService } from '../../../../shared/common/services/crud.service';


import { UserFilter, ResponseModel, PaginListResultDto, ItemDto } from '../../../../shared/model/base.model';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';


import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/throw';
import { Observer } from 'rxjs/Observer';
import 'rxjs/add/operator/map';
import * as moment from 'moment';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';
import { DenunciasDto, ExcelDenunciasFilter, DenunciasFilter } from '../model/denuncias.model';
import { HistorialDenunciasDto } from '../model/HistorialDenuncias';
import { HistorialReclamosEmpleadoDto, HistorialDenunciasEmpleadoDto } from '../model/HistorialReclamosEmpleado';
import { FileService } from '../../../../shared/common/file.service';
import { DenunciaImportadorDTO, DenunciaImportadorFileFilter } from './denuncias-importador/denuncias-importador.component';


@Injectable()
export class DenunciasService extends CrudService<DenunciasDto> {

    private artUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService,
        private fileService: FileService
    ) {
        super(http);
        this.artUrl = environment.artUrl + '/Denuncias';
        this.endpoint = this.artUrl;
    }

    HistorialDenunciaPorPrestador(EmpleadoId: number): Observable<ResponseModel<HistorialDenunciasDto[]>> {

        let url = this.endpoint + '/HistorialDenunciaPorPrestador?EmpleadoId=' + EmpleadoId;
        return this.http.get<ResponseModel<HistorialDenunciasDto[]>>(url);
    }
    HistorialDenunciasPorEstado(EmpleadoId: number): Observable<ResponseModel<HistorialDenunciasEmpleadoDto[]>> {

        let url = this.endpoint + '/HistorialDenunciasPorEstado?EmpleadoId=' + EmpleadoId;
        return this.http.get<ResponseModel<HistorialDenunciasEmpleadoDto[]>>(url);
    }
    HistorialReclamosEmpleado(EmpleadoId: number): Observable<ResponseModel<HistorialReclamosEmpleadoDto[]>> {

        let url = this.endpoint + '/HistorialReclamosEmpleado?EmpleadoId=' + EmpleadoId;
        return this.http.get<ResponseModel<HistorialReclamosEmpleadoDto[]>>(url);
    }

    Anular(data: DenunciasDto): Observable<ResponseModel<DenunciasDto>> {

        let url = this.endpoint + '/Anular';
        return this.http.post<ResponseModel<DenunciasDto>>(url, data);
    }

    GetReporteExcel(filter: DenunciasFilter): void {
        let url = this.endpoint + '/GetReporteExcel';
        this.fileService.dowloadAuthenticatedByPost(url, filter);
    }

    UploadExcel(data: any): Observable<ResponseModel<string>> {

        let testData: FormData = new FormData();
        testData.append('file', data, data.name);
        let url = this.endpoint + '/UploadExcel';
        return this.http.post<ResponseModel<string>>(url, testData);
    }


    GenerateReport(data: DenunciasDto): any {

        let url = this.endpoint + '/GenerateReport';
        return this.http.post(url, data, { responseType: 'blob' });

    }

    ImportarDenuncias(input: DenunciaImportadorFileFilter): Observable<ResponseModel<string>> {

        let url = this.endpoint + '/ImportarDenuncias';
        return this.http.post<ResponseModel<string>>(url, input);
    }

    RecuperarPlanilla(filter: DenunciaImportadorFileFilter): Observable<ResponseModel<DenunciaImportadorDTO[]>> {

        var params = new HttpParams();
        if (filter) {
            Object.keys(filter).forEach(function(item) {
                params = params.set(item, filter[item]);
            });
        }
        let url = this.endpoint + '/RecuperarPlanilla';
        return this.http.get<ResponseModel<DenunciaImportadorDTO[]>>(url, { params: params });
    }
}

