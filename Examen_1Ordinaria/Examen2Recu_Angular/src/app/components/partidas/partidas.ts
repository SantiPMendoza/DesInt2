import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { JuegoService } from '../../services/juego.service';

@Component({
  selector: 'app-partidas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './partidas.html',
  styleUrls: ['./partidas.css'],
})
export class PartidasComponent {
  partidas: any[] = [];
  filtroUsuario: string = '';
  soloActivas: boolean = false;
  toggleTexto = 'Mostrar solo activas';

  constructor(private juegoService: JuegoService) {}

  ngOnInit() {
    this.cargarPartidas();
  }

  cargarPartidas() {
    this.juegoService
      .getJuegosFiltrados(this.filtroUsuario, this.soloActivas)
      .subscribe((data) => {
        this.partidas = data;
      });
  }

  toggleMostrar() {
    this.soloActivas = !this.soloActivas;
    this.toggleTexto = this.soloActivas ? 'Mostrar todas' : 'Mostrar solo activas';
    this.cargarPartidas();
  }

  filtrarPorUsuario() {
    this.cargarPartidas();
  }
}
