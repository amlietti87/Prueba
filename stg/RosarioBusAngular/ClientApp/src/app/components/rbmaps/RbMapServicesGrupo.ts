import { Injectable } from '@angular/core';

import { RbMapGrupo } from '../rbmaps/RbMapGrupo';

@Injectable()
export class RbMapServicesGrupo {
    constructor() { }

    save(grupos: RbMapGrupo[]) {
        localStorage.setItem('grupos', JSON.stringify(grupos));
    }

    getAll(): RbMapGrupo[] {
        var grupos = localStorage.getItem('grupos');
        if (!grupos) return new Array() as RbMapGrupo[];
        return JSON.parse(grupos);
    }
}