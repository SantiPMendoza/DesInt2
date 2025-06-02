// src/app/models/reserva.ts
import { FranjaHoraria } from './franjaHoraria';
import { GrupoClase } from './grupoClase';
import { Profesor } from './profesor';



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
