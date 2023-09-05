import { Dto, FilterDTO } from "../../../../shared/model/base.model";

export class RangosHorariosDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Anulado: boolean;
    EsFranco: boolean;
    EsFrancoTrabajado: boolean;
    NovedadId: number;
    HoraDesde: HoraDto;
    HoraHasta: HoraDto;
    Color: string;

    HasError: boolean;
    ErrorMessages: string;
}

export class HoraDto {
    hour: number;
    minute: number;
    second: number;
    fecha: Date;
}

export class RangosHorariosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    Anulado: number;
    GrupoInspectoresId: number;
    RangoHorarioId: number;
}