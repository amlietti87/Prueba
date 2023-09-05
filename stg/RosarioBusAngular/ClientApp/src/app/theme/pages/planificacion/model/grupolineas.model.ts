import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class GrupoLineasDto extends Dto<number> {
    getDescription(): string {
        return this.Nombre;
    }
    Nombre: string;
    SucursalId: number;
    Sucursal: string;
    LineasTotales: number;
    Lineas: ItemDto[];

    constructor(data?: any) {
        super(data);

    }

}


export class GrupoLineasFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    SucursalId: number;
    Linea: ItemDto;
} 
