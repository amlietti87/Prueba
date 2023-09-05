import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class TiposReclamoDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Involucrado: boolean;
    Anulado: boolean;
}


export class TiposReclamoFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}

export enum TiposReclamoBase {
    ART = 1,
    Siniestro = 2,
    Laboral = 3
}