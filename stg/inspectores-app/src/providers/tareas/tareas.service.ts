import { TareasDto, TareasRealizadasDto } from './../../models/tareas.model';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CrudService } from '../../providers/service/crud.service';
import { environment as ENV} from "@app/env";
import { Observable } from 'rxjs';
import { ResponseModel, PaginListResultDto } from 'models/Base/base.model';

@Injectable()
export class TareasService extends CrudService<TareasDto> {

  private identityUrl: string = '';
  constructor(
      protected http: HttpClient) {
      super(http);
      this.identityUrl = ENV.identityUrl + '/Tarea';
      this.endpoint = this.identityUrl;
  }

  GetPagedList(reqParams?: any): Observable<ResponseModel<PaginListResultDto<TareasDto>>> {
    var url= this.endpoint + '/GetPagedList';     
    return this.http.post<ResponseModel<PaginListResultDto<TareasDto>>>(url, reqParams );
  }

}



@Injectable()
export class TareasRealizadasService extends CrudService<TareasRealizadasDto> {

  private identityUrl: string = '';
  constructor(
      protected http: HttpClient) {
      super(http);
      this.identityUrl = ENV.identityUrl + '/InspTareasRealizadas';
      this.endpoint = this.identityUrl;
  }

}