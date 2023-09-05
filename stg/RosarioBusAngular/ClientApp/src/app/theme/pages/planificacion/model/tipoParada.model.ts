import { Dto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class TipoParadaDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }
    Nombre: string;
    Abreviatura: string;

    TiempoEsperadoDeCarga: TiempoEsperadoDeCargaDto[];
}

export class TiempoEsperadoDeCargaDto extends Dto<number> {

    getDescription(): string {
        return "";
    }

    Nombre: string;
    TipodeDiaId: number
    HoraDesde: string;
    HoraHasta: string;
    TiempoDeCarga: string;
    TipoParadaId: number;
    TipoDiaNombre: string;
}
