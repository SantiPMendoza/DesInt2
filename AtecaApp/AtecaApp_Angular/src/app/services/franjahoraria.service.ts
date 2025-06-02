import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FranjaHoraria } from '../models/franjaHoraria';

@Injectable({
  providedIn: 'root'
})
export class FranjaHorariaService {

  constructor(private http: HttpClient) { }

  getFranjasHorarias() {
  return this.http.get<FranjaHoraria[]>('https://localhost:7228/api/FranjaHoraria');
}

}
