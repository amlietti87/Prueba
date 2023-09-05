import { Dto } from "./Base/base.model";

export class MotivoDto implements Dto<number>{

    Id: number;
    Description: string;
    
    getDescription(): string {
        return this.Description;
    } 
}