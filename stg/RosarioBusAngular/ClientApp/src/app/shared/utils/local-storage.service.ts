import { Injectable } from '@angular/core';
import * as localForage from 'localforage';

@Injectable()
export class DBLocalStorageService {

    getItem<T>(key: string, callback: any): Promise<T> {
        if (!localForage) {
            return;
        }

        return localForage.getItem(key, callback);
    }

    //setItem<T>(key: string, value: T, callback?: (err: any, value: T) => void): Promise<T>;
    setItem<T>(key, value): Promise<T> {
        if (!localForage) {
            return;
        }

        if (value === null) {
            value = undefined;
        }

        return localForage.setItem(key, value);
    }

    removeItem(key): void {

        localForage.removeItem(key);
    }

    clear(): void {

        localForage.clear();
    }
}
