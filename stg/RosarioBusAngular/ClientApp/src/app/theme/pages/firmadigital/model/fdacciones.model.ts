import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { FDAccionesPermitidasDto } from "./fdaccionespermitidas.model";


export class FDAccionesDto extends Dto<number> {
    getDescription(): string {
        return "";
    }

    TipoDocumentoId: number;
    AccionPermitidaId: number;
    EstadoActualId: number;
    EstadoNuevoId: number;
    MostrarBdempleador: boolean;
    MostrarBdempleado: boolean;
    AccionBdempleador: boolean;
    EsFin: boolean;
    GeneraNotificacion: boolean;
    NotificacionId: number;

    PermiteMarcarLote: boolean;
    MenorFechaPrimero: boolean;

    AccionPermitida: FDAccionesPermitidasDto;
}


export class FDAccionesFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}
