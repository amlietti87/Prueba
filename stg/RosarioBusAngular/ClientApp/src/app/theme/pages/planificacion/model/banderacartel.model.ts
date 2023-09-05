import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';





export class BanderaCartelDto extends Dto<number> {
    getDescription(): string {
        return "";
    }

    CodHfecha: number;
    CodLinea: number;

    BolBanderasCartelDetalle: BanderasCartelDetalleDto[];

}

export class BanderasCartelDetalleDto extends Dto<number> {
    getDescription(): string {
        return this.AbrBan;
    }

    CodBanderaCartel: number;
    CodBan: number;
    NroSecuencia: number;
    TextoBandera: string;
    Movible: string;
    ObsBandera: string;
    AbrBan: string;
    EsPosicionamiento: boolean;

}

export class BanderaCartelFilter extends FilterDTO {
    PlanillaId: string;
    CodHfecha: number;

}
export class PlaSentidoBanderaSubeDto extends Dto<number>{
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
}
