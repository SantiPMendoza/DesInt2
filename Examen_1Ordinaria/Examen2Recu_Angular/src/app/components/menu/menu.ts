import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PartidasComponent } from '../partidas/partidas';
import { RankingComponent } from '../ranking/ranking';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, PartidasComponent, RankingComponent],
  templateUrl: './menu.html',
})
export class MenuComponent {
  pestaniaSeleccionada: string = 'partidas';

  seleccionarPestania(pestania: string) {
    this.pestaniaSeleccionada = pestania;
  }
}
