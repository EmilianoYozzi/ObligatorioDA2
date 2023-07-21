import { Component, OnInit } from '@angular/core';
import { RankingService, DateRange } from '../ranking.service';
import { ToastrService } from 'ngx-toastr';
import { OutModelUserScore } from '../_types/OutModelUserScore';

@Component({
  selector: 'app-user-ranking',
  templateUrl: './user-ranking.component.html',
  styleUrls: ['./user-ranking.component.css']
})
export class UserRankingComponent implements OnInit {
  rankingType: 'activity' | 'offenses' = 'activity';
  startDate: string = '';
  endDate: string = '';
  userScores: OutModelUserScore[] = [];

  constructor(
    private rankingService: RankingService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
  }

  loadRanking(): void {
    const range: DateRange = {
      start: this.startDate,
      end: this.endDate
    };
  
    if (this.rankingType === 'activity') {
      this.rankingService.getUserActivityRanking(range).subscribe(
        (data: OutModelUserScore[]) => {
          this.userScores = data;
        },
        error => {
          this.toastr.error(error);
        }
      );
    } else {
      this.rankingService.getUserOffensesRanking(range).subscribe(
        (data: OutModelUserScore[]) => {
          this.userScores = data;
        },
        error => {
          this.toastr.error(error);
        }
      );
    }
  }
}
