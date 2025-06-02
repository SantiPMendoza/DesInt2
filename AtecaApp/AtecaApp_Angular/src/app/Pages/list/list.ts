import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReservaService } from '../../services/reserva.service';
import { FranjaHorariaService } from '../../services/franjahoraria.service';
import { GrupoClaseService } from '../../services/grupoclase.service';
import { Reserva } from '../../models/reserva';
import { FranjaHoraria } from '../../models/franjaHoraria';
import { GrupoClase } from '../../models/grupoClase';
import { AuthService } from '../../services/auth.service'; // ⬅️ Para obtener el profesor actual

@Component({
  selector: 'app-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './list.html',
  styleUrls: ['./list.css']
})
export class ListComponent implements OnInit {
  reservas: Reserva[] = [];
  reservasFiltradas: Reserva[] = [];
  groupedReservas: { [fecha: string]: Reserva[] } = {};
  expandedIndexMap: { [id: number]: boolean } = {};
  loading = true;
  error = '';
  fechaFiltro: string = '';

  // Modal
  showModal = false;
  fechaNuevaReserva: string = '';
  selectedFranjaId: number | null = null;
  selectedGrupoId: number | null = null;

  franjasHorarias: FranjaHoraria[] = [];
  grupos: GrupoClase[] = [];

  constructor(
    private reservaService: ReservaService,
    private franjaService: FranjaHorariaService,
    private grupoService: GrupoClaseService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.cargarReservas();
    this.cargarFranjasYGrupos();
  }

  cargarReservas() {
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

cargarFranjasYGrupos() {
  this.franjaService.getFranjasHorarias().subscribe({
    next: (data) => {
      console.log('Franjas horarias recibidas:', data);
      this.franjasHorarias = data;
    },
    error: (err) => console.error('Error cargando franjas:', err)
  });

  this.grupoService.getGrupos().subscribe({
    next: (data) => {
      console.log('Grupos recibidos:', data);
      this.grupos = data;
    },
    error: (err) => console.error('Error cargando grupos:', err)
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
    this.reservasFiltradas = this.fechaFiltro
      ? this.reservas.filter(r => r.fecha.startsWith(this.fechaFiltro))
      : [...this.reservas];
    this.groupedReservas = this.groupByFecha(this.reservasFiltradas);
  }

  limpiarFiltro() {
    this.fechaFiltro = '';
    this.filtrarReservas();
  }

  abrirModal() {
    this.showModal = true;
  }

  cerrarModal() {
    this.showModal = false;
    this.fechaNuevaReserva = '';
    this.selectedFranjaId = null;
    this.selectedGrupoId = null;
  }

  crearReserva() {
    const profesorId = this.authService.getProfesorId(); // ⬅️ Obtenido desde AuthService

    if (!this.fechaNuevaReserva || !this.selectedFranjaId || !this.selectedGrupoId) {
      alert('Completa todos los campos.');
      return;
    }

    const nuevaReserva = {
      fecha: this.fechaNuevaReserva,
      franjaHorariaId: this.selectedFranjaId,
      grupoClaseId: this.selectedGrupoId,
      profesorId: profesorId
    };

    this.reservaService.createReserva(nuevaReserva).subscribe({
      next: () => {
        this.cerrarModal();
        this.cargarReservas();
      },
      error: () => alert('Error al crear la reserva.')
    });
  }
}
