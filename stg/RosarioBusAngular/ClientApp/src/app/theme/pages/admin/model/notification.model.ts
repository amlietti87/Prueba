﻿import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class NotificationDto extends Dto<number> {
    getDescription(): string {
        return this.DisplayName;
    }
    Name: string
    DisplayName: string;
    IsDefault: boolean;
    IsStatic: boolean;
}


export class NotificationFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}