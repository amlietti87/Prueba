import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RbmapsComponent } from './rbmaps.component';

describe('RbmapsComponent', () => {
    let component: RbmapsComponent;
    let fixture: ComponentFixture<RbmapsComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [RbmapsComponent]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(RbmapsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
