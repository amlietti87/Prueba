import { Dto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { RutaDto } from "./ruta.model";


export class SucursalDto extends Dto<number> {
    getDescription(): string {
        return this.DscSucursal;
    }
    DscSucursal: string

    NomServidor: string

    EntornoActivo: string

    Rutas: RutaDto[]
}


export class RolFilter {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}