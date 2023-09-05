import { Dto, FilterDTO } from "./Base/base.model";

export class SentidoBanderaDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }
    Id: number;
    Descripcion: string;
    
}

export class PlaSentidoBanderaFilter extends FilterDTO {    
    Fecha: string;
    SectorId: number;
}

