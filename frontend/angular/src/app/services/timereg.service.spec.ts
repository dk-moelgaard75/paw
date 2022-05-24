import { TestBed } from '@angular/core/testing';

import { TimeregService } from './timereg.service';

describe('TimeregService', () => {
  let service: TimeregService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TimeregService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
