import { Dto } from "../../../../shared/model/base.model";
import * as moment from 'moment';

export class ResponsableInformesDto extends Dto<string> {
    getDescription(): string {
        return this.Description;
    }
}