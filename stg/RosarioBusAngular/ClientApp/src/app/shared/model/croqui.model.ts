import { FilterDTO, Dto } from "./base.model";

export class CroquisDto extends Dto<number> {
    getDescription(): string {
        return this.Description;
    }

    Svg: string;
    TipoId: number;
    SiniestroId: number;
}


export class CrosquisFilter extends FilterDTO {
    Id: number;
    Page: number;
    PageSize: number;
    Sort: String;
}
