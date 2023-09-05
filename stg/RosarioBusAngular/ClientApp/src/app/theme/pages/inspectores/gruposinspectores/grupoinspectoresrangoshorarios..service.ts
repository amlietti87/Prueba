import { InspGrupoInspectoresRangosHorariosDto } from '../model/gruposinspectores.model';
import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import 'rxjs/Rx';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import { AuthService } from '../../../../auth/auth.service';
import { environment } from '../../../../../environments/environment';

@Injectable()
export class GrupoInspectoresRangosHorariosService extends CrudService<InspGrupoInspectoresRangosHorariosDto> {

    private planificacionUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.planificacionUrl = environment.planificacionUrl + '/InspGrupoInspectoresRangosHorarios';
        this.endpoint = this.planificacionUrl;
    }


}

