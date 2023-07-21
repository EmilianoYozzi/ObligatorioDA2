import { InModelComment } from './_types/InModelComment';
import { OutModelComment } from './_types/OutModelComment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class CommentService {
  private baseUrl = 'https://localhost:5001/api/comments';

  constructor(private http: HttpClient) { }

  createComment(comment: InModelComment): Observable<OutModelComment> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization', token);
    return this.http.post<OutModelComment>(this.baseUrl, comment, { headers }).pipe(
      map(response => {
        console.log('Comment created!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }
  
  getCommentById(id: string): Observable<OutModelComment> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;

    const headers = new HttpHeaders().set('Authorization', token);
    return this.http.get<OutModelComment>(`${this.baseUrl}/${id}`, { headers }).pipe(
      map(response => {
        console.log('Comment fetched!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }

  deleteComment(id: string): Observable<void> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization', token);
    return this.http.delete<void>(`${this.baseUrl}/${id}`, { headers }).pipe(
      map(response => {
        console.log('Comment deleted!', response);
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
    console.error(JSON.stringify(error))
    return throwError(errorMessage);
  }
}
