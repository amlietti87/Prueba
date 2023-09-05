
import * as moment from 'moment';
import { Dto, FilterDTO } from '../../../../../shared/model/base.model';


export class DashboardDto extends Dto<number> {
    getDescription(): string {
        return this.Descripcion;
    }
    Descripcion: string;

    TipoDashboardId: number;
    Selected: boolean = false;

}


export class DashboardFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;

}


export class UsuarioDashboardItemDto extends Dto<number> {
    getDescription(): string {
        return "";
    }

    DashboardId: number;
    CodUsuario: number;
    Columna: number;
    Orden: number;


    TipoDashboardId: number;

}

export class UsuarioDashboardInput extends Dto<number> {
    getDescription(): string {
        return "";
    }

    Items: UsuarioDashboardItemDto[] = [];
    DashboardLayoutId: number

}

