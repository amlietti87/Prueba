import { FilterDTO, Dto, ItemDto } from "../../../../shared/model/base.model";
import { HTposHorasDto } from './htposhoras.model';
import { TipoDiaDto } from './tipoDia.model';
import { SelectItem } from 'primeng/api';





export class HMinxtipoDto extends Dto<number> {
    getDescription(): string {
        return "";
    }
    CodHfecha: number;
    CodTdia: number;
    CodBan: number;
    TipoHora: string;
    TipoHoraDesc: string;
    TotalMin: number;
    Suma: number;
    Dif: number;
    CodMinxtipobsas: number;

    HDetaminxtipo: HDetaminxtipoDto[] = [];
    HasError: boolean;
    ErrorMessages: string[] = [];

}


export class HDetaminxtipoDto extends ItemDto {
    getDescription(): string {
        return "";
    }
    CodMinxtipo: number;
    Minuto: number;
    MinutoOriginal: number;
    CodHsector: number;
    HasError: boolean;
    Orden: number;
    indexInput: number;

    /*
      public int CodMinxtipo { get; set; }
        public decimal? Minuto { get; set; }
        public int CodHsector { get; set; }
        */
}

export class HMinxtipoFilter extends FilterDTO {

    CodHfecha: number;
    CodTdia: number;
    CodBan: number;
    TipoHora: string;
    TipoDeHora: any;
    PlanillaId: string;
    BanderasId: number[];

}


export class CopiarHMinxtipoInput extends FilterDTO {

    LineaId: number;
    CodHfechaOrigen: number;
    CodTdiaOrigen: number;
    TipoHoraOrigen: string;
    BanderasId: number[];

    CodHfechaDestino: number;
    CodTdiaDestino: number;
    TipoHoraDestino: string;
    FechaDestino: string;
}







export class MinutosPorSectorDto extends ItemDto {
    getDescription(): string {
        return "";
    }
    Id: number;


    Minutos: HMinxtipoDto[];
    Sectores: HSectoresDto[];

}

export class HSectoresDto extends ItemDto {
    getDescription(): string {
        return "";
    }

    CodSec: number;
    CodHsector: number;
    CodSectorTarifario: number;
    Orden: number;
    VerEnResumen: boolean;
    LlegaA: boolean
    SaleDe: boolean
    Calle1: string;
    Calle2: string;
    DescripcionSectorTarifario: string;
    VerEnResumenOriginal: boolean;

}




export class ImportadorHMinxtipoResult {
    HMinxtipoImportados: HMinxtipoImportado[] = [];
    Sectores: SectorImportador[] = [];
}

export class SectorImportador {
    Orden: number;
    Descripcion: string;
}

export class HMinxtipoImportado {
    CodTdia: number;
    DescripcionTdia: string;
    CodBan: number;
    AbrBan: string;
    TipoHora: number;
    DescripcionTipoHora: string;
    TotalMin: number;
    Detalles: HDetaminxtipoImportado[];
    Errors: any[];
    IsValid: boolean;
}

export class HDetaminxtipoImportado {
    Minuto: number;
    CodHsector: number;
    DescripcionCodHsector: string;
}


