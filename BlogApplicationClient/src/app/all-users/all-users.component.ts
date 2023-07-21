import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { OutModelUser } from '../_types/OutModelUser';

@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html',
  styleUrls: ['./all-users.component.css']
})
export class AllUsersComponent implements OnInit {
  users: OutModelUser[] = [];

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getAllUsers().subscribe(users => {
      this.users = users;
    });
  }
}
