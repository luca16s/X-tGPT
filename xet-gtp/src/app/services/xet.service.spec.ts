import { TestBed } from '@angular/core/testing';

import { XetService } from './xet.service';

describe('XetService', () => {
  let service: XetService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(XetService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
