import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PawCustomerformComponent } from './paw-customerform.component';

describe('PawCustomerformComponent', () => {
  let component: PawCustomerformComponent;
  let fixture: ComponentFixture<PawCustomerformComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PawCustomerformComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PawCustomerformComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
