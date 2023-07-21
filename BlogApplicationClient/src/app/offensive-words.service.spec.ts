import { TestBed } from '@angular/core/testing';

import { OffensiveWordsService } from './offensive-words.service';

describe('OffensiveWordsService', () => {
  let service: OffensiveWordsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OffensiveWordsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
