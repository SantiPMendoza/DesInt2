import { Component, NgZone, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';

declare const google: any;

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent implements AfterViewInit {

  constructor(private ngZone: NgZone, private router: Router) {}

  ngAfterViewInit() {
    this.loadGoogleScriptAndInitialize();
  }

  loadGoogleScriptAndInitialize() {
    if (typeof google === 'undefined' || !google.accounts || !google.accounts.id) {
      console.log('Google API no cargada aún, intentando en 100ms...');
      setTimeout(() => this.loadGoogleScriptAndInitialize(), 100);
      return;
    }
    this.initializeGoogleSignIn();
  }

  initializeGoogleSignIn() {
    console.log('Inicializando Google Sign-In');

    google.accounts.id.initialize({
      client_id: '436178196669-n0lg9ld8afis4kdju82a8lff2m6mqa4c.apps.googleusercontent.com',
      callback: (response: any) => this.handleCredentialResponse(response),
    });

    const div = document.getElementById('googleSignInDiv');
    if (div) {
      google.accounts.id.renderButton(div, {
        theme: 'outline',
        size: 'large',
        width: 300
      });
      google.accounts.id.prompt();
      console.log('Botón de Google Sign-In renderizado');
    } else {
      console.error('No se encontró el div #googleSignInDiv');
    }
  }

  handleCredentialResponse(response: any) {
    const token = response.credential;
    const payload = JSON.parse(atob(token.split('.')[1]));
    const email = payload.email;
const googleId = payload.sub; // ID único de Google

if (email.endsWith('@iescomercio.com') || email === '14santi3c@gmail.com') {
  localStorage.setItem('authToken', token);
  localStorage.setItem('userEmail', email);
  localStorage.setItem('googleId', googleId);

  // Verifica si ya existe en la API
  this.checkProfesorExistence(googleId);
} else {
  alert('Solo se permiten cuentas institucionales de iescomercio.com');
}
  }

  checkProfesorExistence(googleId: string) {
  fetch(`https://localhost:7228/api/Profesor/google/${googleId}`)
    .then(async res => {
      if (res.ok) {
        const profesor = await res.json();
        localStorage.setItem('profesorId', profesor.id);
        this.ngZone.run(() => this.router.navigate(['/list']));
      } else {
        this.ngZone.run(() => this.router.navigate(['/register']));
      }
    })
    .catch(err => {
      console.error('Error consultando al API:', err);
      alert('Hubo un error consultando tu perfil. Inténtalo más tarde.');
    });
}

}
