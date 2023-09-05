import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { EmpresaDto } from "../../planificacion/model/empresa.model";


export class CochesDto extends Dto<string> {
    getDescription(): string {
        return this.Dominio;
    }
    Ficha: number;
    Interno: string;
    FecIng: DateTimeFormat;
    Dominio: string;
    Anio: number;
    NroChasis: string;
    NroMotor: string;
    Marca: string;
    FecHab: DateTimeFormat;
    NroHab: string;
    Kilometraje: number;
    CodGruTar: number;
    CodEmpr: number;
    Carroceria: string;
    Titular: string;
    Proveedor: string;
    CodTpoAsiento: number;
    CantAsientos: number;
    AireAcondicionado: string;
    Cortinas: string;
    Gps: string;
    Visible: string;
    AsientosHab: number;
    RampaDiscapacitados: string;
    InternoSap: string;
    InternoAnterior: string;
    CodEmpresaSube: number;
    NroInternoBa: string;

    Empresa: EmpresaDto;
}



export class CochesFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}


