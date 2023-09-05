import { ItemDto, Dto } from "../../../../shared/model/base.model";
import { HHorariosConfiDto } from './hhorariosconfi.model';


export class HChoxser extends Dto<number> {
    getDescription(): string {
        return "";
    }


    CodUni: number;
    CodServicio: number;
    Sale: Date;
    Llega: Date;
    CodEmp: string;
    TipoMultiple: number;

}


export class HChoxserExtendedDto extends HChoxser {
    DescripcionServicio: string;
    SaleRelevo: Date;
    SaleAuxiliar: Date;
    Llega: Date;
    LlegaRelevo: Date;
    LlegaAuxiliar: Date;


    Duracion: number;
    DuracionRelevo: number;
    DuracionAuxiliar: number;

    canEditSale: boolean;
    canEditSaleR: boolean;
    canEditSaleA: boolean;
    canEditLlega: boolean;
    canEditLlegaR: boolean;
    canEditLlegaA: boolean;


    isRequiredSale: boolean = false;
    isRequiredSaleR: boolean = false;
    isRequiredSaleA: boolean = false;
    isRequiredLlega: boolean = false;
    isRequiredLlegaR: boolean = false;
    isRequiredLlegaA: boolean = false;

    HasChange: boolean;
    HasError: boolean;
    HasErrorLlega: boolean;
    HasErrorSale: boolean;
    HasErrorLlegaR: boolean;
    HasErrorSaleR: boolean;
    HasErrorLlegaA: boolean;
    HasErrorSaleA: boolean;
    ErrorMessages: string[];

}

export class HChoxserFilter {
    CodHfecha: number;
    CodSubg: number;
    CodTdia: number;

    PlanillaId: string;

}


export class ImportadorDuracionResult {
    List: ChofXServImportado[] = [];

}

export class ChofXServImportado {
    servicio: string
    sale: string;
    saleRelevo: string;
    saleAuxiliar: string;
    llega: string;
    llegaRelevo: string;
    llegaAuxiliar: string;


    duracion: number;
    duracionRelevo: number;
    duracionAuxiliar: number;

    Errors: any[];
    IsValid: boolean;

}

export class HorarioDuracion {
    Duracion: HChoxserExtendedDto;
    Horario: HHorariosConfiDto;
}
