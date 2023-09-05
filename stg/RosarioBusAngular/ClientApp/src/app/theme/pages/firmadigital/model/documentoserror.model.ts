import { Dto, FilterDTO } from "../../../../shared/model/base.model";
import * as moment from 'moment';
import { FDAccionesDto } from "./fdacciones.model";
import { FDEstadosDto } from "./fdestados.model";


export class DocumentosErrorDto extends Dto<number> {
    getDescription(): string {
        return null;
    }

    NombreArchivo: string;
    TipoDocumentoId: number;
    EmpleadoId: number;
    SucursalEmpleadoId: number;
    EmpresaEmpleadoId: number;
    LegajoEmpleado: string;
    Cuilempleado: string;
    Fecha: Date;
    FechaProcesado: Date;
    DetalleError: string;
    Revisado: boolean;
    Errors: string[];
    File: string;

    TipoDocumentoDescripcion: string;
    EmpresaDescripcion: string;
    SucursalDescripcion: string;
    NombreEmpleado: string;

}


export class DocumentosErrorFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;

    //revisarerrores component
    FechaDesde: Date;
    FechaHasta: Date;
    Revisado: number;
    AccionId: number;
}
