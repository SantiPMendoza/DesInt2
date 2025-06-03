// src/app/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = 'https://localhost:7228/api/Profesor';

  constructor(private http: HttpClient) {}

  setSessionData(token: string, email: string, googleId: string) {
    localStorage.setItem('authToken', token);
    localStorage.setItem('userEmail', email);
    localStorage.setItem('googleId', googleId);
  }

  setProfesorId(id: number) {
    localStorage.setItem('profesorId', id.toString());
  }

  getProfesorId(): number | null {
    const id = localStorage.getItem('profesorId');
    return id ? +id : null;
  }

  getUserEmail(): string | null {
    return localStorage.getItem('userEmail');
  }

  getToken(): string | null {
    return localStorage.getItem('authToken');
  }

  clearSession() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userEmail');
    localStorage.removeItem('googleId');
    localStorage.removeItem('profesorId');
  }

  // Nuevo método que consulta si el profesor existe en la API
  checkProfesorExistence(googleId: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/google/${googleId}`);
  }
}
