import { Injectable } from '@angular/core';

@Injectable()
export class SettingService {

    get(name: string): string {
        return "";
    }

    getBoolean(name: string): boolean {
        return false;
    }

    getInt(name: string): number {
        return 0;
    }

}