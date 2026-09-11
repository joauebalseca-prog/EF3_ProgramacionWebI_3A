export interface Movimiento {
  idMovimiento?: number;
  idProducto: number;
  tipo: string;
  cantidad: number;
  fecha?: string;
  observacion?: string;
  stockAnterior?: number;
  stockResultante?: number;
}
