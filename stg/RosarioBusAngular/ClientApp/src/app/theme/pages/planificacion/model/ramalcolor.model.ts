import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';


export class RamalColorDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }
    Nombre: string;
    NombreLinea: string;
    NombreUN: string;
    Activo: boolean;
    SucursalId: number;
    LineaId: number;
    ColorTupid: number;
    RamalSube: RamalSubeDto[];
    PlaTipoLineaId: number;

}


export class RamalSubeDto extends Dto<number>
{

    getDescription(): string {
        return this.CodigoSube;
    }

    RamalColorId: number;
    EmpresaId: number;
    CodigoSube: string;
    EmpresaNombre: string;


}



export class RamalColorFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
    LineaId: number;
}
