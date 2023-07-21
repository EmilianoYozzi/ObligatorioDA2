import { Component } from '@angular/core';
import { OnInit } from '@angular/core';
import { ArticleService } from '../article.service';
import { ToastrService } from 'ngx-toastr';
import { OutModelArticle } from '../_types/OutModelArticle';


@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css']
})
export class FeedComponent implements OnInit {
  articles: OutModelArticle[] = [];

  constructor(private articleService: ArticleService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.articleService.getArticles().subscribe(
      (data: OutModelArticle[]) => {
        console.log('Fetched articles:', data);
        this.articles = data;
        console.log('First article data:', this.articles[0])
        console.log('First article title:', this.articles[0].title);
        console.log('First article text:', this.articles[0].text);
      },
      error => {
        console.error('Error fetching articles:', error);
        this.toastr.error(error);
      }
    );
  }
  
}
