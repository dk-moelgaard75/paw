import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PawLogonComponent } from './paw-logon.component';

describe('PawLogonComponent', () => {
  let component: PawLogonComponent;
  let fixture: ComponentFixture<PawLogonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PawLogonComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PawLogonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
