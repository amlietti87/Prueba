import { Injectable } from '@angular/core';

import { RbMapLinea } from './RbMapLinea';

@Injectable()
export class RbMapServicesLinea {
    constructor() { }

    save(linea: RbMapLinea) {
        var lineas = this.getAll();
        var found = false;

        for (let i = 0; i < lineas.length && !found; i++) {
            const element = lineas[i];
            if (element.id == linea.id) {
                element.points = linea.points;
                element.nombre = linea.nombre;
                element.diametro = linea.diametro;
                element.color = linea.color;
                element.grupoId = linea.grupoId;
                found = true;
            }
        }

        if (!found) lineas.push(linea);

        localStorage.setItem('lineas', JSON.stringify(lineas));
    }

    getAll(): RbMapLinea[] {
        var resultado = new Array() as RbMapLinea[];
        var lineas = localStorage.getItem('lineas');

        if (lineas) {
            var list = JSON.parse(lineas);
            list.forEach(element => {
                var l = this.getLinea(element);
                resultado.push(l);
            });
        }

        return resultado;
    }

    get(id: string): RbMapLinea {
        var linea;
        var lineas_ls = localStorage.getItem('lineas');

        if (lineas_ls) {
            var lineas = JSON.parse(lineas_ls) as RbMapLinea[];
            var result = lineas.filter(item => item.id == id);
            if (result && result.length > 0) linea = this.getLinea(result[0]);
        }

        return linea;
    }

    private getLinea(element: RbMapLinea) {
        var l = new RbMapLinea();
        l.color = element.color;
        l.diametro = element.diametro;
        l.grupoId = element.grupoId;
        l.id = element.id;
        l.nombre = element.nombre;
        l.points = element.points;

        return l;
    }
}