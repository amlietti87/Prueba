import { Dto, FilterDTO } from "./Base/base.model";

export class LineaDto implements Dto<number>{

    Id: number;
    Description: string;
    
    getDescription(): string {
        return this.Description;
    } 
}

export class LineaFilter extends FilterDTO {    
    SucursalId: number;
}