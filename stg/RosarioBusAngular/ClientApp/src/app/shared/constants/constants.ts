import { NativeDateAdapter } from "@angular/material";

export const ITEMS_PER_PAGE = 5; // define el numero de pagina para las tablas
export const SEARCH_PARAM_ATTR_NAME = 'q'; // define el atributo usado para buscar, q=valor_a_buscar
export const SEARCH_PARAM_PATH = '_search'; // define el path para los endpoint de busqueda, ej: endpoint/_search?q=valor_a_buscar
export const CURRENT_USER_KEY = 'currentUser';
export const AUTHORIZATION_HEADER = 'Authorization';
export const SESSION_TIME_LIMIT_IN_SECONDS = 120;
export const ENABLED_TRACING = false;
export const MY_DATE_FORMATS = {
    parse: {
        dateInput: { month: '2-digit', year: 'numeric', day: '2-digit' },
    },
    display: {
        dateInput: { month: '2-digit', year: 'numeric', day: '2-digit' },
        monthYearLabel: { month: '2-digit', year: 'numeric', },
        dateA11yLabel: { day: '2-digit', month: 'long', year: 'numeric' },
        monthYearA11yLabel: { month: 'long', year: 'numeric' },
    },
};
export const ESTADOS_RUTAS = {
    Borrador: 1,
    Aprobada: 2,
    Descartada: 3
};


export class CustomDateAdapter extends NativeDateAdapter {

    parse(value: any): Date | null {

        if ((typeof value === 'string') && (value.indexOf('/') > -1)) {
            const str = value.split('/');

            const year = Number(str[2]);
            const month = Number(str[1]) - 1;
            const date = Number(str[0]);

            return new Date(year, month, date);
        }
        else if ((typeof value === 'string') && (value.indexOf('.') > -1)) {
            const str = value.split('.');

            const year = Number(str[2]);
            const month = Number(str[1]) - 1;
            const date = Number(str[0]);

            return new Date(year, month, date);
        }
        const timestamp = typeof value === 'number' ? value : Date.parse(value);
        return isNaN(timestamp) ? null : new Date(timestamp);
    }

    // retirar quando for feito o merge da data por mmalerba
    format(date: Date, displayFormat: Object): string {
        date = new Date(Date.UTC(
            date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(),
            date.getMinutes(), date.getSeconds(), date.getMilliseconds()));
        displayFormat = Object.assign({}, displayFormat, { timeZone: 'utc' });


        const dtf = new Intl.DateTimeFormat(this.locale, displayFormat);
        return dtf.format(date).replace(/[\u200e\u200f]/g, '');
    }

}