import { Dto, FilterDTO } from "../../../../shared/model/base.model";

export class FDCertificadosDto extends Dto<number> {
    getDescription(): string {
        return "";
    }


    UsuarioId: number;
    ArchivoId: string;
    FechaActivacion: Date;
    Activo: boolean;
    FechaRevocacion: Date;
    UsuarioNombre: string;
    ArchivoNombre: string;
}


export class FDCertificadosFilter extends FilterDTO {
    UsuarioId: number;
    Activo: boolean;
    ArchivoId: string;
    UserEmail: string;
}