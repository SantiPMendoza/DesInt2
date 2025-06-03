import { Component, NgZone, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import {AuthService} from '../../services/auth.service'

declare const google: any; // Declaración de la API de Google

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements AfterViewInit {

 constructor(private ngZone: NgZone, private router: Router, private http: HttpClient, private authService: AuthService ) {}

  // Se ejecuta después de que la vista está completamente cargada
  ngAfterViewInit() {
    this.loadGoogleScriptAndInitialize();
  }

  // Verifica si la API de Google está disponible y luego inicializa
  loadGoogleScriptAndInitialize() {
    if (typeof google === 'undefined' || !google.accounts || !google.accounts.id) {
      console.log('Google API no cargada aún, intentando en 100ms...');
      setTimeout(() => this.loadGoogleScriptAndInitialize(), 100); // Reintenta tras 100ms
      return;
    }
    this.initializeGoogleSignIn();
  }

  // Configura el inicio de sesión de Google y renderiza el botón
  initializeGoogleSignIn() {
    console.log('Inicializando Google Sign-In');

    // Inicializa el cliente de Google con el ID y callback
    google.accounts.id.initialize({
      client_id: '436178196669-n0lg9ld8afis4kdju82a8lff2m6mqa4c.apps.googleusercontent.com',
      callback: (response: any) => this.handleCredentialResponse(response),
    });

    const div = document.getElementById('googleSignInDiv'); // Elemento donde se monta el botón
    if (div) {
      // Renderiza el botón de Google
      google.accounts.id.renderButton(div, {
        theme: 'outline',
        size: 'large',
        width: 300
      });

      google.accounts.id.prompt(); // Muestra el prompt de login
      console.log('Botón de Google Sign-In renderizado');
    } else {
      console.error('No se encontró el div #googleSignInDiv');
    }
  }

  // Maneja el token recibido tras el login con Google
  handleCredentialResponse(response: any) {
  const token = response.credential;
  const payload = JSON.parse(atob(token.split('.')[1]));
  const email = payload.email;
  const googleId = payload.sub;

  if (email.endsWith('@iescomercio.com') || email === '14santi3c@gmail.com') {
    this.authService.setSessionData(token, email, googleId);
    this.checkProfesorExistence(googleId);
  } else {
    alert('Solo se permiten cuentas institucionales de iescomercio.com');
  }
}

checkProfesorExistence(googleId: string) {
  this.authService.checkProfesorExistence(googleId)
    .subscribe({
      next: (profesor: any) => {
        this.authService.setProfesorId(profesor.id);
        this.ngZone.run(() => this.router.navigate(['/list']));
      },
      error: () => {
        this.ngZone.run(() => this.router.navigate(['/register']));
      }
    });
}



}


