import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ArticleService } from '../article.service';
import { CommentService } from '../comment.service';
import { OutModelArticle } from '../_types/OutModelArticle';
import { InModelComment } from '../_types/InModelComment';
import { OutModelComment } from '../_types/OutModelComment';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-article-read',
  templateUrl: './article-read.component.html',
  styleUrls: ['./article-read.component.css']
})
export class ArticleReadComponent implements OnInit {
  article: OutModelArticle | null = null;
  newCommentText: string = '';
  newReplyTexts: { [commentId: string]: string } = {};

  constructor(
    private route: ActivatedRoute,
    private articleService: ArticleService,
    private commentService: CommentService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.articleService.getArticleById(id).subscribe(article => {
        this.article = article;
      });
    }
  }

  onSubmit(): void {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let comment: InModelComment = {
      OwnerUsername: userInfo.username,
      Text: this.newCommentText,
      contentId: this.article?.id || ''
    };

    this.commentService.createComment(comment).subscribe(
      (response: OutModelComment) => {
        if (this.article && this.article.comments) {
          this.article.comments.push(response);
        }

        this.newCommentText = '';
        this.toastr.success('Comment posted successfully');
      },
      error => {
        this.toastr.error('There was an error while posting your comment');
      }
    );
  }

  onSubmitReply(commentId: string): void {
    let userInfo = JSON.parse(localStorage.getItem('userInfo') || '{}');
    let commentIndex = this.article?.comments?.findIndex(c => c.id === commentId);
  
    if (commentIndex !== undefined && commentIndex !== -1 && this.article && this.article.comments) {
      let reply: InModelComment = {
        OwnerUsername: userInfo.username,
        Text: this.newReplyTexts[commentId],
        contentId: commentId
      };
  
      this.commentService.createComment(reply).subscribe(
        (response: OutModelComment) => {
          if (this.article?.comments && commentIndex !== undefined) {
            this.article.comments[commentIndex].answer = response;
          }
          this.newReplyTexts[commentId] = '';
          this.toastr.success('Reply posted successfully');
        },
        error => {
          this.toastr.error('There was an error while posting your reply');
        }
      );
    }
  }
  onDelete(): void {
    if(this.article) {
      this.articleService.deleteArticle(this.article.id).subscribe(
        response => {
          this.toastr.success('Article deleted successfully');
        },
        error => {
          this.toastr.error('There was an error while deleting the article');
        }
      );
    }
  }
  

  getImgPositionClass(index: number): string {
    switch (this.article?.template) {
      case 'ImageAtTopLeft':
      case 'ImageAtTop':
        return index === 0 ? 'top-img' : '';
      case 'ImageAtBottom':
        return index === 0 ? 'bottom-img' : '';
      case 'ImageAtTopAndBottom':
        return index === 0 ? 'top-img' : 'bottom-img';
      default:
        return '';
    }
  }

  deleteComment(commentId: string): void {
    this.commentService.deleteComment(commentId).subscribe(
      () => {
        if (this.article?.comments) {
          const index = this.article.comments.findIndex(c => c.id === commentId);
          if (index !== -1) {
            this.article.comments.splice(index, 1);
          }
        }
        this.toastr.success('Comment deleted successfully');
      },
      error => {
        this.toastr.error('There was an error while deleting the comment');
      }
    );
  }  
}
