import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ReservaService } from '../../services/reserva.service';
import { FranjaHorariaService } from '../../services/franjahoraria.service';
import { GrupoClaseService } from '../../services/grupoclase.service';
import { Reserva } from '../../models/reserva';
import { FranjaHoraria } from '../../models/franjaHoraria';
import { GrupoClase } from '../../models/grupoClase';
import { AuthService } from '../../services/auth.service'; // Para obtener el profesor actual

@Component({
  selector: 'app-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './list.html',
  styleUrls: ['./list.css']
})
export class ListComponent implements OnInit {
  // Listado total de reservas cargadas
  reservas: Reserva[] = [];

  // Reservas tras aplicar filtros
  reservasFiltradas: Reserva[] = [];

  // Reservas agrupadas por fecha
  groupedReservas: { [fecha: string]: Reserva[] } = {};

  // Control de tarjetas expandidas por ID de reserva
  expandedIndexMap: { [id: number]: boolean } = {};

  // Estado de carga y errores
  loading = true;
  error = '';

  // Filtro de fecha para la búsqueda
  fechaFiltro: string = '';

  // Mensajes de éxito/error de creación de reserva
  mensajeExito: string = '';
  mensajeError: string = '';

  // Filtro para mostrar solo reservas del usuario actual
  soloMisReservas: boolean = false;

  // ID del profesor autenticado
  profesorIdActual: number | null = null;

  // Estado y datos del modal de nueva reserva
  showModal = false;
  fechaNuevaReserva: string = '';
  selectedFranjaId: number | null = null;
  selectedGrupoId: number | null = null;

  // Listados para los selectores del formulario
  franjasHorarias: FranjaHoraria[] = [];
  grupos: GrupoClase[] = [];

  constructor(
    private reservaService: ReservaService,
    private franjaService: FranjaHorariaService,
    private grupoService: GrupoClaseService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    // Obtiene el ID del profesor logueado y carga datos iniciales
    this.profesorIdActual = this.authService.getProfesorId();
    this.cargarReservas();
    this.cargarFranjasYGrupos();
  }

  // Carga todas las reservas aprobadas
  cargarReservas() {
    this.reservaService.getReservasAprobadas().subscribe({
      next: (data) => {
        this.reservas = data;
        this.filtrarReservas(); // Aplica filtros iniciales
        this.loading = false;
      },
      error: () => {
        this.error = 'Error cargando reservas';
        this.loading = false;
      }
    });
  }

  // Carga franjas horarias y grupos de clase
  cargarFranjasYGrupos() {
    this.franjaService.getFranjasHorarias().subscribe({
      next: (data) => {
        this.franjasHorarias = data;
      },
      error: (err) => console.error('Error cargando franjas:', err)
    });

    this.grupoService.getGrupos().subscribe({
      next: (data) => {
        this.grupos = data;
      },
      error: (err) => console.error('Error cargando grupos:', err)
    });
  }

  // Agrupa reservas por fecha (formato YYYY-MM-DD)
  groupByFecha(reservas: Reserva[]): { [fecha: string]: Reserva[] } {
    return reservas.reduce((acc, reserva) => {
      const fecha = reserva.fecha.split('T')[0];
      if (!acc[fecha]) acc[fecha] = [];
      acc[fecha].push(reserva);
      return acc;
    }, {} as { [fecha: string]: Reserva[] });
  }

  // Alterna el estado expandido de una reserva
  toggleExpand(id: number) {
    this.expandedIndexMap[id] = !this.expandedIndexMap[id];
  }

  // Aplica filtros por fecha y por profesor actual
  filtrarReservas() {
    this.reservasFiltradas = this.reservas.filter(r => {
      const coincideFecha = this.fechaFiltro ? r.fecha.startsWith(this.fechaFiltro) : true;
      const esDelProfesor = this.soloMisReservas ? r.profesor.id === this.profesorIdActual : true;
      return coincideFecha && esDelProfesor;
    });

    this.groupedReservas = this.groupByFecha(this.reservasFiltradas);
  }

  // Limpia el filtro de fecha
  limpiarFiltro() {
    this.fechaFiltro = '';
    this.filtrarReservas();
  }

  // Abre el modal de nueva reserva y limpia mensajes
  abrirModal() {
    this.showModal = true;
    this.limpiarMensajes();
  }

  // Limpia mensajes de éxito y error
  limpiarMensajes() {
    this.mensajeExito = '';
    this.mensajeError = '';
  }

  // Cierra el modal y limpia campos del formulario
  cerrarModal() {
    this.showModal = false;
    this.fechaNuevaReserva = '';
    this.selectedFranjaId = null;
    this.selectedGrupoId = null;
  }

  // Envía nueva reserva al servidor
  crearReserva() {
    const profesorId = this.authService.getProfesorId();

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
        this.mensajeExito = 'Reserva creada con éxito. A la espera de aprobación.';
        this.mensajeError = '';
        this.cargarReservas(); // Refresca lista
      },
      error: (err) => {
        this.mensajeExito = '';
        if (err.status === 409 && err.error?.mensaje) {
          this.mensajeError = err.error.mensaje;
        } else {
          this.mensajeError = 'Error al crear la reserva.';
        }
      }
    });
  }
}
