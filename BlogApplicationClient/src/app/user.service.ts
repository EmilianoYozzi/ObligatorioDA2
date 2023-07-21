import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse,HttpHeaders} from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { catchError, map,tap} from 'rxjs/operators';
import { InModelUser } from './_types/InModelUser';
import { OutModelNotification } from './_types/OutModelNotification';
import { OutModelUser } from './_types/OutModelUser';
import { OutModelArticle } from './_types/OutModelArticle';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = 'https://localhost:5001/api/users';
  

  constructor(private http: HttpClient) { }

  createUser(user: InModelUser): Observable<any> {
    return this.http.post(this.baseUrl, user).pipe(
      map(response => {
        console.log('User created!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }

  getUserNotifications(username: string): Observable<OutModelNotification[]> {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let token = userInfo.token;
  
    const headers = new HttpHeaders().set('Authorization',token);
    return this.http.get<OutModelNotification[]>(`${this.baseUrl}/${username}/notifications`,{headers}).pipe(
      map(response => {
        console.log('Notifications fetched!', response);
        return response;
      }),
      catchError(this.handleError)
    );
  }

  updateUser(user: InModelUser): Observable<OutModelUser> {
  let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
  let token = userInfo.token;

  const headers = new HttpHeaders().set('Authorization', token);
  return this.http.put<OutModelUser>(`${this.baseUrl}/${user.Username}`, user, { headers }).pipe(
    map(response => {
      console.log('User updated!', response);
      return response;
    }),
    catchError(this.handleError)
  );
}

deleteUser(username: string): Observable<any> {
  let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
  let token = userInfo.token;

  const headers = new HttpHeaders().set('Authorization', token);
  return this.http.delete(`${this.baseUrl}/${username}`, { headers }).pipe(
    map(response => {
      console.log('User deleted!', response);
      return response;
    }),
    catchError(this.handleError)
  );
}


getAllUsers(): Observable<OutModelUser[]> {
  let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
  let token = userInfo.token;

  const headers = new HttpHeaders().set('Authorization', token);
  return this.http.get<OutModelUser[]>(`${this.baseUrl}`, { headers }).pipe(
    map(response => {
      console.log('Fetched all users!', response);
      return response;
    }),
    catchError(this.handleError)
  );
}

getUserArticles(username: string): Observable<OutModelArticle[]> {
  let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
  let token = userInfo.token;

  const headers = new HttpHeaders().set('Authorization', token);
  return this.http.get<OutModelArticle[]>(`${this.baseUrl}/${username}/articles`, { headers }).pipe(
    map(response => {
      console.log('Fetched user articles!', response);
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