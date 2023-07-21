import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { ArticleCreationComponent } from './article-creation/article-creation.component';
import { AuthenticationGuard } from './guards/authentication.guard';
import { ArticleReadComponent } from './article-read/article-read.component';
import { FeedComponent } from './feed/feed.component';
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

const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent},
  {path: 'articleCreation',canActivate:[AuthenticationGuard], component: ArticleCreationComponent},
  { path: 'article-read/:id',canActivate:[AuthenticationGuard], component: ArticleReadComponent },
  {path: 'comment/:id',canActivate:[AuthenticationGuard], component: CommentComponent},
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  {path: 'feed',canActivate:[AuthenticationGuard], component: FeedComponent},
  {path: 'search',canActivate:[AuthenticationGuard], component: SearchArticleComponent},
  {path: 'user-ranking',canActivate:[AuthenticationGuard], component: UserRankingComponent},
  {path: 'offensive-words',canActivate:[AuthenticationGuard], component: OffensiveWordsComponent},
  {path: 'import-article',canActivate:[AuthenticationGuard], component: ImportArticleComponent},
  {path: 'notifications',canActivate:[AuthenticationGuard], component: NotificationsComponent},
  {path: 'user-update/:username', component: UserUpdateComponent },
  {path: 'all-users',canActivate:[AuthenticationGuard], component: AllUsersComponent},
  {path: 'user-articles',canActivate:[AuthenticationGuard], component: UserArticlesComponent},
  {path: 'article-update/:id',canActivate:[AuthenticationGuard], component: ArticleUpdateComponent}

  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

