// src/app/services/reserva.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Reserva } from '../models/reserva';

@Injectable({
  providedIn: 'root'
})
export class ReservaService {

private apiUrl = 'https://localhost:7228/api/Reserva/';


  constructor(private http: HttpClient) {}

  getReservasAprobadas(): Observable<Reserva[]> {
    return this.http.get<Reserva[]>(this.apiUrl+'aprobadas');
  }

  createReserva(reserva: any) {
  return this.http.post(this.apiUrl, reserva);
}

}
