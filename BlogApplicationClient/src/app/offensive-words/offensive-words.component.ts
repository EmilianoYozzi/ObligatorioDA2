import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { OffensiveWordsService } from '../offensive-words.service';

@Component({
  selector: 'app-offensive-words',
  templateUrl: './offensive-words.component.html',
  styleUrls: ['./offensive-words.component.css']
})
export class OffensiveWordsComponent implements OnInit {
  offensiveWords: string[] = [];
  newWord = '';
  wordToDelete = '';

  constructor(private offensiveWordsService: OffensiveWordsService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadOffensiveWords();
  }

  loadOffensiveWords(): void {
    this.offensiveWordsService.getOffensiveWords().subscribe(
      (data: string[]) => {
        console.log('Fetched offensive words:', data);
        this.offensiveWords = data;
      },
      error => {
        console.error('Error fetching offensive words:', error);
        this.toastr.error(error);
      }
    );
  }

  addWord(): void {
    this.offensiveWordsService.addOffensiveWord([this.newWord]).subscribe(
      (data: string[]) => {
        console.log('Added offensive word:', data);
        this.toastr.success(`Added word: ${this.newWord}`);
        this.loadOffensiveWords();
      },
      error => {
        console.error('Error adding offensive word:', error);
        this.toastr.error(error);
      }
    );
    this.newWord = '';
  }

  deleteSelectedWord(): void {
    this.offensiveWordsService.deleteOffensiveWord([this.wordToDelete]).subscribe(
      () => {
        console.log('Deleted offensive word');
        this.toastr.success(`Deleted word: ${this.wordToDelete}`);
        this.loadOffensiveWords();
      },
      error => {
        console.error('Error deleting offensive word:', error);
        this.toastr.error(error);
      }
    );
    this.wordToDelete = '';
  }
}
