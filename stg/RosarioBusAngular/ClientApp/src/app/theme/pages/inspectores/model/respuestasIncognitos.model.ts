import { Dto, FilterDTO } from "../../../../shared/model/base.model";

export class RespuestasIncognitosDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }

    Descripcion: string;
    Anulado: boolean;
}

export class InspRespuestasIncognitosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    Anulado: number;
}