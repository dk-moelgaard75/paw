import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PawTasklistComponent } from './paw-tasklist.component';

describe('PawTasklistComponent', () => {
  let component: PawTasklistComponent;
  let fixture: ComponentFixture<PawTasklistComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PawTasklistComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PawTasklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
