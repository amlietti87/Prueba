import { Dto, FilterDTO, ItemDto, GroupItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class HKilometrosDto extends Dto<number> {
    getDescription(): string {
        return "";
    }
    Kmr: number;
    Km: number;
    CodBanderaColor: number;
    CodBanderaTup: number;
    CodBan: number;
    CodSec: number;
}


export class HKilometrosFilter extends FilterDTO {
    CodBan: number;
    CodSec: number;
}

