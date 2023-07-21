import { ToastrService } from 'ngx-toastr';
import { Component } from '@angular/core';
import { SessionService } from '../session.service';
import { LogInModel } from '../_types/LogInModel';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  Username: string;
  Password: string;
  Email: string;

  constructor(private sessionService: SessionService, private toastr: ToastrService,private router: Router ) {
    this.Username = '';
    this.Password = '';
    this.Email = '';
  }

  onSubmit() {
    const login : LogInModel = {
        Username: this.Username,
        Password: this.Password,
        Email: this.Email
    };

    this.sessionService.PostSession(login).subscribe(
      (response: any) => {
          const userInfo = {
              token: response.token,
              username: this.Username,
              role: response.role
          };
  
          localStorage.setItem('userInfo', JSON.stringify(userInfo));
          this.toastr.success('User logged in!');
          this.router.navigateByUrl('/notifications');
      },
      error => {
          this.toastr.error(error)
      }
  );
  
}
}
