import { Dto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class HBanderasColoresDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }

    DscBanderaColor: string;

}
