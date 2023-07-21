import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommentService } from '../comment.service';
import { InModelComment } from '../_types/InModelComment';
import { OutModelComment } from '../_types/OutModelComment';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {
  comment: OutModelComment | null = null;
  newReplyText: string = '';

  constructor(
    private route: ActivatedRoute,
    private commentService: CommentService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.commentService.getCommentById(id).subscribe(comment => {
        this.comment = comment;
      });
    }
  }

  onSubmitReply(): void {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');

    if (this.comment) {
      let reply: InModelComment = {
        OwnerUsername: userInfo.username,
        Text: this.newReplyText,
        contentId: this.comment.id
      };

      this.commentService.createComment(reply).subscribe(
        (response: OutModelComment) => {
          if (this.comment) {
            this.comment.answer = response;
            this.newReplyText = '';
            this.toastr.success('Reply posted successfully');
          }
        },
        error => {
          this.toastr.error('There was an error while posting your reply');
        }
      );

    }
  }
}
