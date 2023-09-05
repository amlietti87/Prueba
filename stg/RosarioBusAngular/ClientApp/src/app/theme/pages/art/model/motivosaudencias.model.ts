import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class MotivosAudienciasDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Anulado: boolean;
}


export class MotivosAudienciasFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}
