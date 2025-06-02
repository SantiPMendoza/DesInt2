// src/app/models/reserva.ts

export interface Profesor {
  id: number;
  nombre: string;
  // agrega más propiedades según tu DTO ProfesorDTO
}

export interface GrupoClase {
  id: number;
  nombre: string;
  // más propiedades si las tienes en GrupoClaseDTO
}

export interface FranjaHoraria {
  id: number;
  horaInicio: string;  // o Date si parseas
  horaFin: string;     // o Date si parseas
  // otras propiedades si existen
}

export interface Reserva {
  id: number;
  profesor: Profesor;
  grupoClase: GrupoClase;
  fecha: string;
  franjaHoraria: FranjaHoraria;
  estado: string;
  fechaSolicitud: string;   // ISO date string, puedes parsear a Date en TS si quieres
  fechaResolucion?: string | null;
}
