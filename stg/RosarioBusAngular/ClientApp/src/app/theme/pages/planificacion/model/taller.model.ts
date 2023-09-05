import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { PuntoDto } from "./punto.model";
import { RutaDto } from "./ruta.model";


export class TallerDto extends Dto<string> {
    isNew: boolean;
    getDescription(): string {
        return this.Nombre;
    }
    Nombre: string;
    SucursalId: number;
    Lat: number;
    Long: number;
    Rutas: RutaDto[];
    BanderasAEliminar: number[];
    PosGal: string;

    constructor(data?: any) {
        super(data);
    }
}

//export class TallerPuntoDto extends Dto<string> {
//    getDescription(): string {
//        return '';
//    }
//    taller: TallerDto;
//    puntoTaller: PuntoDto;
//    ruta: RutaDto;



//}


export class TallerFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    SucursalId: number;
    Lat: number;
    Long: number;
    Nombre: string;
} 
