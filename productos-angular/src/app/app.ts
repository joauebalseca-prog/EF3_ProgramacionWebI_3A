import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Producto } from './models/producto';
import { Movimiento } from './models/movimiento';
import { ProductoSoapService } from './services/producto-soap.service';
import { MovimientoService } from './services/movimiento.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  productos: Producto[] = [];
  movimientos: Movimiento[] = [];
  mensaje = '';

  movimiento: Movimiento = {
    idProducto: 1,
    tipo: 'ENTRADA',
    cantidad: 1,
    observacion: ''
  };

  constructor(
    private productoSoap: ProductoSoapService,
    private movimientoService: MovimientoService
  ) {}

  ngOnInit(): void {
    this.cargarProductos();
    this.cargarMovimientos();
  }

  cargarProductos(): void {
    this.productoSoap.listarProductos().subscribe({
      next: data => this.productos = data,
      error: () => this.mensaje = 'No fue posible consultar ProductosSOAP.'
    });
  }

  cargarMovimientos(): void {
    this.movimientoService.listar().subscribe({
      next: data => this.movimientos = data,
      error: () => this.mensaje = 'No fue posible consultar MovimientosREST.'
    });
  }

  guardar(): void {
    this.mensaje = '';
    this.movimientoService.registrar(this.movimiento).subscribe({
      next: () => {
        this.mensaje = 'Movimiento registrado.';
        this.movimiento.cantidad = 1;
        this.movimiento.observacion = '';
        this.cargarProductos();
        this.cargarMovimientos();
      },
      error: err => {
        this.mensaje = typeof err.error === 'string'
          ? err.error
          : 'El movimiento no pudo registrarse.';
      }
    });
  }
}
