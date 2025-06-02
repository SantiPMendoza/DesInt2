import { TestBed } from '@angular/core/testing';

import { GrupoclaseService } from './grupoclase.service';

describe('GrupoclaseService', () => {
  let service: GrupoclaseService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GrupoclaseService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
