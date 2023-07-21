import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { OutModelUserScore } from './_types/OutModelUserScore';
import { HttpErrorResponse } from '@angular/common/http';

export interface DateRange {
  start: string;
  end: string;
}

@Injectable({
  providedIn: 'root'
})
export class RankingService {
  private baseUrl = 'https://localhost:5001/api/ranking';

  constructor(private http: HttpClient) { }

  getUserActivityRanking(range: DateRange): Observable<OutModelUserScore[]> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization',token);
    const params = {
      start: range.start,
      end: range.end
    };
    return this.http.get<OutModelUserScore[]>(`${this.baseUrl}/activity`, { params, headers }).pipe(
      map(response => {
        console.log('User activity ranking fetched!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }

  getUserOffensesRanking(range: DateRange): Observable<OutModelUserScore[]> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization',token);
    const params = {
      start: range.start,
      end: range.end
    };
    return this.http.get<OutModelUserScore[]>(`${this.baseUrl}/offenses`, { params, headers }).pipe(
      map(response => {
        console.log('User offenses ranking fetched!', response);
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
