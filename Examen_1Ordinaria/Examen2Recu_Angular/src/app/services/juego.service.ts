import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

const API_URL = 'http://localhost:7777/api/Juego';
const USERNAME = 'AngularInfoUser';
const PASSWORD = 'tu_password'; // fija en el código (no recomendado en producción)

@Injectable({
  providedIn: 'root'
})
export class JuegoService {

  private httpOptions = {
    headers: new HttpHeaders({
      'Authorization': 'Basic ' + btoa(`${USERNAME}:${PASSWORD}`),
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) {}

  getAllJuegos(): Observable<any> {
    return this.http.get(API_URL, this.httpOptions);
  }

  getJuegosFiltrados(userName: string, soloActivos: boolean): Observable<any> {
    // ejemplo para filtrar en backend, si está implementado
    const params: any = {};
    if(userName) params.userName = userName;
    if(soloActivos) params.onlyActive = true;
    return this.http.get(API_URL, { params, ...this.httpOptions });
  }

  getTopRanking(): Observable<any> {
    return this.http.get(`${API_URL}/top10`, this.httpOptions);
  }
}
