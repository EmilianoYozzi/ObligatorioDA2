import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { HttpHeaders } from '@angular/common/http';
import { OutModelArticle } from './_types/OutModelArticle';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private baseUrl = 'https://localhost:5001/api/search';

  constructor(private http: HttpClient) { }

  searchArticles(query: string): Observable<OutModelArticle[]> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization',token);
    return this.http.get<OutModelArticle[]>(`${this.baseUrl}/${query}`, { headers }).pipe(
      map(response => {
        console.log('Articles fetched!', response);
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
