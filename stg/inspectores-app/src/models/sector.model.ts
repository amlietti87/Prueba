import { Dto, FilterDTO } from "./Base/base.model";

export class SectorDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }
    Id: number;
    Descripcion: string;
    
}

export class HDesignarFilter extends FilterDTO {    
    Fecha: string;
    SectorId: number;
}

