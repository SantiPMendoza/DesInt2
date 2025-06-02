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
    if (this.isSubmitting) return;
    this.isSubmitting = true;

    const googleId = localStorage.getItem('googleId');
    const email = localStorage.getItem('userEmail');

    if (!googleId || !email) {
      alert('No se encontró tu sesión de Google. Por favor, inicia sesión nuevamente.');
      this.isSubmitting = false;
      return;
    }

    if (!this.nombre.trim()) {
      alert('Por favor, introduce tu nombre completo.');
      this.isSubmitting = false;
      return;
    }

    const body = { nombre: this.nombre.trim(), email, googleId };

    try {
      // Verificar existencia
      const existsResponse = await fetch(`https://localhost:7228/api/Profesor/google/${googleId}`);

      if (existsResponse.ok) {
        const profesor = await existsResponse.json();
        localStorage.setItem('profesorId', profesor.id);
        this.router.navigate(['/list']);
        return;
      } else if (existsResponse.status !== 404) {
        // Error distinto a "no encontrado"
        const errorText = await existsResponse.text();
        alert(`Error consultando tu usuario: ${errorText}`);
        this.isSubmitting = false;
        return;
      }

      // Crear profesor si no existe
      const res = await fetch('https://localhost:7228/api/Profesor/create-if-not-exists', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
      });

      if (res.ok) {
        const profesor = await res.json();
        localStorage.setItem('profesorId', profesor.id);
        this.router.navigate(['/list']);
      } else {
        const errorText = await res.text();
        alert(`Error creando el profesor: ${errorText}`);
      }
    } catch (error) {
      alert('Error en la conexión. Intenta más tarde.');
      console.error(error);
    } finally {
      this.isSubmitting = false;
    }
  }
}
