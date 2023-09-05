import { Injectable } from "@angular/core";
import { CrudService } from "../../../../shared/common/services/crud.service";
import { HttpClient, HttpParams } from "@angular/common/http";
import { AuthService } from "../../../../auth/auth.service";
import { environment } from "../../../../../environments/environment";
import { FDCertificadosDto, FDCertificadosFilter } from '../model/fdcertificados.model';
import { Observable } from "rxjs";
import { ResponseModel } from "../../../../shared/model/base.model";
import { FDAccionesPermitidasFilter } from '../model/fdaccionespermitidas.model';
import { UserDto } from '../../admin/model/user.model';

@Injectable()
export class FdCertificadosService extends CrudService<FDCertificadosDto> {

    private firmaDigitalUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.firmaDigitalUrl = environment.firmaDigitalUrl + '/FdCertificados';
        this.endpoint = this.firmaDigitalUrl;
    }

    HistorialCertificadosPorUsuario(reqParams?: any): Observable<ResponseModel<any[]>> {
        var params = new HttpParams();
        if (reqParams) {
            Object.keys(reqParams).forEach(function(item) {
                params = params.set(item, reqParams[item]);
            });
        }

        let url = this.endpoint + '/HistorialCertificadosPorUsuario';
        return this.http.get<ResponseModel<any[]>>(url, { params: params });
    }

    RevocarCertificado(filter: FDAccionesPermitidasFilter): Observable<ResponseModel<any>> {
        let url = this.endpoint + '/RevocarCertificado';
        return this.http.post<ResponseModel<any>>(url, filter);
    }

    searhCertificate(filter: FDCertificadosFilter): Observable<ResponseModel<any>> {
        let url = this.endpoint + '/searhCertificate';
        return this.http.post<ResponseModel<any>>(url, filter);
    }

    downloadCertificate(filter: FDCertificadosFilter): Observable<ResponseModel<any>> {
        let url = this.endpoint + '/downloadCertificate';
        return this.http.post<ResponseModel<any>>(url, filter);
    }

    sendCertificateByEmail(userFilter: FDCertificadosFilter): Observable<ResponseModel<any>> {
        let url = this.endpoint + '/sendCertificateByEmail';
        return this.http.post<ResponseModel<any>>(url, userFilter);
    }
}