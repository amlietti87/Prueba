import { Dto, FilterDTO } from "./Base/base.model";

export class HFechasDto implements Dto<number>{

    Id: number;
    Description: string;
    CodTdia: number;
    
    getDescription(): string {
        return this.Description;
    } 

}

export class HFechasFilter extends FilterDTO {    
    LineaId: number;
    Fecha: string;
}