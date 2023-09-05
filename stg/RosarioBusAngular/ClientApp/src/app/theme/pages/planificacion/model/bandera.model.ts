import { Dto, FilterDTO, ItemDto, GroupItemDto } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class BanderaDto extends Dto<number> {
    getDescription(): string {
        return this.Nombre;
    }
    AbrBan: string;
    Nombre: string;
    RamalColorNombre: string;
    RamalColorId: number;
    CodigoVarianteLinea: string;
    Descripcion: string;
    Cartel: string;
    Sentido: string;
    Ramalero: string;
    Activo: boolean;
    LineaId: number;
    LineaNombre: string;
    TipoBanderaId: number;
    tbn: string;
    SucursalId: number;
    SentidoBanderaId: number;
    Cortado: boolean;
    Origen: string;
    Destino: string;
    PorDonde: string;
    PlaCodigoSubeBandera: PlaCodigoSubeBanderaDto[];
    DescripcionPasajeros: string;

}


export class PlaCodigoSubeBanderaDto extends Dto<number>
{

    getDescription(): string {
        return this.CodigoSube;
    }

    CodBan: number;
    CodEmpresa: number;
    CodigoSube: string;
    EmpresaNombre: string;
    Descripcion: string;
    SentidoBanderaSubeId: number;

    SentidoBanderaSubeNombre: string;

}




export class BanderaFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    SucursalId: number;
    RamaColorId: number;
    CodigoVarianteLinea: string;
    Linea: ItemDto;
    TipoBanderaId: number;
    RamalesID: number[];
    CodHfecha: number;
    LineaIdRelacionadas: number;
    CodTdia: number;
    ShowDecimalValues: boolean;
    Ramal: ItemDto;
    Ramales: GroupItemDto[];
    RamalesSelect: GroupItemDto[];

    //Sabana
    BanderaRelacionadaID: number;
    LineaId: number;
    SentidoBanderaId: number;
    BanderasSeleccionadas: number[];
    Fecha: string;
    Activo: boolean;
    cod_servicio: number;
    cod_Conductor: string;
    NoDescartarPrimeryUltimoMV = false;
    ValidarMediasVueltasIncompletas: boolean;


}

