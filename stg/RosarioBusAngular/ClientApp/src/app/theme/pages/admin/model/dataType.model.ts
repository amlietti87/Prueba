import { Dto } from "../../../../shared/model/base.model";

export class DataTypeDto extends Dto<number>{
    getDescription(): string {
        return this.Name;
    }
    Name: string;
}