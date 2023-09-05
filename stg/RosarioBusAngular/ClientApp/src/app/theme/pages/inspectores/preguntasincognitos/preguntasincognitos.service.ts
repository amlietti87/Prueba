import { PreguntasIncognitosDto } from './../model/preguntasincognitos.model';
import { Injectable } from '@angular/core';
import { CrudService } from '../../../../shared/common/services/crud.service';
import { HttpClient } from '@angular/common/http';
import 'rxjs/Rx';
import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/map';
import { environment } from '../../../../../environments/environment';
import { AuthService } from '../../../../auth/auth.service';

@Injectable()
export class PreguntasIncognitosService extends CrudService<PreguntasIncognitosDto> {

    private planificacionUrl: string = '';
    constructor(
        protected http: HttpClient,
        private authService: AuthService) {
        super(http);
        this.planificacionUrl = environment.planificacionUrl + '/InspPreguntasIncognitos';
        this.endpoint = this.planificacionUrl;
    }
}

