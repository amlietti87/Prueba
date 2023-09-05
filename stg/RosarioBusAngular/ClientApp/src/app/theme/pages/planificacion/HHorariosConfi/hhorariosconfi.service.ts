import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import 'rxjs/Rx';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import { environment } from '../../../../../environments/environment';
import { HHorariosConfiDto, HHorariosConfiFilter, DetalleSalidaRelevosFilter, ReporteHorarioPasajerosFilter, ReporteDistribucionCochesFilter, ReportePasajerosFilter } from '../model/hhorariosconfi.model';
import { Observable, Subscription } from 'rxjs/Rx';
import { ResponseModel } from '../../../../shared/model/base.model';
import { FileService } from '../../../../shared/common/file.service';
import { FileDTO } from '../../../../shared/common/models/fileDTO.model';


@Injectable()
export class HHorariosConfiService extends CrudService<HHorariosConfiDto> {


    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        protected fileService: FileService

    ) {
        super(http);
        this.identityUrl = environment.identityUrl + '/HHorariosConfi';
        this.endpoint = this.identityUrl;
    }


    ReporteParadasPasajeros(filtro: ReportePasajerosFilter) {
        var _url = this.identityUrl + "/ReporteParadasPasajeros";
        return this.fileService.dowloadAuthenticatedByPost(_url, filtro);

    }

    reporteDetalleSalidasYRelevos(filtro: DetalleSalidaRelevosFilter) {
        var _url = this.identityUrl + "/ReporteDetalleSalidasYRelevos";
        return this.fileService.dowloadAuthenticatedByPost(_url, filtro);

    }



    reporteHorarioPasajeros(filtro: ReporteHorarioPasajerosFilter) {
        var _url = this.identityUrl + "/ReporteHorarioPasajeros";
        return this.fileService.dowloadAuthenticatedByPost(_url, filtro);

    }


    reporteDistribucionCoches(filtro: ReporteDistribucionCochesFilter) {
        var _url = this.identityUrl + "/ReporteDistribucionCoches";
        return this.fileService.dowloadAuthenticatedByPost(_url, filtro);

    }



    deleteDuracionesServicio(IdServicio: number): any {
        let url = this.endpoint + '/DeleteDuracionesServicio?IdServicio=' + IdServicio
        return this.http.post(url, null);
    }

    updateCantidadCochesReales(hHorariosConfiDto: HHorariosConfiDto): Observable<ResponseModel<boolean>> {
        let url = this.endpoint + '/UpdateCantidadCochesReales';
        return this.http.post<ResponseModel<boolean>>(url, hHorariosConfiDto);

    }


}