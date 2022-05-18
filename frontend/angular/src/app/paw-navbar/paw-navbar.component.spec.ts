import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PawNavbarComponent } from './paw-navbar.component';

describe('PawNavbarComponent', () => {
  let component: PawNavbarComponent;
  let fixture: ComponentFixture<PawNavbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PawNavbarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PawNavbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
