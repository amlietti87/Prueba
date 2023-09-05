import { Pipe, PipeTransform } from '@angular/core';
import { RbMapLinea } from './RbMapLinea';

@Pipe({
    name: 'lineaPorGrupo',
    pure: false
})
export class RbMapLineaPorGrupoFilter implements PipeTransform {
    transform(items: RbMapLinea[], filter: string): any {
        if (!items || !filter) {
            return new Array() as RbMapLinea[];
        }
        // filter items array, items which match and return true will be
        // kept, false will be filtered out
        return items.filter(item => item.grupoId == filter);
    }
}