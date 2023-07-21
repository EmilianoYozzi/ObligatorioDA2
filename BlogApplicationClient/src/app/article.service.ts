import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { InModelArticle } from './_types/InModelArticle';
import { HttpHeaders } from '@angular/common/http';
import { OutModelArticle } from './_types/OutModelArticle';
import { OutModelImport } from './_types/OutModelImport';
import { InModelImport } from './_types/InModelImport';


@Injectable({
  providedIn: 'root'
})
export class ArticleService {
  private baseUrl = 'https://localhost:5001/api/articles';

  constructor(private http: HttpClient) { }

  createArticle(article: InModelArticle): Observable<any> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization',token);
    return this.http.post(this.baseUrl, article, { headers }).pipe(
      map(response => {
        console.log('Article created!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }

  getArticles(): Observable<OutModelArticle[]> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization',token);
    return this.http.get<OutModelArticle[]>(this.baseUrl, { headers }).pipe(
      map(response => {
        console.log('Articles fetched!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }
  
  getArticleById(id: string): Observable<OutModelArticle> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization', token);
    return this.http.get<OutModelArticle>(`${this.baseUrl}/${id}`, { headers }).pipe(
      map(response => {
        console.log('Article fetched!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }
  
  getImporters(): Observable<OutModelImport[]> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization',token);
    return this.http.get<OutModelImport[]>(`${this.baseUrl}/importers`, { headers }).pipe(
      map(response => {
        console.log('Importers fetched!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }
  
  importArticles(importer: InModelImport): Observable<any> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization',token);
    return this.http.post(`${this.baseUrl}/import`, importer, { headers }).pipe(
      map(response => {
        console.log('Articles imported!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }

  deleteArticle(id: string): Observable<any> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization', token);
    return this.http.delete(`${this.baseUrl}/${id}`, { headers }).pipe(
      map(response => {
        console.log('Article deleted!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }

  updateArticle(id: string, article: InModelArticle): Observable<OutModelArticle> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization',token);
    return this.http.put<OutModelArticle>(`${this.baseUrl}/${id}`, article, { headers }).pipe(
      map(response => {
        console.log('Article updated!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }
  
  
  
  
  private handleError(error: HttpErrorResponse) {
    let errorMessage = 'Unknown error!';
    if (error.error instanceof ErrorEvent) {
      // Client-side errors
      errorMessage = `Error: ${error.error.message}`;
    } else {
      // Server-side errors
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.error}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }
}
