import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PawNoAccessComponent } from './paw-no-access.component';

describe('PawNoAccessComponent', () => {
  let component: PawNoAccessComponent;
  let fixture: ComponentFixture<PawNoAccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PawNoAccessComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PawNoAccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
