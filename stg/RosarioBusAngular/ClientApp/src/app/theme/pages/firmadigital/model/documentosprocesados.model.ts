import { Dto, FilterDTO, ItemDto } from '../../../../shared/model/base.model';
import * as moment from 'moment';
import { FDAccionesDto } from "./fdacciones.model";
import { FDEstadosDto } from "./fdestados.model";
import { FileDTO } from '../../../../shared/common/models/fileDTO.model';


export class DocumentosProcesadosDto extends ItemDto {
    getDescription(): string {
        return null;
    }

    TipoDocumentoId: number;
    EmpleadoId: number;
    SucursalEmpleadoId: number;
    EmpresaEmpleadoId: number;
    LegajoEmpleado: string;
    Cuilempleado: string;
    Fecha: Date;
    FechaProcesado: Date;
    ArchivoId: number;
    EstadoId: number;
    //CodMinisterio: string;
    Rechazado: boolean;
    MotivoRechazo: string;
    Cerrado: boolean;
    EmpleadoConforme: boolean
    Mes: string;
    Anio: string;
    NombreEmpleado: string;
    TipoDocumentoDescripcion: string;
    EmpresaDescripcion: string;
    SucursalDescripcion: string;

    AccionesDisponibles: FDAccionesDto[];
    EstadosConHistorico: FDEstadosDto[];

    //custom prop
    PermiteRechazo: boolean = false;
}


export class DocumentosProcesadosFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;

    //Bandejas Empleado y Empleador
    SucursalId: number;
    EmpresaId: number;
    FechaDesde: Date;
    FechaHasta: Date;
    TipoDocumentoId: number;
    EmpleadoId: number;
    EstadoId: number;
    Rechazado: number;
    Cerrado: number;
    EsEmpleador: boolean;
    AccionId: number;
}

export class ArchivosTotalesPorEstado {
    Estado: string;
    Cantidad: number;
}

export class VisorArchivos {
    NombreEmpleado: string;
    Fecha: string;
    TipoDocumento: string;
    Archivo: string;
}


export class AplicarAccioneResponseDto {
    IsValid: boolean;
    Detalles: DetalleResponse[] = [];
    FileDto: FileDTO;
    FirmadorId: number;
}

export class DetalleResponse {
    Documento: string;
    Error: string;
    IsValid: boolean;
}

export class VerDetalle {

    NombreEmpleado: string;
    TipoDocumento: string;
    Fecha: string;
    Archivo: string;
    ArchivoNombre: string;
    CuilEmpleado: string;
    Empresa: string;
    Legajo: string;
    FechaProcesado: string;
    Estado: string;
    Cerrado: boolean;
    Rechazado: boolean;
    MotivoRechazo: string;
    UNegocio: string;
    EmpleadoConforme: boolean;
    Historicos: HistoricosDocumentos[] = [];
}

export class HistoricosDocumentos {
    EstadoH: string;
    FechaHoraString: string;
    RechazadoH: boolean;
    Usuario: string;
}


