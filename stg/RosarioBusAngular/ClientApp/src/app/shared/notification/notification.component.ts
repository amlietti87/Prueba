import { Component, OnInit, OnDestroy } from '@angular/core';

import { NotificationService, Notification, NotificationType } from './notification.service';
import 'rxjs/add/operator/takeWhile';

declare var $: any

@Component({
    selector: 'app-notification',
    templateUrl: './notification.component.html'
})

export class NotificationComponent implements OnInit, OnDestroy {
    alerts: Notification[] = [];
    private alive: boolean = true;

    constructor(
        private alertService: NotificationService
    ) { }


    ngOnInit() {
        this.alertService.getNotification()
            .takeWhile(() => this.alive)
            .subscribe((alert: Notification) => {

                if (!alert) {
                    // clear alerts when an empty alert is received
                    this.alerts = [];
                    return;
                }

                // add alert to array
                this.alerts.push(alert);
                //inicia la notificacion
                this.notifyStart(alert);

                // remove alert after 5 seconds
                setTimeout(() => this.removeAlert(alert), 5000);
            });

    }


    removeAlert(alert: Notification) {
        //remueve la alerta del pool de alertas
        this.alerts = this.alerts.filter(x => x !== alert);
        //termina con la notificacion de la alerta
        this.notifyStop(alert);
    }

    type(alert: Notification) {
        if (!alert) {
            return;
        }

        // return css class based on alert type
        switch (alert.type) {
            case NotificationType.Success:
                return 'success';
            case NotificationType.Error:
                return 'danger';
            case NotificationType.Info:
                return 'info';
            case NotificationType.Warning:
                return 'warning';
            case NotificationType.Brand:
                return 'brand';
            case NotificationType.Primary:
                return 'primary';
        }
    }

    icon(alert: Notification) {
        if (!alert) {
            return;
        }

        // return css class based on alert type
        switch (alert.type) {
            case NotificationType.Success:
                return 'icon la la-check';
            case NotificationType.Error:
                return 'icon la la-warning';
            case NotificationType.Info:
                return '';
            case NotificationType.Warning:
                return 'icon flaticon-exclamation-2';
            case NotificationType.Brand:
                return '';
            case NotificationType.Primary:
                return '';
        }
    }

    notifyStart(alert: Notification) {

        var type = this.type(alert);
        if (alert.type == NotificationType.Success) {
            var placementFrom = 'top';
            var placementAlign = 'right';
        } else if (alert.type == NotificationType.Error) {
            var placementFrom = 'top';
            var placementAlign = 'right';
        }
        var message = alert.message;
        var animateEnter;
        var animateExit;
        //llamo a la construccion de la notificacion
        this.showNotification(alert, type, message, placementFrom, placementAlign, animateEnter, animateExit);

    };

    showNotification(alert: Notification, type, text, placementFrom, placementAlign, animateEnter, animateExit) {
        if (type === null || type === '') { type = 'bg-black'; }
        if (text === null || text === '') { text = 'Turning standard Bootstrap alerts '; }
        if (animateEnter === null || animateEnter === '') { animateEnter = 'animated fadeInDown'; }
        if (animateExit === null || animateExit === '') { animateExit = 'animated fadeOutUp'; }
        var allowDismiss = true;
        //incio la notificacion

        var _icon = this.icon(alert);
        var options = {
            type: type,
            allow_dismiss: allowDismiss,
            newest_on_top: true,
            mouse_over: true,

            timer: 0,
            delay: 0,
            z_index: 999999,
            placement: {
                from: placementFrom,
                align: placementAlign
            },
            animate: {
                enter: animateEnter,
                exit: animateExit
            }
        };

        var notify = $.notify({
            message: text,
            icon: _icon,
            title: alert.title || ''
        }, options);

        //asigno la notificacion creada a la propiedad notifyElement de la alerta
        alert.notifyElement = notify;
    }


    notifyStop(alert: Notification) {
        //termino la notificacion
        alert.notifyElement.close();
    }

    ngOnDestroy() {
        this.alive = false;
    }

}
