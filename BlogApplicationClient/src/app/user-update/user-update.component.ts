import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { ToastrService } from 'ngx-toastr';
import { InModelUser } from '../_types/InModelUser';
import { OutModelUser } from '../_types/OutModelUser';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-user-update',
  templateUrl: './user-update.component.html',
  styleUrls: ['./user-update.component.css']
})
export class UserUpdateComponent implements OnInit, InModelUser {
  Username: string;
  Password: string;
  Name: string;
  LastName: string;
  Email: string;

  currentUser: OutModelUser | null = null;

  constructor(
    private userService: UserService, 
    private toastr: ToastrService, 
    private route: ActivatedRoute
  ) {
    this.Username = '';
    this.Password = '';
    this.Name = '';
    this.LastName = '';
    this.Email = '';
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.Username = params['username'];
    });
  }

  onSubmit(): void {
    let user: InModelUser = {
      Username: this.Username,
      Password: this.Password,
      Name: this.Name,
      LastName: this.LastName,
      Email: this.Email,
    };

    this.userService.updateUser(user).subscribe(
      (response: OutModelUser) => {
        this.currentUser = response;
        this.toastr.success('User updated!');
      },
      (error) => {
        this.toastr.error('There was an error while updating the user');
      }
    );
  }
  // UserUpdateComponent

deleteUser(): void {
  this.userService.deleteUser(this.Username)
    .subscribe(
      () => {
        this.toastr.success('User deleted!');
        this.currentUser = null;
      },
      error => this.toastr.error(error)
    );
}

}
