import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { ActivatedRoute } from '@angular/router';
import { OutModelArticle } from '../_types/OutModelArticle';

@Component({
  selector: 'app-user-articles',
  templateUrl: './user-articles.component.html',
  styleUrls: ['./user-articles.component.css']
})
export class UserArticlesComponent implements OnInit {
  username: string;
  articles: OutModelArticle[] = [];

  constructor(
    private userService: UserService,
    private route: ActivatedRoute
  ) { 
    this.username = '';
  }

  ngOnInit(): void {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    this.username = userInfo.username;
    
    this.userService.getUserArticles(this.username).subscribe(articles => {
      this.articles = articles;
    });
  }
  
}
