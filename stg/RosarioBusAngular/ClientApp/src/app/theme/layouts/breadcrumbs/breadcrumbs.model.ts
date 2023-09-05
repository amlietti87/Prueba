

export class AppBreadcrumbs {

    title: string;
    icon: string;
    items: BreadcrumbsItem[];


    constructor(title: string, icon: string, items?: BreadcrumbsItem[]) {
        this.title = title;
        this.icon = icon;
        this.items = items || [];
    }
}


export class BreadcrumbsItem {
    name: string;
    icon: string;
    route: string;
    funtion: any;
    isLast: boolean = false;
    key: string;
    constructor(name: string, icon?: string, route?: string, key?: string, funtion?: any) {
        this.key = key;
        this.name = name;
        this.icon = icon;
        this.route = route;
        this.funtion = funtion;

    }
}

