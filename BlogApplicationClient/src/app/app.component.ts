import { Component } from '@angular/core';
import { SessionService } from './session.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  isLoggedIn: boolean = false;
  username: string = '';

  constructor(private sessionService: SessionService, private toastr: ToastrService) { 
    const userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    this.isLoggedIn = !!userInfo.token;
    this.username = userInfo.username;
  }

  logout(): void {
    this.sessionService.deleteSession().subscribe(
      response => {
        localStorage.removeItem('userInfo');
        this.isLoggedIn = false;
        this.toastr.success('Logged out successfully!');
      },
      error => this.toastr.error('Failed to log out')
    );
  }
}
