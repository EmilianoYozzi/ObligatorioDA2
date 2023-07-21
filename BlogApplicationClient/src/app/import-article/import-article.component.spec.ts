import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportArticleComponent } from './import-article.component';

describe('ImportArticleComponent', () => {
  let component: ImportArticleComponent;
  let fixture: ComponentFixture<ImportArticleComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ImportArticleComponent]
    });
    fixture = TestBed.createComponent(ImportArticleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
