import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse,HttpHeaders} from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError, map,tap} from 'rxjs/operators';
import { LogInModel } from './_types/LogInModel';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private baseUrl = 'https://localhost:5001/api/session';

  constructor(private http: HttpClient) { }

  PostSession(login : LogInModel): Observable<any> {
    return this.http.post(this.baseUrl, login).pipe(
      map(response => {
        console.log('User Logged In', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }

  deleteSession(): Observable<any> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
    let username = userInfo.username;
    
    const headers = new HttpHeaders().set('Authorization',token);
    
    return this.http.delete(`${this.baseUrl}/${username}`, { headers }).pipe(
      map(response => {
        console.log('User Logged Out', response);
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
        errorMessage = `Error Code: ${error.status}\nMessage: ${error.error.title}\nErrors: ${JSON.stringify(error.error.errors)}`;
    }

    console.error(errorMessage);
    return throwError(errorMessage);
}

}
