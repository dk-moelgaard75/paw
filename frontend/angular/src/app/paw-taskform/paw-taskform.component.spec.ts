import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PawTaskformComponent } from './paw-taskform.component';

describe('PawTaskformComponent', () => {
  let component: PawTaskformComponent;
  let fixture: ComponentFixture<PawTaskformComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PawTaskformComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PawTaskformComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
