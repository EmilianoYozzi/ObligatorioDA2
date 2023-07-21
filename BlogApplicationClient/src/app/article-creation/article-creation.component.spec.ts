import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ArticleCreationComponent } from './article-creation.component';

describe('ArticleCreationComponent', () => {
  let component: ArticleCreationComponent;
  let fixture: ComponentFixture<ArticleCreationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ArticleCreationComponent]
    });
    fixture = TestBed.createComponent(ArticleCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
