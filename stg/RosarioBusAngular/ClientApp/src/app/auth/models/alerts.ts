export class Alert {
    type: AlertType;
    message: string;
    notifyElement: any;
}

export enum AlertType {
    Success,
    Error,
    Info,
    Warning
}

