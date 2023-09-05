import { Dto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class HTposHorasDto extends Dto<string> {
    getDescription(): string {
        return this.Description;
    }

    DscTpoHora: string;
    Orden: string;
} 
