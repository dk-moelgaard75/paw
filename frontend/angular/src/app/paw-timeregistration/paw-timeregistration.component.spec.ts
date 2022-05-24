import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PawTimeregistrationComponent } from './paw-timeregistration.component';

describe('PawTimeregistrationComponent', () => {
  let component: PawTimeregistrationComponent;
  let fixture: ComponentFixture<PawTimeregistrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PawTimeregistrationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PawTimeregistrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
