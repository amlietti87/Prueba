import { Dto, FilterDTO, ItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { HServiciosDto } from "./hServicios.model";


export class MediasVueltasDto extends Dto<number> {
    getDescription(): string {
        return "";
    }
    CodServicio: number;
    Sale: Date;
    Llega: Date;
    DifMin: number;
    Orden: number;
    CodBan: number;
    CodTpoHora: string;
    //CodMvueltabsas: number;


    SaleOriginal: Date;
    LlegaOriginal: Date;
    CodBanOriginal: number;
    CodTpoHoraOriginal: string;

    DescripcionBandera: string;
    DescripcionTpoHora: string;
    Dia: number;

    constructor(data?: any) {
        super(data);

    }


    HasChange: boolean;
    HasError: boolean;
    HasErrorLlega: boolean;
    HasErrorSale: boolean;
    ErrorMessages: string[] = [];



}


export class HMediasVueltasImportadaView {
    NumServicio: string;
    Sale: Date;
    Llega: Date;
    DifMin: number;
    CodBan: number;
    DescripcionBandera: string;
    CodTpoHora: string;
    DescripcionTpoHora: string;
    CodSubGalpon: number;
    DescripcionSubGalpon: string;
}



export class MediasVueltasFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    CodServicio: number;
    CodHfecha: number;
    CodTdia: number;
    CodSubg: number;
    CodLinea: number;
    Servicios: HServiciosDto[];
} 
