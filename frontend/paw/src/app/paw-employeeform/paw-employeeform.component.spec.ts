import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PawEmployeeformComponent } from './paw-employeeform.component';

describe('PawEmployeeformComponent', () => {
  let component: PawEmployeeformComponent;
  let fixture: ComponentFixture<PawEmployeeformComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PawEmployeeformComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PawEmployeeformComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
