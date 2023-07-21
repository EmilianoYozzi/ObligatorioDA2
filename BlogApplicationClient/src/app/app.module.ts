import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { RegisterComponent } from './register/register.component';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './login/login.component';
import { FeedComponent } from './feed/feed.component';
import { ArticleCreationComponent } from './article-creation/article-creation.component';
import { ArticleReadComponent } from './article-read/article-read.component';
import { CommentNotificationComponent } from './comment-notification/comment-notification.component';
import { SearchArticleComponent } from './search-article/search-article.component';
import { UserRankingComponent } from './user-ranking/user-ranking.component';
import { OffensiveWordsComponent } from './offensive-words/offensive-words.component';
import { ImportArticleComponent } from './import-article/import-article.component';
import { NotificationsComponent } from './notifications/notifications.component';
import { CommentComponent } from './comment/comment.component';
import { UserUpdateComponent } from './user-update/user-update.component';
import { AllUsersComponent } from './all-users/all-users.component';
import { UserArticlesComponent } from './user-articles/user-articles.component';
import { ArticleUpdateComponent } from './article-update/article-update.component';


@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    FeedComponent,
    ArticleCreationComponent,
    ArticleReadComponent,
    CommentNotificationComponent,
    SearchArticleComponent,
   UserRankingComponent,
   OffensiveWordsComponent,
   ImportArticleComponent,
   NotificationsComponent,
   CommentComponent,
   UserUpdateComponent,
   AllUsersComponent,
   UserArticlesComponent,
   ArticleUpdateComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ToastrModule.forRoot({
      positionClass: 'toast-top-right',
    }),
    BrowserAnimationsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }