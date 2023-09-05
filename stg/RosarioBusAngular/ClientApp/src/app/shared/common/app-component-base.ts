import { Injector, HostListener } from '@angular/core';
import { LocalizationService } from './localization.service';
import { PermissionCheckerService } from './permission-checker.service';
import { NotifyService } from './notify.service';
import { NotificationService } from '../notification/notification.service';
import { SettingService } from './setting.service';
import { MessageService } from './message.service';
import { PrimengDatatableHelper } from '../helpers/PrimengDatatableHelper';
import { Subject } from 'rxjs';


export abstract class AppComponentBase {

    localization: LocalizationService;
    permission: PermissionCheckerService;
    notify: NotifyService;
    notificationService: NotificationService;
    setting: SettingService;
    message: MessageService;
    primengDatatableHelper: PrimengDatatableHelper;
    protected unsubscriber = new Subject();

    constructor(injector: Injector) {
        this.localization = injector.get(LocalizationService);
        this.permission = injector.get(PermissionCheckerService);
        this.notify = injector.get(NotifyService);
        this.setting = injector.get(SettingService);
        this.message = injector.get(MessageService);
        this.notificationService = injector.get(NotificationService);
        this.primengDatatableHelper = new PrimengDatatableHelper();
    }

    l(key: string, ...args: any[]): string {
        return this.ls("", key, args);
    }

    ls(sourcename: string, key: string, ...args: any[]): string {
        let localizedText = this.localization.localize(key, sourcename);

        if (!localizedText) {
            localizedText = key;
        }

        if (!args || !args.length) {
            return localizedText;
        }

        args[0].unshift(localizedText);

        return key;
        //abp.utils.formatString.apply(this, args[0]);
    }

    isGranted(permissionName: string): boolean {
        return this.permission.isGranted(permissionName);
    }

    isGrantedAny(...permissions: string[]): boolean {
        if (!permissions) {
            return false;
        }

        for (const permission of permissions) {
            if (this.isGranted(permission)) {
                return true;
            }
        }

        return false;
    }

    s(key: string): string {
        return "";
        //abp.setting.get(key);
    }
}


export interface ComponentCanDeactivate {
    canDeactivate(): boolean;
    confirmMessage(): string;
}
