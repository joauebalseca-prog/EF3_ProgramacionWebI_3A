using CoreWCF;
using ProductosSOAP.Models;

namespace ProductosSOAP.Services;

[ServiceContract(Namespace = "http://programacionweb/examen3a")]
public interface IProductoService
{
    [OperationContract(Action = "http://programacionweb/examen3a/IProductoService/ListarProductos", ReplyAction = "*")]
    List<Producto> ListarProductos();

    [OperationContract(Action = "http://programacionweb/examen3a/IProductoService/ObtenerProducto", ReplyAction = "*")]
    Producto? ObtenerProducto(int idProducto);

    [OperationContract(Action = "http://programacionweb/examen3a/IProductoService/CrearProducto", ReplyAction = "*")]
    Producto CrearProducto(Producto producto);

    [OperationContract(Action = "http://programacionweb/examen3a/IProductoService/ActualizarProducto", ReplyAction = "*")]
    Producto? ActualizarProducto(Producto producto);

    [OperationContract(Action = "http://programacionweb/examen3a/IProductoService/EliminarProducto", ReplyAction = "*")]
    bool EliminarProducto(int idProducto);

    [OperationContract(Action = "http://programacionweb/examen3a/IProductoService/ActualizarStock", ReplyAction = "*")]
    bool ActualizarStock(int idProducto, int nuevoStock);
}
