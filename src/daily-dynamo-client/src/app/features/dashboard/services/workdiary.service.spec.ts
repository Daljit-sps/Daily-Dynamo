import { TestBed } from '@angular/core/testing';

import { WorkdiaryService } from './workdiary.service';

describe('WorkdiaryService', () => {
  let service: WorkdiaryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WorkdiaryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
