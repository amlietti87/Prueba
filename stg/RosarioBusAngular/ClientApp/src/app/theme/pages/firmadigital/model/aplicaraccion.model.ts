import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { FDAccionesPermitidasDto } from "./fdaccionespermitidas.model";
import { Strategy } from "ngx-permissions";


export class AplicarAccionDto {
    AccionId: number;

    Documentos: number[] = [];

    Empleador: boolean;
    Motivo: string;
    Correo: string;
    DescripcionAccion: string;
}



export class RechazarDto {

    Motivo: string;


}

