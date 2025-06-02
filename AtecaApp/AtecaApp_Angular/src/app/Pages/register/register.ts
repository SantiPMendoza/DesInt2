import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.html',
  styleUrls: ['./register.css'],
  imports: [CommonModule, FormsModule]
})
export class RegisterComponent {
  nombre = '';
  isSubmitting = false;

  constructor(private router: Router) {}

  async submit() {
    if (this.isSubmitting) return; // prevenir múltiples envíos simultáneos
    this.isSubmitting = true;

    const googleId = localStorage.getItem('googleId');
    const email = localStorage.getItem('userEmail');

    const body = { nombre: this.nombre, email, googleId };

    try {
      // Primero verifica si el profesor ya existe por googleId
      const existsResponse = await fetch(`https://localhost:7228/api/Profesor/${googleId}`);
      if (existsResponse.ok) {
        // Profesor ya existe, solo redirige
        const profesor = await existsResponse.json();
        localStorage.setItem('profesorId', profesor.id);
        this.router.navigate(['/list']);
        return;
      }

      // Si no existe, crear nuevo profesor
      const res = await fetch('https://localhost:7228/api/Profesor', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
      });

      if (res.ok) {
        const profesor = await res.json();
        localStorage.setItem('profesorId', profesor.id);
        this.router.navigate(['/list']);
      } else {
        alert('Error creando el profesor');
      }
    } catch (error) {
      alert('Error en la conexión. Intenta más tarde.');
      console.error(error);
    } finally {
      this.isSubmitting = false;
    }
  }
}

