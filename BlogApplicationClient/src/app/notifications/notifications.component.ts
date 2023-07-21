import { Component, OnInit } from '@angular/core';
import { UserService } from '../user.service';
import { OutModelNotification } from '../_types/OutModelNotification';
import { Router } from '@angular/router';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.css']
})
export class NotificationsComponent implements OnInit {
  notifications: OutModelNotification[] = [];

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    if(userInfo.username){
      this.userService.getUserNotifications(userInfo.username).subscribe(
        (notifications: OutModelNotification[]) => {
          this.notifications = notifications;
        },
        (error) => {
          console.error('Error fetching notifications:', error);
        }
      );
    }
  }

  navigateToNotification(uri: string): void {
    console.log('Notification URI:', uri);
    const [route, id] = uri.split('/');
    console.log('Parsed route:', route, 'ID:', id);
    if(route === 'articles') {
      this.router.navigateByUrl(`/article-read/${id}`);
    } else if(route === 'comments') {
      this.router.navigateByUrl(`/comment/${id}`);
    }
}
}
