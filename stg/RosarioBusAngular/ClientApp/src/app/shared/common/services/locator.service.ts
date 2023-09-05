import { Injectable, Injector, ReflectiveInjector } from '@angular/core';

export class LocatorService {
    //public static injector: Injector;

    public static injector: Injector = ReflectiveInjector.resolveAndCreate([]);

    public static getInstance(obj: Object) {
        return LocatorService.injector.get(obj);
    }
} 