import { Dto, FilterDTO } from "./Base/base.model";

export class TipoLineaDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }
    Nombre: string
    CantidadConductoresPorServicio: number
}

export class HDesignarFilter extends FilterDTO {    
    Fecha: string;
    SectorId: number;
    SentidoId: number;
}