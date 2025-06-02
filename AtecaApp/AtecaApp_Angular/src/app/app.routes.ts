// src/app/app.routes.ts
import { Routes } from '@angular/router';
import { LoginComponent } from './Pages/login/login';
import { ListComponent } from './Pages/list/list';
import { RegisterComponent } from './Pages/register/register';

export const routes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'lista', component: ListComponent },
  { path: 'register', component: RegisterComponent },
];
