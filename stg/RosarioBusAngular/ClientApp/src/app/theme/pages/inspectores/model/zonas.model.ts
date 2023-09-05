import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class ZonasDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;
    Detalle: string;
    Anulado: boolean;
}


export class ZonasFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    Anulado: number;
    GrupoInspectoresId: number;
}