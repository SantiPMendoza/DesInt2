// src/app/services/auth.service.ts
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  getProfesorId(): number | null {
    const id = localStorage.getItem('profesorId');
    return id ? +id : null;
  }

  getUserEmail(): string | null {
    return localStorage.getItem('userEmail');
  }

  clearSession() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userEmail');
    localStorage.removeItem('googleId');
    localStorage.removeItem('profesorId');
  }
}
