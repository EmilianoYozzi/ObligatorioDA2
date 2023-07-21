import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class OffensiveWordsService {
  private baseUrl = 'https://localhost:5001/api/offensiveWords';

  constructor(private http: HttpClient) { }

  getHeaders() {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
    const headers = new HttpHeaders().set('Authorization',token);
    return headers;
  }

  getOffensiveWords(): Observable<string[]> {
    const headers = this.getHeaders();
    return this.http.get<string[]>(this.baseUrl, { headers })
    .pipe(
      map(response => {
        console.log('Offensive Words fetched!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }

  addOffensiveWord(words: string[]): Observable<string[]> {
    const headers = this.getHeaders();
    return this.http.post<string[]>(this.baseUrl, words, { headers })
    .pipe(
      map(response => {
        console.log('Offensive Words added!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }

  deleteOffensiveWord(words: string[]): Observable<void> {
    const headers = this.getHeaders();
    const options = {
      headers,
      body: words
    };
    return this.http.delete<void>(this.baseUrl, options)
    .pipe(
      map(() => {
        console.log('Offensive Words deleted!');
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
