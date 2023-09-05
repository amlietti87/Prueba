/* SystemJS module definition */
declare var module: NodeModule;
interface NodeModule {
    id: string;
}
interface JQuery {
    mMenu(options: any): JQuery;
    animateClass(options: any): JQuery;
    setActiveItem(item: any): JQuery;
    getPageTitle(item: any): JQuery;
    getBreadcrumbs(item: any): JQuery;
    validate(options: any): JQuery;
    valid(): JQuery;
    resetForm(): JQuery;
    markdown(): JQuery;
    jstree(...args: any[]): any;
}
interface JQuery {
    selectpicker(...any): any;
    mDatatable(...any): any;
}

interface JQuery {
    daterangepicker(...any): any;
}



interface JQuery {
    datepicker(...any): any;
}

interface JQuery {
    datetimepicker(...any): any;
}

//metronic
interface JQuery {
    mPortlet(...any): any;
}