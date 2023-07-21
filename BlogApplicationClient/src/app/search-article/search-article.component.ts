import { Component, OnInit } from '@angular/core';
import { SearchService } from '../search.service';
import { ToastrService } from 'ngx-toastr';
import { OutModelArticle } from '../_types/OutModelArticle';

@Component({
  selector: 'app-search-article',
  templateUrl: './search-article.component.html',
  styleUrls: ['./search-article.component.css']
})
export class SearchArticleComponent implements OnInit {
  articles: OutModelArticle[] = [];
  query: string = '';

  constructor(private searchService: SearchService, private toastr: ToastrService) { }

  ngOnInit(): void {}

  search(): void {
    this.searchService.searchArticles(this.query).subscribe(
      (data: OutModelArticle[]) => {
        console.log('Fetched articles:', data);
        this.articles = data;
      },
      error => {
        console.error('Error fetching articles:', error);
        this.toastr.error(error);
      }
    );
  }
}
