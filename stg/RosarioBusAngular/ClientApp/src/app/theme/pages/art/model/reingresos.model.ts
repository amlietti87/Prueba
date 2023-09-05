import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class ReingresosDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }

    DenunciaId: number;
    FechaReingreso: Date;
    AltaMedica: boolean;
    FechaAltaMedica: Date;
    AltaLaboral: boolean;
    FechaAltaLaboral: Date;
    Observacion: string;
    CantidadDias: number;
    FechaProbableAlta: Date;


    Anulado: boolean;
}


export class ReingresosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}
