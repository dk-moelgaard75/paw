import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PawHomeComponent } from './paw-home.component';

describe('PawHomeComponent', () => {
  let component: PawHomeComponent;
  let fixture: ComponentFixture<PawHomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PawHomeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PawHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
