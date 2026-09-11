import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Movimiento } from '../models/movimiento';

@Injectable({ providedIn: 'root' })
export class MovimientoService {

  private readonly apiUrl =
    'http://localhost:5265/api/MovimientoInventario';

  constructor(private http: HttpClient) {}

  listar(): Observable<Movimiento[]> {
    return this.http.get<Movimiento[]>(this.apiUrl);
  }

  registrar(movimiento: Movimiento): Observable<Movimiento> {
    return this.http.post<Movimiento>(
      this.apiUrl,
      movimiento
    );
  }
}