import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PawEmployeeComponent } from './paw-employeeform.component';

describe('PawEmployeeformComponent', () => {
  let component: PawEmployeeComponent;
  let fixture: ComponentFixture<PawEmployeeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PawEmployeeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PawEmployeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
