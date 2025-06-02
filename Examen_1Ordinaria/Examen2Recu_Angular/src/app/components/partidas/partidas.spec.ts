import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Partidas } from './partidas';

describe('Partidas', () => {
  let component: Partidas;
  let fixture: ComponentFixture<Partidas>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Partidas]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Partidas);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
