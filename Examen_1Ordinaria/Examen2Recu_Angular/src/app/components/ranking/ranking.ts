import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { JuegoService } from '../../services/juego.service';

@Component({
  selector: 'app-ranking',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './ranking.html',
  styleUrls: ['./ranking.css']
})
export class RankingComponent implements OnInit {
  ranking: any[] = [];

  constructor(private juegoService: JuegoService) {}

  ngOnInit() {
    this.cargarRanking();
  }

  cargarRanking() {
    this.juegoService.getTopRanking().subscribe(data => {
      this.ranking = data;
    });
  }
}
