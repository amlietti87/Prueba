import { CrudService } from '../../shared/common/services/crud.service';
import { TipoViajeDto } from '../../theme/pages/planificacion/model/tipoviaje.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { AuthService } from '../../auth/auth.service';
import { FileService } from '../../shared/common/file.service';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs/Observable';
import { ResponseModel, PaginListResultDto } from '../../shared/model/base.model';

@Injectable()
export class TipoViajeService extends CrudService<TipoViajeDto>{

    private identityUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService,
        private fileService: FileService
    ) {
        super(http);
        this.identityUrl = environment.identityUrl + '/PlaTipoViaje';
        this.endpoint = this.identityUrl;
    }

}