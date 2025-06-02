import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GrupoClase } from '../models/grupoClase';

@Injectable({
  providedIn: 'root'
})
export class GrupoClaseService {

  constructor(private http: HttpClient) { }

  getGrupos() {
  return this.http.get<GrupoClase[]>('https://localhost:7228/api/GrupoClase');
}
}
