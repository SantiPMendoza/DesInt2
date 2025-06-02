import { bootstrapApplication } from '@angular/platform-browser';
import { MenuComponent } from './app/components/menu/menu';
import { provideHttpClient } from '@angular/common/http';
import { JuegoService } from './app/services/juego.service';

bootstrapApplication(MenuComponent, {
  providers: [
    provideHttpClient(),
    JuegoService // <-- aquí añades el servicio para toda la app
  ]
});
