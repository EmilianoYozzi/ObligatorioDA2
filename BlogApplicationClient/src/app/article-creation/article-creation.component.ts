import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ArticleService } from '../article.service';
import { InModelArticle } from '../_types/InModelArticle';

@Component({
  selector: 'app-article-creation',
  templateUrl: './article-creation.component.html',
  styleUrls: ['./article-creation.component.css']
})
export class ArticleCreationComponent implements OnInit {
  articleForm: FormGroup;
  imageBase64Strings: string[] = [];
  imagesLength: number = 0;

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

  constructor(private fb: FormBuilder, private articleService: ArticleService, private toastr: ToastrService) { 
    
    this.articleForm = this.fb.group({
      Title: ['', Validators.required],
      Text: ['', Validators.required],
      Visibility: ['', Validators.required],
      Images: [''],
      Template: [''],
    });
  }

  ngOnInit(): void {
  }

  onFileChange(event: any): void {
    if (event.target.files && event.target.files.length) {
      this.imagesLength = event.target.files.length;

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
    this.articleService.createArticle(article)
    .subscribe(
      () => this.toastr.success('Article created!'),
      error => this.toastr.error(error)
    );
  }

}
