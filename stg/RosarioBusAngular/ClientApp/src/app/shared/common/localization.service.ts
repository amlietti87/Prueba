import { Injectable } from '@angular/core';

@Injectable()
export class LocalizationService {

    get languages(): string[] {
        return ["es", "en"]
    }

    get currentLanguage(): string {
        return "";
    }

    localize(key: string, sourceName: string): string {
        return key;
    }

    //getSource(sourceName: string): (key: string) => string {
    //    return "";
    //}

}