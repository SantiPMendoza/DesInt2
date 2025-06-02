import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReservaService } from '../../services/reserva.service';
import { Reserva } from '../../models/reserva';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-list',
  standalone: true,
  imports: [CommonModule, FormsModule],  // Añadir FormsModule para ngModel
  templateUrl: './list.html',
  styleUrls: ['./list.css']
})
export class ListComponent implements OnInit {
  reservas: Reserva[] = [];
  reservasFiltradas: Reserva[] = [];
  loading = true;
  error = '';
  expandedIndexMap: { [id: number]: boolean } = {};
  groupedReservas: { [fecha: string]: Reserva[] } = {};
  fechaFiltro: string = ''; // almacena fecha filtro en formato yyyy-mm-dd

  constructor(private reservaService: ReservaService) {}

  ngOnInit() {
    this.reservaService.getReservasAprobadas().subscribe({
      next: (data) => {
        this.reservas = data;
        this.reservasFiltradas = data;
        this.groupedReservas = this.groupByFecha(this.reservasFiltradas);
        this.loading = false;
      },
      error: () => {
        this.error = 'Error cargando reservas';
        this.loading = false;
      }
    });
  }

  groupByFecha(reservas: Reserva[]): { [fecha: string]: Reserva[] } {
    return reservas.reduce((acc, reserva) => {
    const fecha = reserva.fecha.split('T')[0];

      if (!acc[fecha]) acc[fecha] = [];
      acc[fecha].push(reserva);
      return acc;
    }, {} as { [fecha: string]: Reserva[] });
  }

  toggleExpand(id: number) {
    this.expandedIndexMap[id] = !this.expandedIndexMap[id];
  }

filtrarReservas() {
  if (!this.fechaFiltro) {
    this.reservasFiltradas = [...this.reservas];
  } else {
    this.reservasFiltradas = this.reservas.filter(r => r.fecha.startsWith(this.fechaFiltro));
  }
  this.groupedReservas = this.groupByFecha(this.reservasFiltradas);
}

  limpiarFiltro() {
    this.fechaFiltro = '';
    this.filtrarReservas();
  }
}
