import { RbMapServices } from "../rbmaps/RbMapServices";

export class RbMapGrupo {
    id: string;
    nombre: string;

    constructor() {
        this.id = RbMapServices.guid();
    }
}