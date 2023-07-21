import { ToastrService } from 'ngx-toastr';
import { UserService } from '../user.service';
import { Component } from '@angular/core';
import { InModelUser } from '../_types/InModelUser';

@Component({
  selector: 'app-register',
  templateUrl: 'register.component.html',
  styleUrls: ['register.component.css']
})
export class RegisterComponent implements InModelUser {
  Username: string;
  Password: string;
  Name: string;
  LastName: string;
  Email: string;

  constructor(private userService: UserService, private toastr: ToastrService) {
    this.Username = '';
    this.Password = '';
    this.Name = '';
    this.LastName = '';
    this.Email = '';
  }

  onSubmit() {
    const user: InModelUser = {
      Username: this.Username,
      Password: this.Password,
      Name: this.Name,
      LastName: this.LastName,
      Email: this.Email,
    };
    this.userService.createUser(user)
      .subscribe(
        () => this.toastr.success('User created!'),
        error => this.toastr.error(error)
      );
}
}
