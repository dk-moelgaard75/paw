import { TestBed } from '@angular/core/testing';

import { TaskobjService } from './taskobj.service';

describe('TaskobjService', () => {
  let service: TaskobjService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TaskobjService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
