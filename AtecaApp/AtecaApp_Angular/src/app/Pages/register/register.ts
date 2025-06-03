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
  nombre = ''; // Nombre ingresado por el usuario
  isSubmitting = false; // Controla si se está enviando el formulario

  constructor(private router: Router) {}

  // Maneja el envío del formulario de registro
  async submit() {
    if (this.isSubmitting) return; // Evita múltiples envíos
    this.isSubmitting = true;

    const googleId = localStorage.getItem('googleId'); // ID de Google almacenado
    const email = localStorage.getItem('userEmail');   // Email del usuario

    // Verifica que haya sesión válida
    if (!googleId || !email) {
      alert('No se encontró tu sesión de Google. Por favor, inicia sesión nuevamente.');
      this.isSubmitting = false;
      return;
    }

    // Verifica que el nombre no esté vacío
    if (!this.nombre.trim()) {
      alert('Por favor, introduce tu nombre completo.');
      this.isSubmitting = false;
      return;
    }

    // Datos para el backend
    const body = { nombre: this.nombre.trim(), email, googleId };

    try {
      // Verifica si el profesor ya existe en la API
      const existsResponse = await fetch(`https://localhost:7228/api/Profesor/google/${googleId}`);

      if (existsResponse.ok) {
        // Ya existe: guarda el ID y redirige a la lista
        const profesor = await existsResponse.json();
        localStorage.setItem('profesorId', profesor.id);
        this.router.navigate(['/list']);
        return;
      } else if (existsResponse.status !== 404) {
        // Otro error distinto a "no encontrado"
        const errorText = await existsResponse.text();
        alert(`Error consultando tu usuario: ${errorText}`);
        this.isSubmitting = false;
        return;
      }

      // Si no existe, lo crea
      const res = await fetch('https://localhost:7228/api/Profesor/create-if-not-exists', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(body)
      });

      if (res.ok) {
        // Registro exitoso: guarda el ID y navega
        const profesor = await res.json();
        localStorage.setItem('profesorId', profesor.id);
        this.router.navigate(['/list']);
      } else {
        // Error al crear el profesor
        const errorText = await res.text();
        alert(`Error creando el profesor: ${errorText}`);
      }
    } catch (error) {
      // Error de conexión
      alert('Error en la conexión. Intenta más tarde.');
      console.error(error);
    } finally {
      // Libera el estado de envío
      this.isSubmitting = false;
    }
  }
}
