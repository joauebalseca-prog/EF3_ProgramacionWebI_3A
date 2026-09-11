export interface Producto {
  idProducto: number;
  idCategoria: number;
  nombre: string;
  descripcion?: string;
  precio: number;
  stock: number;
  estado: boolean;
}
