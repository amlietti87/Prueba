import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../../../environments/environment';
import { DiagramasInspectoresDto, DiagramaMesAnioDto, InspDiagramaInspectoresTurnosDto, InspectorDiaDto, DiasMesDto, ValidationResult } from '../model/diagramasinspectores.model';
import { ResponseModel } from '../../../../shared/model/base.model';
import { Observable } from 'rxjs/Observable';
import { PersTurnosDto } from '../model/persturnos.model';
import { DiagramasInspectoresValidatorService } from './diagramas-inspectores-validator.service';
import { FileService } from '../../../../shared/common/file.service';

@Injectable()
export class DiagramasInspectoresService extends CrudService<DiagramasInspectoresDto> {

    notify: any;
    private planificacionUrl: string = '';
    private listModelOriginal: DiasMesDto[] = [];
    constructor(
        protected http: HttpClient,
        private fileService: FileService) {
        super(http);
        this.planificacionUrl = environment.planificacionUrl + '/InspDiagramasInspectores';
        this.endpoint = this.planificacionUrl;

    }

    getDiagramaMesAnio(id: number, turnoId: number[], blockentity: boolean): Observable<ResponseModel<DiagramaMesAnioDto>> {
        let url = this.endpoint + '/DiagramaMesAnioGrupo';
        return this.http.post<ResponseModel<DiagramaMesAnioDto>>(url, { Id: id, TurnoId: turnoId, Blockentity: blockentity });
    }


    getTurnosDeLaDiagramacion(id: number): Observable<ResponseModel<InspDiagramaInspectoresTurnosDto[]>> {
        let url = this.endpoint + '/TurnosDeLaDiagramacion?Id=' + id;
        return this.http.get<ResponseModel<InspDiagramaInspectoresTurnosDto[]>>(url);
    }

    saveDiagramacion(Inspectores: InspectorDiaDto[], Id: number, blockDate: Date): Observable<ResponseModel<string>> {
        let url = this.endpoint + '/SaveDiagramacion';
        return this.http.post<ResponseModel<string>>(url, { Inspectores: Inspectores, Id: Id, BlockDate: blockDate });
    }

    eliminarCelda(model: DiasMesDto): Observable<ResponseModel<InspectorDiaDto>> {
        let url = this.endpoint + '/EliminarCelda';
        return this.http.post<ResponseModel<InspectorDiaDto>>(url, model);
    }

    publicarDiagramacion(Diagrmacion: DiagramasInspectoresDto): Observable<ResponseModel<string>> {
        let url = this.endpoint + '/PublicarDiagramacion';
        return this.http.post<ResponseModel<string>>(url, Diagrmacion);
    }

    setDiagramaOriginal(listModelOriginal: DiasMesDto[]) {
        this.listModelOriginal = listModelOriginal;
    }

    getRowFromDiagramaOriginal(row: DiasMesDto): DiasMesDto {
        return this.listModelOriginal.find(e => e.NumeroDia == row.NumeroDia);
    }

    getImprimirDiagrama(id: number, turnoId: number[]): void {
        let url = this.endpoint + '/ImprimirDiagrama';
        this.fileService.dowloadAuthenticatedByPost(url, { Id: id, TurnoId: turnoId });
    }

}

