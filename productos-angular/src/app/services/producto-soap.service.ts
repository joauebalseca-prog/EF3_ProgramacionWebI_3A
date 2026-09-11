import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Producto } from '../models/producto';

@Injectable({ providedIn: 'root' })
export class ProductoSoapService {

  private readonly soapUrl =
    'http://localhost:5163/ProductoService.svc';

  private readonly ns =
    'http://programacionweb/examen3a';

  constructor(private http: HttpClient) {}

  listarProductos(): Observable<Producto[]> {

    const envelope = `<?xml version="1.0" encoding="utf-8"?>
<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">
  <soap:Body>
    <ListarProductos xmlns="${this.ns}" />
  </soap:Body>
</soap:Envelope>`;

    const headers = new HttpHeaders({
      'Content-Type': 'text/xml; charset=utf-8',
      'SOAPAction':
        '"http://programacionweb/examen3a/IProductoService/ListarProductos"'
    });

    return this.http.post(
      this.soapUrl,
      envelope,
      {
        headers,
        responseType: 'text'
      }
    ).pipe(
      map(xml => this.convertirProductos(xml))
    );
  }

  private convertirProductos(xml: string): Producto[] {

    const doc =
      new DOMParser()
        .parseFromString(xml, 'text/xml');

    const nodos =
      Array.from(
        doc.getElementsByTagNameNS('*', 'Producto')
      );

    return nodos.map(nodo => ({
      idProducto:
        this.numero(nodo, 'IdProducto'),

      idCategoria:
        this.numero(nodo, 'IdCategoria'),

      nombre:
        this.texto(nodo, 'Nombre'),

      descripcion:
        this.texto(nodo, 'Descripcion'),

      precio:
        this.numero(nodo, 'Precio'),

      stock:
        this.numero(nodo, 'Stock'),

      estado:
        this.texto(nodo, 'Estado')
          .toLowerCase() === 'true'
    }));
  }

  private texto(
    nodo: Element,
    nombre: string
  ): string {

    return nodo
      .getElementsByTagNameNS('*', nombre)[0]
      ?.textContent ?? '';
  }

  private numero(
    nodo: Element,
    nombre: string
  ): number {

    return Number(
      this.texto(nodo, nombre) || 0
    );
  }
}