import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from '../article.service';
import { InModelArticle } from '../_types/InModelArticle';
import { OutModelArticle } from '../_types/OutModelArticle';

@Component({
  selector: 'app-article-update',
  templateUrl: './article-update.component.html',
  styleUrls: ['./article-update.component.css']
})
export class ArticleUpdateComponent implements OnInit {
  articleForm: FormGroup;
  imageBase64Strings: string[] = [];
  imagesLength: number = 0;
  articleId: string;

  mapTemplateValue(value: string): string {
    let mappedValue: string;
    switch(value) {
      case 'Top':
        mappedValue = 'ImageAtTop';
        break;
      case 'Bottom':
        mappedValue = 'ImageAtBottom';
        break;
      case 'Top-Left':
        mappedValue = 'ImageAtTopLeft';
        break;
      case 'Top and Bottom':
        mappedValue = 'ImageAtTopAndBottom';
        break;
      default:
        mappedValue = 'NoImage';
    }
    return mappedValue;
  }

  constructor(
    private fb: FormBuilder,
    private articleService: ArticleService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.articleId = this.route.snapshot.params['id'];

    this.articleForm = this.fb.group({
      Title: ['', Validators.required],
      Text: ['', Validators.required],
      Visibility: ['', Validators.required],
      Images: [''],
      Template: [''],
    });

    this.articleService.getArticleById(this.articleId).subscribe((article: OutModelArticle) => {
      this.articleForm.patchValue({
        Title: article.title,
        Text: article.text,
        Images: article.images,
        Template: this.unmapTemplateValue(article.template),
      });
      this.imageBase64Strings = article.images;
      this.imagesLength = this.imageBase64Strings.length;
    });
  }

  unmapTemplateValue(value: string): string {
    let unmappedValue: string;
    switch(value) {
      case 'ImageAtTop':
        unmappedValue = 'Top';
        break;
      case 'ImageAtBottom':
        unmappedValue = 'Bottom';
        break;
      case 'ImageAtTopLeft':
        unmappedValue = 'Top-Left';
        break;
      case 'ImageAtTopAndBottom':
        unmappedValue = 'Top and Bottom';
        break;
      default:
        unmappedValue = 'NoImage';
    }
    return unmappedValue;
  }

  ngOnInit(): void {}

  onFileChange(event: any): void {
    if (event.target.files && event.target.files.length) {
      this.imagesLength = event.target.files.length;
      this.imageBase64Strings = []; 

      for (let i = 0; i < event.target.files.length; i++) {
        const reader = new FileReader();
        
        reader.onload = () => {
          if (typeof reader.result === 'string') {
            this.imageBase64Strings.push(reader.result);
          }
        };
        
        reader.readAsDataURL(event.target.files[i]);
      }
    }
  }

  onSubmit() {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    const templateValue = this.mapTemplateValue(this.articleForm.get('Template')?.value ?? '');
    const article: InModelArticle = {
      Title: this.articleForm.get('Title')?.value ?? '',
      Text: this.articleForm.get('Text')?.value ?? '',
      Visibility: this.articleForm.get('Visibility')?.value ?? '',
      Images: this.imageBase64Strings,
      Template: templateValue,
      OwnerUsername: userInfo.username,
    };
    this.articleService.updateArticle(this.articleId, article)
    .subscribe(
      () => {
        this.toastr.success('Article updated!');
        this.router.navigate(['/article-read', this.articleId]);
      },
      error => this.toastr.error(error)
    );
  }
}
